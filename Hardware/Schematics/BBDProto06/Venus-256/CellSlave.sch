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
	4900 5700 5200 5700
$Comp
L Device:C_Small C404
U 1 1 5AE9F047
P 2400 6000
AR Path="/5AE9ECEB/5AE9F047" Ref="C404"  Part="1" 
AR Path="/5AE958E5/5AE9F047" Ref="C604"  Part="1" 
AR Path="/5AE96312/5AE9F047" Ref="C704"  Part="1" 
AR Path="/5AE96ED4/5AE9F047" Ref="C804"  Part="1" 
AR Path="/5AFE7089/5AE9F047" Ref="C1004"  Part="1" 
F 0 "C1004" H 2492 6046 50  0000 L CNN
F 1 "10uF,X5R" H 2492 5955 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2400 6000 50  0001 C CNN
F 3 "~" H 2400 6000 50  0001 C CNN
	1    2400 6000
	-1   0    0    -1  
$EndComp
Text HLabel 5200 2500 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 2600 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 2500 5200 2500
Wire Wire Line
	4900 2600 5200 2600
Text HLabel 5200 5500 2    50   Output ~ 0
BUSY
Text HLabel 5200 4900 2    50   Input ~ 0
~CS
Text HLabel 5200 2800 2    50   Input ~ 0
VDRIVE_3V3
Wire Wire Line
	4900 2800 5200 2800
Wire Wire Line
	4900 3400 5200 3400
Text HLabel 5200 4000 2    50   Input ~ 0
SDI
Text HLabel 5200 4200 2    50   Output ~ 0
SDOA
Wire Wire Line
	4900 4000 5200 4000
Wire Wire Line
	4900 4100 5200 4100
Wire Wire Line
	4900 4200 5200 4200
Wire Wire Line
	4900 4900 5200 4900
Wire Wire Line
	4900 5500 5200 5500
Text HLabel 5200 4800 2    50   Input ~ 0
SCLK
Wire Wire Line
	4900 4800 5200 4800
Text HLabel 2600 900  0    50   UnSpc ~ 0
AGND
Text HLabel 5000 900  2    50   UnSpc ~ 0
DGND
Text HLabel 5000 800  2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 2600 800  0    50   Input ~ 0
VCC_5V0
Wire Wire Line
	2100 2200 2700 2200
Wire Wire Line
	2100 2400 2700 2400
Text HLabel 1800 5300 0    50   Input ~ 0
SGND
Text HLabel 5200 4300 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 4400 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 4500 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 4300 5200 4300
Wire Wire Line
	4900 4400 5200 4400
Wire Wire Line
	4900 4500 5200 4500
Text HLabel 5200 4700 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 4700 5200 4700
Text HLabel 5200 5100 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 5200 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 5300 2    50   UnSpc ~ 0
DGND
$Comp
L Analog_ADC:AD7616 U401
U 1 1 5AE20D71
P 3800 4300
AR Path="/5AE9ECEB/5AE20D71" Ref="U401"  Part="1" 
AR Path="/5AE958E5/5AE20D71" Ref="U601"  Part="1" 
AR Path="/5AE96312/5AE20D71" Ref="U701"  Part="1" 
AR Path="/5AE96ED4/5AE20D71" Ref="U801"  Part="1" 
AR Path="/5AFE7089/5AE20D71" Ref="U1001"  Part="1" 
F 0 "U1001" H 2900 6550 50  0000 C CNN
F 1 "AD7616" H 4650 6550 50  0000 C CNN
F 2 "Package_QFP:LQFP-80_14x14mm_P0.65mm" H 3800 4300 50  0001 C CIN
F 3 "http://www.analog.com/media/en/technical-documentation/data-sheets/AD7616.pdf" H 3900 1300 50  0001 C CNN
	1    3800 4300
	1    0    0    -1  
$EndComp
Wire Wire Line
	4900 5100 5200 5100
Wire Wire Line
	4900 5200 5200 5200
Wire Wire Line
	4900 5300 5200 5300
Text HLabel 5200 5700 2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 1800 6100 0    50   UnSpc ~ 0
AGND
Text HLabel 5200 2300 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 2300 5200 2300
Text HLabel 5200 2200 2    50   Input ~ 0
~RESET
Wire Wire Line
	4900 2200 5200 2200
Text HLabel 5200 3000 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 3100 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 3200 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 3300 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 3000 5200 3000
Wire Wire Line
	4900 3100 5200 3100
Wire Wire Line
	4900 3200 5200 3200
Wire Wire Line
	4900 3300 5200 3300
Text HLabel 5200 3400 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 3500 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 3500 5200 3500
Text HLabel 5200 3600 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 3700 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 3600 5200 3600
Wire Wire Line
	4900 3700 5200 3700
