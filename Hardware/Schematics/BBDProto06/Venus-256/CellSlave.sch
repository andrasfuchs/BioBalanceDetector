EESchema Schematic File Version 4
LIBS:Venus-256-cache
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 4 10
Title "Venus-256: Cell Slave"
Date "2018-05-02"
Rev "A"
Comp "Bio Balance Detector"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
Wire Wire Line
	5800 5500 6100 5500
$Comp
L Device:C_Small C404
U 1 1 5AE9F047
P 3300 5800
AR Path="/5AE9ECEB/5AE9F047" Ref="C404"  Part="1" 
AR Path="/5AE958E5/5AE9F047" Ref="C604"  Part="1" 
AR Path="/5AE96312/5AE9F047" Ref="C704"  Part="1" 
AR Path="/5AE96ED4/5AE9F047" Ref="C804"  Part="1" 
AR Path="/5AFE7089/5AE9F047" Ref="C1004"  Part="1" 
F 0 "C404" H 3392 5846 50  0000 L CNN
F 1 "10uF,X5R" H 3392 5755 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3300 5800 50  0001 C CNN
F 3 "~" H 3300 5800 50  0001 C CNN
	1    3300 5800
	-1   0    0    -1  
$EndComp
Text HLabel 6100 2300 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 2400 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 2300 6100 2300
Wire Wire Line
	5800 2400 6100 2400
Text HLabel 6100 5300 2    50   Output ~ 0
BUSY
Text HLabel 6100 4700 2    50   Input ~ 0
~CS
Text HLabel 6100 2600 2    50   Input ~ 0
VDRIVE_3V3
Wire Wire Line
	5800 2600 6100 2600
Wire Wire Line
	5800 3200 6100 3200
Text HLabel 6100 3800 2    50   Input ~ 0
SDI
Text HLabel 6100 4000 2    50   Output ~ 0
SDOA
Wire Wire Line
	5800 3800 6100 3800
Wire Wire Line
	5800 3900 6100 3900
Wire Wire Line
	5800 4000 6100 4000
Wire Wire Line
	5800 4700 6100 4700
Wire Wire Line
	5800 5300 6100 5300
Text HLabel 6100 4600 2    50   Input ~ 0
SCLK
Wire Wire Line
	5800 4600 6100 4600
Text HLabel 3500 700  0    50   UnSpc ~ 0
AGND
Text HLabel 5900 700  2    50   UnSpc ~ 0
DGND
Text HLabel 5900 600  2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 3500 600  0    50   Input ~ 0
VCC_5V0
Wire Wire Line
	2000 2000 3600 2000
Wire Wire Line
	2100 2200 3600 2200
Text HLabel 2700 5100 0    50   Input ~ 0
SGND
Text HLabel 6100 4100 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 4200 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 4300 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 4100 6100 4100
Wire Wire Line
	5800 4200 6100 4200
Wire Wire Line
	5800 4300 6100 4300
Text HLabel 6100 4500 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 4500 6100 4500
Text HLabel 6100 4900 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 5000 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 5100 2    50   UnSpc ~ 0
DGND
$Comp
L Analog_ADC:AD7616 U401
U 1 1 5AE20D71
P 4700 4100
AR Path="/5AE9ECEB/5AE20D71" Ref="U401"  Part="1" 
AR Path="/5AE958E5/5AE20D71" Ref="U601"  Part="1" 
AR Path="/5AE96312/5AE20D71" Ref="U701"  Part="1" 
AR Path="/5AE96ED4/5AE20D71" Ref="U801"  Part="1" 
AR Path="/5AFE7089/5AE20D71" Ref="U1001"  Part="1" 
F 0 "U401" H 3800 6350 50  0000 C CNN
F 1 "AD7616" H 5550 6350 50  0000 C CNN
F 2 "Package_QFP:LQFP-80_14x14mm_P0.65mm" H 4700 4100 50  0001 C CIN
F 3 "http://www.analog.com/media/en/technical-documentation/data-sheets/AD7616.pdf" H 4800 1100 50  0001 C CNN
	1    4700 4100
	1    0    0    -1  
$EndComp
Wire Wire Line
	5800 4900 6100 4900
Wire Wire Line
	5800 5000 6100 5000
Wire Wire Line
	5800 5100 6100 5100
Text HLabel 6100 5500 2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 2700 5900 0    50   UnSpc ~ 0
AGND
Text HLabel 6100 2100 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 2100 6100 2100
Text HLabel 6100 2000 2    50   Input ~ 0
~RESET
Wire Wire Line
	5800 2000 6100 2000
Text HLabel 6100 2800 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 2900 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 3000 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 3100 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 2800 6100 2800
Wire Wire Line
	5800 2900 6100 2900
Wire Wire Line
	5800 3000 6100 3000
