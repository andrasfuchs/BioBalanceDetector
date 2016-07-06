/*
  Bio Balance Detector - Proto #3 - Arduino

 Created: 2016-04-01
 Updated: 2016-04-26
 by Andras Fuchs (andras.fuchs@gmail.com)

 Changelog:
 * 2016-04-28
	+ moved to Visual Studio
	= changed the ATTiny SS port to pin #8
 * 2016-04-26
	- removed unused parts
	= simplified the ADC value sending code
*/


// inslude the SPI library:
#include <SPI.h>

#include "TimerOne\TimerOne.h"

// set pin #9 (ATmega pin #15) as the slave select for the ADC
const int pinSelectADC = 2;

// set pin #10 (ATmega pin #16) as the slave select for the DAC
const int pinSelectDAC = 7;

const int pinRGBLEDRed = 5;
const int pinRGBLEDGreen = 10;
const int pinRGBLEDBlue = 9;
const int pinRGBLEDPower = 6;

const int pinSelectChannelS0 = A0;
const int pinSelectChannelS1 = A1;
const int pinSelectChannelS2 = A2;
const int pinSelectChannelS3 = A3;
const int pinSelectChannelS4 = A4;
const int pinSelectChannelS5 = A5;
const int pinSelectChannelS6 = 3;
const int pinSelectChannelS7 = 4;


const long UARTBaudRate = 115200*16;

// We need to declare the data exchange
// variable to be volatile - the value is
// read from memory.
unsigned short values[256];
volatile unsigned short channel = 0;
volatile unsigned short j = 0;
char previousChannelBits[8];

// Install the interrupt routine.
ISR(INT0_vect) {
	Serial.println("interrupt 0");
}

void setup() {

	// set the RGB-LED's pins as an output:
	pinMode(pinRGBLEDRed, OUTPUT);
	pinMode(pinRGBLEDGreen, OUTPUT);
	pinMode(pinRGBLEDBlue, OUTPUT);
	pinMode(pinRGBLEDPower, OUTPUT);

	// channel select pins
	pinMode(pinSelectChannelS0, OUTPUT);
	pinMode(pinSelectChannelS1, OUTPUT);
	pinMode(pinSelectChannelS2, OUTPUT);
	pinMode(pinSelectChannelS3, OUTPUT);
	pinMode(pinSelectChannelS4, OUTPUT);
	pinMode(pinSelectChannelS5, OUTPUT);
	pinMode(pinSelectChannelS6, OUTPUT);
	pinMode(pinSelectChannelS7, OUTPUT);

	// SPI slave selectors
	pinMode(pinSelectADC, OUTPUT);
	pinMode(pinSelectDAC, OUTPUT);

	// initialize SPI:
	SPI.begin();

	// set the baud rate to 2400, 4800, 9600, 19200, 38400, 57600 or 115200
	Serial.begin(UARTBaudRate);
	bitSet(UCSR0A, U2X0);     // change the UART speed divider from 16 to 8

	// set the sensitivity to the default (valid range is 0-4096)
	setDAC(2048);

	setChannel(0);
	for (int i = 0; i < 256; i++)
	{
		values[i] = 0;
	}


	// set up the timer to count every 0.0001 seconds
	//Timer1.initialize(100);
	//Timer1.attachInterrupt(timerInterrupt);
}

volatile unsigned long timerCounter = 0; // use volatile for shared variables

void timerInterrupt(void)
{
	timerCounter++;
}

void loop() {
	//sendBenchmarkFlag('a');
	j++;
	if (j > 512) j = 0;
	setLED(j, j + 128, j + 256, 50);

	for (int i = 0; i < 256; i++)
	{
		//setChannel(((i%16) * 16) + (i/16));
		setChannel(i);
		readADC(true);
		//delay(1);
	}
	delay(60);
	//sendBenchmarkFlag('f');
	sendAllValues();
	//sendBenchmarkFlag('g');
	//delay(5);
}

void setLED(int red, int green, int blue, int power)
{
	if (red >= 256) red = 511 - (red % 512);
	if (green >= 256) green = 511 - (green % 512);
	if (blue >= 256) blue = 511 - (blue % 512);

	analogWrite(pinRGBLEDRed, red);
	analogWrite(pinRGBLEDGreen, green);
	analogWrite(pinRGBLEDBlue, blue);
	analogWrite(pinRGBLEDPower, 255-power);
	//analogWrite(pinRGBLEDPower, 0);
}