Text HLabel 5200 3800 2    50   UnSpc ~ 0
DGND
Text HLabel 5200 3900 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4900 3800 5200 3800
Wire Wire Line
	4900 3900 5200 3900
NoConn ~ 5200 4100
Text HLabel 5200 5600 2    50   Input ~ 0
CONVST
Wire Wire Line
	4900 5600 5200 5600
Text HLabel 1500 6400 0    50   UnSpc ~ 0
AGND
Text HLabel 6100 6400 2    50   UnSpc ~ 0
DGND
$Comp
L Device:C_Small C416
U 1 1 5AF1A3C9
P 5800 6300
AR Path="/5AE9ECEB/5AF1A3C9" Ref="C416"  Part="1" 
AR Path="/5AE958E5/5AF1A3C9" Ref="C616"  Part="1" 
AR Path="/5AE96312/5AF1A3C9" Ref="C716"  Part="1" 
AR Path="/5AE96ED4/5AF1A3C9" Ref="C816"  Part="1" 
AR Path="/5AFE7089/5AF1A3C9" Ref="C1016"  Part="1" 
F 0 "C1016" H 5892 6346 50  0000 L CNN
F 1 "10uF" H 5892 6255 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5800 6300 50  0001 C CNN
F 3 "~" H 5800 6300 50  0001 C CNN
	1    5800 6300
	1    0    0    -1  
$EndComp
Wire Wire Line
	2100 2600 2700 2600
Wire Wire Line
	2100 2800 2700 2800
Wire Wire Line
	2100 3200 2700 3200
Wire Wire Line
	2100 3400 2700 3400
Wire Wire Line
	2700 3600 2100 3600
Wire Wire Line
	2100 3800 2700 3800
Wire Wire Line
	2700 4000 2100 4000
Wire Wire Line
	2100 4200 2700 4200
Wire Wire Line
	2700 4400 2100 4400
Wire Wire Line
	2100 4600 2700 4600
Wire Wire Line
	2700 4800 2100 4800
Wire Wire Line
	2100 5000 2700 5000
Wire Wire Line
	2700 5200 2100 5200
$Comp
L Device:C_Small C403
U 1 1 5AF62C30
P 2400 5700
AR Path="/5AE9ECEB/5AF62C30" Ref="C403"  Part="1" 
AR Path="/5AE958E5/5AF62C30" Ref="C603"  Part="1" 
AR Path="/5AE96312/5AF62C30" Ref="C703"  Part="1" 
AR Path="/5AE96ED4/5AF62C30" Ref="C803"  Part="1" 
AR Path="/5AFE7089/5AF62C30" Ref="C1003"  Part="1" 
F 0 "C1003" H 2492 5746 50  0000 L CNN
F 1 "0.1uF,X8R" H 2492 5655 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2400 5700 50  0001 C CNN
F 3 "~" H 2400 5700 50  0001 C CNN
	1    2400 5700
	-1   0    0    -1  
$EndComp
Text HLabel 1800 5800 0    50   UnSpc ~ 0
AGND
$Comp
L Device:C_Small C414
U 1 1 5AE39B5F
P 4600 1500
AR Path="/5AE9ECEB/5AE39B5F" Ref="C414"  Part="1" 
AR Path="/5AE958E5/5AE39B5F" Ref="C614"  Part="1" 
AR Path="/5AE96312/5AE39B5F" Ref="C714"  Part="1" 
AR Path="/5AE96ED4/5AE39B5F" Ref="C814"  Part="1" 
AR Path="/5AFE7089/5AE39B5F" Ref="C1014"  Part="1" 
F 0 "C1014" V 4500 1500 50  0000 C CNN
F 1 "0.1uF" V 4700 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4600 1500 50  0001 C CNN
F 3 "~" H 4600 1500 50  0001 C CNN
	1    4600 1500
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C413
U 1 1 5AE424D3
P 4600 1100
AR Path="/5AE9ECEB/5AE424D3" Ref="C413"  Part="1" 
AR Path="/5AE958E5/5AE424D3" Ref="C613"  Part="1" 
AR Path="/5AE96312/5AE424D3" Ref="C713"  Part="1" 
AR Path="/5AE96ED4/5AE424D3" Ref="C813"  Part="1" 
AR Path="/5AFE7089/5AE424D3" Ref="C1013"  Part="1" 
F 0 "C1013" V 4500 1100 50  0000 C CNN
F 1 "10uF" V 4700 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4600 1100 50  0001 C CNN
F 3 "~" H 4600 1100 50  0001 C CNN
	1    4600 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C405