Wire Wire Line
	5800 3100 6100 3100
Text HLabel 6100 3200 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 3300 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 3300 6100 3300
Text HLabel 6100 3400 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 3500 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 3400 6100 3400
Wire Wire Line
	5800 3500 6100 3500
Text HLabel 6100 3600 2    50   UnSpc ~ 0
DGND
Text HLabel 6100 3700 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	5800 3600 6100 3600
Wire Wire Line
	5800 3700 6100 3700
NoConn ~ 6100 3900
Text HLabel 6100 5400 2    50   Input ~ 0
CONVST
Wire Wire Line
	5800 5400 6100 5400
Text HLabel 2400 6200 0    50   UnSpc ~ 0
AGND
Text HLabel 7000 6200 2    50   UnSpc ~ 0
DGND
$Comp
L Device:C_Small C416
U 1 1 5AF1A3C9
P 6700 6100
AR Path="/5AE9ECEB/5AF1A3C9" Ref="C416"  Part="1" 
AR Path="/5AE958E5/5AF1A3C9" Ref="C616"  Part="1" 
AR Path="/5AE96312/5AF1A3C9" Ref="C716"  Part="1" 
AR Path="/5AE96ED4/5AF1A3C9" Ref="C816"  Part="1" 
AR Path="/5AFE7089/5AF1A3C9" Ref="C1016"  Part="1" 
F 0 "C416" H 6792 6146 50  0000 L CNN
F 1 "10uF" H 6792 6055 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 6700 6100 50  0001 C CNN
F 3 "~" H 6700 6100 50  0001 C CNN
	1    6700 6100
	1    0    0    -1  
$EndComp
Wire Wire Line
	2200 2400 3600 2400
Wire Wire Line
	2300 2600 3600 2600
Wire Wire Line
	1700 3000 3600 3000
Wire Wire Line
	1800 3200 3600 3200
Wire Wire Line
	3600 3400 1900 3400
Wire Wire Line
	800  3600 3600 3600
Wire Wire Line
	3600 3800 900  3800
Wire Wire Line
	1000 4000 3600 4000
Wire Wire Line
	3600 4200 1100 4200
Wire Wire Line
	1500 4400 3600 4400
Wire Wire Line
	3600 4600 1400 4600
Wire Wire Line
	1300 4800 3600 4800
Wire Wire Line
	3600 5000 1200 5000
$Comp
L Device:C_Small C403
U 1 1 5AF62C30
P 3300 5500
AR Path="/5AE9ECEB/5AF62C30" Ref="C403"  Part="1" 
AR Path="/5AE958E5/5AF62C30" Ref="C603"  Part="1" 
AR Path="/5AE96312/5AF62C30" Ref="C703"  Part="1" 
AR Path="/5AE96ED4/5AF62C30" Ref="C803"  Part="1" 
AR Path="/5AFE7089/5AF62C30" Ref="C1003"  Part="1" 
F 0 "C403" H 3392 5546 50  0000 L CNN
F 1 "0.1uF,X8R" H 3392 5455 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3300 5500 50  0001 C CNN
F 3 "~" H 3300 5500 50  0001 C CNN
	1    3300 5500
	-1   0    0    -1  
$EndComp
Text HLabel 2700 5600 0    50   UnSpc ~ 0
AGND
$Comp
L Device:C_Small C414
U 1 1 5AE39B5F
P 5500 1300
AR Path="/5AE9ECEB/5AE39B5F" Ref="C414"  Part="1" 
AR Path="/5AE958E5/5AE39B5F" Ref="C614"  Part="1" 
AR Path="/5AE96312/5AE39B5F" Ref="C714"  Part="1" 
AR Path="/5AE96ED4/5AE39B5F" Ref="C814"  Part="1" 
AR Path="/5AFE7089/5AE39B5F" Ref="C1014"  Part="1" 
F 0 "C414" V 5400 1300 50  0000 C CNN
F 1 "0.1uF" V 5600 1300 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5500 1300 50  0001 C CNN
F 3 "~" H 5500 1300 50  0001 C CNN
	1    5500 1300
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C413
U 1 1 5AE424D3
P 5500 900
AR Path="/5AE9ECEB/5AE424D3" Ref="C413"  Part="1" 
AR Path="/5AE958E5/5AE424D3" Ref="C613"  Part="1" 
AR Path="/5AE96312/5AE424D3" Ref="C713"  Part="1" 
AR Path="/5AE96ED4/5AE424D3" Ref="C813"  Part="1" 
AR Path="/5AFE7089/5AE424D3" Ref="C1013"  Part="1" 
F 0 "C413" V 5400 900 50  0000 C CNN
F 1 "10uF" V 5600 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5500 900 50  0001 C CNN
F 3 "~" H 5500 900 50  0001 C CNN
	1    5500 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C405
