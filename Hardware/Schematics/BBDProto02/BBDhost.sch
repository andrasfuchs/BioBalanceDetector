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
LIBS:BBDhost-cache
EELAYER 25 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title "Bio Balance Detector Proto #2 - 3-sensor host"
Date "2016-01-20"
Rev "1"
Comp "Copyright © Andras Fuchs (andras.fuchs@gmail.com)"
Comment1 "This version is based on Atmel ATtiny84A as host and ATtiny25 as sensor."
Comment2 "health imbalance."
Comment3 "the electromagnetic energy field around human beings and indicate any potential"
Comment4 "Bio Balance Detector is a software and hardware combination to detect, meter and show "
$EndDescr
$Comp
L ATTINY84A-P IC2
U 1 1 561D0407
P 5900 4300
F 0 "IC2" H 5050 5050 40  0000 C CNN
F 1 "ATTINY84A-P" H 6600 3550 40  0000 C CNN
F 2 "Housings_DIP:DIP-14_W7.62mm" H 5900 4100 35  0000 C CIN
F 3 "" H 5900 4300 60  0000 C CNN
	1    5900 4300
	1    0    0    -1  
$EndComp
$Comp
L USB_OTG P1
U 1 1 561D051E
P 3300 3200
F 0 "P1" H 3625 3075 50  0000 C CNN
F 1 "USB_OTG" H 3300 3400 50  0000 C CNN
F 2 "Connect:USB_Mini-B" V 3250 3100 60  0001 C CNN
F 3 "" V 3250 3100 60  0000 C CNN
	1    3300 3200
	0    -1   -1   0   
$EndComp
$Comp
L SW_PUSH SW6
U 1 1 561D0563
P 7600 5500
F 0 "SW6" H 7750 5610 50  0000 C CNN
F 1 "CHANGE MODE" H 7600 5420 50  0000 C CNN
F 2 "Buttons_Switches_ThroughHole:SW_PUSH" H 7600 5500 60  0001 C CNN
F 3 "" H 7600 5500 60  0000 C CNN
	1    7600 5500
	1    0    0    -1  
$EndComp
$Comp
L LED_RCBG D4
U 1 1 561D05B8
P 8100 3500
F 0 "D4" H 8100 3850 50  0000 C CNN
F 1 "LED_RCBG" H 8100 3150 50  0000 C CNN
F 2 "Buttons_Switches_ThroughHole:SW_DIP_x2_Piano" H 8100 3450 50  0001 C CNN
F 3 "" H 8100 3450 50  0000 C CNN
	1    8100 3500
	-1   0    0    1   
$EndComp
$Comp
L DIL8 P9
U 1 1 561D0725
P 6750 2400
F 0 "P9" H 6750 2650 60  0000 C CNN
F 1 "DIL8" V 6750 2400 50  0000 C CNN
F 2 "Pin_Headers:Pin_Header_Straight_2x04" H 6750 2400 60  0001 C CNN
F 3 "" H 6750 2400 60  0000 C CNN
	1    6750 2400
	1    0    0    -1  
$EndComp
$Comp
L Crystal Y5
U 1 1 561D094A
P 8700 5000
F 0 "Y5" H 8700 5150 50  0000 C CNN
F 1 "20 Mhz" H 8700 4850 50  0000 C CNN
F 2 "Crystals:Crystal_HC49-U_Vertical" H 8700 5000 60  0001 C CNN
F 3 "" H 8700 5000 60  0000 C CNN
	1    8700 5000
	-1   0    0    1   
$EndComp
$Comp
L DIL8 P8
U 1 1 561D1280
P 5250 2400
F 0 "P8" H 5250 2650 60  0000 C CNN
F 1 "DIL8" V 5250 2400 50  0000 C CNN
F 2 "Pin_Headers:Pin_Header_Straight_2x04" H 5250 2400 60  0001 C CNN
F 3 "" H 5250 2400 60  0000 C CNN
	1    5250 2400
	1    0    0    -1  
