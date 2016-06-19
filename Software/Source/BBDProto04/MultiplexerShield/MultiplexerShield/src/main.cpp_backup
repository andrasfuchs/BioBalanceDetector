/*
 * AVRMultiplexer.cpp
 *
 * BBD Proto #4 - Multiplexer Arduino Shield Controller (2016-05-12 - 2016-05-12)
 *
 * Created: 2016-05-12
 * Author : Andras Fuchs (andras.fuchs@gmail.com)
 *
 * Controller:	Atmel ATxmega128A4U
 * Pinout:		1 - MOSI - P-orange			20 - S0 on minor multiplexer
 *				2 - MISO - P-blue			19 - S1 on minor multiplexer
 *				3 - SCK - P-green			18 - S2 on minor multiplexer
 *				4 - ^SS - Slave Select		17 - S3 on minor multiplexer
 *				5 - Vcc						16 - AGND
 *				6 - GND						15 - AVcc
 *				7 - Heartbeat				14 - S0 on major multiplexer
 *				8 - ADC Vref / DAC out		13 - S1 on major multiplexer
 *				9 - Multiplexer I/O select	12 - S2 on major multiplexer
 *				10 - ^Reset - P-yellow		11 - S3 on major multiplexer
 * Description:
 *				This is the source code of the Atmel mikrocontroller on the Arduino Multiplexer Shield which is able to control 256 I/O channels.
 *				The has a DAC and an ADC and 17 16ch multiplexers.
 * Changelog:
 *	2016-05-12
 *		Initial release
 *
 */

#define F_CPU 9000000 // ATtiny861's internal clock runs at 9.0 Mhz with CKDIV8 set (effective 1.125 Mhz), but changePrescaler speeds it up to 9 Mhz



#include <avr/io.h>
#include <avr/interrupt.h>
//#include <avr/wdt.h>
#include <util/delay.h>

void changePrescaler(void);

int main(void)
{
	changePrescaler();

	PCMSK0 = 0b00000000;
	PCMSK1 = 0b00000000;

	//wdt_disable();
	//wdt_enable(WDTO_8S);

	// Set Port As as outputs (binary 1) or as inputs (binary 0)
	// PORTA bit 0 = physical pin #20 on the ATtiny861 - S0 (LSB) of the minor multiplexer
	// PORTA bit 1 = physical pin #19 on the ATtiny861 - S1 of the minor multiplexer
	// PORTA bit 2 = physical pin #18 on the ATtiny861 - S2 of the minor multiplexer
	// PORTA bit 3 = physical pin #17 on the ATtiny861 - S3 of the minor multiplexer
	// PORTA bit 4 = physical pin #14 on the ATtiny861 - S0 of the major multiplexer
	// PORTA bit 5 = physical pin #13 on the ATtiny861 - S1 of the major multiplexer
	// PORTA bit 6 = physical pin #12 on the ATtiny861 - S2 of the major multiplexer
	// PORTA bit 7 = physical pin #11 on the ATtiny861 - S3 (MSB) of the major multiplexer
	DDRA = 0b11111111;

	// Set Port Bs as outputs (binary 1) or as inputs (binary 0)
	// PORTB bit 0 = physical pin #1 on the ATtiny861 - MOSI (ATtiny is slave now, so effectively this is the SPI input)
	// PORTB bit 1 = physical pin #2 on the ATtiny861 - MISO (ATtiny is slave now, so effectively this is the SPI ouput)
	// PORTB bit 2 = physical pin #3 on the ATtiny861 - SCK
	// PORTB bit 3 = physical pin #4 on the ATtiny861 - ^SS (ATtiny is slave now, so this is the slave select)
	// PORTB bit 4 = physical pin #7 on the ATtiny861 - heartbeat
	// PORTB bit 5 = physical pin #8 on the ATtiny861 - Vref ADC input
	// PORTB bit 7 = physical pin #10 on the ATtiny861 - ^RESET
	DDRB = 0b00010010;


	DDRE = 0b11111111;

	// select the 19th input on the multiplexers
	PORTA = 0b11111111;

	PORTB = 0b00000000;
	 
	 // USICR: USI Control Register | USISIE - Start Condition Interrupt Enable, USIWM0 - SPI Mode 1, USICS0-USICS1-USICLK - Clock to External negative edge (slave mode)
	 USICR = (1 << USIOIE) | (1 << USIWM0) | (1 << USICS0) | (1 << USICS1) | (0 << USICLK);
		
	/* Enable Global Interrupts */
	//sei();


    while (1) 
    {
		//wdt_reset();

		PORTE ^= (1 << 4);
		_delay_ms(1000);
    }
}

void changePrescaler(void) 
{
	//cli();
	CLKPR = 1 << CLKPCE;		// Set the prescaler change enable bit to 1
	CLKPR = 0;					// Set the prescaler value to 1 and set the prescaler change enable bit to 0
	//sei();
}