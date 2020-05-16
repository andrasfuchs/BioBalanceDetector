EESchema Schematic File Version 4
EELAYER 30 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title "Mars-64"
Date "2020-05-10"
Rev "E"
Comp "Bio Balance Detector"
Comment1 "PmodM: Samtec TSM-106-01-?-DH"
Comment2 "PmodF: Samtec SMH-106-02-?-D"
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Analog_Switch:ADG731 U101
U 1 1 5E2A9665
P 3300 3900
F 0 "U101" H 3000 5950 50  0000 L CNN
F 1 "ADG731" H 3000 5850 50  0000 L CNN
F 2 "Package_QFP:TQFP-48_7x7mm_P0.5mm" H 3550 1950 50  0001 L CNN
F 3 "https://www.analog.com/media/en/technical-documentation/data-sheets/ADG725_731.pdf" H 3310 4300 50  0001 C CNN
	1    3300 3900
	1    0    0    -1  
$EndComp
$Comp
L Analog_Switch:ADG731 U102
U 1 1 5E2C09E7
P 6600 3900
F 0 "U102" H 6300 5950 50  0000 L CNN
F 1 "ADG731" H 6300 5850 50  0000 L CNN
F 2 "Package_QFP:TQFP-48_7x7mm_P0.5mm" H 6850 1950 50  0001 L CNN
F 3 "https://www.analog.com/media/en/technical-documentation/data-sheets/ADG725_731.pdf" H 6610 4300 50  0001 C CNN
	1    6600 3900
	1    0    0    -1  
$EndComp
Wire Wire Line
	2300 2400 2900 2400
$Comp
L Connector:TestPoint_Small TP101
U 1 1 5E316390
P 2300 2400
F 0 "TP101" V 2393 2400 50  0000 C CNN
F 1 "TestPoint_Small" V 2394 2400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2500 2400 50  0001 C CNN
F 3 "~" H 2500 2400 50  0001 C CNN
	1    2300 2400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP102
U 1 1 5E317DB9
P 2100 2500
F 0 "TP102" V 2193 2500 50  0000 C CNN
F 1 "TestPoint_Small" V 2194 2500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2300 2500 50  0001 C CNN
F 3 "~" H 2300 2500 50  0001 C CNN
	1    2100 2500
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP103
U 1 1 5E31886C
P 1900 2600
F 0 "TP103" V 1993 2600 50  0000 C CNN
F 1 "TestPoint_Small" V 1994 2600 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2100 2600 50  0001 C CNN
F 3 "~" H 2100 2600 50  0001 C CNN
	1    1900 2600
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP104
U 1 1 5E318C8E
P 1700 2700
F 0 "TP104" V 1793 2700 50  0000 C CNN
F 1 "TestPoint_Small" V 1794 2700 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1900 2700 50  0001 C CNN
F 3 "~" H 1900 2700 50  0001 C CNN
	1    1700 2700
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP105
U 1 1 5E3191F8
P 1500 2800
F 0 "TP105" V 1593 2800 50  0000 C CNN
F 1 "TestPoint_Small" V 1594 2800 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1700 2800 50  0001 C CNN
F 3 "~" H 1700 2800 50  0001 C CNN
	1    1500 2800
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP106
U 1 1 5E3197C1
P 1300 2900
F 0 "TP106" V 1393 2900 50  0000 C CNN
F 1 "TestPoint_Small" V 1394 2900 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1500 2900 50  0001 C CNN
F 3 "~" H 1500 2900 50  0001 C CNN
	1    1300 2900
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP107
U 1 1 5E319C05
P 1100 3000
F 0 "TP107" V 1193 3000 50  0000 C CNN
F 1 "TestPoint_Small" V 1194 3000 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1300 3000 50  0001 C CNN
F 3 "~" H 1300 3000 50  0001 C CNN
	1    1100 3000
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP108
U 1 1 5E319EED
P 900 3100
F 0 "TP108" V 993 3100 50  0000 C CNN
F 1 "TestPoint_Small" V 994 3100 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1100 3100 50  0001 C CNN
F 3 "~" H 1100 3100 50  0001 C CNN
	1    900  3100
	0    -1   -1   0   
$EndComp
Wire Wire Line
	2100 2500 2900 2500
Wire Wire Line
	2900 2600 1900 2600
Wire Wire Line
	2900 2700 1700 2700
Wire Wire Line
	2900 2800 1500 2800
Wire Wire Line
	2900 2900 1300 2900
Wire Wire Line
	2900 3100 900  3100
Wire Wire Line
	2300 3200 2900 3200
$Comp
L Connector:TestPoint_Small TP109
U 1 1 5E31F668
P 2300 3200
F 0 "TP109" V 2393 3200 50  0000 C CNN
F 1 "TestPoint_Small" V 2394 3200 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2500 3200 50  0001 C CNN
F 3 "~" H 2500 3200 50  0001 C CNN
	1    2300 3200
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP110
U 1 1 5E31F672
P 2100 3300
F 0 "TP110" V 2193 3300 50  0000 C CNN
F 1 "TestPoint_Small" V 2194 3300 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2300 3300 50  0001 C CNN
F 3 "~" H 2300 3300 50  0001 C CNN
	1    2100 3300
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP111
U 1 1 5E31F67C
P 1900 3400
F 0 "TP111" V 1993 3400 50  0000 C CNN
F 1 "TestPoint_Small" V 1994 3400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2100 3400 50  0001 C CNN
F 3 "~" H 2100 3400 50  0001 C CNN
	1    1900 3400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP112
