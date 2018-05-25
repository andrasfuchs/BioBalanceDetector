EESchema Schematic File Version 4
LIBS:Venus-256-cache
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 7 10
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
	4400 5800 4700 5800
$Comp
L Device:C_Small C712
U 1 1 5AE9F047
P 4700 6300
AR Path="/5AE96312/5AE9F047" Ref="C712"  Part="1" 
AR Path="/5AE9ECEB/5AE9F047" Ref="C412"  Part="1" 
AR Path="/5AE958E5/5AE9F047" Ref="C612"  Part="1" 
AR Path="/5AE96ED4/5AE9F047" Ref="C812"  Part="1" 
AR Path="/5AFE7089/5AE9F047" Ref="C1012"  Part="1" 
F 0 "C1012" H 4792 6346 50  0000 L CNN
F 1 "10uF,X5R" H 4792 6255 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 6300 50  0001 C CNN
F 3 "~" H 4700 6300 50  0001 C CNN
	1    4700 6300
	1    0    0    -1  
$EndComp
Text HLabel 4700 2500 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 2600 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 2500 4700 2500
Wire Wire Line
	4400 2600 4700 2600
Text HLabel 4700 5500 2    50   Output ~ 0
BUSY
Text HLabel 4700 4900 2    50   Input ~ 0
~CS
Text HLabel 4700 2800 2    50   Input ~ 0
VDRIVE_3V3
Wire Wire Line
	4400 2800 4700 2800
Wire Wire Line
	4400 3400 4700 3400
Text HLabel 4700 4000 2    50   Input ~ 0
SDI
Text HLabel 4700 4200 2    50   Output ~ 0
SDOA
Wire Wire Line
	4400 4000 4700 4000
Wire Wire Line
	4400 4100 4700 4100
Wire Wire Line
	4400 4200 4700 4200
Wire Wire Line
	4400 4900 4700 4900
Wire Wire Line
	4400 5500 4700 5500
Text HLabel 4700 4800 2    50   Input ~ 0
SCLK
Wire Wire Line
	4400 4800 4700 4800
Text HLabel 2100 900  0    50   UnSpc ~ 0
AGND
Text HLabel 4500 900  2    50   UnSpc ~ 0
DGND
Text HLabel 4500 800  2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 2100 800  0    50   Input ~ 0
VCC_5V0
Wire Wire Line
	1600 2300 2200 2300
Wire Wire Line
	1600 2600 2200 2600
Text HLabel 2100 7500 3    50   Input ~ 0
SGND
Wire Wire Line
	2100 7500 2100 6900
Wire Wire Line
	2100 6900 2200 6900
Wire Wire Line
	2100 6900 2100 6600
Wire Wire Line
	2100 6600 2200 6600
Wire Wire Line
	2100 6600 2100 6300
Wire Wire Line
	2100 6300 2200 6300
Connection ~ 2100 6600
Wire Wire Line
	2100 6300 2100 6000
Wire Wire Line
	2100 6000 2200 6000
Connection ~ 2100 6300
Wire Wire Line
	2100 6000 2100 5700
Wire Wire Line
	2100 5700 2200 5700
Connection ~ 2100 6000
Wire Wire Line
	2100 5700 2100 5400
Wire Wire Line
	2100 5400 2200 5400
Connection ~ 2100 5700
Wire Wire Line
	2100 5400 2100 5100
Wire Wire Line
	2100 5100 2200 5100
Connection ~ 2100 5400
Wire Wire Line
	2100 5100 2100 4800
Wire Wire Line
	2100 4800 2200 4800
Connection ~ 2100 5100
Wire Wire Line
	2100 4800 2100 4500
Wire Wire Line
	2100 4500 2200 4500
Connection ~ 2100 4800
Wire Wire Line
	2100 4500 2100 4200
Wire Wire Line
	2100 4200 2200 4200
Connection ~ 2100 4500
Wire Wire Line
	2100 4200 2100 3900
Wire Wire Line
	2100 3900 2200 3900
Connection ~ 2100 4200
Wire Wire Line
	2100 3900 2100 3600
Wire Wire Line
	2100 3600 2200 3600
Connection ~ 2100 3900
Wire Wire Line
	2100 3600 2100 3300
Wire Wire Line
	2100 3300 2200 3300
Connection ~ 2100 3600
Wire Wire Line
	2100 3300 2100 3000
Wire Wire Line
	2100 3000 2200 3000
Connection ~ 2100 3300
Wire Wire Line
	2100 3000 2100 2700
Wire Wire Line
	2100 2700 2200 2700
Connection ~ 2100 3000
Wire Wire Line
	2100 2700 2100 2400
Wire Wire Line
	2100 2400 2200 2400
