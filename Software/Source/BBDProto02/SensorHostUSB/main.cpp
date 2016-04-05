/*
 * BBD Proto #2 - EM Field Color Led and Antenna Tester with Power LEDs (2016-01-20 - 2016-03-12)
 *
 * Created: 2016-01-20
 * Author : Andras Fuchs (andras.fuchs@gmail.com)
 *
 * Controller:	Atmel ATmega168A
 * Pinout:		1 - Reset - button / Programmer yellow	28 - PC5 - Heartbeat
 *				2 - PD0 - Power level 8 LED				27 - PC4 - LED red
 *				3 - PD1 - Power level 7 LED				26 - PC3 - LED green
 *				4 - PD2 - Power level 6 LED				25 - NC
 *				5 - PD3 - Power level 5 LED				24 - NC
 *				6 - PD4 - Power level 4 LED				23 - ADC0 - antenna
 *				7 - Vcc									22 - GND
 *				8 - GND									21 - ARef - Battery 3.3V
 *				9 - NC									20 - AVcc = Vcc
 *				10 - NC									19 - SCK - Programmer green
 *				11 - PD5 - Power level 3 LED			18 - MISO - Programmer blue
 *				12 - PD6 - Power level 2 LED			17 - MOSI - Programmer orange
 *				13 - PD7 - Power level 1 LED			16 - PB2 - LED blue
 *				14 - PB0 - LED power					15 - NC
 * Description:
 *				The device detects the changes in the electro-magnetic field in the environment and changes the color of the LED accordingly.
 *				After startup it waits a few seconds (LED continuous red), and then it starts to sample the environment (LED blinking red).
 *				After the sampling finished it saves the average value of the field, and compares it to the current value:
 *				If the value is similar to the initial value the LED is green
 *				If the value is smaller (negative delta) than the initial value the LED is blue
 *				If the value is higher (positive delta) than the initial value the LED is red
 * Changelog:
 * 2016-03-12
 *  + added potmeter to ARef and changed the ADC's reference to ARef
 *  = changed the RGB LED pins
 *  - fixed the 55-second-freeze bug
 * 2016-03-11
 *  + 8 power leds on port Ds
 * 2016-03-10
 *	= changed delay values
 *  + adaptive long term average value
 *  + 2 selector buttons and antennas
 *  + reset button
 *
 * Notes:
 * Don't forget to set the fuses to use the external 20Mhz crystal as a clock (if you have one) by running
 * avrdude.exe -c usbasp -p atmega168 -U lfuse:w:0x6F:m
 *
*/

//#define F_CPU 8000000 // ATmega168's internal clock runs at 8.0 Mhz with CKDIV8 set (effective 1 Mhz), but changePrescaler speeds it up to 8 Mhz
#define F_CPU 20000000 // the external clock runs at 20.0 Mhz (with the 20Mhz extarnal clock the 1000ms delay takes 8 seconds, and with the internal 8 Mhz setting 20 seconds)

#include <avr/io.h>
#include <avr/sleep.h>
#include <avr/interrupt.h>
#include <avr/wdt.h>
#include <util/delay.h>

void changePrescaler(void);
double arrayAvg(double array[], int count);
void setLedPorts(int counter, int ledPower, int ledR, int ledG, int ledB);
int set_PORTB_bit(int position, int value);
int set_PORTC_bit(int position, int value);
void set_power_leds(int level);
void spectral_color(double &r,double &g,double &b,double l);
void adc_init(uint8_t ch);
uint16_t adc_read();
void adc_read_async();

const int avgSamplesCountShort = 5;
const int avgSamplesCountLong = 100;		// ~5 seconds


double adcValuesShort[avgSamplesCountShort];
double adcValuesLong[avgSamplesCountLong];

int ledPower = 50;	// 0 - 255
int ledR = 255;
int ledG = 0;
int ledB = 0;

int i = 0;
double adcValueAvgLong = 0.0;
double adcValueAvg = 0.0;
double adcValue = 0;


int main(void)
{
	changePrescaler();

	wdt_disable();
	wdt_enable(WDTO_8S);

	int counter = 0;
	
	// Set Port Bs as outputs (binary 1) or as inputs (binary 0)
	// PORTB bit 0 = physical pin #14 on the ATmega168A - LED Power
	// PORTB bit 1 = physical pin #16 on the ATmega168A - LED Blue
	// PORTB bit 3 = physical pin #17 on the ATmega168A - LED RED
	// PORTB bit 4 = physical pin #18 on the ATmega168A - LED GREEN
	DDRB = 0b00000101;

	// Set Port Cs as outputs (binary 1) or as inputs (binary 0)
	// PORTC bit 3 = physical pin #26 on the ATmega168 - LED Green
	// PORTC bit 4 = physical pin #27 on the ATmega168 - LED Red
	// PORTC bit 5 = physical pin #28 on the ATmega168 - Heartbeat LED
	DDRC = 0b00111000;

	// Set Port Ds as outputs (binary 1) or as inputs (binary 0)
	// PORTDs are the power level LEDs
	DDRD = 0b11111111;

	double r, g, b;
	double l = 400.0;

	setLedPorts(128, 255, 255, 0, 255);
	_delay_ms(5000);

	// Enable Noise Reduction Sleep Mode
	set_sleep_mode( SLEEP_MODE_ADC );
	sleep_enable();
		
	sei(); // enable global interrupts
		
	adc_init(0);

	// Set up a forever loop
    while (1) 
    {
		// version A: easy to read, but causes flicker
		//counter = (counter+1) % (120*256);

		// version B: looks more complicated but does not flicker
		counter = (counter+1);
		while (counter >= (120*256))
		{
			counter = counter - (120*256);

			PORTC ^= (1 << 5);
		}

		if ((counter%256) == 0)
		{
			//adc_read_async();
			sleep_cpu(); // this method automatically call the ADC interrupt method (so the adc_read_async() call is not neccessary)
		}
		
		if (i <= avgSamplesCountLong)
		{
			setLedPorts(counter, 0, 0, 0, 0);
		} else
		{
			setLedPorts(counter, ledPower, ledR, ledG, ledB);
		}

		wdt_reset();
	}
	
	return 1;
}