$EndComp
$Comp
L DIL8 P10
U 1 1 561D3C21
P 8150 2400
F 0 "P10" H 8150 2650 60  0000 C CNN
F 1 "DIL8" V 8150 2400 50  0000 C CNN
F 2 "Pin_Headers:Pin_Header_Straight_2x04" H 8150 2400 60  0001 C CNN
F 3 "" H 8150 2400 60  0000 C CNN
	1    8150 2400
	1    0    0    -1  
$EndComp
$Comp
L CP1 C11
U 1 1 561DC024
P 8400 5300
F 0 "C11" H 8425 5400 50  0000 L CNN
F 1 "12-22pF" H 8425 5200 50  0000 L CNN
F 2 "Capacitors_Elko_ThroughHole:Elko_vert_11.2x6.3mm_RM2.5_CopperClear" H 8400 5300 60  0001 C CNN
F 3 "" H 8400 5300 60  0000 C CNN
	1    8400 5300
	1    0    0    -1  
$EndComp
$Comp
L CP1 C12
U 1 1 561DC089
P 9000 5300
F 0 "C12" H 9025 5400 50  0000 L CNN
F 1 "12-22pF" H 9025 5200 50  0000 L CNN
F 2 "Capacitors_Elko_ThroughHole:Elko_vert_11.2x6.3mm_RM2.5_CopperClear" H 9000 5300 60  0001 C CNN
F 3 "" H 9000 5300 60  0000 C CNN
	1    9000 5300
	1    0    0    -1  
$EndComp
Wire Wire Line
	3600 3000 4450 3000
Wire Wire Line
	4450 2850 4450 5700
Wire Wire Line
	4450 4900 4850 4900
Wire Wire Line
	4550 3700 4850 3700
Wire Wire Line
	6950 3700 7300 3700
Wire Wire Line
	7300 3700 7300 3300
Wire Wire Line
	7300 3300 7800 3300
Wire Wire Line
	6950 3800 7400 3800
Wire Wire Line
	7400 3800 7400 3500
Wire Wire Line
	7400 3500 7800 3500
Wire Wire Line
	7500 3900 6950 3900
Wire Wire Line
	7500 3700 7500 3900
Wire Wire Line
	6950 4000 8650 4000
Wire Wire Line
	8650 4000 8650 3500
Wire Wire Line
	8650 3500 8400 3500
Wire Wire Line
	4000 5500 7300 5500
Wire Wire Line
	4550 1500 4550 3700
Wire Wire Line
	3600 3400 4550 3400
Wire Wire Line
	4000 3400 4000 5500
Connection ~ 4000 3400
Wire Wire Line
	6950 4400 8000 4400
Wire Wire Line
	6950 4900 7150 4900
Wire Wire Line
	7150 4900 7150 5300
Wire Wire Line
	7150 5300 4200 5300
Wire Wire Line
	4200 5300 4200 3200
Wire Wire Line
	4200 3200 3600 3200
Wire Wire Line
	3600 3300 4100 3300
Wire Wire Line
	4100 3300 4100 5400
Wire Wire Line
	4100 5400 7300 5400
Wire Wire Line
	7300 5400 7300 4800
Wire Wire Line
	7300 4800 6950 4800
Wire Wire Line
	6950 4700 9300 4700
Connection ~ 4450 3000
Connection ~ 4550 3400
Wire Wire Line
	9200 4600 9200 1700
Wire Wire Line
	9200 1700 4700 1700
Wire Wire Line
	4700 1700 4700 2350
Wire Wire Line
	4700 2350 4900 2350
Wire Wire Line
	4900 2450 4600 2450
Wire Wire Line
	4600 2450 4600 1600
Wire Wire Line
	4600 1600 9300 1600
Wire Wire Line
	8900 4100 6950 4100