U 1 1 5AE748DC
P 3000 1100
AR Path="/5AE9ECEB/5AE748DC" Ref="C405"  Part="1" 
AR Path="/5AE958E5/5AE748DC" Ref="C605"  Part="1" 
AR Path="/5AE96312/5AE748DC" Ref="C705"  Part="1" 
AR Path="/5AE96ED4/5AE748DC" Ref="C805"  Part="1" 
AR Path="/5AFE7089/5AE748DC" Ref="C1005"  Part="1" 
F 0 "C1005" V 2900 1100 50  0000 C CNN
F 1 "10uF" V 3100 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3000 1100 50  0001 C CNN
F 3 "~" H 3000 1100 50  0001 C CNN
	1    3000 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C406
U 1 1 5AE748E2
P 3000 1500
AR Path="/5AE9ECEB/5AE748E2" Ref="C406"  Part="1" 
AR Path="/5AE958E5/5AE748E2" Ref="C606"  Part="1" 
AR Path="/5AE96312/5AE748E2" Ref="C706"  Part="1" 
AR Path="/5AE96ED4/5AE748E2" Ref="C806"  Part="1" 
AR Path="/5AFE7089/5AE748E2" Ref="C1006"  Part="1" 
F 0 "C1006" V 2900 1500 50  0000 C CNN
F 1 "0.1uF" V 3100 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3000 1500 50  0001 C CNN
F 3 "~" H 3000 1500 50  0001 C CNN
	1    3000 1500
	0    -1   -1   0   
$EndComp
Wire Wire Line
	3100 900  3100 1100
Wire Wire Line
	2600 800  2900 800 
Wire Wire Line
	2900 800  2900 1100
$Comp
L Device:C_Small C407
U 1 1 5AE9D928
P 3400 1100
AR Path="/5AE9ECEB/5AE9D928" Ref="C407"  Part="1" 
AR Path="/5AE958E5/5AE9D928" Ref="C607"  Part="1" 
AR Path="/5AE96312/5AE9D928" Ref="C707"  Part="1" 
AR Path="/5AE96ED4/5AE9D928" Ref="C807"  Part="1" 
AR Path="/5AFE7089/5AE9D928" Ref="C1007"  Part="1" 
F 0 "C1007" V 3300 1100 50  0000 C CNN
F 1 "10uF" V 3500 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3400 1100 50  0001 C CNN
F 3 "~" H 3400 1100 50  0001 C CNN
	1    3400 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C408
U 1 1 5AE9D92E
P 3400 1500
AR Path="/5AE9ECEB/5AE9D92E" Ref="C408"  Part="1" 
AR Path="/5AE958E5/5AE9D92E" Ref="C608"  Part="1" 
AR Path="/5AE96312/5AE9D92E" Ref="C708"  Part="1" 
AR Path="/5AE96ED4/5AE9D92E" Ref="C808"  Part="1" 
AR Path="/5AFE7089/5AE9D92E" Ref="C1008"  Part="1" 
F 0 "C1008" V 3300 1500 50  0000 C CNN
F 1 "0.1uF" V 3500 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3400 1500 50  0001 C CNN
F 3 "~" H 3400 1500 50  0001 C CNN
	1    3400 1500
	0    1    1    0   
$EndComp
Wire Wire Line
	3500 900  3500 1100
Connection ~ 3300 1500
Wire Wire Line
	3300 800  3300 1100
$Comp
L Device:C_Small C409
U 1 1 5AEA3FF2
P 3800 1100
AR Path="/5AE9ECEB/5AEA3FF2" Ref="C409"  Part="1" 
AR Path="/5AE958E5/5AEA3FF2" Ref="C609"  Part="1" 
AR Path="/5AE96312/5AEA3FF2" Ref="C709"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF2" Ref="C809"  Part="1" 
AR Path="/5AFE7089/5AEA3FF2" Ref="C1009"  Part="1" 
F 0 "C1009" V 3700 1100 50  0000 C CNN
F 1 "10uF" V 3900 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3800 1100 50  0001 C CNN
F 3 "~" H 3800 1100 50  0001 C CNN
	1    3800 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C410
U 1 1 5AEA3FF8
P 3800 1500
AR Path="/5AE9ECEB/5AEA3FF8" Ref="C410"  Part="1" 
AR Path="/5AE958E5/5AEA3FF8" Ref="C610"  Part="1" 
AR Path="/5AE96312/5AEA3FF8" Ref="C710"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF8" Ref="C810"  Part="1" 
AR Path="/5AFE7089/5AEA3FF8" Ref="C1010"  Part="1" 
F 0 "C1010" V 3700 1500 50  0000 C CNN
F 1 "0.1uF" V 3900 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3800 1500 50  0001 C CNN
F 3 "~" H 3800 1500 50  0001 C CNN
	1    3800 1500
	0    1    1    0   
$EndComp
Wire Wire Line
	3900 900  3900 1100
