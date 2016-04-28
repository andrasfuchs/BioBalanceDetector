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

// set pin #8 (ATmega pin #14) as the slave select for the ATTiny
const int slaveSelectPinMultiplexer = 8;

// set pin #9 (ATmega pin #15) as the slave select for the ADC
const int slaveSelectPinADC = 9;

// set pin #10 (ATmega pin #16) as the slave select for the DAC
const int slaveSelectPinDAC = 10;

// We need to declare the data exchange
// variable to be volatile - the value is
// read from memory.
volatile int value = 0;
volatile int channel = 0;

// Install the interrupt routine.
ISR(INT0_vect) {
	Serial.println("interrupt 0");
}

void setup() {
	// set the slaveSelectPin as an output:
	pinMode(slaveSelectPinADC, OUTPUT);
	digitalWrite(slaveSelectPinADC, HIGH);

	pinMode(slaveSelectPinDAC, OUTPUT);
	digitalWrite(slaveSelectPinDAC, HIGH);

	pinMode(slaveSelectPinMultiplexer, OUTPUT);
	digitalWrite(slaveSelectPinMultiplexer, HIGH);

	// initialize SPI:
	SPI.begin();

	// set the baud rate to 115200
	Serial.begin(115200);

	// set the sensitivity to the default
	setDAC(4096);
}

void loop() {
	for (int i = 0; i < 10; i++)
	{
		setChannel(0);
		readADC();
		delay(10);
	}
}

void setChannel(unsigned int ch)
{
}

void readADC()
{
	SPI.beginTransaction(SPISettings(1000000, MSBFIRST, SPI_MODE1));

	// take the SS pin low to select the chip:
	digitalWrite(slaveSelectPinADC, LOW);

	byte rx1 = SPI.transfer(0);
	byte rx2 = SPI.transfer(0);

	// take the SS pin high to de-select the chip:
	digitalWrite(slaveSelectPinADC, HIGH);

	SPI.endTransaction();

	unsigned int adcValue = (rx1 * 256) + rx2;
	Serial.print("ch");
	Serial.print(channel);
	Serial.print("|");
	Serial.println(adcValue);
}


void setDAC(word value)
{
	if (value > 4095) value = 4095;
	if (value < 0) value = 0;

	Serial.print("-- set DAC to ");
	Serial.print(value);
	Serial.print("\t|\t");

	//  send the data via SPI:
	byte channel = 0b0;  // 0 - select DAC-A | 0 - select DAC-B
	byte gain = 0b0;  // 0 - x2 gain | 1 - x1 gain
	byte active = 0b1;  // 0 - shut down DAC channel | 1 - enable DAC channel
	byte DACdataH = (value & 0b111100000000) >> 8;    // higher 4 bits of value
	byte DACdataL = (value & 0b000011111111);         // lower 8 bits of value

	byte tx1 = (channel << 7) + (gain << 5) + (active << 4) + DACdataH;
	byte tx2 = DACdataL;

	//  tx1 = 0b00010000 + DACdataH;
	//  tx2 = 0b11111111;

	SPI.beginTransaction(SPISettings(1000000, MSBFIRST, SPI_MODE0));

	// take the SS pin low to select the chip:
	digitalWrite(slaveSelectPinDAC, LOW);

	byte rx1 = SPI.transfer(tx1);
	byte rx2 = SPI.transfer(tx2);

	Serial.print("tx:\t");
	Serial.print(tx1);
	Serial.print("\t");
	Serial.print(tx2);

	Serial.print("\t|\t");

	Serial.print("rx:\t");
	Serial.print(rx1);
	Serial.print("\t");
	Serial.println(rx2);

	// take the SS pin high to de-select the chip:
	digitalWrite(slaveSelectPinDAC, HIGH);

	SPI.endTransaction();
}