Connection ~ 2100 2700
Text HLabel 4700 4300 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 4400 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 4500 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 4300 4700 4300
Wire Wire Line
	4400 4400 4700 4400
Wire Wire Line
	4400 4500 4700 4500
Text HLabel 4700 4700 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 4700 4700 4700
Text HLabel 4700 5100 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 5200 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 5300 2    50   UnSpc ~ 0
DGND
$Comp
L Analog_ADC:AD7616 U701
U 1 1 5AE20D71
P 3300 4600
AR Path="/5AE96312/5AE20D71" Ref="U701"  Part="1" 
AR Path="/5AE9ECEB/5AE20D71" Ref="U401"  Part="1" 
AR Path="/5AE958E5/5AE20D71" Ref="U601"  Part="1" 
AR Path="/5AE96ED4/5AE20D71" Ref="U801"  Part="1" 
AR Path="/5AFE7089/5AE20D71" Ref="U1001"  Part="1" 
F 0 "U1001" H 2400 7150 50  0000 C CNN
F 1 "AD7616" H 4150 7150 50  0000 C CNN
F 2 "Package_QFP:LQFP-80_14x14mm_P0.65mm" H 3300 4600 50  0001 C CIN
F 3 "http://www.analog.com/media/en/technical-documentation/data-sheets/AD7616.pdf" H 3400 1600 50  0001 C CNN
	1    3300 4600
	1    0    0    -1  
$EndComp
Wire Wire Line
	4400 5100 4700 5100
Wire Wire Line
	4400 5200 4700 5200
Wire Wire Line
	4400 5300 4700 5300
Text HLabel 4700 5800 2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 5300 6400 2    50   UnSpc ~ 0
AGND
Text HLabel 4700 2300 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 2300 4700 2300
Text HLabel 4700 2200 2    50   Input ~ 0
~RESET
Wire Wire Line
	4400 2200 4700 2200
Text HLabel 4700 3000 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3100 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3200 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3300 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 3000 4700 3000
Wire Wire Line
	4400 3100 4700 3100
Wire Wire Line
	4400 3200 4700 3200
Wire Wire Line
	4400 3300 4700 3300
Text HLabel 4700 3400 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3500 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 3500 4700 3500
Text HLabel 4700 3600 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3700 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 3600 4700 3600
Wire Wire Line
	4400 3700 4700 3700
Text HLabel 4700 3800 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3900 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 3800 4700 3800
Wire Wire Line
	4400 3900 4700 3900
NoConn ~ 4700 4100
Text HLabel 4700 5600 2    50   Input ~ 0
CONVST
Wire Wire Line
	4400 5600 4700 5600
Text HLabel 5600 6700 2    50   UnSpc ~ 0
AGND
Text HLabel 5600 7000 2    50   UnSpc ~ 0
DGND
$Comp
L Device:C_Small C716
U 1 1 5AF1A3C9
P 5300 6900
AR Path="/5AE96312/5AF1A3C9" Ref="C716"  Part="1" 
AR Path="/5AE9ECEB/5AF1A3C9" Ref="C416"  Part="1" 
AR Path="/5AE958E5/5AF1A3C9" Ref="C616"  Part="1" 
AR Path="/5AE96ED4/5AF1A3C9" Ref="C816"  Part="1" 
AR Path="/5AFE7089/5AF1A3C9" Ref="C1016"  Part="1" 
F 0 "C1016" H 5392 6946 50  0000 L CNN
F 1 "10uF" H 5392 6855 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5300 6900 50  0001 C CNN
F 3 "~" H 5300 6900 50  0001 C CNN
	1    5300 6900
	1    0    0    -1  
$EndComp
Wire Wire Line
	1600 2900 2200 2900
Wire Wire Line
	1600 3200 2200 3200
Wire Wire Line
	1600 3800 2200 3800
Wire Wire Line
	1600 4100 2200 4100
Wire Wire Line
	2200 4400 1600 4400
Wire Wire Line
	1600 4700 2200 4700
Wire Wire Line
	2200 5000 1600 5000
Wire Wire Line
	1600 5300 2200 5300
Wire Wire Line
	2200 5600 1600 5600
Wire Wire Line
	1600 5900 2200 5900
Wire Wire Line
	2200 6200 1600 6200
Wire Wire Line
	1600 6500 2200 6500
Wire Wire Line
	2200 6800 1600 6800
$Comp
L Device:C_Small C711
U 1 1 5AF62C30
P 4700 6000
AR Path="/5AE96312/5AF62C30" Ref="C711"  Part="1" 
AR Path="/5AE9ECEB/5AF62C30" Ref="C411"  Part="1" 
AR Path="/5AE958E5/5AF62C30" Ref="C611"  Part="1" 
AR Path="/5AE96ED4/5AF62C30" Ref="C811"  Part="1" 
AR Path="/5AFE7089/5AF62C30" Ref="C1011"  Part="1" 
F 0 "C1011" H 4792 6046 50  0000 L CNN
F 1 "0.1uF,X8R" H 4792 5955 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 6000 50  0001 C CNN
F 3 "~" H 4700 6000 50  0001 C CNN
	1    4700 6000
	1    0    0    -1  