U 1 1 5E31F686
P 1700 3500
F 0 "TP112" V 1793 3500 50  0000 C CNN
F 1 "TestPoint_Small" V 1794 3500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1900 3500 50  0001 C CNN
F 3 "~" H 1900 3500 50  0001 C CNN
	1    1700 3500
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP113
U 1 1 5E31F690
P 1500 3600
F 0 "TP113" V 1593 3600 50  0000 C CNN
F 1 "TestPoint_Small" V 1594 3600 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1700 3600 50  0001 C CNN
F 3 "~" H 1700 3600 50  0001 C CNN
	1    1500 3600
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP114
U 1 1 5E31F69A
P 1300 3700
F 0 "TP114" V 1393 3700 50  0000 C CNN
F 1 "TestPoint_Small" V 1394 3700 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1500 3700 50  0001 C CNN
F 3 "~" H 1500 3700 50  0001 C CNN
	1    1300 3700
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP115
U 1 1 5E31F6A4
P 1100 3800
F 0 "TP115" V 1193 3800 50  0000 C CNN
F 1 "TestPoint_Small" V 1194 3800 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1300 3800 50  0001 C CNN
F 3 "~" H 1300 3800 50  0001 C CNN
	1    1100 3800
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP116
U 1 1 5E31F6AE
P 900 3900
F 0 "TP116" V 993 3900 50  0000 C CNN
F 1 "TestPoint_Small" V 994 3900 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1100 3900 50  0001 C CNN
F 3 "~" H 1100 3900 50  0001 C CNN
	1    900  3900
	0    -1   -1   0   
$EndComp
Wire Wire Line
	2100 3300 2900 3300
Wire Wire Line
	2900 3400 1900 3400
Wire Wire Line
	2900 3500 1700 3500
Wire Wire Line
	2900 3600 1500 3600
Wire Wire Line
	2900 3700 1300 3700
Wire Wire Line
	2900 3800 1100 3800
Wire Wire Line
	2900 3900 900  3900
Wire Wire Line
	2300 4000 2900 4000
$Comp
L Connector:TestPoint_Small TP117
U 1 1 5E327820
P 2300 4000
F 0 "TP117" V 2393 4000 50  0000 C CNN
F 1 "TestPoint_Small" V 2394 4000 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2500 4000 50  0001 C CNN
F 3 "~" H 2500 4000 50  0001 C CNN
	1    2300 4000
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP118
U 1 1 5E32782A
P 2100 4100
F 0 "TP118" V 2193 4100 50  0000 C CNN
F 1 "TestPoint_Small" V 2194 4100 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2300 4100 50  0001 C CNN
F 3 "~" H 2300 4100 50  0001 C CNN
	1    2100 4100
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP119
U 1 1 5E327834
P 1900 4200
F 0 "TP119" V 1993 4200 50  0000 C CNN
F 1 "TestPoint_Small" V 1994 4200 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2100 4200 50  0001 C CNN
F 3 "~" H 2100 4200 50  0001 C CNN
	1    1900 4200
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP120
U 1 1 5E32783E
P 1700 4300
F 0 "TP120" V 1793 4300 50  0000 C CNN
F 1 "TestPoint_Small" V 1794 4300 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1900 4300 50  0001 C CNN
F 3 "~" H 1900 4300 50  0001 C CNN
	1    1700 4300
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP121
U 1 1 5E327848
P 1500 4400
F 0 "TP121" V 1593 4400 50  0000 C CNN
F 1 "TestPoint_Small" V 1594 4400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1700 4400 50  0001 C CNN
F 3 "~" H 1700 4400 50  0001 C CNN
	1    1500 4400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP122
U 1 1 5E327852
P 1300 4500
F 0 "TP122" V 1393 4500 50  0000 C CNN
F 1 "TestPoint_Small" V 1394 4500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1500 4500 50  0001 C CNN
F 3 "~" H 1500 4500 50  0001 C CNN
	1    1300 4500
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP123
U 1 1 5E32785C
P 1100 4600
F 0 "TP123" V 1193 4600 50  0000 C CNN
F 1 "TestPoint_Small" V 1194 4600 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1300 4600 50  0001 C CNN
F 3 "~" H 1300 4600 50  0001 C CNN
	1    1100 4600
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP124
U 1 1 5E327866
P 900 4700
F 0 "TP124" V 993 4700 50  0000 C CNN
F 1 "TestPoint_Small" V 994 4700 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1100 4700 50  0001 C CNN
F 3 "~" H 1100 4700 50  0001 C CNN
	1    900  4700
	0    -1   -1   0   
$EndComp
Wire Wire Line
	2100 4100 2900 4100
Wire Wire Line
	2900 4200 1900 4200
Wire Wire Line
	2900 4300 1700 4300
Wire Wire Line
	2900 4400 1500 4400
Wire Wire Line
	2900 4500 1300 4500
Wire Wire Line
	2900 4600 1100 4600
Wire Wire Line
	2900 4700 900  4700
Wire Wire Line
	2300 4800 2900 4800