U 1 1 5AE748DC
P 3900 900
AR Path="/5AE9ECEB/5AE748DC" Ref="C405"  Part="1" 
AR Path="/5AE958E5/5AE748DC" Ref="C605"  Part="1" 
AR Path="/5AE96312/5AE748DC" Ref="C705"  Part="1" 
AR Path="/5AE96ED4/5AE748DC" Ref="C805"  Part="1" 
AR Path="/5AFE7089/5AE748DC" Ref="C1005"  Part="1" 
F 0 "C405" V 3800 900 50  0000 C CNN
F 1 "10uF" V 4000 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3900 900 50  0001 C CNN
F 3 "~" H 3900 900 50  0001 C CNN
	1    3900 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C406
U 1 1 5AE748E2
P 3900 1300
AR Path="/5AE9ECEB/5AE748E2" Ref="C406"  Part="1" 
AR Path="/5AE958E5/5AE748E2" Ref="C606"  Part="1" 
AR Path="/5AE96312/5AE748E2" Ref="C706"  Part="1" 
AR Path="/5AE96ED4/5AE748E2" Ref="C806"  Part="1" 
AR Path="/5AFE7089/5AE748E2" Ref="C1006"  Part="1" 
F 0 "C406" V 3800 1300 50  0000 C CNN
F 1 "0.1uF" V 4000 1300 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3900 1300 50  0001 C CNN
F 3 "~" H 3900 1300 50  0001 C CNN
	1    3900 1300
	0    -1   -1   0   
$EndComp
Wire Wire Line
	4000 700  4000 900 
Wire Wire Line
	3500 600  3800 600 
Wire Wire Line
	3800 600  3800 900 
$Comp
L Device:C_Small C407
U 1 1 5AE9D928
P 4300 900
AR Path="/5AE9ECEB/5AE9D928" Ref="C407"  Part="1" 
AR Path="/5AE958E5/5AE9D928" Ref="C607"  Part="1" 
AR Path="/5AE96312/5AE9D928" Ref="C707"  Part="1" 
AR Path="/5AE96ED4/5AE9D928" Ref="C807"  Part="1" 
AR Path="/5AFE7089/5AE9D928" Ref="C1007"  Part="1" 
F 0 "C407" V 4200 900 50  0000 C CNN
F 1 "10uF" V 4400 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4300 900 50  0001 C CNN
F 3 "~" H 4300 900 50  0001 C CNN
	1    4300 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C408
U 1 1 5AE9D92E
P 4300 1300
AR Path="/5AE9ECEB/5AE9D92E" Ref="C408"  Part="1" 
AR Path="/5AE958E5/5AE9D92E" Ref="C608"  Part="1" 
AR Path="/5AE96312/5AE9D92E" Ref="C708"  Part="1" 
AR Path="/5AE96ED4/5AE9D92E" Ref="C808"  Part="1" 
AR Path="/5AFE7089/5AE9D92E" Ref="C1008"  Part="1" 
F 0 "C408" V 4200 1300 50  0000 C CNN
F 1 "0.1uF" V 4400 1300 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4300 1300 50  0001 C CNN
F 3 "~" H 4300 1300 50  0001 C CNN
	1    4300 1300
	0    1    1    0   
$EndComp
Wire Wire Line
	4400 700  4400 900 
Connection ~ 4200 1300
Wire Wire Line
	4200 600  4200 900 
$Comp
L Device:C_Small C409
U 1 1 5AEA3FF2
P 4700 900
AR Path="/5AE9ECEB/5AEA3FF2" Ref="C409"  Part="1" 
AR Path="/5AE958E5/5AEA3FF2" Ref="C609"  Part="1" 
AR Path="/5AE96312/5AEA3FF2" Ref="C709"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF2" Ref="C809"  Part="1" 
AR Path="/5AFE7089/5AEA3FF2" Ref="C1009"  Part="1" 
F 0 "C409" V 4600 900 50  0000 C CNN
F 1 "10uF" V 4800 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 900 50  0001 C CNN
F 3 "~" H 4700 900 50  0001 C CNN
	1    4700 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C410
U 1 1 5AEA3FF8
P 4700 1300
AR Path="/5AE9ECEB/5AEA3FF8" Ref="C410"  Part="1" 
AR Path="/5AE958E5/5AEA3FF8" Ref="C610"  Part="1" 
AR Path="/5AE96312/5AEA3FF8" Ref="C710"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF8" Ref="C810"  Part="1" 
AR Path="/5AFE7089/5AEA3FF8" Ref="C1010"  Part="1" 
F 0 "C410" V 4600 1300 50  0000 C CNN
F 1 "0.1uF" V 4800 1300 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 1300 50  0001 C CNN
F 3 "~" H 4700 1300 50  0001 C CNN
	1    4700 1300
	0    1    1    0   