Wire Wire Line
	3700 800  3700 1100
$Comp
L Device:C_Small C411
U 1 1 5AEB23BE
P 4200 1100
AR Path="/5AE9ECEB/5AEB23BE" Ref="C411"  Part="1" 
AR Path="/5AE958E5/5AEB23BE" Ref="C611"  Part="1" 
AR Path="/5AE96312/5AEB23BE" Ref="C711"  Part="1" 
AR Path="/5AE96ED4/5AEB23BE" Ref="C811"  Part="1" 
AR Path="/5AFE7089/5AEB23BE" Ref="C1011"  Part="1" 
F 0 "C1011" V 4100 1100 50  0000 C CNN
F 1 "10uF" V 4300 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4200 1100 50  0001 C CNN
F 3 "~" H 4200 1100 50  0001 C CNN
	1    4200 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C412
U 1 1 5AEB23C4
P 4200 1500
AR Path="/5AE9ECEB/5AEB23C4" Ref="C412"  Part="1" 
AR Path="/5AE958E5/5AEB23C4" Ref="C612"  Part="1" 
AR Path="/5AE96312/5AEB23C4" Ref="C712"  Part="1" 
AR Path="/5AE96ED4/5AEB23C4" Ref="C812"  Part="1" 
AR Path="/5AFE7089/5AEB23C4" Ref="C1012"  Part="1" 
F 0 "C1012" V 4100 1500 50  0000 C CNN
F 1 "0.1uF" V 4300 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4200 1500 50  0001 C CNN
F 3 "~" H 4200 1500 50  0001 C CNN
	1    4200 1500
	0    1    1    0   
$EndComp
Wire Wire Line
	4300 900  4300 1100
Connection ~ 2900 800 
Connection ~ 3300 800 
Wire Wire Line
	3300 800  3700 800 
Connection ~ 3700 800 
Connection ~ 3500 900 
Wire Wire Line
	3500 900  3900 900 
Connection ~ 3900 900 
Wire Wire Line
	3300 1500 3300 1800
$Comp
L Device:C_Small C401
U 1 1 5AF66660
P 1800 6300
AR Path="/5AE9ECEB/5AF66660" Ref="C401"  Part="1" 
AR Path="/5AE958E5/5AF66660" Ref="C601"  Part="1" 
AR Path="/5AE96312/5AF66660" Ref="C701"  Part="1" 
AR Path="/5AE96ED4/5AF66660" Ref="C801"  Part="1" 
AR Path="/5AFE7089/5AF66660" Ref="C1001"  Part="1" 
F 0 "C1001" H 1892 6346 50  0000 L CNN
F 1 "10uF" H 1892 6255 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 1800 6300 50  0001 C CNN
F 3 "~" H 1800 6300 50  0001 C CNN
	1    1800 6300
	-1   0    0    -1  
$EndComp
$Comp
L Device:C_Small C415
U 1 1 5AFC3D5B
P 5200 6300
AR Path="/5AE9ECEB/5AFC3D5B" Ref="C415"  Part="1" 
AR Path="/5AE958E5/5AFC3D5B" Ref="C615"  Part="1" 
AR Path="/5AE96312/5AFC3D5B" Ref="C715"  Part="1" 
AR Path="/5AE96ED4/5AFC3D5B" Ref="C815"  Part="1" 
AR Path="/5AFE7089/5AFC3D5B" Ref="C1015"  Part="1" 
F 0 "C1015" H 5292 6346 50  0000 L CNN
F 1 "0.1uF" H 5292 6255 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5200 6300 50  0001 C CNN
F 3 "~" H 5200 6300 50  0001 C CNN
	1    5200 6300
	1    0    0    -1  
$EndComp
Wire Wire Line
	4100 1100 4100 800 
$Comp
L Venus-256-rescue:Test_Point-Connector_Specialized TP401
U 1 1 5AE6BFCE
P 1800 5600
AR Path="/5AE9ECEB/5AE6BFCE" Ref="TP401"  Part="1" 
AR Path="/5AE958E5/5AE6BFCE" Ref="TP601"  Part="1" 
AR Path="/5AE96312/5AE6BFCE" Ref="TP701"  Part="1" 
AR Path="/5AE96ED4/5AE6BFCE" Ref="TP801"  Part="1" 
AR Path="/5AFE7089/5AE6BFCE" Ref="TP1001"  Part="1" 
F 0 "TP1001" V 1754 5787 50  0000 L CNN
F 1 "ADC_REF_OUT" V 1845 5787 50  0000 L CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2000 5600 50  0001 C CNN
F 3 "~" H 2000 5600 50  0001 C CNN
	1    1800 5600
	0    -1   1    0   
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP402
U 1 1 5AECD270
P 2100 2200
AR Path="/5AE9ECEB/5AECD270" Ref="TP402"  Part="1" 
AR Path="/5AE958E5/5AECD270" Ref="TP602"  Part="1" 
AR Path="/5AE96312/5AECD270" Ref="TP702"  Part="1" 
AR Path="/5AE96ED4/5AECD270" Ref="TP802"  Part="1" 
AR Path="/5AFE7089/5AECD270" Ref="TP1002"  Part="1" 
F 0 "TP1002" H 2100 2300 50  0000 R CNN
F 1 "S01" H 2300 2275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 2200 50  0001 C CNN
F 3 "~" H 2300 2200 50  0001 C CNN
	1    2100 2200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP403