$Comp
L Connector:TestPoint_Small TP125
U 1 1 5E32C0DC
P 2300 4800
F 0 "TP125" V 2393 4800 50  0000 C CNN
F 1 "TestPoint_Small" V 2394 4800 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2500 4800 50  0001 C CNN
F 3 "~" H 2500 4800 50  0001 C CNN
	1    2300 4800
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP126
U 1 1 5E32C0E6
P 2100 4900
F 0 "TP126" V 2193 4900 50  0000 C CNN
F 1 "TestPoint_Small" V 2194 4900 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2300 4900 50  0001 C CNN
F 3 "~" H 2300 4900 50  0001 C CNN
	1    2100 4900
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP127
U 1 1 5E32C0F0
P 1900 5000
F 0 "TP127" V 1993 5000 50  0000 C CNN
F 1 "TestPoint_Small" V 1994 5000 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 2100 5000 50  0001 C CNN
F 3 "~" H 2100 5000 50  0001 C CNN
	1    1900 5000
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP128
U 1 1 5E32C0FA
P 1700 5100
F 0 "TP128" V 1793 5100 50  0000 C CNN
F 1 "TestPoint_Small" V 1794 5100 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1900 5100 50  0001 C CNN
F 3 "~" H 1900 5100 50  0001 C CNN
	1    1700 5100
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP129
U 1 1 5E32C104
P 1500 5200
F 0 "TP129" V 1593 5200 50  0000 C CNN
F 1 "TestPoint_Small" V 1594 5200 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1700 5200 50  0001 C CNN
F 3 "~" H 1700 5200 50  0001 C CNN
	1    1500 5200
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP130
U 1 1 5E32C10E
P 1300 5300
F 0 "TP130" V 1393 5300 50  0000 C CNN
F 1 "TestPoint_Small" V 1394 5300 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1500 5300 50  0001 C CNN
F 3 "~" H 1500 5300 50  0001 C CNN
	1    1300 5300
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP131
U 1 1 5E32C118
P 1100 5400
F 0 "TP131" V 1193 5400 50  0000 C CNN
F 1 "TestPoint_Small" V 1194 5400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1300 5400 50  0001 C CNN
F 3 "~" H 1300 5400 50  0001 C CNN
	1    1100 5400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP132
U 1 1 5E32C122
P 900 5500
F 0 "TP132" V 993 5500 50  0000 C CNN
F 1 "TestPoint_Small" V 994 5500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 1100 5500 50  0001 C CNN
F 3 "~" H 1100 5500 50  0001 C CNN
	1    900  5500
	0    -1   -1   0   
$EndComp
Wire Wire Line
	2100 4900 2900 4900
Wire Wire Line
	2900 5000 1900 5000
Wire Wire Line
	2900 5100 1700 5100
Wire Wire Line
	2900 5200 1500 5200
Wire Wire Line
	2900 5300 1300 5300
Wire Wire Line
	2900 5400 1100 5400
Wire Wire Line
	2900 5500 900  5500
$Comp
L power:GND #PWR0101
U 1 1 5E2A6B18
P 3500 5900
F 0 "#PWR0101" H 3500 5650 50  0001 C CNN
F 1 "GND" H 3505 5727 50  0000 C CNN
F 2 "" H 3500 5900 50  0001 C CNN
F 3 "" H 3500 5900 50  0001 C CNN
	1    3500 5900
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0102
U 1 1 5E2A787D
P 6800 5900
F 0 "#PWR0102" H 6800 5650 50  0001 C CNN
F 1 "GND" H 6805 5727 50  0000 C CNN
F 2 "" H 6800 5900 50  0001 C CNN
F 3 "" H 6800 5900 50  0001 C CNN
	1    6800 5900
	1    0    0    -1  
$EndComp
Wire Wire Line
	3400 6200 3400 5800
Wire Wire Line
	5600 2400 6200 2400
$Comp
L Connector:TestPoint_Small TP201
U 1 1 5E2B0A26
P 5600 2400
F 0 "TP201" V 5693 2400 50  0000 C CNN
F 1 "TestPoint_Small" V 5694 2400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5800 2400 50  0001 C CNN
F 3 "~" H 5800 2400 50  0001 C CNN
	1    5600 2400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP202
U 1 1 5E2B0A30
P 5400 2500
F 0 "TP202" V 5493 2500 50  0000 C CNN
F 1 "TestPoint_Small" V 5494 2500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5600 2500 50  0001 C CNN
F 3 "~" H 5600 2500 50  0001 C CNN
	1    5400 2500
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP203
U 1 1 5E2B0A3A
P 5200 2600
F 0 "TP203" V 5293 2600 50  0000 C CNN
F 1 "TestPoint_Small" V 5294 2600 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5400 2600 50  0001 C CNN
F 3 "~" H 5400 2600 50  0001 C CNN
	1    5200 2600
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP204
U 1 1 5E2B0A44
P 5000 2700
F 0 "TP204" V 5093 2700 50  0000 C CNN
F 1 "TestPoint_Small" V 5094 2700 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5200 2700 50  0001 C CNN
F 3 "~" H 5200 2700 50  0001 C CNN
	1    5000 2700
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP205
U 1 1 5E2B0A4E
P 4800 2800
F 0 "TP205" V 4893 2800 50  0000 C CNN
F 1 "TestPoint_Small" V 4894 2800 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5000 2800 50  0001 C CNN
F 3 "~" H 5000 2800 50  0001 C CNN
	1    4800 2800
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP206
U 1 1 5E2B0A58
P 4600 2900
F 0 "TP206" V 4693 2900 50  0000 C CNN
F 1 "TestPoint_Small" V 4694 2900 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4800 2900 50  0001 C CNN
F 3 "~" H 4800 2900 50  0001 C CNN
	1    4600 2900
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP207
U 1 1 5E2B0A62
P 4400 3000
F 0 "TP207" V 4493 3000 50  0000 C CNN
F 1 "TestPoint_Small" V 4494 3000 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4600 3000 50  0001 C CNN
F 3 "~" H 4600 3000 50  0001 C CNN
	1    4400 3000
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP208
U 1 1 5E2B0A6C
P 4200 3100
F 0 "TP208" V 4293 3100 50  0000 C CNN
F 1 "TestPoint_Small" V 4294 3100 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4400 3100 50  0001 C CNN
F 3 "~" H 4400 3100 50  0001 C CNN
	1    4200 3100
	0    -1   -1   0   