$EndComp
Wire Wire Line
	4800 700  4800 900 
Wire Wire Line
	4600 600  4600 900 
$Comp
L Device:C_Small C411
U 1 1 5AEB23BE
P 5100 900
AR Path="/5AE9ECEB/5AEB23BE" Ref="C411"  Part="1" 
AR Path="/5AE958E5/5AEB23BE" Ref="C611"  Part="1" 
AR Path="/5AE96312/5AEB23BE" Ref="C711"  Part="1" 
AR Path="/5AE96ED4/5AEB23BE" Ref="C811"  Part="1" 
AR Path="/5AFE7089/5AEB23BE" Ref="C1011"  Part="1" 
F 0 "C411" V 5000 900 50  0000 C CNN
F 1 "10uF" V 5200 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5100 900 50  0001 C CNN
F 3 "~" H 5100 900 50  0001 C CNN
	1    5100 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C412
U 1 1 5AEB23C4
P 5100 1300
AR Path="/5AE9ECEB/5AEB23C4" Ref="C412"  Part="1" 
AR Path="/5AE958E5/5AEB23C4" Ref="C612"  Part="1" 
AR Path="/5AE96312/5AEB23C4" Ref="C712"  Part="1" 
AR Path="/5AE96ED4/5AEB23C4" Ref="C812"  Part="1" 
AR Path="/5AFE7089/5AEB23C4" Ref="C1012"  Part="1" 
F 0 "C412" V 5000 1300 50  0000 C CNN
F 1 "0.1uF" V 5200 1300 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5100 1300 50  0001 C CNN
F 3 "~" H 5100 1300 50  0001 C CNN
	1    5100 1300
	0    1    1    0   
$EndComp
Wire Wire Line
	5200 700  5200 900 
Connection ~ 3800 600 
Connection ~ 4200 600 
Wire Wire Line
	4200 600  4600 600 
Connection ~ 4600 600 
Connection ~ 4400 700 
Wire Wire Line
	4400 700  4800 700 
Connection ~ 4800 700 
Wire Wire Line
	4200 1300 4200 1600
$Comp
L Device:C_Small C401
U 1 1 5AF66660
P 2700 6100
AR Path="/5AE9ECEB/5AF66660" Ref="C401"  Part="1" 
AR Path="/5AE958E5/5AF66660" Ref="C601"  Part="1" 
AR Path="/5AE96312/5AF66660" Ref="C701"  Part="1" 
AR Path="/5AE96ED4/5AF66660" Ref="C801"  Part="1" 
AR Path="/5AFE7089/5AF66660" Ref="C1001"  Part="1" 
F 0 "C401" H 2792 6146 50  0000 L CNN
F 1 "10uF" H 2792 6055 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2700 6100 50  0001 C CNN
F 3 "~" H 2700 6100 50  0001 C CNN
	1    2700 6100
	-1   0    0    -1  
$EndComp
$Comp
L Device:C_Small C415
U 1 1 5AFC3D5B
P 6100 6100
AR Path="/5AE9ECEB/5AFC3D5B" Ref="C415"  Part="1" 
AR Path="/5AE958E5/5AFC3D5B" Ref="C615"  Part="1" 
AR Path="/5AE96312/5AFC3D5B" Ref="C715"  Part="1" 
AR Path="/5AE96ED4/5AFC3D5B" Ref="C815"  Part="1" 
AR Path="/5AFE7089/5AFC3D5B" Ref="C1015"  Part="1" 
F 0 "C415" H 6192 6146 50  0000 L CNN
F 1 "0.1uF" H 6192 6055 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 6100 6100 50  0001 C CNN
F 3 "~" H 6100 6100 50  0001 C CNN
	1    6100 6100
	1    0    0    -1  
$EndComp
Wire Wire Line
	5000 900  5000 600 
$Comp
L Venus-256-rescue:Test_Point-Connector_Specialized TP417
U 1 1 5AE6BFCE
P 2700 5400
AR Path="/5AE9ECEB/5AE6BFCE" Ref="TP417"  Part="1" 
AR Path="/5AE958E5/5AE6BFCE" Ref="TP617"  Part="1" 
AR Path="/5AE96312/5AE6BFCE" Ref="TP717"  Part="1" 
AR Path="/5AE96ED4/5AE6BFCE" Ref="TP817"  Part="1" 
AR Path="/5AFE7089/5AE6BFCE" Ref="TP1017"  Part="1" 
F 0 "TP417" V 2654 5587 50  0000 L CNN
F 1 "ADC_REF_OUT" V 2745 5587 50  0000 L CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2900 5400 50  0001 C CNN
F 3 "~" H 2900 5400 50  0001 C CNN
	1    2700 5400
	0    -1   1    0   
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP401
U 1 1 5AECD270
P 800 3600
AR Path="/5AE9ECEB/5AECD270" Ref="TP401"  Part="1" 
AR Path="/5AE958E5/5AECD270" Ref="TP601"  Part="1" 
AR Path="/5AE96312/5AECD270" Ref="TP701"  Part="1" 
AR Path="/5AE96ED4/5AECD270" Ref="TP801"  Part="1" 
AR Path="/5AFE7089/5AECD270" Ref="TP1001"  Part="1" 
F 0 "TP401" H 800 3700 50  0000 R CNN
F 1 "S01" H 1000 3675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1000 3600 50  0001 C CNN
F 3 "~" H 1000 3600 50  0001 C CNN
	1    800  3600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP402