$EndComp
Text HLabel 5300 6100 2    50   UnSpc ~ 0
AGND
$Comp
L Device:C_Small C710
U 1 1 5AE39B5F
P 4100 1500
AR Path="/5AE96312/5AE39B5F" Ref="C710"  Part="1" 
AR Path="/5AE9ECEB/5AE39B5F" Ref="C410"  Part="1" 
AR Path="/5AE958E5/5AE39B5F" Ref="C610"  Part="1" 
AR Path="/5AE96ED4/5AE39B5F" Ref="C810"  Part="1" 
AR Path="/5AFE7089/5AE39B5F" Ref="C1010"  Part="1" 
F 0 "C1010" V 4000 1500 50  0000 C CNN
F 1 "0.1uF" V 4200 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4100 1500 50  0001 C CNN
F 3 "~" H 4100 1500 50  0001 C CNN
	1    4100 1500
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C709
U 1 1 5AE424D3
P 4100 1100
AR Path="/5AE96312/5AE424D3" Ref="C709"  Part="1" 
AR Path="/5AE9ECEB/5AE424D3" Ref="C409"  Part="1" 
AR Path="/5AE958E5/5AE424D3" Ref="C609"  Part="1" 
AR Path="/5AE96ED4/5AE424D3" Ref="C809"  Part="1" 
AR Path="/5AFE7089/5AE424D3" Ref="C1009"  Part="1" 
F 0 "C1009" V 4000 1100 50  0000 C CNN
F 1 "10uF" V 4200 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4100 1100 50  0001 C CNN
F 3 "~" H 4100 1100 50  0001 C CNN
	1    4100 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C701
U 1 1 5AE748DC
P 2500 1100
AR Path="/5AE96312/5AE748DC" Ref="C701"  Part="1" 
AR Path="/5AE9ECEB/5AE748DC" Ref="C401"  Part="1" 
AR Path="/5AE958E5/5AE748DC" Ref="C601"  Part="1" 
AR Path="/5AE96ED4/5AE748DC" Ref="C801"  Part="1" 
AR Path="/5AFE7089/5AE748DC" Ref="C1001"  Part="1" 
F 0 "C1001" V 2400 1100 50  0000 C CNN
F 1 "10uF" V 2600 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2500 1100 50  0001 C CNN
F 3 "~" H 2500 1100 50  0001 C CNN
	1    2500 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C702
U 1 1 5AE748E2
P 2500 1500
AR Path="/5AE96312/5AE748E2" Ref="C702"  Part="1" 
AR Path="/5AE9ECEB/5AE748E2" Ref="C402"  Part="1" 
AR Path="/5AE958E5/5AE748E2" Ref="C602"  Part="1" 
AR Path="/5AE96ED4/5AE748E2" Ref="C802"  Part="1" 
AR Path="/5AFE7089/5AE748E2" Ref="C1002"  Part="1" 
F 0 "C1002" V 2400 1500 50  0000 C CNN
F 1 "0.1uF" V 2600 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2500 1500 50  0001 C CNN
F 3 "~" H 2500 1500 50  0001 C CNN
	1    2500 1500
	0    -1   -1   0   
$EndComp
Wire Wire Line
	2600 900  2600 1100
Wire Wire Line
	2100 800  2400 800 
Wire Wire Line
	2400 800  2400 1100
$Comp
L Device:C_Small C703
U 1 1 5AE9D928
P 2900 1100
AR Path="/5AE96312/5AE9D928" Ref="C703"  Part="1" 
AR Path="/5AE9ECEB/5AE9D928" Ref="C403"  Part="1" 
AR Path="/5AE958E5/5AE9D928" Ref="C603"  Part="1" 
AR Path="/5AE96ED4/5AE9D928" Ref="C803"  Part="1" 
AR Path="/5AFE7089/5AE9D928" Ref="C1003"  Part="1" 
F 0 "C1003" V 2800 1100 50  0000 C CNN
F 1 "10uF" V 3000 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2900 1100 50  0001 C CNN
F 3 "~" H 2900 1100 50  0001 C CNN
	1    2900 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C704