$EndComp
Wire Wire Line
	5400 2500 6200 2500
Wire Wire Line
	6200 2600 5200 2600
Wire Wire Line
	6200 2700 5000 2700
Wire Wire Line
	6200 2800 4800 2800
Wire Wire Line
	6200 2900 4600 2900
Wire Wire Line
	6200 3000 4400 3000
Wire Wire Line
	6200 3100 4200 3100
Wire Wire Line
	5600 3200 6200 3200
$Comp
L Connector:TestPoint_Small TP209
U 1 1 5E2B0A7E
P 5600 3200
F 0 "TP209" V 5693 3200 50  0000 C CNN
F 1 "TestPoint_Small" V 5694 3200 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5800 3200 50  0001 C CNN
F 3 "~" H 5800 3200 50  0001 C CNN
	1    5600 3200
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP210
U 1 1 5E2B0A88
P 5400 3300
F 0 "TP210" V 5493 3300 50  0000 C CNN
F 1 "TestPoint_Small" V 5494 3300 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5600 3300 50  0001 C CNN
F 3 "~" H 5600 3300 50  0001 C CNN
	1    5400 3300
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP211
U 1 1 5E2B0A92
P 5200 3400
F 0 "TP211" V 5293 3400 50  0000 C CNN
F 1 "TestPoint_Small" V 5294 3400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5400 3400 50  0001 C CNN
F 3 "~" H 5400 3400 50  0001 C CNN
	1    5200 3400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP212
U 1 1 5E2B0A9C
P 5000 3500
F 0 "TP212" V 5093 3500 50  0000 C CNN
F 1 "TestPoint_Small" V 5094 3500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5200 3500 50  0001 C CNN
F 3 "~" H 5200 3500 50  0001 C CNN
	1    5000 3500
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP213
U 1 1 5E2B0AA6
P 4800 3600
F 0 "TP213" V 4893 3600 50  0000 C CNN
F 1 "TestPoint_Small" V 4894 3600 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5000 3600 50  0001 C CNN
F 3 "~" H 5000 3600 50  0001 C CNN
	1    4800 3600
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP214
U 1 1 5E2B0AB0
P 4600 3700
F 0 "TP214" V 4693 3700 50  0000 C CNN
F 1 "TestPoint_Small" V 4694 3700 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4800 3700 50  0001 C CNN
F 3 "~" H 4800 3700 50  0001 C CNN
	1    4600 3700
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP215
U 1 1 5E2B0ABA
P 4400 3800
F 0 "TP215" V 4493 3800 50  0000 C CNN
F 1 "TestPoint_Small" V 4494 3800 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4600 3800 50  0001 C CNN
F 3 "~" H 4600 3800 50  0001 C CNN
	1    4400 3800
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP216
U 1 1 5E2B0AC4
P 4200 3900
F 0 "TP216" V 4293 3900 50  0000 C CNN
F 1 "TestPoint_Small" V 4294 3900 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4400 3900 50  0001 C CNN
F 3 "~" H 4400 3900 50  0001 C CNN
	1    4200 3900
	0    -1   -1   0   
$EndComp
Wire Wire Line
	5400 3300 6200 3300
Wire Wire Line
	6200 3400 5200 3400
Wire Wire Line
	6200 3500 5000 3500
Wire Wire Line
	6200 3600 4800 3600
Wire Wire Line
	6200 3700 4600 3700
Wire Wire Line
	6200 3800 4400 3800
Wire Wire Line
	6200 3900 4200 3900
Wire Wire Line
	5600 4000 6200 4000
$Comp
L Connector:TestPoint_Small TP217
U 1 1 5E2B0AD6
P 5600 4000
F 0 "TP217" V 5693 4000 50  0000 C CNN
F 1 "TestPoint_Small" V 5694 4000 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5800 4000 50  0001 C CNN
F 3 "~" H 5800 4000 50  0001 C CNN
	1    5600 4000
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP218
U 1 1 5E2B0AE0
P 5400 4100
F 0 "TP218" V 5493 4100 50  0000 C CNN
F 1 "TestPoint_Small" V 5494 4100 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5600 4100 50  0001 C CNN
F 3 "~" H 5600 4100 50  0001 C CNN
	1    5400 4100
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP219
U 1 1 5E2B0AEA
P 5200 4200
F 0 "TP219" V 5293 4200 50  0000 C CNN
F 1 "TestPoint_Small" V 5294 4200 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5400 4200 50  0001 C CNN
F 3 "~" H 5400 4200 50  0001 C CNN
	1    5200 4200
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP220
U 1 1 5E2B0AF4
P 5000 4300
F 0 "TP220" V 5093 4300 50  0000 C CNN
F 1 "TestPoint_Small" V 5094 4300 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5200 4300 50  0001 C CNN
F 3 "~" H 5200 4300 50  0001 C CNN
	1    5000 4300
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP221
U 1 1 5E2B0AFE
P 4800 4400
F 0 "TP221" V 4893 4400 50  0000 C CNN
F 1 "TestPoint_Small" V 4894 4400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5000 4400 50  0001 C CNN
F 3 "~" H 5000 4400 50  0001 C CNN
	1    4800 4400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP222