U 1 1 5AECE3AF
P 900 3800
AR Path="/5AE9ECEB/5AECE3AF" Ref="TP402"  Part="1" 
AR Path="/5AE958E5/5AECE3AF" Ref="TP602"  Part="1" 
AR Path="/5AE96312/5AECE3AF" Ref="TP702"  Part="1" 
AR Path="/5AE96ED4/5AECE3AF" Ref="TP802"  Part="1" 
AR Path="/5AFE7089/5AECE3AF" Ref="TP1002"  Part="1" 
F 0 "TP402" H 900 3900 50  0000 R CNN
F 1 "S02" H 1100 3875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1100 3800 50  0001 C CNN
F 3 "~" H 1100 3800 50  0001 C CNN
	1    900  3800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP403
U 1 1 5AECE3FB
P 1000 4000
AR Path="/5AE9ECEB/5AECE3FB" Ref="TP403"  Part="1" 
AR Path="/5AE958E5/5AECE3FB" Ref="TP603"  Part="1" 
AR Path="/5AE96312/5AECE3FB" Ref="TP703"  Part="1" 
AR Path="/5AE96ED4/5AECE3FB" Ref="TP803"  Part="1" 
AR Path="/5AFE7089/5AECE3FB" Ref="TP1003"  Part="1" 
F 0 "TP403" H 1000 4100 50  0000 R CNN
F 1 "S03" H 1200 4075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1200 4000 50  0001 C CNN
F 3 "~" H 1200 4000 50  0001 C CNN
	1    1000 4000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP404
U 1 1 5AECE449
P 1100 4200
AR Path="/5AE9ECEB/5AECE449" Ref="TP404"  Part="1" 
AR Path="/5AE958E5/5AECE449" Ref="TP604"  Part="1" 
AR Path="/5AE96312/5AECE449" Ref="TP704"  Part="1" 
AR Path="/5AE96ED4/5AECE449" Ref="TP804"  Part="1" 
AR Path="/5AFE7089/5AECE449" Ref="TP1004"  Part="1" 
F 0 "TP404" H 1100 4300 50  0000 R CNN
F 1 "S04" H 1300 4275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1300 4200 50  0001 C CNN
F 3 "~" H 1300 4200 50  0001 C CNN
	1    1100 4200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP405
U 1 1 5AECE499
P 1200 5000
AR Path="/5AE9ECEB/5AECE499" Ref="TP405"  Part="1" 
AR Path="/5AE958E5/5AECE499" Ref="TP605"  Part="1" 
AR Path="/5AE96312/5AECE499" Ref="TP705"  Part="1" 
AR Path="/5AE96ED4/5AECE499" Ref="TP805"  Part="1" 
AR Path="/5AFE7089/5AECE499" Ref="TP1005"  Part="1" 
F 0 "TP405" H 1200 5100 50  0000 R CNN
F 1 "S05" H 1400 5075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1400 5000 50  0001 C CNN
F 3 "~" H 1400 5000 50  0001 C CNN
	1    1200 5000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP406
U 1 1 5AECE4E7
P 1300 4800
AR Path="/5AE9ECEB/5AECE4E7" Ref="TP406"  Part="1" 
AR Path="/5AE958E5/5AECE4E7" Ref="TP606"  Part="1" 
AR Path="/5AE96312/5AECE4E7" Ref="TP706"  Part="1" 
AR Path="/5AE96ED4/5AECE4E7" Ref="TP806"  Part="1" 
AR Path="/5AFE7089/5AECE4E7" Ref="TP1006"  Part="1" 
F 0 "TP406" H 1300 4900 50  0000 R CNN
F 1 "S06" H 1500 4875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1500 4800 50  0001 C CNN
F 3 "~" H 1500 4800 50  0001 C CNN
	1    1300 4800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP407