U 1 1 5AECE3AF
P 2100 2400
AR Path="/5AE9ECEB/5AECE3AF" Ref="TP403"  Part="1" 
AR Path="/5AE958E5/5AECE3AF" Ref="TP603"  Part="1" 
AR Path="/5AE96312/5AECE3AF" Ref="TP703"  Part="1" 
AR Path="/5AE96ED4/5AECE3AF" Ref="TP803"  Part="1" 
AR Path="/5AFE7089/5AECE3AF" Ref="TP1003"  Part="1" 
F 0 "TP1003" H 2100 2500 50  0000 R CNN
F 1 "S02" H 2300 2475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 2400 50  0001 C CNN
F 3 "~" H 2300 2400 50  0001 C CNN
	1    2100 2400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP404
U 1 1 5AECE3FB
P 2100 2600
AR Path="/5AE9ECEB/5AECE3FB" Ref="TP404"  Part="1" 
AR Path="/5AE958E5/5AECE3FB" Ref="TP604"  Part="1" 
AR Path="/5AE96312/5AECE3FB" Ref="TP704"  Part="1" 
AR Path="/5AE96ED4/5AECE3FB" Ref="TP804"  Part="1" 
AR Path="/5AFE7089/5AECE3FB" Ref="TP1004"  Part="1" 
F 0 "TP1004" H 2100 2700 50  0000 R CNN
F 1 "S03" H 2300 2675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 2600 50  0001 C CNN
F 3 "~" H 2300 2600 50  0001 C CNN
	1    2100 2600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP405
U 1 1 5AECE449
P 2100 2800
AR Path="/5AE9ECEB/5AECE449" Ref="TP405"  Part="1" 
AR Path="/5AE958E5/5AECE449" Ref="TP605"  Part="1" 
AR Path="/5AE96312/5AECE449" Ref="TP705"  Part="1" 
AR Path="/5AE96ED4/5AECE449" Ref="TP805"  Part="1" 
AR Path="/5AFE7089/5AECE449" Ref="TP1005"  Part="1" 
F 0 "TP1005" H 2100 2900 50  0000 R CNN
F 1 "S04" H 2300 2875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 2800 50  0001 C CNN
F 3 "~" H 2300 2800 50  0001 C CNN
	1    2100 2800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP406
U 1 1 5AECE499
P 2100 3000
AR Path="/5AE9ECEB/5AECE499" Ref="TP406"  Part="1" 
AR Path="/5AE958E5/5AECE499" Ref="TP606"  Part="1" 
AR Path="/5AE96312/5AECE499" Ref="TP706"  Part="1" 
AR Path="/5AE96ED4/5AECE499" Ref="TP806"  Part="1" 
AR Path="/5AFE7089/5AECE499" Ref="TP1006"  Part="1" 
F 0 "TP1006" H 2100 3100 50  0000 R CNN
F 1 "S05" H 2300 3075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 3000 50  0001 C CNN
F 3 "~" H 2300 3000 50  0001 C CNN
	1    2100 3000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP407
U 1 1 5AECE4E7
P 2100 3200
AR Path="/5AE9ECEB/5AECE4E7" Ref="TP407"  Part="1" 
AR Path="/5AE958E5/5AECE4E7" Ref="TP607"  Part="1" 
AR Path="/5AE96312/5AECE4E7" Ref="TP707"  Part="1" 
AR Path="/5AE96ED4/5AECE4E7" Ref="TP807"  Part="1" 
AR Path="/5AFE7089/5AECE4E7" Ref="TP1007"  Part="1" 
F 0 "TP1007" H 2100 3300 50  0000 R CNN
F 1 "S06" H 2300 3275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 3200 50  0001 C CNN
F 3 "~" H 2300 3200 50  0001 C CNN
	1    2100 3200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP408