U 1 1 5AE9D92E
P 2900 1500
AR Path="/5AE96312/5AE9D92E" Ref="C704"  Part="1" 
AR Path="/5AE9ECEB/5AE9D92E" Ref="C404"  Part="1" 
AR Path="/5AE958E5/5AE9D92E" Ref="C604"  Part="1" 
AR Path="/5AE96ED4/5AE9D92E" Ref="C804"  Part="1" 
AR Path="/5AFE7089/5AE9D92E" Ref="C1004"  Part="1" 
F 0 "C1004" V 2800 1500 50  0000 C CNN
F 1 "0.1uF" V 3000 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2900 1500 50  0001 C CNN
F 3 "~" H 2900 1500 50  0001 C CNN
	1    2900 1500
	0    1    1    0   
$EndComp
Wire Wire Line
	3000 900  3000 1100
Connection ~ 2800 1500
Wire Wire Line
	2800 800  2800 1100
$Comp
L Device:C_Small C705
U 1 1 5AEA3FF2
P 3300 1100
AR Path="/5AE96312/5AEA3FF2" Ref="C705"  Part="1" 
AR Path="/5AE9ECEB/5AEA3FF2" Ref="C405"  Part="1" 
AR Path="/5AE958E5/5AEA3FF2" Ref="C605"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF2" Ref="C805"  Part="1" 
AR Path="/5AFE7089/5AEA3FF2" Ref="C1005"  Part="1" 
F 0 "C1005" V 3200 1100 50  0000 C CNN
F 1 "10uF" V 3400 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3300 1100 50  0001 C CNN
F 3 "~" H 3300 1100 50  0001 C CNN
	1    3300 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C706
U 1 1 5AEA3FF8
P 3300 1500
AR Path="/5AE96312/5AEA3FF8" Ref="C706"  Part="1" 
AR Path="/5AE9ECEB/5AEA3FF8" Ref="C406"  Part="1" 
AR Path="/5AE958E5/5AEA3FF8" Ref="C606"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF8" Ref="C806"  Part="1" 
AR Path="/5AFE7089/5AEA3FF8" Ref="C1006"  Part="1" 
F 0 "C1006" V 3200 1500 50  0000 C CNN
F 1 "0.1uF" V 3400 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3300 1500 50  0001 C CNN
F 3 "~" H 3300 1500 50  0001 C CNN
	1    3300 1500
	0    1    1    0   
$EndComp
Wire Wire Line
	3400 900  3400 1100
Wire Wire Line
	3200 800  3200 1100
$Comp
L Device:C_Small C707
U 1 1 5AEB23BE
P 3700 1100
AR Path="/5AE96312/5AEB23BE" Ref="C707"  Part="1" 
AR Path="/5AE9ECEB/5AEB23BE" Ref="C407"  Part="1" 
AR Path="/5AE958E5/5AEB23BE" Ref="C607"  Part="1" 
AR Path="/5AE96ED4/5AEB23BE" Ref="C807"  Part="1" 
AR Path="/5AFE7089/5AEB23BE" Ref="C1007"  Part="1" 
F 0 "C1007" V 3600 1100 50  0000 C CNN
F 1 "10uF" V 3800 1100 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3700 1100 50  0001 C CNN
F 3 "~" H 3700 1100 50  0001 C CNN
	1    3700 1100
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C708
U 1 1 5AEB23C4
P 3700 1500
AR Path="/5AE96312/5AEB23C4" Ref="C708"  Part="1" 
AR Path="/5AE9ECEB/5AEB23C4" Ref="C408"  Part="1" 
AR Path="/5AE958E5/5AEB23C4" Ref="C608"  Part="1" 
AR Path="/5AE96ED4/5AEB23C4" Ref="C808"  Part="1" 
AR Path="/5AFE7089/5AEB23C4" Ref="C1008"  Part="1" 
F 0 "C1008" V 3600 1500 50  0000 C CNN
F 1 "0.1uF" V 3800 1500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3700 1500 50  0001 C CNN
F 3 "~" H 3700 1500 50  0001 C CNN
	1    3700 1500
	0    1    1    0   
$EndComp
Wire Wire Line
	3800 900  3800 1100
Connection ~ 2400 800 
Connection ~ 2800 800 
Wire Wire Line
	2800 800  3200 800 
Connection ~ 3200 800 
Connection ~ 3000 900 
Wire Wire Line
	3000 900  3400 900 
Connection ~ 3400 900 
Wire Wire Line
	2800 1500 2800 1800
$Comp
L Device:C_Small C715
U 1 1 5AF66660
P 5300 6600
AR Path="/5AE96312/5AF66660" Ref="C715"  Part="1" 
AR Path="/5AE9ECEB/5AF66660" Ref="C415"  Part="1" 
AR Path="/5AE958E5/5AF66660" Ref="C615"  Part="1" 
AR Path="/5AE96ED4/5AF66660" Ref="C815"  Part="1" 
AR Path="/5AFE7089/5AF66660" Ref="C1015"  Part="1" 
F 0 "C1015" H 5392 6646 50  0000 L CNN
F 1 "10uF" H 5392 6555 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5300 6600 50  0001 C CNN
F 3 "~" H 5300 6600 50  0001 C CNN
	1    5300 6600
	1    0    0    -1  