void setChannel(unsigned int ch)
{
	char channelBits[8];

	channelBits[0] = (ch & (1 << 0)) >> 0;
	channelBits[1] = (ch & (1 << 1)) >> 1;
	channelBits[2] = (ch & (1 << 2)) >> 2;
	channelBits[3] = (ch & (1 << 3)) >> 3;
	channelBits[4] = (ch & (1 << 4)) >> 4;
	channelBits[5] = (ch & (1 << 5)) >> 5;
	channelBits[6] = (ch & (1 << 6)) >> 6;
	channelBits[7] = (ch & (1 << 7)) >> 7;

	if (channelBits[0] != previousChannelBits[0])
	{
		digitalWrite(pinSelectChannelS0, channelBits[0]);
		previousChannelBits[0] = channelBits[0];
	}

	if (channelBits[1] != previousChannelBits[1])
	{
		digitalWrite(pinSelectChannelS1, channelBits[1]);
		previousChannelBits[1] = channelBits[1];
	}

	if (channelBits[2] != previousChannelBits[2])
	{
		digitalWrite(pinSelectChannelS2, channelBits[2]);
		previousChannelBits[2] = channelBits[2];
	}

	if (channelBits[3] != previousChannelBits[3])
	{
		digitalWrite(pinSelectChannelS3, channelBits[3]);
		previousChannelBits[3] = channelBits[3];
	}

	if (channelBits[4] != previousChannelBits[4])
	{
		digitalWrite(pinSelectChannelS4, channelBits[4]);
		previousChannelBits[4] = channelBits[4];
	}

	if (channelBits[5] != previousChannelBits[5])
	{
		digitalWrite(pinSelectChannelS5, channelBits[5]);
		previousChannelBits[5] = channelBits[5];
	}

	if (channelBits[6] != previousChannelBits[6])
	{
		digitalWrite(pinSelectChannelS6, channelBits[6]);
		previousChannelBits[6] = channelBits[6];
	}

	if (channelBits[7] != previousChannelBits[7])
	{
		digitalWrite(pinSelectChannelS7, channelBits[7]);
		previousChannelBits[7] = channelBits[7];
	}

	channel = ch;
}

unsigned int readADC(bool storeValue)
{
	SPI.beginTransaction(SPISettings(16000000, MSBFIRST, SPI_MODE1));

	// take the SS pin low to select the chip:
	digitalWrite(pinSelectADC, LOW);

	byte rx1 = SPI.transfer(0b10000000);
	byte rx2 = SPI.transfer(0b00000000);

	// take the SS pin high to de-select the chip:
	digitalWrite(pinSelectADC, HIGH);

	SPI.endTransaction();

	int value = (rx1 * 256) + rx2;

	if (storeValue)
	{
		values[channel] = value;
	}

	return value;
}

void sendSingleValue(int channel, unsigned int adcValue)
{
	if (adcValue > 0)
	{
		char buffer[6+2*8];
		snprintf(buffer, sizeof buffer, "<|ch|%u,%u|>", channel, adcValue);
		Serial.print(buffer);
	}
}

void sendAllValues()
{
	uint8_t buffer[5+256*2+2];

	for (int i = 0; i < 256; i++)
	{
		buffer[5 + i * 2 + 0] = (uint8_t)(values[i] >> 8);
		buffer[5 + i * 2 + 1] = (uint8_t)(values[i] & 0xFF);
	}

	//Serial.print("<|ll|");
	buffer[0] = 60;
	buffer[1] = 124;
	buffer[2] = 108;
	buffer[3] = 108;
	buffer[4] = 124;
	//Serial.print("|>");
	buffer[5 + 256 * 2 + 0] = 124;
	buffer[5 + 256 * 2 + 1] = 62;

	Serial.write(buffer, sizeof(buffer));	
}

void sendBenchmarkFlag(char flag)
{
	char buffer[15];

	if (flag == 'a') timerCounter = 0;

	snprintf(buffer, sizeof buffer, "<|bm|%c,%lu|>", flag, timerCounter);
	Serial.print(buffer);
}

void setDAC(word value)
{
	if (value > 4095) value = 4095;
	if (value < 0) value = 0;

	Serial.print("<|sd|");
	Serial.print(value);
	Serial.print("|>");

	//  send the data via SPI:
	byte channel = 0b0;  // 0 - select DAC-A | 0 - select DAC-B
	byte gain = 0b0;  // 0 - x2 gain | 1 - x1 gain
	byte active = 0b1;  // 0 - shut down DAC channel | 1 - enable DAC channel
	byte DACdataH = (value & 0b111100000000) >> 8;    // higher 4 bits of value
	byte DACdataL = (value & 0b000011111111);         // lower 8 bits of value

	byte tx1 = (channel << 7) + (gain << 5) + (active << 4) + DACdataH;
	byte tx2 = DACdataL;

	  //tx1 = 0b00010000 + DACdataH;
	  //tx2 = 0b11111111;

	SPI.beginTransaction(SPISettings(1000000, MSBFIRST, SPI_MODE0));

	// take the SS pin low to select the chip:
	digitalWrite(pinSelectDAC, LOW);

	SPI.transfer(tx1);
	SPI.transfer(tx2);

	// take the SS pin high to de-select the chip:
	digitalWrite(pinSelectDAC, HIGH);

	SPI.endTransaction();
}