U 1 1 5E2B0B08
P 4600 4500
F 0 "TP222" V 4693 4500 50  0000 C CNN
F 1 "TestPoint_Small" V 4694 4500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4800 4500 50  0001 C CNN
F 3 "~" H 4800 4500 50  0001 C CNN
	1    4600 4500
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP223
U 1 1 5E2B0B12
P 4400 4600
F 0 "TP223" V 4493 4600 50  0000 C CNN
F 1 "TestPoint_Small" V 4494 4600 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4600 4600 50  0001 C CNN
F 3 "~" H 4600 4600 50  0001 C CNN
	1    4400 4600
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP224
U 1 1 5E2B0B1C
P 4200 4700
F 0 "TP224" V 4293 4700 50  0000 C CNN
F 1 "TestPoint_Small" V 4294 4700 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4400 4700 50  0001 C CNN
F 3 "~" H 4400 4700 50  0001 C CNN
	1    4200 4700
	0    -1   -1   0   
$EndComp
Wire Wire Line
	5400 4100 6200 4100
Wire Wire Line
	6200 4200 5200 4200
Wire Wire Line
	6200 4300 5000 4300
Wire Wire Line
	6200 4400 4800 4400
Wire Wire Line
	6200 4500 4600 4500
Wire Wire Line
	6200 4600 4400 4600
Wire Wire Line
	6200 4700 4200 4700
Wire Wire Line
	5600 4800 6200 4800
$Comp
L Connector:TestPoint_Small TP225
U 1 1 5E2B0B2E
P 5600 4800
F 0 "TP225" V 5693 4800 50  0000 C CNN
F 1 "TestPoint_Small" V 5694 4800 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5800 4800 50  0001 C CNN
F 3 "~" H 5800 4800 50  0001 C CNN
	1    5600 4800
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP226
U 1 1 5E2B0B38
P 5400 4900
F 0 "TP226" V 5493 4900 50  0000 C CNN
F 1 "TestPoint_Small" V 5494 4900 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5600 4900 50  0001 C CNN
F 3 "~" H 5600 4900 50  0001 C CNN
	1    5400 4900
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP227
U 1 1 5E2B0B42
P 5200 5000
F 0 "TP227" V 5293 5000 50  0000 C CNN
F 1 "TestPoint_Small" V 5294 5000 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5400 5000 50  0001 C CNN
F 3 "~" H 5400 5000 50  0001 C CNN
	1    5200 5000
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP228
U 1 1 5E2B0B4C
P 5000 5100
F 0 "TP228" V 5093 5100 50  0000 C CNN
F 1 "TestPoint_Small" V 5094 5100 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5200 5100 50  0001 C CNN
F 3 "~" H 5200 5100 50  0001 C CNN
	1    5000 5100
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP229
U 1 1 5E2B0B56
P 4800 5200
F 0 "TP229" V 4893 5200 50  0000 C CNN
F 1 "TestPoint_Small" V 4894 5200 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 5000 5200 50  0001 C CNN
F 3 "~" H 5000 5200 50  0001 C CNN
	1    4800 5200
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP230
U 1 1 5E2B0B60
P 4600 5300
F 0 "TP230" V 4693 5300 50  0000 C CNN
F 1 "TestPoint_Small" V 4694 5300 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4800 5300 50  0001 C CNN
F 3 "~" H 4800 5300 50  0001 C CNN
	1    4600 5300
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP231
U 1 1 5E2B0B6A
P 4400 5400
F 0 "TP231" V 4493 5400 50  0000 C CNN
F 1 "TestPoint_Small" V 4494 5400 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4600 5400 50  0001 C CNN
F 3 "~" H 4600 5400 50  0001 C CNN
	1    4400 5400
	0    -1   -1   0   
$EndComp
$Comp
L Connector:TestPoint_Small TP232
U 1 1 5E2B0B74
P 4200 5500
F 0 "TP232" V 4293 5500 50  0000 C CNN
F 1 "TestPoint_Small" V 4294 5500 50  0001 C CNN
F 2 "TestPoint:TestPoint_Pad_4.0x4.0mm" H 4400 5500 50  0001 C CNN
F 3 "~" H 4400 5500 50  0001 C CNN
	1    4200 5500
	0    -1   -1   0   
$EndComp
Wire Wire Line
	5400 4900 6200 4900
Wire Wire Line
	6200 5000 5200 5000
Wire Wire Line
	6200 5100 5000 5100
Wire Wire Line
	6200 5200 4800 5200
Wire Wire Line
	6200 5300 4600 5300
Wire Wire Line
	6200 5400 4400 5400
Wire Wire Line
	6200 5500 4200 5500
Wire Wire Line
	9300 2100 10400 2100
Wire Wire Line
	8100 5200 8100 5100
Wire Wire Line
	7000 3900 7400 3900
Wire Wire Line
	7400 3900 7400 4900
Wire Wire Line
	7400 4900 7900 4900