$EndComp
$Comp
L Device:C_Small C714
U 1 1 5AFC3D5B
P 4700 6900
AR Path="/5AE96312/5AFC3D5B" Ref="C714"  Part="1" 
AR Path="/5AE9ECEB/5AFC3D5B" Ref="C414"  Part="1" 
AR Path="/5AE958E5/5AFC3D5B" Ref="C614"  Part="1" 
AR Path="/5AE96ED4/5AFC3D5B" Ref="C814"  Part="1" 
AR Path="/5AFE7089/5AFC3D5B" Ref="C1014"  Part="1" 
F 0 "C1014" H 4792 6946 50  0000 L CNN
F 1 "0.1uF" H 4792 6855 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 6900 50  0001 C CNN
F 3 "~" H 4700 6900 50  0001 C CNN
	1    4700 6900
	1    0    0    -1  
$EndComp
Wire Wire Line
	3600 1100 3600 800 
$Comp
L Venus-256-rescue:Test_Point-Connector_Specialized TP717
U 1 1 5AE6BFCE
P 5300 5900
AR Path="/5AE96312/5AE6BFCE" Ref="TP717"  Part="1" 
AR Path="/5AE9ECEB/5AE6BFCE" Ref="TP417"  Part="1" 
AR Path="/5AE958E5/5AE6BFCE" Ref="TP617"  Part="1" 
AR Path="/5AE96ED4/5AE6BFCE" Ref="TP817"  Part="1" 
AR Path="/5AFE7089/5AE6BFCE" Ref="TP1017"  Part="1" 
F 0 "TP1017" V 5254 6087 50  0000 L CNN
F 1 "ADC_REF_OUT" V 5345 6087 50  0000 L CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 5500 5900 50  0001 C CNN
F 3 "~" H 5500 5900 50  0001 C CNN
	1    5300 5900
	0    1    1    0   
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP701
U 1 1 5AECD270
P 1600 2300
AR Path="/5AE96312/5AECD270" Ref="TP701"  Part="1" 
AR Path="/5AE9ECEB/5AECD270" Ref="TP401"  Part="1" 
AR Path="/5AE958E5/5AECD270" Ref="TP601"  Part="1" 
AR Path="/5AE96ED4/5AECD270" Ref="TP801"  Part="1" 
AR Path="/5AFE7089/5AECD270" Ref="TP1001"  Part="1" 
F 0 "TP1001" H 1600 2400 50  0000 R CNN
F 1 "S01" H 1800 2375 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 2300 50  0001 C CNN
F 3 "~" H 1800 2300 50  0001 C CNN
	1    1600 2300
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP702
U 1 1 5AECE3AF
P 1600 2600
AR Path="/5AE96312/5AECE3AF" Ref="TP702"  Part="1" 
AR Path="/5AE9ECEB/5AECE3AF" Ref="TP402"  Part="1" 
AR Path="/5AE958E5/5AECE3AF" Ref="TP602"  Part="1" 
AR Path="/5AE96ED4/5AECE3AF" Ref="TP802"  Part="1" 
AR Path="/5AFE7089/5AECE3AF" Ref="TP1002"  Part="1" 
F 0 "TP1002" H 1600 2700 50  0000 R CNN
F 1 "S02" H 1800 2675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 2600 50  0001 C CNN
F 3 "~" H 1800 2600 50  0001 C CNN
	1    1600 2600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP703
U 1 1 5AECE3FB
P 1600 2900
AR Path="/5AE96312/5AECE3FB" Ref="TP703"  Part="1" 
AR Path="/5AE9ECEB/5AECE3FB" Ref="TP403"  Part="1" 
AR Path="/5AE958E5/5AECE3FB" Ref="TP603"  Part="1" 
AR Path="/5AE96ED4/5AECE3FB" Ref="TP803"  Part="1" 
AR Path="/5AFE7089/5AECE3FB" Ref="TP1003"  Part="1" 
F 0 "TP1003" H 1600 3000 50  0000 R CNN
F 1 "S03" H 1800 2975 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 2900 50  0001 C CNN
F 3 "~" H 1800 2900 50  0001 C CNN
	1    1600 2900
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP704
U 1 1 5AECE449
P 1600 3200
AR Path="/5AE96312/5AECE449" Ref="TP704"  Part="1" 
AR Path="/5AE9ECEB/5AECE449" Ref="TP404"  Part="1" 
AR Path="/5AE958E5/5AECE449" Ref="TP604"  Part="1" 
AR Path="/5AE96ED4/5AECE449" Ref="TP804"  Part="1" 
AR Path="/5AFE7089/5AECE449" Ref="TP1004"  Part="1" 
F 0 "TP1004" H 1600 3300 50  0000 R CNN
F 1 "S04" H 1800 3275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 3200 50  0001 C CNN
F 3 "~" H 1800 3200 50  0001 C CNN
	1    1600 3200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP705
