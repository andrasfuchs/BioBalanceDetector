EESchema Schematic File Version 4
LIBS:Venus-256-cache
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 3 10
Title "Venus-256: ARM JTAG"
Date "2018-05-02"
Rev "A"
Comp "Bio Balance Detector"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Connector_Generic:Conn_02x10_Odd_Even J2
U 1 1 5AE9A682
P 2300 1400
F 0 "J2" H 2350 1900 50  0000 C CNN
F 1 "ARM JTAG" H 2350 800 50  0000 C CNN
F 2 "Connector_PinHeader_2.54mm:PinHeader_2x10_P2.54mm_Vertical" H 2300 1400 50  0001 C CNN
F 3 "~" H 2300 1400 50  0001 C CNN
	1    2300 1400
	1    0    0    -1  
$EndComp
NoConn ~ 2100 1100
NoConn ~ 2100 1200
NoConn ~ 2100 1500
NoConn ~ 2100 1800
NoConn ~ 2100 1900
Wire Wire Line
	2600 1100 2700 1100
Wire Wire Line
	2700 1100 2700 1200
Wire Wire Line
	2600 1200 2700 1200
Connection ~ 2700 1200
Wire Wire Line
	2600 1300 2700 1300
Wire Wire Line
	2700 1200 2700 1300
Connection ~ 2700 1300
Wire Wire Line
	2700 1300 2700 1400
Wire Wire Line
	2600 1400 2700 1400
Connection ~ 2700 1400
Wire Wire Line
	2700 1400 2700 1500
Wire Wire Line
	2600 1500 2700 1500
Connection ~ 2700 1500
Wire Wire Line
	2700 1500 2700 1600
Wire Wire Line
	2600 1600 2700 1600
Connection ~ 2700 1600
Wire Wire Line
	2700 1600 2700 1700
Wire Wire Line
	2600 1700 2700 1700
Connection ~ 2700 1700
Wire Wire Line
	2700 1700 2700 1800
Wire Wire Line
	2600 1800 2700 1800
Connection ~ 2700 1800
Wire Wire Line
	2700 1800 2700 1900
Wire Wire Line
	2600 1900 2700 1900
Connection ~ 2700 1900
Wire Wire Line
	2700 1900 2900 1900
Wire Wire Line
	2100 1000 2000 1000
Wire Wire Line
	2000 1000 2000 800 
Wire Wire Line
	2000 800  2700 800 
Wire Wire Line
	2700 800  2700 1000
Wire Wire Line
	2700 1000 2600 1000
Wire Wire Line
	2700 800  2900 800 
Connection ~ 2700 800 
$Comp
L Device:R_Small R3
U 1 1 5AE9A6B6
P 1800 1300
F 0 "R3" H 1741 1254 50  0000 R CNN
F 1 "0R" H 1741 1345 50  0000 R CNN
F 2 "Resistor_SMD:R_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 1800 1300 50  0001 C CNN
F 3 "~" H 1800 1300 50  0001 C CNN
	1    1800 1300
	0    1    1    0   
$EndComp
$Comp
L Device:R_Small R1
U 1 1 5AE9A6BD
P 1600 1400
F 0 "R1" H 1541 1354 50  0000 R CNN
F 1 "0R" H 1541 1445 50  0000 R CNN
F 2 "Resistor_SMD:R_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 1600 1400 50  0001 C CNN
F 3 "~" H 1600 1400 50  0001 C CNN
	1    1600 1400
	0    -1   -1   0   
$EndComp
$Comp
L Device:R_Small R4
U 1 1 5AE9A6C4
P 1800 1600
F 0 "R4" H 1741 1554 50  0000 R CNN
F 1 "0R" H 1741 1645 50  0000 R CNN
F 2 "Resistor_SMD:R_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 1800 1600 50  0001 C CNN
F 3 "~" H 1800 1600 50  0001 C CNN
	1    1800 1600
	0    1    1    0   
$EndComp
$Comp
L Device:R_Small R2
U 1 1 5AE9A6CB
P 1600 1700
F 0 "R2" H 1541 1654 50  0000 R CNN
F 1 "0R" H 1541 1745 50  0000 R CNN
F 2 "Resistor_SMD:R_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 1600 1700 50  0001 C CNN
F 3 "~" H 1600 1700 50  0001 C CNN
	1    1600 1700
	0    -1   -1   0   
$EndComp
Text HLabel 2900 1900 2    50   UnSpc ~ 0
GND
Text HLabel 2900 800  2    50   Input ~ 0
VCC_SAMPLE
Text HLabel 1300 1300 0    50   BiDi ~ 0
SWDIO
Text HLabel 1300 1400 0    50   Output ~ 0
SWCLK
Wire Wire Line
	1700 1400 2100 1400
Wire Wire Line
	1900 1300 2100 1300
Wire Wire Line
	1500 1400 1300 1400
Wire Wire Line
	1300 1300 1700 1300
Text HLabel 1300 1600 0    50   Input ~ 0
TRACESWO
Text HLabel 1300 1700 0    50   Output ~ 0
TARGET_RESET
Wire Wire Line
	2100 1600 1900 1600
Wire Wire Line
	2100 1700 1700 1700
Wire Wire Line
	1500 1700 1300 1700
Wire Wire Line
	1700 1600 1300 1600
$EndSCHEMATC