Wire Wire Line
	3700 3900 3900 3900
Wire Wire Line
	3900 3900 3900 6400
Wire Wire Line
	3900 6400 7600 6400
Wire Wire Line
	7600 6400 7600 4700
Wire Wire Line
	7600 4700 8800 4700
Wire Wire Line
	6800 5900 6800 5800
Wire Wire Line
	3500 5800 3500 5900
Wire Wire Line
	2900 2100 2800 2100
Wire Wire Line
	2800 2100 2800 1100
Wire Wire Line
	2800 1100 6100 1100
Wire Wire Line
	7900 1100 7900 2300
Wire Wire Line
	7900 2300 8800 2300
Wire Wire Line
	6200 2100 6100 2100
Wire Wire Line
	6100 2100 6100 1100
Connection ~ 6100 1100
Wire Wire Line
	6100 1100 7900 1100
Wire Wire Line
	6200 2200 6000 2200
Wire Wire Line
	6000 2200 6000 900 
Wire Wire Line
	6000 900  8100 900 
Wire Wire Line
	8100 900  8100 2100
Wire Wire Line
	8100 2100 8800 2100
Wire Wire Line
	2900 2200 2700 2200
Wire Wire Line
	2700 2200 2700 900 
Connection ~ 6000 900 
Wire Wire Line
	2900 2300 2600 2300
Wire Wire Line
	2600 2300 2600 800 
Wire Wire Line
	8200 800  8200 2000
Wire Wire Line
	8200 2000 8800 2000
Wire Wire Line
	8800 2500 8400 2500
Wire Wire Line
	7700 2500 7700 1300
Wire Wire Line
	8400 2500 8400 2800
Wire Wire Line
	8400 2800 9700 2800
Wire Wire Line
	9700 2800 9700 2500
Wire Wire Line
	9700 2500 9300 2500
Connection ~ 8400 2500
Wire Wire Line
	8400 2500 7700 2500
NoConn ~ 8800 2200
$Comp
L Device:Jumper_NC_Dual JP4
U 1 1 5E309A8E
P 10400 5200
F 0 "JP4" V 10400 5302 50  0000 L CNN
F 1 "Jumper_NC_Dual" V 10445 5302 50  0001 L CNN
F 2 "Jumper:SolderJumper-3_P1.3mm_Bridged2Bar12_Pad1.0x1.5mm_NumberLabels" H 10400 5200 50  0001 C CNN
F 3 "~" H 10400 5200 50  0001 C CNN
	1    10400 5200
	0    1    1    0   
$EndComp
Wire Wire Line
	10400 2100 10400 3900
$Comp
L power:GND #PWR0105
U 1 1 5E31D05C
P 10400 5600
F 0 "#PWR0105" H 10400 5350 50  0001 C CNN
F 1 "GND" H 10405 5427 50  0000 C CNN
F 2 "" H 10400 5600 50  0001 C CNN
F 3 "" H 10400 5600 50  0001 C CNN
	1    10400 5600
	1    0    0    -1  
$EndComp
Wire Wire Line
	10400 5600 10400 5450
Wire Wire Line
	8100 5200 9000 5200
$Comp
L power:GND #PWR0106
U 1 1 5E32D300
P 10100 2900
F 0 "#PWR0106" H 10100 2650 50  0001 C CNN
F 1 "GND" H 10105 2727 50  0000 C CNN
F 2 "" H 10100 2900 50  0001 C CNN
F 3 "" H 10100 2900 50  0001 C CNN
	1    10100 2900
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0107
U 1 1 5E32E821
P 8300 2900
F 0 "#PWR0107" H 8300 2650 50  0001 C CNN
F 1 "GND" H 8305 2727 50  0000 C CNN
F 2 "" H 8300 2900 50  0001 C CNN
F 3 "" H 8300 2900 50  0001 C CNN
	1    8300 2900
	1    0    0    -1  
$EndComp
Wire Wire Line
	8300 2900 8300 2400
Wire Wire Line
	8300 2400 8800 2400
Wire Wire Line
	9300 2400 10100 2400
Wire Wire Line
	10100 2400 10100 2800
Wire Wire Line
	9300 2300 10200 2300
Wire Wire Line
	10200 2300 10200 3900
Wire Wire Line
	10200 3900 7400 3900
Connection ~ 7400 3900
Wire Wire Line
	9300 2200 10300 2200
Wire Wire Line
	10300 2200 10300 4400
Wire Wire Line
	10300 4400 7600 4400
Wire Wire Line
	7600 4400 7600 4700
Connection ~ 7600 4700
Connection ~ 10400 3900
Wire Wire Line
	10400 3900 10400 4950
Text Label 8400 2000 0    50   ~ 0
~SPI_CE
Text Label 8400 2100 0    50   ~ 0
SPI_MOSI
Text Label 8400 2300 0    50   ~ 0
SPI_CLK
Text Label 8400 2400 0    50   ~ 0
GND
Text Label 8400 2500 0    50   ~ 0
VCC3V3
Text Label 9400 2100 0    50   ~ 0
AGND
Text Label 9400 2200 0    50   ~ 0
DA
Text Label 9400 2300 0    50   ~ 0
DB
Text Label 9400 2400 0    50   ~ 0
GND
Text Label 9400 2500 0    50   ~ 0
VCC3V3
Wire Wire Line
	9000 4900 9000 5200