void changePrescaler(void) {
	cli();
	CLKPR = 1 << CLKPCE;		// Set the prescaler change enable bit to 1
	CLKPR = 0;					// Set the prescaler value to 1 and set the prescaler change enable bit to 0
	sei();
}




double arrayAvg(double array[], int count)
{
	double avg = 0.0;
	
	for (int j=0; j<count; j++)
	{
		avg = avg + array[j];
	}
	avg = avg / count;

	return avg;
}

void setLedPorts(int counter, int ledPower, int ledR, int ledG, int ledB)
{
	counter = counter % 256;
	
	if (counter <= ledPower)
	{
		set_PORTB_bit(0,1);
	}
	else
	{
		set_PORTB_bit(0,0);
	}


	if (counter >= ledR)
	{
		// turn RED led off
		set_PORTC_bit(4,1);
	}
	else
	{
		// turn RED led on
		set_PORTC_bit(4,0);
	}

	if (counter >= ledG)
	{
		set_PORTC_bit(3,1);
	}
	else
	{
		set_PORTC_bit(3,0);
	}

	if (counter >= ledB)
	{
		set_PORTB_bit(2,1);
	}
	else
	{
		set_PORTB_bit(2,0);
	}
}

int set_PORTB_bit(int position, int value)
{
	// Sets or clears the bit in position 'position'
	// either high or low (1 or 0) to match 'value'.
	// Leaves all other bits in PORTB unchanged.
	
	if (value == 0)
	{
		PORTB &= ~(1 << position);      // Set bit position low
	}
	else
	{
		PORTB |= (1 << position);       // Set high, leave others alone
	}

	return 1;
}

int set_PORTC_bit(int position, int value)
{
	// Sets or clears the bit in position 'position'
	// either high or low (1 or 0) to match 'value'.
	// Leaves all other bits in PORTC unchanged.
	
	if (value == 0)
	{
		PORTC &= ~(1 << position);      // Set bit position low
	}
	else
	{
		PORTC |= (1 << position);       // Set high, leave others alone
	}

	return 1;
}

void set_power_leds(int level)
{
	switch (level)
	{
		case 0: 
		PORTD = 0b00000000;
		break;

		case 1:
		PORTD = 0b10000000;
		break;

		case 2:
		PORTD = 0b11000000;
		break;

		case 3:
		PORTD = 0b11100000;
		break;

		case 4:
		PORTD = 0b11110000;
		break;

		case 5:
		PORTD = 0b11111000;
		break;

		case 6:
		PORTD = 0b11111100;
		break;

		case 7:
		PORTD = 0b11111110;
		break;

		case 8:
		PORTD = 0b11111111;
		break;
	}
}

void spectral_color(double &r,double &g,double &b,double l) // RGB <0,1> <- lambda l <400,700> [nm]
{
	double t;  r=0.0; g=0.0; b=0.0;
	if ((l>=400.0)&&(l<410.0)) { t=(l-400.0)/(410.0-400.0); r=    +(0.33*t)-(0.20*t*t); }
	else if ((l>=410.0)&&(l<475.0)) { t=(l-410.0)/(475.0-410.0); r=0.14         -(0.13*t*t); }
	else if ((l>=545.0)&&(l<595.0)) { t=(l-545.0)/(595.0-545.0); r=    +(1.98*t)-(     t*t); }
	else if ((l>=595.0)&&(l<650.0)) { t=(l-595.0)/(650.0-595.0); r=0.98+(0.06*t)-(0.40*t*t); }
	else if ((l>=650.0)&&(l<700.0)) { t=(l-650.0)/(700.0-650.0); r=0.65-(0.84*t)+(0.20*t*t); }
	if ((l>=415.0)&&(l<475.0)) { t=(l-415.0)/(475.0-415.0); g=             +(0.80*t*t); }
	else if ((l>=475.0)&&(l<590.0)) { t=(l-475.0)/(590.0-475.0); g=0.8 +(0.76*t)-(0.80*t*t); }
	else if ((l>=585.0)&&(l<639.0)) { t=(l-585.0)/(639.0-585.0); g=0.84-(0.84*t)           ; }
	if ((l>=400.0)&&(l<475.0)) { t=(l-400.0)/(475.0-400.0); b=    +(2.20*t)-(1.50*t*t); }
	else if ((l>=475.0)&&(l<560.0)) { t=(l-475.0)/(560.0-475.0); b=0.7 -(     t)+(0.30*t*t); }
}