U 1 1 5AECE499
P 1600 3500
AR Path="/5AE96312/5AECE499" Ref="TP705"  Part="1" 
AR Path="/5AE9ECEB/5AECE499" Ref="TP405"  Part="1" 
AR Path="/5AE958E5/5AECE499" Ref="TP605"  Part="1" 
AR Path="/5AE96ED4/5AECE499" Ref="TP805"  Part="1" 
AR Path="/5AFE7089/5AECE499" Ref="TP1005"  Part="1" 
F 0 "TP1005" H 1600 3600 50  0000 R CNN
F 1 "S05" H 1800 3575 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 3500 50  0001 C CNN
F 3 "~" H 1800 3500 50  0001 C CNN
	1    1600 3500
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP706
U 1 1 5AECE4E7
P 1600 3800
AR Path="/5AE96312/5AECE4E7" Ref="TP706"  Part="1" 
AR Path="/5AE9ECEB/5AECE4E7" Ref="TP406"  Part="1" 
AR Path="/5AE958E5/5AECE4E7" Ref="TP606"  Part="1" 
AR Path="/5AE96ED4/5AECE4E7" Ref="TP806"  Part="1" 
AR Path="/5AFE7089/5AECE4E7" Ref="TP1006"  Part="1" 
F 0 "TP1006" H 1600 3900 50  0000 R CNN
F 1 "S06" H 1800 3875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 3800 50  0001 C CNN
F 3 "~" H 1800 3800 50  0001 C CNN
	1    1600 3800
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP707
U 1 1 5AECE53B
P 1600 4100
AR Path="/5AE96312/5AECE53B" Ref="TP707"  Part="1" 
AR Path="/5AE9ECEB/5AECE53B" Ref="TP407"  Part="1" 
AR Path="/5AE958E5/5AECE53B" Ref="TP607"  Part="1" 
AR Path="/5AE96ED4/5AECE53B" Ref="TP807"  Part="1" 
AR Path="/5AFE7089/5AECE53B" Ref="TP1007"  Part="1" 
F 0 "TP1007" H 1600 4200 50  0000 R CNN
F 1 "S07" H 1800 4175 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 4100 50  0001 C CNN
F 3 "~" H 1800 4100 50  0001 C CNN
	1    1600 4100
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP708
U 1 1 5AECE591
P 1600 4400
AR Path="/5AE96312/5AECE591" Ref="TP708"  Part="1" 
AR Path="/5AE9ECEB/5AECE591" Ref="TP408"  Part="1" 
AR Path="/5AE958E5/5AECE591" Ref="TP608"  Part="1" 
AR Path="/5AE96ED4/5AECE591" Ref="TP808"  Part="1" 
AR Path="/5AFE7089/5AECE591" Ref="TP1008"  Part="1" 
F 0 "TP1008" H 1600 4500 50  0000 R CNN
F 1 "S08" H 1800 4475 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 4400 50  0001 C CNN
F 3 "~" H 1800 4400 50  0001 C CNN
	1    1600 4400
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP709
U 1 1 5AECE5E5
P 1600 4700
AR Path="/5AE96312/5AECE5E5" Ref="TP709"  Part="1" 
AR Path="/5AE9ECEB/5AECE5E5" Ref="TP409"  Part="1" 
AR Path="/5AE958E5/5AECE5E5" Ref="TP609"  Part="1" 
AR Path="/5AE96ED4/5AECE5E5" Ref="TP809"  Part="1" 
AR Path="/5AFE7089/5AECE5E5" Ref="TP1009"  Part="1" 
F 0 "TP1009" H 1600 4800 50  0000 R CNN
F 1 "S09" H 1800 4775 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 4700 50  0001 C CNN
F 3 "~" H 1800 4700 50  0001 C CNN
	1    1600 4700
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP710
U 1 1 5AECEE9D
P 1600 5000
AR Path="/5AE96312/5AECEE9D" Ref="TP710"  Part="1" 
AR Path="/5AE9ECEB/5AECEE9D" Ref="TP410"  Part="1" 
AR Path="/5AE958E5/5AECEE9D" Ref="TP610"  Part="1" 
AR Path="/5AE96ED4/5AECEE9D" Ref="TP810"  Part="1" 
AR Path="/5AFE7089/5AECEE9D" Ref="TP1010"  Part="1" 
F 0 "TP1010" H 1600 5100 50  0000 R CNN
F 1 "S10" H 1800 5075 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 5000 50  0001 C CNN
F 3 "~" H 1800 5000 50  0001 C CNN
	1    1600 5000
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP711
U 1 1 5AECEEE7
P 1600 5300
AR Path="/5AE96312/5AECEEE7" Ref="TP711"  Part="1" 
AR Path="/5AE9ECEB/5AECEEE7" Ref="TP411"  Part="1" 
AR Path="/5AE958E5/5AECEEE7" Ref="TP611"  Part="1" 
AR Path="/5AE96ED4/5AECEEE7" Ref="TP811"  Part="1" 
AR Path="/5AFE7089/5AECEEE7" Ref="TP1011"  Part="1" 
F 0 "TP1011" H 1600 5400 50  0000 R CNN
F 1 "S11" H 1800 5375 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 5300 50  0001 C CNN
F 3 "~" H 1800 5300 50  0001 C CNN
	1    1600 5300
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP712
U 1 1 5AECEF33
P 1600 5600
AR Path="/5AE96312/5AECEF33" Ref="TP712"  Part="1" 
AR Path="/5AE9ECEB/5AECEF33" Ref="TP412"  Part="1" 
AR Path="/5AE958E5/5AECEF33" Ref="TP612"  Part="1" 
AR Path="/5AE96ED4/5AECEF33" Ref="TP812"  Part="1" 
AR Path="/5AFE7089/5AECEF33" Ref="TP1012"  Part="1" 
F 0 "TP1012" H 1600 5700 50  0000 R CNN
F 1 "S12" H 1800 5675 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 5600 50  0001 C CNN
F 3 "~" H 1800 5600 50  0001 C CNN
	1    1600 5600
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP713
U 1 1 5AECEF81
P 1600 5900
AR Path="/5AE96312/5AECEF81" Ref="TP713"  Part="1" 
AR Path="/5AE9ECEB/5AECEF81" Ref="TP413"  Part="1" 
AR Path="/5AE958E5/5AECEF81" Ref="TP613"  Part="1" 
AR Path="/5AE96ED4/5AECEF81" Ref="TP813"  Part="1" 
AR Path="/5AFE7089/5AECEF81" Ref="TP1013"  Part="1" 
F 0 "TP1013" H 1600 6000 50  0000 R CNN
F 1 "S13" H 1800 5975 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 5900 50  0001 C CNN
F 3 "~" H 1800 5900 50  0001 C CNN
	1    1600 5900
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP714
U 1 1 5AECEFD1
P 1600 6200
AR Path="/5AE96312/5AECEFD1" Ref="TP714"  Part="1" 
AR Path="/5AE9ECEB/5AECEFD1" Ref="TP414"  Part="1" 
AR Path="/5AE958E5/5AECEFD1" Ref="TP614"  Part="1" 
AR Path="/5AE96ED4/5AECEFD1" Ref="TP814"  Part="1" 
AR Path="/5AFE7089/5AECEFD1" Ref="TP1014"  Part="1" 
F 0 "TP1014" H 1600 6300 50  0000 R CNN
F 1 "S14" H 1800 6275 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 6200 50  0001 C CNN
F 3 "~" H 1800 6200 50  0001 C CNN
	1    1600 6200
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP715
U 1 1 5AECF023
P 1600 6500
AR Path="/5AE96312/5AECF023" Ref="TP715"  Part="1" 
AR Path="/5AE9ECEB/5AECF023" Ref="TP415"  Part="1" 
AR Path="/5AE958E5/5AECF023" Ref="TP615"  Part="1" 
AR Path="/5AE96ED4/5AECF023" Ref="TP815"  Part="1" 
AR Path="/5AFE7089/5AECF023" Ref="TP1015"  Part="1" 
F 0 "TP1015" H 1600 6600 50  0000 R CNN
F 1 "S15" H 1800 6575 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 6500 50  0001 C CNN
F 3 "~" H 1800 6500 50  0001 C CNN
	1    1600 6500
	-1   0    0    -1  
