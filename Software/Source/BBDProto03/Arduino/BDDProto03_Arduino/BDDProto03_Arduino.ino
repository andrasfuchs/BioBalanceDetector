/*
  Bio Balance Detector - Proto #3 - Arduino

 Created: 2016-04-01
 Updated: 2016-04-10
 by Andras Fuchs (andras.fuchs@gmail.com)
*/


// inslude the SPI library:
#include <SPI.h>


// set pin 10 as the slave select for the digital pot:
const int slaveSelectPinPot = 10;

const int slaveSelectPinADC = 9;

const int slaveSelectPinDAC = 10;

// We need to declare the data exchange
// variable to be volatile - the value is
// read from memory.
volatile int value = 0;

// Install the interrupt routine.
ISR(INT0_vect) { 
  Serial.println("interrupt 0");
}

void setup() {
  // set the slaveSelectPin as an output:
  pinMode(slaveSelectPinPot, OUTPUT);
  pinMode(slaveSelectPinADC, OUTPUT);
  pinMode(slaveSelectPinDAC, OUTPUT);
  
  // initialize SPI:
  SPI.begin();
//  SPI.usingInterrupt(0);
  
  Serial.begin(115200);
  
  setDAC(4096);
  
  Serial.println("setup done");
}

void loop() {   
    Serial.println("- - - - - - - - - - - - - - - -");   
    for (int i=0; i<10; i++)
    {
      readADC();
      delay(100);
    }
  
    //setDigitalPot(25);
    //delay(250);
     
    //setDigitalPot(255);
    //delay(250);    
    
    //setDigitalPot(200);
    //delay(5000); 
 
//    for (int i=0; i<4096; i=i+7)
//    {
//      setDAC(i);
//      delay(25);
//    }
}

void setDigitalPot(byte value) 
{  
  Serial.print("-- set pot level to ");   
  Serial.print(value);   
  Serial.print("\t|\t");   

  //  send the data via SPI: 
  byte address = 0b0000;
  byte command = 0b00;    // write data
  
  byte tx1 = (address << 4) + (command << 2) + (value >> 6);
  byte tx2 = (value << 2);

  
  SPI.beginTransaction(SPISettings(1000000, MSBFIRST, SPI_MODE1));
  
  // take the SS pin low to select the chip:
  digitalWrite(slaveSelectPinPot, LOW);
  
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

  Serial.print("-- get pot level back\t|\t");
  
  address = 0b0000;
  command = 0b11;    // read data
  tx1 = (address << 4) + (command << 2) + (0b00);
  tx2 = 0;
  
  rx1 = SPI.transfer(tx1);
  rx2 = SPI.transfer(tx2);

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
  digitalWrite(slaveSelectPinPot, HIGH);
  
  SPI.endTransaction();
}

void readADC() 
{
  Serial.print("-- getting ADC value\t|\t");   
  
  SPI.beginTransaction(SPISettings(1000000, MSBFIRST, SPI_MODE1));
  
  // take the SS pin low to select the chip:
  digitalWrite(slaveSelectPinADC, LOW);

  byte rx1 = SPI.transfer(0);
  byte rx2 = SPI.transfer(0);

  // take the SS pin high to de-select the chip:
  digitalWrite(slaveSelectPinADC, HIGH);
  
  SPI.endTransaction();
  
  Serial.print("tx:\t--\t--\t|\t");

  Serial.print("rx:\t");
  Serial.print(rx1);
  Serial.print("\t");
  Serial.print(rx2);  
  
  int total = (rx1*100) / 256;
  
  Serial.print("\t|\t");
  Serial.print(total);  
  Serial.println("%");  
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