Connection ~ 9000 5200
Wire Wire Line
	9000 5200 10300 5200
$Comp
L Mechanical:MountingHole H1
U 1 1 5E2B4EA5
P 1100 900
F 0 "H1" H 1200 900 50  0000 L CNN
F 1 "MountingHole" H 1200 855 50  0001 L CNN
F 2 "MountingHole:MountingHole_2.7mm_M2.5" H 1100 900 50  0001 C CNN
F 3 "~" H 1100 900 50  0001 C CNN
	1    1100 900 
	1    0    0    -1  
$EndComp
$Comp
L Mechanical:MountingHole H2
U 1 1 5E2B5DA6
P 1400 900
F 0 "H2" H 1500 900 50  0000 L CNN
F 1 "MountingHole" H 1500 855 50  0001 L CNN
F 2 "MountingHole:MountingHole_2.7mm_M2.5" H 1400 900 50  0001 C CNN
F 3 "~" H 1400 900 50  0001 C CNN
	1    1400 900 
	1    0    0    -1  
$EndComp
$Comp
L Mechanical:MountingHole H3
U 1 1 5E2B6620
P 1100 1100
F 0 "H3" H 1200 1100 50  0000 L CNN
F 1 "MountingHole" H 1200 1055 50  0001 L CNN
F 2 "MountingHole:MountingHole_2.7mm_M2.5" H 1100 1100 50  0001 C CNN
F 3 "~" H 1100 1100 50  0001 C CNN
	1    1100 1100
	1    0    0    -1  
$EndComp
$Comp
L Mechanical:MountingHole H4
U 1 1 5E2B679E
P 1400 1100
F 0 "H4" H 1500 1100 50  0000 L CNN
F 1 "MountingHole" H 1500 1055 50  0001 L CNN
F 2 "MountingHole:MountingHole_2.7mm_M2.5" H 1400 1100 50  0001 C CNN
F 3 "~" H 1400 1100 50  0001 C CNN
	1    1400 1100
	1    0    0    -1  
$EndComp
Wire Wire Line
	8200 800  8800 800 
Connection ~ 8200 800 
Wire Wire Line
	8100 900  8800 900 
Connection ~ 8100 900 
NoConn ~ 9300 1000
NoConn ~ 9300 1100
NoConn ~ 8800 1000
Wire Wire Line
	7900 1100 8800 1100
Connection ~ 7900 1100
Wire Wire Line
	9300 900  10400 900 
Wire Wire Line
	10400 900  10400 2100
Connection ~ 10400 2100
Wire Wire Line
	8800 1300 8400 1300
Connection ~ 7700 1300
Wire Wire Line
	8400 1300 8400 1600
Wire Wire Line
	8400 1600 9700 1600
Wire Wire Line
	9700 1600 9700 1300
Wire Wire Line
	9700 1300 9300 1300
Connection ~ 8400 1300
Wire Wire Line
	8400 1300 7700 1300
$Comp
L power:GND #PWR0108
U 1 1 5E3AF623
P 8300 1700
F 0 "#PWR0108" H 8300 1450 50  0001 C CNN
F 1 "GND" H 8305 1527 50  0000 C CNN
F 2 "" H 8300 1700 50  0001 C CNN
F 3 "" H 8300 1700 50  0001 C CNN
	1    8300 1700
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0109
U 1 1 5E3AFAD2
P 10100 1700
F 0 "#PWR0109" H 10100 1450 50  0001 C CNN
F 1 "GND" H 10105 1527 50  0000 C CNN
F 2 "" H 10100 1700 50  0001 C CNN
F 3 "" H 10100 1700 50  0001 C CNN
	1    10100 1700
	1    0    0    -1  
$EndComp
Wire Wire Line
	8800 1200 8300 1200
Wire Wire Line
	8300 1200 8300 1700
Wire Wire Line
	9300 1200 10100 1200
Wire Wire Line
	10100 1200 10100 1600
Text Label 8400 1300 0    50   ~ 0
VCC3V3
Text Label 8400 1100 0    50   ~ 0
SPI_CLK
Text Label 8400 1200 0    50   ~ 0
GND
Text Label 8400 900  0    50   ~ 0
SPI_MOSI
Text Label 8400 800  0    50   ~ 0
~SPI_CE
Text Label 9400 900  0    50   ~ 0
AGND
Text Label 9400 1200 0    50   ~ 0
GND
Text Label 9400 1300 0    50   ~ 0
VCC3V3
Wire Wire Line
	2700 900  6000 900 
Wire Wire Line
	5900 800  8200 800 
Wire Wire Line
	2600 800  5900 800 
Connection ~ 5900 800 
Wire Wire Line
	6200 2300 5900 2300
Wire Wire Line
	5900 2300 5900 800 
Wire Wire Line
	2900 3000 1100 3000
Wire Wire Line
	6700 5800 6700 6200
Wire Wire Line
	6700 6200 3400 6200
Wire Wire Line
	6700 6200 10900 6200
Connection ~ 6700 6200
Wire Wire Line
	9300 800  10900 800 
Wire Wire Line
	9300 2000 10900 2000
Connection ~ 10900 2000
Wire Wire Line
	10900 2000 10900 800 
Wire Wire Line
	10900 2000 10900 6200
