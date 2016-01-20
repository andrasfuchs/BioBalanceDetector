/*
 * BBD Proto #1 - EM Field Color Led (2015-12-12 - 2016-01-19)
 * main.cpp
 *
 * Created:		2015-12-12
 * Author:		Andras Fuchs
 * Controller:	Atmel ATtiny85
 * Pinout:		1 - NC						8 - Vcc
 *				2 - LED red					7 - sensor (ADC input)
 *				3 - LED green				6 - LED blue
 *				4 - GND						8 - LED power
 * Description:
 *				The device detects the changes in the electro-magnetic field in the environment and changes the color of the LED accordingly.
 *				After startup it waits a few seconds (LED continuous red), and then it starts to sample the environment (LED blinking red).
 *				After the sampling finished it saves the average value of the field, and compares it to the current value:
 *				If the value is similar to the initial value the LED is green
 *				If the value is smaller (negative delta) than the initial value the LED is blue
 *				If the value is higher (positive delta) than the initial value the LED is red
 * Changelog:
 */ 

#define F_CPU 1000000 // running at 1.0 Mhz?? The clock should run at 8 Mhz, but the 1 Mhz setting produces proper delay times

#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/sleep.h>
#include <util/delay.h>

double arrayAvg(double array[], int count);
void setLedPorts(int counter, int ledPower, int ledR, int ledG, int ledB);
int  set_PORTB_bit(int position, int value);
void spectral_color(double &r,double &g,double &b,double l);
void adc_init(uint8_t ch);
uint16_t adc_read();
void adc_read_async();
void error_blink(int counter);

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
	int counter = 0;
		
	// Set Port Bs as outputs (binary 1) or as inputs (binary 0)
	// PORTB bit 0 = physical pin #5 on the ATTINY85 - LED POWER
	// PORTB bit 1 = physical pin #6 on the ATTINY85 - LED BLUE
	// PORTB bit 3 = physical pin #2 on the ATTINY85 - LED RED
	// PORTB bit 4 = physical pin #3 on the ATTINY85 - LED GREEN
	DDRB = 0b00011011;

	double r, g, b;
	double l = 400.0;


	setLedPorts(128, 255, 255, 0, 255);
	_delay_ms(3000);


	// Enable Noise Reduction Sleep Mode
	set_sleep_mode( SLEEP_MODE_ADC );
	sleep_enable();
		
	sei(); // enable global interrupts
				
	adc_init(1);

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
		}


		if ((counter%256) == 0)		
		{		
			adc_read_async();	
			sleep_cpu();
		}
		
		if (i <= avgSamplesCountLong)
		{
			setLedPorts(counter, 0, 0, 0, 0);
		} else
		{			
			setLedPorts(counter, ledPower, ledR, ledG, ledB);
		}
	}
	
	return 1;
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
		set_PORTB_bit(3,1);
	}
	else
	{
		// turn RED led on
		set_PORTB_bit(3,0);
	}

	if (counter >= ledG)
	{
		set_PORTB_bit(4,1);
	}
	else
	{
		set_PORTB_bit(4,0);
	}

	if (counter >= ledB)
	{
		set_PORTB_bit(1,1);
	}
	else
	{
		set_PORTB_bit(1,0);
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
	// A: VCC used as Voltage Reference, disconnected from PB0 (AREF).
	int ref = (0<<REFS1)|(0<<REFS0);
	
	// B: 1.1V is used as Voltage Reference, disconnected from PB0 (AREF).
	//int ref =  (0<<REFS2)|(1<<REFS1)|(0<<REFS0);

	// select the corresponding channel 0~7
	// ANDing with ’7? will always keep the value
	// of ‘ch’ between 0 and 7
	ch &= 0b00000111;  // AND operation with 7
	ADMUX = (ref & 0xF8)|ch; // clears the bottom 3 bits before ORing
	
	// ADC Enable, do not start conversion, enable ADC interrupt, prescaler of 128
	// 10.0 Mhz/128 = 625kHz (48kSPS)
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
				
	double adcValue = ((double)ADC) / 1024.0;
				
	// short average
	adcValuesShort[i % avgSamplesCountShort] = adcValue;
	double adcValueAvgShort = arrayAvg(adcValuesShort, avgSamplesCountShort);
				
	// long average
	if (i <= avgSamplesCountLong)
	{
		if (i%10 == 0)
		{
			setLedPorts(i, 255, 255, 0, 0);
			_delay_ms(100);
			setLedPorts(i, 0, 0, 0, 0);
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
}

void error_blink(int counter)
{
	// error blink
	setLedPorts(counter, 255, 255, 0, 0);
	_delay_ms(500);

	setLedPorts(counter, 0, 0, 0, 0);
	_delay_ms(500);

	setLedPorts(counter, 255, 255, 0, 0);
	_delay_ms(500);

	setLedPorts(counter, 0, 0, 0, 0);
	_delay_ms(500);	
}