$EndComp
$Comp
L Venus-256-rescue:Test_Point_Flag-Connector_Specialized TP716
U 1 1 5AECF077
P 1600 6800
AR Path="/5AE96312/5AECF077" Ref="TP716"  Part="1" 
AR Path="/5AE9ECEB/5AECF077" Ref="TP416"  Part="1" 
AR Path="/5AE958E5/5AECF077" Ref="TP616"  Part="1" 
AR Path="/5AE96ED4/5AECF077" Ref="TP816"  Part="1" 
AR Path="/5AFE7089/5AECF077" Ref="TP1016"  Part="1" 
F 0 "TP1016" H 1600 6900 50  0000 R CNN
F 1 "S16" H 1800 6875 50  0000 R CNN
F 2 "TestPoint:TestPoint_Pad_1.0x1.0mm" H 1800 6800 50  0001 C CNN
F 3 "~" H 1800 6800 50  0001 C CNN
	1    1600 6800
	-1   0    0    -1  
$EndComp
Wire Wire Line
	2200 3500 1600 3500
Wire Wire Line
	4400 6200 4700 6200
Wire Wire Line
	4400 6100 4700 6100
Wire Wire Line
	4400 5900 4700 5900
Wire Wire Line
	4400 6400 4700 6400
$Comp
L Device:C_Small C713
U 1 1 5AF0F415
P 4700 6600
AR Path="/5AE96312/5AF0F415" Ref="C713"  Part="1" 
AR Path="/5AE9ECEB/5AF0F415" Ref="C413"  Part="1" 
AR Path="/5AE958E5/5AF0F415" Ref="C613"  Part="1" 
AR Path="/5AE96ED4/5AF0F415" Ref="C813"  Part="1" 
AR Path="/5AFE7089/5AF0F415" Ref="C1013"  Part="1" 
F 0 "C1013" H 4792 6646 50  0000 L CNN
F 1 "0.1uF" H 4792 6555 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 6600 50  0001 C CNN
F 3 "~" H 4700 6600 50  0001 C CNN
	1    4700 6600
	1    0    0    -1  