U 1 1 5AECE53B
P 2100 3400
AR Path="/5AE9ECEB/5AECE53B" Ref="TP408"  Part="1" 
AR Path="/5AE958E5/5AECE53B" Ref="TP608"  Part="1" 
AR Path="/5AE96312/5AECE53B" Ref="TP708"  Part="1" 
AR Path="/5AE96ED4/5AECE53B" Ref="TP808"  Part="1" 
AR Path="/5AFE7089/5AECE53B" Ref="TP1008"  Part="1" 
F 0 "TP1008" H 2100 3500 50  0000 R CNN
F 1 "S07" H 2300 3475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 3400 50  0001 C CNN
F 3 "~" H 2300 3400 50  0001 C CNN
	1    2100 3400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP409
U 1 1 5AECE591
P 2100 3600
AR Path="/5AE9ECEB/5AECE591" Ref="TP409"  Part="1" 
AR Path="/5AE958E5/5AECE591" Ref="TP609"  Part="1" 
AR Path="/5AE96312/5AECE591" Ref="TP709"  Part="1" 
AR Path="/5AE96ED4/5AECE591" Ref="TP809"  Part="1" 
AR Path="/5AFE7089/5AECE591" Ref="TP1009"  Part="1" 
F 0 "TP1009" H 2100 3700 50  0000 R CNN
F 1 "S08" H 2300 3675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 3600 50  0001 C CNN
F 3 "~" H 2300 3600 50  0001 C CNN
	1    2100 3600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP410
U 1 1 5AECE5E5
P 2100 3800
AR Path="/5AE9ECEB/5AECE5E5" Ref="TP410"  Part="1" 
AR Path="/5AE958E5/5AECE5E5" Ref="TP610"  Part="1" 
AR Path="/5AE96312/5AECE5E5" Ref="TP710"  Part="1" 
AR Path="/5AE96ED4/5AECE5E5" Ref="TP810"  Part="1" 
AR Path="/5AFE7089/5AECE5E5" Ref="TP1010"  Part="1" 
F 0 "TP1010" H 2100 3900 50  0000 R CNN
F 1 "S09" H 2300 3875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 3800 50  0001 C CNN
F 3 "~" H 2300 3800 50  0001 C CNN
	1    2100 3800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP411
U 1 1 5AECEE9D
P 2100 4000
AR Path="/5AE9ECEB/5AECEE9D" Ref="TP411"  Part="1" 
AR Path="/5AE958E5/5AECEE9D" Ref="TP611"  Part="1" 
AR Path="/5AE96312/5AECEE9D" Ref="TP711"  Part="1" 
AR Path="/5AE96ED4/5AECEE9D" Ref="TP811"  Part="1" 
AR Path="/5AFE7089/5AECEE9D" Ref="TP1011"  Part="1" 
F 0 "TP1011" H 2100 4100 50  0000 R CNN
F 1 "S10" H 2300 4075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 4000 50  0001 C CNN
F 3 "~" H 2300 4000 50  0001 C CNN
	1    2100 4000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP412
U 1 1 5AECEEE7
P 2100 4200
AR Path="/5AE9ECEB/5AECEEE7" Ref="TP412"  Part="1" 
AR Path="/5AE958E5/5AECEEE7" Ref="TP612"  Part="1" 
AR Path="/5AE96312/5AECEEE7" Ref="TP712"  Part="1" 
AR Path="/5AE96ED4/5AECEEE7" Ref="TP812"  Part="1" 
AR Path="/5AFE7089/5AECEEE7" Ref="TP1012"  Part="1" 
F 0 "TP1012" H 2100 4300 50  0000 R CNN
F 1 "S11" H 2300 4275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 4200 50  0001 C CNN
F 3 "~" H 2300 4200 50  0001 C CNN
	1    2100 4200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP413
U 1 1 5AECEF33
P 2100 4400
AR Path="/5AE9ECEB/5AECEF33" Ref="TP413"  Part="1" 
AR Path="/5AE958E5/5AECEF33" Ref="TP613"  Part="1" 
AR Path="/5AE96312/5AECEF33" Ref="TP713"  Part="1" 
AR Path="/5AE96ED4/5AECEF33" Ref="TP813"  Part="1" 
AR Path="/5AFE7089/5AECEF33" Ref="TP1013"  Part="1" 
F 0 "TP1013" H 2100 4500 50  0000 R CNN
F 1 "S12" H 2300 4475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 4400 50  0001 C CNN
F 3 "~" H 2300 4400 50  0001 C CNN
	1    2100 4400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP414
U 1 1 5AECEF81
P 2100 4600
AR Path="/5AE9ECEB/5AECEF81" Ref="TP414"  Part="1" 
AR Path="/5AE958E5/5AECEF81" Ref="TP614"  Part="1" 
AR Path="/5AE96312/5AECEF81" Ref="TP714"  Part="1" 
AR Path="/5AE96ED4/5AECEF81" Ref="TP814"  Part="1" 
AR Path="/5AFE7089/5AECEF81" Ref="TP1014"  Part="1" 
F 0 "TP1014" H 2100 4700 50  0000 R CNN
F 1 "S13" H 2300 4675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 4600 50  0001 C CNN
F 3 "~" H 2300 4600 50  0001 C CNN
	1    2100 4600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP415
