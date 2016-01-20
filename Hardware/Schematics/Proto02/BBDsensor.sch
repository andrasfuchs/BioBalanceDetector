EESchema Schematic File Version 2
LIBS:power
LIBS:device
LIBS:transistors
LIBS:conn
LIBS:linear
LIBS:regul
LIBS:74xx
LIBS:cmos4000
LIBS:adc-dac
LIBS:memory
LIBS:xilinx
LIBS:microcontrollers
LIBS:dsp
LIBS:microchip
LIBS:analog_switches
LIBS:motorola
LIBS:texas
LIBS:intel
LIBS:audio
LIBS:interface
LIBS:digital-audio
LIBS:philips
LIBS:display
LIBS:cypress
LIBS:siliconi
LIBS:opto
LIBS:atmel
LIBS:contrib
LIBS:valves
EELAYER 25 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title "Bio Balance Detector Proto #2 - sensor"
Date "2016-01-20"
Rev "1"
Comp "Copyright © Andras Fuchs (andras.fuchs@gmail.com)"
Comment1 "This version is based on Atmel ATtiny84A as host and ATtiny25 as sensor."
Comment2 "health imbalance."
Comment3 "the electromagnetic energy field around human beings and indicate any potential"
Comment4 "Bio Balance Detector is a software and hardware combination to detect, meter and show "
$EndDescr
$Comp
L ATTINY25-S IC3
U 1 1 561D0468
P 5800 2650
F 0 "IC3" H 4650 3050 40  0000 C CNN
F 1 "ATTINY25-S" H 6800 2250 40  0000 C CNN
F 2 "Housings_SOIC:SOIC-8_3.9x4.9mm_Pitch1.27mm" H 6750 2650 35  0000 C CIN
F 3 "" H 5800 2650 60  0000 C CNN
	1    5800 2650
	1    0    0    -1  
$EndComp
$Comp
L DIL8 P7
U 1 1 561D1227
P 5850 4100
F 0 "P7" H 5850 4350 60  0000 C CNN
F 1 "DIL8" V 5850 4100 50  0000 C CNN
F 2 "Pin_Headers:Pin_Header_Angled_2x04" H 5850 4100 60  0001 C CNN
F 3 "" H 5850 4100 60  0000 C CNN
	1    5850 4100
	0    -1   -1   0   
$EndComp
Wire Wire Line
	4450 2700 4300 2700
Wire Wire Line
	4300 2700 4300 4650
Wire Wire Line
	4300 4650 5800 4650
Wire Wire Line
	5800 4650 5800 4450
Wire Wire Line
	4450 2800 4200 2800
Wire Wire Line
	4200 2800 4200 4750
Wire Wire Line
	4200 4750 5900 4750
Wire Wire Line
	5900 4750 5900 4450
Wire Wire Line
	7150 2900 7300 2900
Wire Wire Line
	7300 2900 7300 4750
Wire Wire Line
	7300 4750 6000 4750
Wire Wire Line
	6000 4750 6000 4450
Wire Wire Line
	7150 2400 7400 2400
Wire Wire Line
	7400 2400 7400 3200
Wire Wire Line
	7400 3200 5700 3200
Wire Wire Line
	5700 3200 5700 3750
Wire Wire Line
	4450 2600 4100 2600
Wire Wire Line
	4100 2600 4100 3300
Wire Wire Line
	4100 3300 5800 3300
Wire Wire Line
	5800 3300 5800 3750
Wire Wire Line
	4450 2500 4000 2500
Wire Wire Line
	4000 2500 4000 3400
Wire Wire Line
	4000 3400 5900 3400
Wire Wire Line
	5900 3400 5900 3750
Wire Wire Line
	4450 2400 3900 2400
Wire Wire Line
	3900 2400 3900 3500
Wire Wire Line
	3900 3500 6000 3500
Wire Wire Line
	6000 3500 6000 3750
$Comp
L CONN_01X01 P13
U 1 1 561DD29E
P 4700 3900
F 0 "P13" H 4700 4000 50  0000 C CNN
F 1 "SENSOR" V 4800 3900 50  0000 C CNN
F 2 "Pin_Headers:Pin_Header_Straight_1x01" H 4700 3900 60  0001 C CNN
F 3 "" H 4700 3900 60  0000 C CNN
	1    4700 3900
	1    0    0    -1  
$EndComp
Wire Wire Line
	4450 2900 4400 2900
Wire Wire Line
	4400 2900 4400 3900
Wire Wire Line
	4400 3900 4500 3900
$EndSCHEMATC
