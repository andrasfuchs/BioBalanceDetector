EESchema Schematic File Version 4
LIBS:Venus-256-cache
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 3 5
Title "Raspberry Pi GOIP Header"
Date "2018-04-27"
Rev "A"
Comp "Bio Balance Deterctor"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Connector_Generic:Conn_02x20_Odd_Even J?
U 1 1 5AE76166
P 2100 2000
F 0 "J?" H 2150 3000 50  0000 C CNN
F 1 "RaspberryPi header" H 2150 900 50  0000 C CNN
F 2 "" H 2100 2000 50  0001 C CNN
F 3 "~" H 2100 2000 50  0001 C CNN
	1    2100 2000
	1    0    0    -1  
$EndComp
Text HLabel 1600 1100 0    50   Output ~ 0
DC_POWER_3V3
Wire Wire Line
	1900 1900 1600 1900
Text HLabel 1600 2000 0    50   Output ~ 0
SPI_MOSI
Text HLabel 1600 2100 0    50   Input ~ 0
SPI_MISO
Text HLabel 1600 2200 0    50   Output ~ 0
SPI_CLK
Text HLabel 1600 1900 0    50   Output ~ 0
DC_POWER_3V3
Wire Wire Line
	1600 1100 1900 1100
Wire Wire Line
	1900 2000 1600 2000
Wire Wire Line
	1900 2100 1600 2100
Wire Wire Line
	1900 2200 1600 2200
Text HLabel 1600 1500 0    50   UnSpc ~ 0
GND
Text HLabel 1600 2300 0    50   UnSpc ~ 0
GND
Text HLabel 1600 3000 0    50   UnSpc ~ 0
GND
Text HLabel 2700 2700 2    50   UnSpc ~ 0
GND
Text HLabel 2700 2500 2    50   UnSpc ~ 0
GND
Text HLabel 2700 2000 2    50   UnSpc ~ 0
GND
Text HLabel 2700 1700 2    50   UnSpc ~ 0
GND
Text HLabel 2700 1300 2    50   UnSpc ~ 0
GND
Text HLabel 1600 1200 0    50   BiDi ~ 0
I2C_SDA1
Text HLabel 1600 1300 0    50   BiDi ~ 0
I2C_SCL1
Text HLabel 1600 2400 0    50   BiDi ~ 0
ID_SD
Text HLabel 2700 2400 2    50   BiDi ~ 0
ID_SC
Wire Wire Line
	1600 1200 1900 1200
Wire Wire Line
	1600 1300 1900 1300
Wire Wire Line
	1600 1500 1900 1500
Wire Wire Line
	1600 2300 1900 2300
Wire Wire Line
	1600 2400 1900 2400
Wire Wire Line
	1600 3000 1900 3000
Wire Wire Line
	2400 2700 2700 2700
Wire Wire Line
	2400 2500 2700 2500
Wire Wire Line
	2400 2400 2700 2400
Wire Wire Line
	2400 2000 2700 2000
Wire Wire Line
	2400 1700 2700 1700
Wire Wire Line
	2400 1300 2700 1300
Text HLabel 2700 1100 2    50   Output ~ 0
DC_POWER_5V0
Text HLabel 2700 1200 2    50   Output ~ 0
DC_POWER_5V0
Wire Wire Line
	2400 1100 2700 1100
Wire Wire Line
	2400 1200 2700 1200
$EndSCHEMATC