Wire Wire Line
	8900 1800 8900 4100
Wire Wire Line
	6950 4200 9000 4200
Wire Wire Line
	4900 2550 4850 2550
Wire Wire Line
	4850 2550 4850 2850
Connection ~ 4850 2850
Wire Wire Line
	4450 2850 7700 2850
Wire Wire Line
	6350 2850 6350 2550
Wire Wire Line
	6350 2550 6400 2550
Wire Wire Line
	5600 2550 5700 2550
Wire Wire Line
	5700 2550 5700 2750
Wire Wire Line
	5700 2750 9000 2750
Wire Wire Line
	9000 2750 9000 4200
Wire Wire Line
	9100 2450 9100 4300
Wire Wire Line
	9100 4300 6950 4300
Wire Wire Line
	5700 1800 8900 1800
Wire Wire Line
	7200 1800 7200 2350
Wire Wire Line
	7200 2350 7100 2350
Wire Wire Line
	5600 2350 5700 2350
Wire Wire Line
	5700 2350 5700 1800
Connection ~ 7200 1800
Wire Wire Line
	4550 1500 8700 1500
Wire Wire Line
	5800 1500 5800 2250
Wire Wire Line
	5800 2250 5600 2250
Wire Wire Line
	7300 1500 7300 2250
Wire Wire Line
	7300 2250 7100 2250
Connection ~ 5800 1500
Wire Wire Line
	6400 2350 6300 2350
Wire Wire Line
	6300 2350 6300 1700
Connection ~ 6300 1700
Wire Wire Line
	6400 2450 6200 2450
Wire Wire Line
	6200 2450 6200 1600
Connection ~ 6200 1600
Wire Wire Line
	7700 2850 7700 2550
Wire Wire Line
	7700 2550 7800 2550
Connection ~ 6350 2850
Wire Wire Line
	8500 2350 8600 2350
Wire Wire Line
	8600 2350 8600 1800
Connection ~ 8600 1800
Wire Wire Line
	8700 1500 8700 2250
Wire Wire Line
	8700 2250 8500 2250
Connection ~ 7300 1500
Wire Wire Line
	7800 2350 7700 2350
Wire Wire Line
	7700 2350 7700 1700
Connection ~ 7700 1700
Wire Wire Line
	7600 1600 7600 2450
Wire Wire Line
	7600 2450 7800 2450
Connection ~ 7600 1600
Wire Wire Line
	8500 2450 9100 2450
Wire Wire Line
	7100 2550 7200 2550
Wire Wire Line
	7200 2550 7200 2650
Wire Wire Line
	7200 2650 5800 2650
Wire Wire Line
	5800 2650 5800 2450
Wire Wire Line
	5800 2450 5600 2450
Wire Wire Line
	7100 2450 7300 2450
Wire Wire Line
	7300 2450 7300 2650
Wire Wire Line
	7300 2650 8600 2650
Wire Wire Line
	8600 2650 8600 2550
Wire Wire Line
	8600 2550 8500 2550
Wire Wire Line
	6950 4600 9200 4600
Wire Wire Line
	9300 1600 9300 4700
Wire Wire Line
	8400 4700 8400 5150
Connection ~ 8400 4700
Wire Wire Line
	9000 4600 9000 5150
Connection ~ 9000 4600
Wire Wire Line
	8850 5000 9000 5000
Connection ~ 9000 5000
Wire Wire Line
	8550 5000 8400 5000
Connection ~ 8400 5000
Wire Wire Line
	4450 5700 9000 5700
Wire Wire Line
	9000 5700 9000 5450
Connection ~ 4450 4900
Wire Wire Line
	8400 5450 8400 5700
Connection ~ 8400 5700
Wire Wire Line
	7500 3700 7800 3700
Wire Wire Line
	7900 5500 8000 5500
Wire Wire Line
	8000 5500 8000 4400
$EndSCHEMATC