U 1 1 5AECE53B
P 1400 4600
AR Path="/5AE9ECEB/5AECE53B" Ref="TP407"  Part="1" 
AR Path="/5AE958E5/5AECE53B" Ref="TP607"  Part="1" 
AR Path="/5AE96312/5AECE53B" Ref="TP707"  Part="1" 
AR Path="/5AE96ED4/5AECE53B" Ref="TP807"  Part="1" 
AR Path="/5AFE7089/5AECE53B" Ref="TP1007"  Part="1" 
F 0 "TP407" H 1400 4700 50  0000 R CNN
F 1 "S07" H 1600 4675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1600 4600 50  0001 C CNN
F 3 "~" H 1600 4600 50  0001 C CNN
	1    1400 4600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP408
U 1 1 5AECE591
P 1500 4400
AR Path="/5AE9ECEB/5AECE591" Ref="TP408"  Part="1" 
AR Path="/5AE958E5/5AECE591" Ref="TP608"  Part="1" 
AR Path="/5AE96312/5AECE591" Ref="TP708"  Part="1" 
AR Path="/5AE96ED4/5AECE591" Ref="TP808"  Part="1" 
AR Path="/5AFE7089/5AECE591" Ref="TP1008"  Part="1" 
F 0 "TP408" H 1500 4500 50  0000 R CNN
F 1 "S08" H 1700 4475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1700 4400 50  0001 C CNN
F 3 "~" H 1700 4400 50  0001 C CNN
	1    1500 4400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP409
U 1 1 5AECE5E5
P 1600 2800
AR Path="/5AE9ECEB/5AECE5E5" Ref="TP409"  Part="1" 
AR Path="/5AE958E5/5AECE5E5" Ref="TP609"  Part="1" 
AR Path="/5AE96312/5AECE5E5" Ref="TP709"  Part="1" 
AR Path="/5AE96ED4/5AECE5E5" Ref="TP809"  Part="1" 
AR Path="/5AFE7089/5AECE5E5" Ref="TP1009"  Part="1" 
F 0 "TP409" H 1600 2900 50  0000 R CNN
F 1 "S09" H 1800 2875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 2800 50  0001 C CNN
F 3 "~" H 1800 2800 50  0001 C CNN
	1    1600 2800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP410
U 1 1 5AECEE9D
P 1700 3000
AR Path="/5AE9ECEB/5AECEE9D" Ref="TP410"  Part="1" 
AR Path="/5AE958E5/5AECEE9D" Ref="TP610"  Part="1" 
AR Path="/5AE96312/5AECEE9D" Ref="TP710"  Part="1" 
AR Path="/5AE96ED4/5AECEE9D" Ref="TP810"  Part="1" 
AR Path="/5AFE7089/5AECEE9D" Ref="TP1010"  Part="1" 
F 0 "TP410" H 1700 3100 50  0000 R CNN
F 1 "S10" H 1900 3075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1900 3000 50  0001 C CNN
F 3 "~" H 1900 3000 50  0001 C CNN
	1    1700 3000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP411
U 1 1 5AECEEE7
P 1800 3200
AR Path="/5AE9ECEB/5AECEEE7" Ref="TP411"  Part="1" 
AR Path="/5AE958E5/5AECEEE7" Ref="TP611"  Part="1" 
AR Path="/5AE96312/5AECEEE7" Ref="TP711"  Part="1" 
AR Path="/5AE96ED4/5AECEEE7" Ref="TP811"  Part="1" 
AR Path="/5AFE7089/5AECEEE7" Ref="TP1011"  Part="1" 
F 0 "TP411" H 1800 3300 50  0000 R CNN
F 1 "S11" H 2000 3275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2000 3200 50  0001 C CNN
F 3 "~" H 2000 3200 50  0001 C CNN
	1    1800 3200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP412
U 1 1 5AECEF33
P 1900 3400
AR Path="/5AE9ECEB/5AECEF33" Ref="TP412"  Part="1" 
AR Path="/5AE958E5/5AECEF33" Ref="TP612"  Part="1" 
AR Path="/5AE96312/5AECEF33" Ref="TP712"  Part="1" 
AR Path="/5AE96ED4/5AECEF33" Ref="TP812"  Part="1" 
AR Path="/5AFE7089/5AECEF33" Ref="TP1012"  Part="1" 
F 0 "TP412" H 1900 3500 50  0000 R CNN
F 1 "S12" H 2100 3475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2100 3400 50  0001 C CNN
F 3 "~" H 2100 3400 50  0001 C CNN
	1    1900 3400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP413
U 1 1 5AECEF81
P 2000 2000
AR Path="/5AE9ECEB/5AECEF81" Ref="TP413"  Part="1" 
AR Path="/5AE958E5/5AECEF81" Ref="TP613"  Part="1" 
AR Path="/5AE96312/5AECEF81" Ref="TP713"  Part="1" 
AR Path="/5AE96ED4/5AECEF81" Ref="TP813"  Part="1" 
AR Path="/5AFE7089/5AECEF81" Ref="TP1013"  Part="1" 
F 0 "TP413" H 2000 2100 50  0000 R CNN
F 1 "S13" H 2200 2075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2200 2000 50  0001 C CNN
F 3 "~" H 2200 2000 50  0001 C CNN
	1    2000 2000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP414