U 1 1 5AECEFD1
P 2100 4800
AR Path="/5AE9ECEB/5AECEFD1" Ref="TP415"  Part="1" 
AR Path="/5AE958E5/5AECEFD1" Ref="TP615"  Part="1" 
AR Path="/5AE96312/5AECEFD1" Ref="TP715"  Part="1" 
AR Path="/5AE96ED4/5AECEFD1" Ref="TP815"  Part="1" 
AR Path="/5AFE7089/5AECEFD1" Ref="TP1015"  Part="1" 
F 0 "TP1015" H 2100 4900 50  0000 R CNN
F 1 "S14" H 2300 4875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 4800 50  0001 C CNN
F 3 "~" H 2300 4800 50  0001 C CNN
	1    2100 4800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP416
U 1 1 5AECF023
P 2100 5000
AR Path="/5AE9ECEB/5AECF023" Ref="TP416"  Part="1" 
AR Path="/5AE958E5/5AECF023" Ref="TP616"  Part="1" 
AR Path="/5AE96312/5AECF023" Ref="TP716"  Part="1" 
AR Path="/5AE96ED4/5AECF023" Ref="TP816"  Part="1" 
AR Path="/5AFE7089/5AECF023" Ref="TP1016"  Part="1" 
F 0 "TP1016" H 2100 5100 50  0000 R CNN
F 1 "S15" H 2300 5075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 5000 50  0001 C CNN
F 3 "~" H 2300 5000 50  0001 C CNN
	1    2100 5000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP417
U 1 1 5AECF077
P 2100 5200
AR Path="/5AE9ECEB/5AECF077" Ref="TP417"  Part="1" 
AR Path="/5AE958E5/5AECF077" Ref="TP617"  Part="1" 
AR Path="/5AE96312/5AECF077" Ref="TP717"  Part="1" 
AR Path="/5AE96ED4/5AECF077" Ref="TP817"  Part="1" 
AR Path="/5AFE7089/5AECF077" Ref="TP1017"  Part="1" 
F 0 "TP1017" H 2100 5300 50  0000 R CNN
F 1 "S16" H 2300 5275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 2300 5200 50  0001 C CNN
F 3 "~" H 2300 5200 50  0001 C CNN
	1    2100 5200
	-1   0    0    -1  
$EndComp
Wire Wire Line
	2700 3000 2100 3000
Wire Wire Line
	2700 5900 2400 5900
Wire Wire Line
	2700 5800 2400 5800
Wire Wire Line
	2700 5600 2400 5600
Wire Wire Line
	2700 6100 2400 6100
$Comp
L Device:C_Small C402
U 1 1 5AF0F415
P 2300 6300
AR Path="/5AE9ECEB/5AF0F415" Ref="C402"  Part="1" 
AR Path="/5AE958E5/5AF0F415" Ref="C602"  Part="1" 
AR Path="/5AE96312/5AF0F415" Ref="C702"  Part="1" 
AR Path="/5AE96ED4/5AF0F415" Ref="C802"  Part="1" 
AR Path="/5AFE7089/5AF0F415" Ref="C1002"  Part="1" 
F 0 "C1002" H 2392 6346 50  0000 L CNN
F 1 "0.1uF" H 2392 6255 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2300 6300 50  0001 C CNN
F 3 "~" H 2300 6300 50  0001 C CNN
	1    2300 6300
	-1   0    0    -1  
$EndComp
Connection ~ 2400 5600
Connection ~ 2400 5800
Wire Wire Line
	2400 5600 1800 5600
Wire Wire Line
	2400 5800 1800 5800
Connection ~ 2400 6100
Wire Wire Line
	2400 6100 1800 6100
Wire Wire Line
	2300 6200 1800 6200
Wire Wire Line
	1800 6400 1500 6400
Wire Wire Line
	4900 6200 5200 6200
Connection ~ 5200 6200
Wire Wire Line
	5200 6200 5800 6200
Wire Wire Line
	4900 6400 5200 6400
Connection ~ 5200 6400
Wire Wire Line
	5800 6400 6100 6400
Connection ~ 1800 6400
Wire Wire Line
	2300 6400 1800 6400
Connection ~ 5800 6400
Wire Wire Line
	5200 6400 5800 6400
Text HLabel 3700 6900 3    50   Input ~ 0
AGND
Wire Wire Line
	3700 6900 3700 6600
Text HLabel 3900 6900 3    50   Input ~ 0
DGND
Wire Wire Line
	3900 6900 3900 6600