$Comp
L Jumper:SolderJumper_2_Bridged JP5
U 1 1 5E66019A
P 3000 6200
F 0 "JP5" H 3000 6313 50  0000 C CNN
F 1 "SolderJumper_2_Bridged" H 3000 6314 50  0001 C CNN
F 2 "Jumper:SolderJumper-2_P1.3mm_Bridged_RoundedPad1.0x1.5mm" H 3000 6200 50  0001 C CNN
F 3 "~" H 3000 6200 50  0001 C CNN
	1    3000 6200
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR0103
U 1 1 5E661405
P 2600 6400
F 0 "#PWR0103" H 2600 6150 50  0001 C CNN
F 1 "GND" H 2605 6227 50  0000 C CNN
F 2 "" H 2600 6400 50  0001 C CNN
F 3 "" H 2600 6400 50  0001 C CNN
	1    2600 6400
	1    0    0    -1  
$EndComp
Wire Wire Line
	2600 6400 2600 6200
Wire Wire Line
	2600 6200 2850 6200
Wire Wire Line
	3150 6200 3400 6200
Connection ~ 3400 6200
Wire Wire Line
	3500 1900 3500 1300
Wire Wire Line
	3500 1300 6800 1300
Wire Wire Line
	6800 1900 6800 1300
Connection ~ 6800 1300
Wire Wire Line
	6800 1300 7700 1300
$Comp
L Device:C_Small C1
U 1 1 5E6BE398
P 9900 1600
F 0 "C1" V 9671 1600 50  0000 C CNN
F 1 "100 nF" V 9762 1600 50  0000 C CNN
F 2 "Capacitor_SMD:C_1206_3216Metric_Pad1.42x1.75mm_HandSolder" H 9900 1600 50  0001 C CNN
F 3 "~" H 9900 1600 50  0001 C CNN
	1    9900 1600
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C2
U 1 1 5E6DCCB1
P 9900 2800
F 0 "C2" V 9671 2800 50  0000 C CNN
F 1 "100 nF" V 9762 2800 50  0000 C CNN
F 2 "Capacitor_SMD:C_1206_3216Metric_Pad1.42x1.75mm_HandSolder" H 9900 2800 50  0001 C CNN
F 3 "~" H 9900 2800 50  0001 C CNN
	1    9900 2800
	0    1    1    0   
$EndComp
Wire Wire Line
	9700 1600 9800 1600
Connection ~ 9700 1600
Wire Wire Line
	10000 1600 10100 1600
Connection ~ 10100 1600
Wire Wire Line
	10100 1600 10100 1700
Wire Wire Line
	9700 2800 9800 2800
Connection ~ 9700 2800
Wire Wire Line
	10000 2800 10100 2800
Connection ~ 10100 2800
Wire Wire Line
	10100 2800 10100 2900
Text Label 9400 2000 0    50   ~ 0
VSS
Text Label 9400 800  0    50   ~ 0
VSS
$Comp
L Connector:TestPoint_Small TPAGND1
U 1 1 5E34D2B6
P 10400 3900
F 0 "TPAGND1" H 10448 3900 50  0000 L CNN
F 1 "TestPoint_Small" H 10448 3855 50  0001 L CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 10600 3900 50  0001 C CNN
F 3 "~" H 10600 3900 50  0001 C CNN
	1    10400 3900
	1    0    0    -1  
$EndComp
$Comp
L Connector:Conn_Coaxial J102
U 1 1 5E2B58A0
P 8100 4900
F 0 "J102" H 8200 4829 50  0000 L CNN
F 1 "Conn_Coaxial" H 8200 4784 50  0001 L CNN
F 2 "Connector_Coaxial:SMA_Amphenol_132134-10_Vertical" H 8100 4900 50  0001 C CNN
F 3 " ~" H 8100 4900 50  0001 C CNN
	1    8100 4900
	1    0    0    -1  
$EndComp
$Comp
L Connector:Conn_Coaxial J101
U 1 1 5E2B6373
P 9000 4700
F 0 "J101" H 9100 4629 50  0000 L CNN
F 1 "Conn_Coaxial" H 9100 4584 50  0001 L CNN
F 2 "Connector_Coaxial:SMA_Amphenol_132134-10_Vertical" H 9000 4700 50  0001 C CNN
F 3 " ~" H 9000 4700 50  0001 C CNN
	1    9000 4700
	1    0    0    -1  
$EndComp
$Comp
L Connector_Generic:Conn_02x06_Top_Bottom PmodF2
U 1 1 5E2E85D1
P 9000 1000
F 0 "PmodF2" H 9050 1417 50  0000 C CNN
F 1 "Conn_02x06_Top_Bottom" H 9050 1326 50  0001 C CNN
F 2 "Mars-64:PinHeader_2x06_P2.54mm_Vertical_SMD_TopBottom" H 9000 1000 50  0001 C CNN
F 3 "~" H 9000 1000 50  0001 C CNN
	1    9000 1000
	1    0    0    -1  
$EndComp
$Comp
L Connector_Generic:Conn_02x06_Top_Bottom PmodM1
U 1 1 5E2AF0E4
P 9000 2200
F 0 "PmodM1" H 9050 2617 50  0000 C CNN
F 1 "Conn_02x06_Top_Bottom" H 9050 2526 50  0001 C CNN
F 2 "Mars-64:PinHeader_2x06_P2.54mm_Vertical_SMD_TopBottom_Mirrored" H 9000 2200 50  0001 C CNN
F 3 "~" H 9000 2200 50  0001 C CNN
	1    9000 2200
	1    0    0    -1  
$EndComp
$EndSCHEMATC