void adc_init(uint8_t ch)
{
	// A: AREF, internal Vref turned off
	int ref = (0<<REFS1)|(0<<REFS0);
	
	// B: AVcc as Voltage Reference with external capacitor at AREF pin (#21).
	// int ref = (0<<REFS1)|(1<<REFS0);

	// C: Internal 1.1V Voltage Reference with external capacitor at AREF pin (#21).
	// int ref = (1<<REFS1)|(1<<REFS0);

	// select the corresponding channel 0~7
	// ANDing with ’7? will always keep the value
	// of ‘ch’ between 0 and 7
	ch &= 0b00000111;  // AND operation with 7
	ADMUX = (ref & 0xF8)|ch; // clears the bottom 3 bits before ORing
	
	// ADC Enable, do not start conversion, enable ADC interrupt, prescaler of 128
	// 20.0 Mhz/128 = 1'250kHz (96kSPS)
	ADCSRA = (1<<ADEN)|(0<<ADSC)|(1 << ADIE)|(1<<ADPS2)|(1<<ADPS1)|(1<<ADPS0);
}

uint16_t adc_read()
{
	// start single conversion
	// write '1' to ADSC
	ADCSRA |= (1<<ADSC);
	
	// wait for conversion to complete
	// ADSC becomes '0' again
	// till then, run loop continuously
	while(ADCSRA & (1<<ADSC));
	
	return (ADC);
}

void adc_read_async()
{
	// start single conversion
	// write '1' to ADSC
	ADCSRA |= (1<<ADSC);
}

ISR(ADC_vect)
{
	i = (i+1);
	if (i >= (120*256))
	{
		i = i - (120*256);
		if (i < avgSamplesCountLong)
		{
			i = avgSamplesCountLong;
		}
	}

	
	double adcValue = ((double)ADC) / 1024.0;
	
	// short average
	adcValuesShort[i % avgSamplesCountShort] = adcValue;
	double adcValueAvgShort = arrayAvg(adcValuesShort, avgSamplesCountShort);

	set_power_leds((int)(adcValueAvgShort*9));
	
	// long average
	if (i <= avgSamplesCountLong)
	{
		if (i%10 == 0)
		{
			setLedPorts(i, 255, 255, 0, 0);
			_delay_ms(200);
			setLedPorts(i, 0, 0, 0, 0);
			_delay_ms(200);
		}

		adcValuesLong[i % avgSamplesCountLong] = adcValue;
		
		if (i == avgSamplesCountLong)
		{
			adcValueAvgLong = arrayAvg(adcValuesLong, avgSamplesCountLong);
		}
	}
	
	
	adcValueAvg = (adcValueAvgShort - adcValueAvgLong) + 0.5;
	//adcValueAvg = adcValueAvg + 0.005;
	
	if (adcValueAvg < 0.0)
	{
		adcValueAvg = 0.0;
	}
	if (adcValueAvg > 1.0)
	{
		adcValueAvg = 1.0;
	}


	// method 1: color spectrum
	//l = (adcValueAvg * 285.0) + 400.0;
	
	//l = l+1.0;
	//if (l > 700.0)
	//{
	//l = l - 300.0;
	//}
	
	
	//spectral_color(r, g, b, l);
	//ledR = (int)(r*256);
	//ledG = (int)(g*256);
	//ledB = (int)(b*256);
	
	//ledPower = (int)(100 * adcValueAvg);
	
	// method 2: green - neutral, red power - positive, blue power - negative delta
	if ((adcValueAvg > 0.4) && (adcValueAvg < 0.6))
	{
		ledR = 0;
		ledG = 255;
		ledB = 0;
		
		if (adcValueAvg < 0.5)
		{
			ledPower = (int)(1000 * (adcValueAvg - 0.4)) + 10;
		} else
		{
			ledPower = (int)(1000 * (0.6 - adcValueAvg)) + 10;
		}
	} else if (adcValueAvg <= 0.4)
	{
		ledR = 0;
		ledG = 0;
		ledB = 255;
		
		ledPower = (int)(256 * (0.4 - adcValueAvg)) + 10;
	} else if (adcValueAvg >= 0.6)
	{
		ledR = 255;
		ledG = 0;
		ledB = 0;
		
		ledPower = (int)(256 * (adcValueAvg - 0.6)) + 10;
	}

	// move the long term average a little
	if (adcValueAvg < 0.5)
	{
		if (adcValueAvgLong > 0.1)
		{
			adcValueAvgLong = adcValueAvgLong - 0.00001;
		}
	} else {
		if (adcValueAvgLong < 0.9)
		{
			adcValueAvgLong = adcValueAvgLong + 0.00001;
		}
	}
}