U 1 1 5AECEFD1
P 2100 2200
AR Path="/5AE9ECEB/5AECEFD1" Ref="TP414"  Part="1" 
AR Path="/5AE958E5/5AECEFD1" Ref="TP614"  Part="1" 
AR Path="/5AE96312/5AECEFD1" Ref="TP714"  Part="1" 
AR Path="/5AE96ED4/5AECEFD1" Ref="TP814"  Part="1" 
AR Path="/5AFE7089/5AECEFD1" Ref="TP1014"  Part="1" 
F 0 "TP414" H 2100 2300 50  0000 R CNN
F 1 "S14" H 2300 2275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 2200 50  0001 C CNN
F 3 "~" H 2300 2200 50  0001 C CNN
	1    2100 2200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP415
U 1 1 5AECF023
P 2200 2400
AR Path="/5AE9ECEB/5AECF023" Ref="TP415"  Part="1" 
AR Path="/5AE958E5/5AECF023" Ref="TP615"  Part="1" 
AR Path="/5AE96312/5AECF023" Ref="TP715"  Part="1" 
AR Path="/5AE96ED4/5AECF023" Ref="TP815"  Part="1" 
AR Path="/5AFE7089/5AECF023" Ref="TP1015"  Part="1" 
F 0 "TP415" H 2200 2500 50  0000 R CNN
F 1 "S15" H 2400 2475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2400 2400 50  0001 C CNN
F 3 "~" H 2400 2400 50  0001 C CNN
	1    2200 2400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP416
U 1 1 5AECF077
P 2300 2600
AR Path="/5AE9ECEB/5AECF077" Ref="TP416"  Part="1" 
AR Path="/5AE958E5/5AECF077" Ref="TP616"  Part="1" 
AR Path="/5AE96312/5AECF077" Ref="TP716"  Part="1" 
AR Path="/5AE96ED4/5AECF077" Ref="TP816"  Part="1" 
AR Path="/5AFE7089/5AECF077" Ref="TP1016"  Part="1" 
F 0 "TP416" H 2300 2700 50  0000 R CNN
F 1 "S16" H 2500 2675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2500 2600 50  0001 C CNN
F 3 "~" H 2500 2600 50  0001 C CNN
	1    2300 2600
	-1   0    0    -1  
$EndComp
Wire Wire Line
	3600 2800 1600 2800
Wire Wire Line
	3600 5700 3300 5700
Wire Wire Line
	3600 5600 3300 5600
Wire Wire Line
	3600 5400 3300 5400
Wire Wire Line
	3600 5900 3300 5900
$Comp
L Device:C_Small C402
U 1 1 5AF0F415
P 3200 6100
AR Path="/5AE9ECEB/5AF0F415" Ref="C402"  Part="1" 
AR Path="/5AE958E5/5AF0F415" Ref="C602"  Part="1" 
AR Path="/5AE96312/5AF0F415" Ref="C702"  Part="1" 
AR Path="/5AE96ED4/5AF0F415" Ref="C802"  Part="1" 
AR Path="/5AFE7089/5AF0F415" Ref="C1002"  Part="1" 
F 0 "C402" H 3292 6146 50  0000 L CNN
F 1 "0.1uF" H 3292 6055 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3200 6100 50  0001 C CNN
F 3 "~" H 3200 6100 50  0001 C CNN
	1    3200 6100
	-1   0    0    -1  
$EndComp
Connection ~ 3300 5400
Connection ~ 3300 5600
Wire Wire Line
	3300 5400 2700 5400
Wire Wire Line
	3300 5600 2700 5600
Connection ~ 3300 5900
Wire Wire Line
	3300 5900 2700 5900
Wire Wire Line
	3200 6000 2700 6000
Wire Wire Line
	2700 6200 2400 6200
Wire Wire Line
	5800 6000 6100 6000
Connection ~ 6100 6000
Wire Wire Line
	6100 6000 6700 6000
Wire Wire Line
	5800 6200 6100 6200
Connection ~ 6100 6200
Wire Wire Line
	6700 6200 7000 6200
Connection ~ 2700 6200
Wire Wire Line
	3200 6200 2700 6200
Connection ~ 6700 6200
Wire Wire Line
	6100 6200 6700 6200
Text HLabel 4600 6700 3    50   Input ~ 0
AGND
Wire Wire Line
	4600 6700 4600 6400
Text HLabel 4800 6700 3    50   Input ~ 0
DGND
Wire Wire Line
	4800 6700 4800 6400