Wire Wire Line
	3400 1900 3400 2000
Wire Wire Line
	2900 1500 2900 1900
Wire Wire Line
	2900 1900 3400 1900
Connection ~ 2900 1500
Wire Wire Line
	3500 1800 3500 2000
Wire Wire Line
	3300 1800 3500 1800
Wire Wire Line
	3700 1500 3700 1700
Wire Wire Line
	3700 1700 3600 1700
Wire Wire Line
	3600 1700 3600 2000
Connection ~ 3700 1500
Wire Wire Line
	3700 800  4100 800 
Wire Wire Line
	3900 900  4300 900 
Wire Wire Line
	2900 800  3300 800 
Wire Wire Line
	3100 900  3500 900 
Connection ~ 2900 1100
Connection ~ 3100 1100
Wire Wire Line
	2900 1100 2900 1500
Wire Wire Line
	3100 1100 3100 1500
Connection ~ 3300 1100
Connection ~ 3500 1100
Wire Wire Line
	3300 1100 3300 1500
Wire Wire Line
	3500 1100 3500 1500
Connection ~ 3700 1100
Wire Wire Line
	3700 1100 3700 1500
Wire Wire Line
	3900 1100 3900 1500
Connection ~ 3900 1100
Connection ~ 4100 1100
Connection ~ 4300 1100
Wire Wire Line
	4100 1100 4100 1500
Wire Wire Line
	4300 1100 4300 1500
Wire Wire Line
	4700 1500 4700 1100
Wire Wire Line
	4700 900  5000 900 
Connection ~ 4700 1100
Wire Wire Line
	4700 1100 4700 900 
Wire Wire Line
	3900 2000 3900 1900
Wire Wire Line
	3900 1900 4500 1900
Wire Wire Line
	4500 1900 4500 1500
Connection ~ 4500 1100
Wire Wire Line
	4500 1100 4500 800 
Connection ~ 4500 1500
Wire Wire Line
	4500 1500 4500 1100
Wire Wire Line
	3700 2000 3700 1800
Wire Wire Line
	3700 1800 4100 1800
Wire Wire Line
	4100 1800 4100 1500
Connection ~ 4100 1500
Wire Wire Line
	4500 800  5000 800 
Wire Wire Line
	3100 900  2600 900 
Connection ~ 3100 900 
Wire Wire Line
	2700 2300 2500 2300
Wire Wire Line
	2500 2300 2500 2500
Wire Wire Line
	2500 5300 1800 5300
Connection ~ 2300 6200
Connection ~ 2300 6400
Wire Wire Line
	2300 6200 2700 6200
Wire Wire Line
	2300 6400 2700 6400
Wire Wire Line
	2500 2500 2700 2500
Connection ~ 2500 2500
Wire Wire Line
	2500 2500 2500 2700
Wire Wire Line
	2500 2700 2700 2700
Connection ~ 2500 2700
Wire Wire Line
	2500 2700 2500 2900
Wire Wire Line
	2500 2900 2700 2900
Connection ~ 2500 2900
Wire Wire Line
	2500 2900 2500 3100
Wire Wire Line
	2500 3100 2700 3100
Connection ~ 2500 3100
Wire Wire Line
	2500 3100 2500 3300
Wire Wire Line
	2500 3300 2700 3300
Connection ~ 2500 3300
Wire Wire Line
	2500 3300 2500 3500
Wire Wire Line
	2500 3500 2700 3500
Connection ~ 2500 3500
Wire Wire Line
	2500 3500 2500 3700
Wire Wire Line
	2500 3700 2700 3700
Connection ~ 2500 3700
Wire Wire Line
	2500 3700 2500 3900
Wire Wire Line
	2500 3900 2700 3900
Connection ~ 2500 3900
Wire Wire Line
	2500 3900 2500 4100
Wire Wire Line
	2500 4100 2700 4100
Connection ~ 2500 4100
Wire Wire Line
	2500 4100 2500 4300
Wire Wire Line
	2500 4300 2700 4300
Connection ~ 2500 4300
Wire Wire Line
	2500 4300 2500 4500
Wire Wire Line
	2500 4500 2700 4500
Connection ~ 2500 4500
Wire Wire Line
	2500 4500 2500 4700
Wire Wire Line
	2500 4700 2700 4700
Connection ~ 2500 4700
Wire Wire Line
	2500 4700 2500 4900
Wire Wire Line
	2500 4900 2700 4900
Connection ~ 2500 4900
Wire Wire Line
	2500 4900 2500 5100
Wire Wire Line
	2500 5100 2700 5100
Connection ~ 2500 5100
Wire Wire Line
	2500 5100 2500 5300
Wire Wire Line
	2500 5300 2700 5300
Connection ~ 2500 5300
$EndSCHEMATC