$EndComp
Connection ~ 4700 5900
Connection ~ 4700 6100
Wire Wire Line
	4700 5900 5300 5900
Wire Wire Line
	4700 6100 5300 6100
Connection ~ 4700 6400
Wire Wire Line
	4700 6400 5300 6400
Wire Wire Line
	4400 6500 4700 6500
Connection ~ 4700 6500
Wire Wire Line
	4700 6500 5300 6500
Wire Wire Line
	4400 6700 4700 6700
Connection ~ 4700 6700
Wire Wire Line
	5300 6700 5600 6700
Wire Wire Line
	4400 6800 4700 6800
Connection ~ 4700 6800
Wire Wire Line
	4700 6800 5300 6800
Wire Wire Line
	4400 7000 4700 7000
Connection ~ 4700 7000
Wire Wire Line
	5300 7000 5600 7000
Connection ~ 5300 6700
Wire Wire Line
	4700 6700 5300 6700
Connection ~ 5300 7000
Wire Wire Line
	4700 7000 5300 7000
Connection ~ 2100 6900
Text HLabel 3200 7500 3    50   Input ~ 0
AGND
Wire Wire Line
	3200 7500 3200 7200
Text HLabel 3400 7500 3    50   Input ~ 0
DGND
Wire Wire Line
	3400 7500 3400 7200
Wire Wire Line
	2900 1900 2900 2000
Wire Wire Line
	2400 1500 2400 1900
Wire Wire Line
	2400 1900 2900 1900
Connection ~ 2400 1500
Wire Wire Line
	3000 1800 3000 2000
Wire Wire Line
	2800 1800 3000 1800
Wire Wire Line
	3200 1500 3200 1700
Wire Wire Line
	3200 1700 3100 1700
Wire Wire Line
	3100 1700 3100 2000
Connection ~ 3200 1500
Wire Wire Line
	3200 800  3600 800 
Wire Wire Line
	3400 900  3800 900 
Wire Wire Line
	2400 800  2800 800 
Wire Wire Line
	2600 900  3000 900 
Connection ~ 2400 1100
Connection ~ 2600 1100
Wire Wire Line
	2400 1100 2400 1500
Wire Wire Line
	2600 1100 2600 1500
Connection ~ 2800 1100
Connection ~ 3000 1100
Wire Wire Line
	2800 1100 2800 1500
Wire Wire Line
	3000 1100 3000 1500
Connection ~ 3200 1100
Wire Wire Line
	3200 1100 3200 1500
Wire Wire Line
	3400 1100 3400 1500
Connection ~ 3400 1100
Connection ~ 3600 1100
Connection ~ 3800 1100
Wire Wire Line
	3600 1100 3600 1500
Wire Wire Line
	3800 1100 3800 1500
Wire Wire Line
	4200 1500 4200 1100
Wire Wire Line
	4200 900  4500 900 
Connection ~ 4200 1100
Wire Wire Line
	4200 1100 4200 900 
Wire Wire Line
	3400 2000 3400 1900
Wire Wire Line
	3400 1900 4000 1900
Wire Wire Line
	4000 1900 4000 1500
Connection ~ 4000 1100
Wire Wire Line
	4000 1100 4000 800 
Connection ~ 4000 1500
Wire Wire Line
	4000 1500 4000 1100
Wire Wire Line
	3200 2000 3200 1800
Wire Wire Line
	3200 1800 3600 1800
Wire Wire Line
	3600 1800 3600 1500
Connection ~ 3600 1500
Wire Wire Line
	4000 800  4500 800 
Wire Wire Line
	2600 900  2100 900 
Connection ~ 2600 900 
$EndSCHEMATC