Wire Wire Line
	4300 1700 4300 1800
Wire Wire Line
	3800 1300 3800 1700
Wire Wire Line
	3800 1700 4300 1700
Connection ~ 3800 1300
Wire Wire Line
	4400 1600 4400 1800
Wire Wire Line
	4200 1600 4400 1600
Wire Wire Line
	4600 1300 4600 1500
Wire Wire Line
	4600 1500 4500 1500
Wire Wire Line
	4500 1500 4500 1800
Connection ~ 4600 1300
Wire Wire Line
	4600 600  5000 600 
Wire Wire Line
	4800 700  5200 700 
Wire Wire Line
	3800 600  4200 600 
Wire Wire Line
	4000 700  4400 700 
Connection ~ 3800 900 
Connection ~ 4000 900 
Wire Wire Line
	3800 900  3800 1300
Wire Wire Line
	4000 900  4000 1300
Connection ~ 4200 900 
Connection ~ 4400 900 
Wire Wire Line
	4200 900  4200 1300
Wire Wire Line
	4400 900  4400 1300
Connection ~ 4600 900 
Wire Wire Line
	4600 900  4600 1300
Wire Wire Line
	4800 900  4800 1300
Connection ~ 4800 900 
Connection ~ 5000 900 
Connection ~ 5200 900 
Wire Wire Line
	5000 900  5000 1300
Wire Wire Line
	5200 900  5200 1300
Wire Wire Line
	5600 1300 5600 900 
Wire Wire Line
	5600 700  5900 700 
Connection ~ 5600 900 
Wire Wire Line
	5600 900  5600 700 
Wire Wire Line
	4800 1800 4800 1700
Wire Wire Line
	4800 1700 5400 1700
Wire Wire Line
	5400 1700 5400 1300
Connection ~ 5400 900 
Wire Wire Line
	5400 900  5400 600 
Connection ~ 5400 1300
Wire Wire Line
	5400 1300 5400 900 
Wire Wire Line
	4600 1800 4600 1600
Wire Wire Line
	4600 1600 5000 1600
Wire Wire Line
	5000 1600 5000 1300
Connection ~ 5000 1300
Wire Wire Line
	5400 600  5900 600 
Wire Wire Line
	4000 700  3500 700 
Connection ~ 4000 700 
Wire Wire Line
	3600 2100 3400 2100
Wire Wire Line
	3400 2100 3400 2300
Wire Wire Line
	3400 5100 2700 5100
Connection ~ 3200 6000
Connection ~ 3200 6200
Wire Wire Line
	3200 6000 3600 6000
Wire Wire Line
	3200 6200 3600 6200
Wire Wire Line
	3400 2300 3600 2300
Connection ~ 3400 2300
Wire Wire Line
	3400 2300 3400 2500
Wire Wire Line
	3400 2500 3600 2500
Connection ~ 3400 2500
Wire Wire Line
	3400 2500 3400 2700
Wire Wire Line
	3400 2700 3600 2700
Connection ~ 3400 2700
Wire Wire Line
	3400 2700 3400 2900
Wire Wire Line
	3400 2900 3600 2900
Connection ~ 3400 2900
Wire Wire Line
	3400 2900 3400 3100
Wire Wire Line
	3400 3100 3600 3100
Connection ~ 3400 3100
Wire Wire Line
	3400 3100 3400 3300
Wire Wire Line
	3400 3300 3600 3300
Connection ~ 3400 3300
Wire Wire Line
	3400 3300 3400 3500
Wire Wire Line
	3400 3500 3600 3500
Connection ~ 3400 3500
Wire Wire Line
	3400 3500 3400 3700
Wire Wire Line
	3400 3700 3600 3700
Connection ~ 3400 3700
Wire Wire Line
	3400 3700 3400 3900
Wire Wire Line
	3400 3900 3600 3900
Connection ~ 3400 3900
Wire Wire Line
	3400 3900 3400 4100
Wire Wire Line
	3400 4100 3600 4100
Connection ~ 3400 4100
Wire Wire Line
	3400 4100 3400 4300
Wire Wire Line
	3400 4300 3600 4300
Connection ~ 3400 4300
Wire Wire Line
	3400 4300 3400 4500
Wire Wire Line
	3400 4500 3600 4500
Connection ~ 3400 4500
Wire Wire Line
	3400 4500 3400 4700
Wire Wire Line
	3400 4700 3600 4700
Connection ~ 3400 4700
Wire Wire Line
	3400 4700 3400 4900
Wire Wire Line
	3400 4900 3600 4900
Connection ~ 3400 4900
Wire Wire Line
	3400 4900 3400 5100
Wire Wire Line
	3400 5100 3600 5100
Connection ~ 3400 5100
$EndSCHEMATC
