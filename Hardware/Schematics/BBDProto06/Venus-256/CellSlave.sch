EESchema Schematic File Version 4
LIBS:Venus-256-cache
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 6 9
Title "Cell Slave"
Date "2018-04-26"
Rev "A"
Comp "Bio Balance Detector"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
Wire Wire Line
	4400 2500 4700 2500
$Comp
L Device:C_Small C11
U 1 1 5AE9F047
P 4700 1900
AR Path="/5AE9ECEB/5AE9F047" Ref="C11"  Part="1" 
AR Path="/5AE958E5/5AE9F047" Ref="C43"  Part="1" 
AR Path="/5AE96312/5AE9F047" Ref="C59"  Part="1" 
AR Path="/5AE96ED4/5AE9F047" Ref="C75"  Part="1" 
F 0 "C11" H 4792 1946 50  0000 L CNN
F 1 "10uF,X5R" H 4792 1855 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 1900 50  0001 C CNN
F 3 "~" H 4700 1900 50  0001 C CNN
	1    4700 1900
	1    0    0    -1  
$EndComp
Wire Wire Line
	4400 2000 4500 2000
Wire Wire Line
	4500 2000 4500 1700
Wire Wire Line
	4500 1700 4700 1700
Wire Wire Line
	4700 1700 4700 1800
Wire Wire Line
	4400 2100 4700 2100
Text HLabel 4700 3000 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3100 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 3000 4700 3000
Wire Wire Line
	4400 3100 4700 3100
Text HLabel 4700 6000 2    50   Output ~ 0
BUSY
Text HLabel 4700 5400 2    50   Input ~ 0
~CS
Text HLabel 4700 3300 2    50   Input ~ 0
VDRIVE_3V3
Wire Wire Line
	4400 3300 4700 3300
Wire Wire Line
	4400 3900 4700 3900
Text HLabel 4700 4500 2    50   Input ~ 0
SDI
Text HLabel 4700 4700 2    50   Output ~ 0
SDOA
Wire Wire Line
	4400 4500 4700 4500
Wire Wire Line
	4400 4600 4700 4600
Wire Wire Line
	4400 4700 4700 4700
Wire Wire Line
	4400 5400 4700 5400
Wire Wire Line
	4400 6000 4700 6000
Text HLabel 4700 5300 2    50   Input ~ 0
SCLK
Wire Wire Line
	4400 5300 4700 5300
Text HLabel 4600 700  2    50   UnSpc ~ 0
AGND
Text HLabel 3700 7700 2    50   UnSpc ~ 0
DGND
Text HLabel 3100 7700 0    50   Input ~ 0
VDRIVE_3V3
Text HLabel 1900 600  0    50   Input ~ 0
VCC_5V0
$Comp
L Connector_Generic:Conn_01x01 J3
U 1 1 5AEA60FF
P 1700 2000
AR Path="/5AE9ECEB/5AEA60FF" Ref="J3"  Part="1" 
AR Path="/5AE958E5/5AEA60FF" Ref="J23"  Part="1" 
AR Path="/5AE96312/5AEA60FF" Ref="J40"  Part="1" 
AR Path="/5AE96ED4/5AEA60FF" Ref="J57"  Part="1" 
F 0 "J3" H 1620 1775 50  0000 C CNN
F 1 "S01" H 1620 1866 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 2000 50  0001 C CNN
F 3 "~" H 1700 2000 50  0001 C CNN
	1    1700 2000
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J4
U 1 1 5AEA6172
P 1700 2300
AR Path="/5AE9ECEB/5AEA6172" Ref="J4"  Part="1" 
AR Path="/5AE958E5/5AEA6172" Ref="J24"  Part="1" 
AR Path="/5AE96312/5AEA6172" Ref="J41"  Part="1" 
AR Path="/5AE96ED4/5AEA6172" Ref="J58"  Part="1" 
F 0 "J4" H 1620 2075 50  0000 C CNN
F 1 "S02" H 1620 2166 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 2300 50  0001 C CNN
F 3 "~" H 1700 2300 50  0001 C CNN
	1    1700 2300
	-1   0    0    1   
$EndComp
Wire Wire Line
	1900 2000 2200 2000
Wire Wire Line
	1900 2300 2200 2300
Text HLabel 2100 7200 3    50   Input ~ 0
SGND
Wire Wire Line
	2100 7200 2100 6600
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
Wire Wire Line
	2100 2400 2100 2100
Wire Wire Line
	2100 2100 2200 2100
Connection ~ 2100 2400
Text HLabel 4700 4800 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 4900 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 5000 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 4800 4700 4800
Wire Wire Line
	4400 4900 4700 4900
Wire Wire Line
	4400 5000 4700 5000
Text HLabel 4700 5200 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 5200 4700 5200
Text HLabel 4700 5600 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 5700 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 5800 2    50   UnSpc ~ 0
DGND
$Comp
L Analog_ADC:AD7616 U2
U 1 1 5AE20D71
P 3300 4300
AR Path="/5AE9ECEB/5AE20D71" Ref="U2"  Part="1" 
AR Path="/5AE958E5/5AE20D71" Ref="U4"  Part="1" 
AR Path="/5AE96312/5AE20D71" Ref="U5"  Part="1" 
AR Path="/5AE96ED4/5AE20D71" Ref="U6"  Part="1" 
F 0 "U2" H 2500 6800 50  0000 C CNN
F 1 "AD7616" H 2600 1800 50  0000 C CNN
F 2 "Package_QFP:LQFP-80_14x14mm_P0.65mm" H 3300 4300 50  0001 C CIN
F 3 "http://www.analog.com/media/en/technical-documentation/data-sheets/AD7616.pdf" H 3400 1300 50  0001 C CNN
	1    3300 4300
	1    0    0    -1  
$EndComp
Wire Wire Line
	4400 5600 4700 5600
Wire Wire Line
	4400 5700 4700 5700
Wire Wire Line
	4400 5800 4700 5800
Text HLabel 4700 2500 2    50   Input ~ 0
VDRIVE_3V3
Text HLabel 5000 2100 2    50   UnSpc ~ 0
AGND
Wire Wire Line
	4700 2100 5000 2100
Connection ~ 4700 2100
Wire Wire Line
	4700 2000 4700 2100
Text HLabel 4700 2800 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 2800 4700 2800
Text HLabel 4700 2700 2    50   Input ~ 0
~RESET
Wire Wire Line
	4400 2700 4700 2700
Text HLabel 4700 3500 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3600 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3700 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 3800 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 3500 4700 3500
Wire Wire Line
	4400 3600 4700 3600
Wire Wire Line
	4400 3700 4700 3700
Wire Wire Line
	4400 3800 4700 3800
Text HLabel 4700 3900 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 4000 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 4000 4700 4000
Text HLabel 4700 4100 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 4200 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 4100 4700 4100
Wire Wire Line
	4400 4200 4700 4200
Text HLabel 4700 4300 2    50   UnSpc ~ 0
DGND
Text HLabel 4700 4400 2    50   UnSpc ~ 0
DGND
Wire Wire Line
	4400 4300 4700 4300
Wire Wire Line
	4400 4400 4700 4400
NoConn ~ 4700 4600
Text HLabel 4700 6100 2    50   Input ~ 0
CONVST
Wire Wire Line
	4400 6100 4700 6100
Text HLabel 5400 6500 2    50   UnSpc ~ 0
AGND
Wire Wire Line
	4400 6500 4700 6500
$Comp
L Device:C_Small C12
U 1 1 5AF0F415
P 4700 6700
AR Path="/5AE9ECEB/5AF0F415" Ref="C12"  Part="1" 
AR Path="/5AE958E5/5AF0F415" Ref="C44"  Part="1" 
AR Path="/5AE96312/5AF0F415" Ref="C60"  Part="1" 
AR Path="/5AE96ED4/5AF0F415" Ref="C76"  Part="1" 
F 0 "C12" H 4792 6746 50  0000 L CNN
F 1 "0.1uF" H 4792 6655 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4700 6700 50  0001 C CNN
F 3 "~" H 4700 6700 50  0001 C CNN
	1    4700 6700
	1    0    0    -1  
$EndComp
Wire Wire Line
	4700 6500 4700 6600
Connection ~ 4700 6500
Wire Wire Line
	4700 6500 5100 6500
Wire Wire Line
	4400 6600 4500 6600
Wire Wire Line
	4500 6600 4500 6900
Wire Wire Line
	4500 6900 4700 6900
Wire Wire Line
	4700 6900 4700 6800
Text HLabel 6700 6300 2    50   UnSpc ~ 0
DGND
$Comp
L Device:C_Small C16
U 1 1 5AF1A3C9
P 6400 6500
AR Path="/5AE9ECEB/5AF1A3C9" Ref="C16"  Part="1" 
AR Path="/5AE958E5/5AF1A3C9" Ref="C48"  Part="1" 
AR Path="/5AE96312/5AF1A3C9" Ref="C64"  Part="1" 
AR Path="/5AE96ED4/5AF1A3C9" Ref="C80"  Part="1" 
F 0 "C16" H 6492 6546 50  0000 L CNN
F 1 "10uF" H 6492 6455 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 6400 6500 50  0001 C CNN
F 3 "~" H 6400 6500 50  0001 C CNN
	1    6400 6500
	1    0    0    -1  
$EndComp
Wire Wire Line
	6400 6300 6400 6400
Connection ~ 6400 6300
Wire Wire Line
	6400 6300 6700 6300
Wire Wire Line
	5800 6400 5800 6700
Wire Wire Line
	5800 6700 6000 6700
Wire Wire Line
	6400 6700 6400 6600
Wire Wire Line
	4400 6300 6000 6300
Wire Wire Line
	4400 6400 5800 6400
$Comp
L Connector_Generic:Conn_01x01 J5
U 1 1 5AF23C74
P 1700 2600
AR Path="/5AE9ECEB/5AF23C74" Ref="J5"  Part="1" 
AR Path="/5AE958E5/5AF23C74" Ref="J25"  Part="1" 
AR Path="/5AE96312/5AF23C74" Ref="J42"  Part="1" 
AR Path="/5AE96ED4/5AF23C74" Ref="J59"  Part="1" 
F 0 "J5" H 1620 2375 50  0000 C CNN
F 1 "S03" H 1620 2466 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 2600 50  0001 C CNN
F 3 "~" H 1700 2600 50  0001 C CNN
	1    1700 2600
	-1   0    0    1   
$EndComp
Wire Wire Line
	1900 2600 2200 2600
$Comp
L Connector_Generic:Conn_01x01 J6
U 1 1 5AF2A3B4
P 1700 2900
AR Path="/5AE9ECEB/5AF2A3B4" Ref="J6"  Part="1" 
AR Path="/5AE958E5/5AF2A3B4" Ref="J26"  Part="1" 
AR Path="/5AE96312/5AF2A3B4" Ref="J43"  Part="1" 
AR Path="/5AE96ED4/5AF2A3B4" Ref="J60"  Part="1" 
F 0 "J6" H 1620 2675 50  0000 C CNN
F 1 "S04" H 1620 2766 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 2900 50  0001 C CNN
F 3 "~" H 1700 2900 50  0001 C CNN
	1    1700 2900
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J7
U 1 1 5AF2A3D6
P 1700 3200
AR Path="/5AE9ECEB/5AF2A3D6" Ref="J7"  Part="1" 
AR Path="/5AE958E5/5AF2A3D6" Ref="J27"  Part="1" 
AR Path="/5AE96312/5AF2A3D6" Ref="J44"  Part="1" 
AR Path="/5AE96ED4/5AF2A3D6" Ref="J61"  Part="1" 
F 0 "J7" H 1620 2975 50  0000 C CNN
F 1 "S05" H 1620 3066 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 3200 50  0001 C CNN
F 3 "~" H 1700 3200 50  0001 C CNN
	1    1700 3200
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J8
U 1 1 5AF2A3FA
P 1700 3500
AR Path="/5AE9ECEB/5AF2A3FA" Ref="J8"  Part="1" 
AR Path="/5AE958E5/5AF2A3FA" Ref="J28"  Part="1" 
AR Path="/5AE96312/5AF2A3FA" Ref="J45"  Part="1" 
AR Path="/5AE96ED4/5AF2A3FA" Ref="J62"  Part="1" 
F 0 "J8" H 1620 3275 50  0000 C CNN
F 1 "S06" H 1620 3366 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 3500 50  0001 C CNN
F 3 "~" H 1700 3500 50  0001 C CNN
	1    1700 3500
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J9
U 1 1 5AF2A420
P 1700 3800
AR Path="/5AE9ECEB/5AF2A420" Ref="J9"  Part="1" 
AR Path="/5AE958E5/5AF2A420" Ref="J29"  Part="1" 
AR Path="/5AE96312/5AF2A420" Ref="J46"  Part="1" 
AR Path="/5AE96ED4/5AF2A420" Ref="J63"  Part="1" 
F 0 "J9" H 1620 3575 50  0000 C CNN
F 1 "S07" H 1620 3666 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 3800 50  0001 C CNN
F 3 "~" H 1700 3800 50  0001 C CNN
	1    1700 3800
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J10
U 1 1 5AF2A483
P 1700 4100
AR Path="/5AE9ECEB/5AF2A483" Ref="J10"  Part="1" 
AR Path="/5AE958E5/5AF2A483" Ref="J30"  Part="1" 
AR Path="/5AE96312/5AF2A483" Ref="J47"  Part="1" 
AR Path="/5AE96ED4/5AF2A483" Ref="J64"  Part="1" 
F 0 "J10" H 1620 3875 50  0000 C CNN
F 1 "S08" H 1620 3966 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 4100 50  0001 C CNN
F 3 "~" H 1700 4100 50  0001 C CNN
	1    1700 4100
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J11
U 1 1 5AF2A4AD
P 1700 4400
AR Path="/5AE9ECEB/5AF2A4AD" Ref="J11"  Part="1" 
AR Path="/5AE958E5/5AF2A4AD" Ref="J31"  Part="1" 
AR Path="/5AE96312/5AF2A4AD" Ref="J48"  Part="1" 
AR Path="/5AE96ED4/5AF2A4AD" Ref="J65"  Part="1" 
F 0 "J11" H 1620 4175 50  0000 C CNN
F 1 "S09" H 1620 4266 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 4400 50  0001 C CNN
F 3 "~" H 1700 4400 50  0001 C CNN
	1    1700 4400
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J12
U 1 1 5AF2A4D9
P 1700 4700
AR Path="/5AE9ECEB/5AF2A4D9" Ref="J12"  Part="1" 
AR Path="/5AE958E5/5AF2A4D9" Ref="J32"  Part="1" 
AR Path="/5AE96312/5AF2A4D9" Ref="J49"  Part="1" 
AR Path="/5AE96ED4/5AF2A4D9" Ref="J66"  Part="1" 
F 0 "J12" H 1620 4475 50  0000 C CNN
F 1 "S10" H 1620 4566 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 4700 50  0001 C CNN
F 3 "~" H 1700 4700 50  0001 C CNN
	1    1700 4700
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J13
U 1 1 5AF2A507
P 1700 5000
AR Path="/5AE9ECEB/5AF2A507" Ref="J13"  Part="1" 
AR Path="/5AE958E5/5AF2A507" Ref="J33"  Part="1" 
AR Path="/5AE96312/5AF2A507" Ref="J50"  Part="1" 
AR Path="/5AE96ED4/5AF2A507" Ref="J67"  Part="1" 
F 0 "J13" H 1620 4775 50  0000 C CNN
F 1 "S11" H 1620 4866 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 5000 50  0001 C CNN
F 3 "~" H 1700 5000 50  0001 C CNN
	1    1700 5000
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J14
U 1 1 5AF2A537
P 1700 5300
AR Path="/5AE9ECEB/5AF2A537" Ref="J14"  Part="1" 
AR Path="/5AE958E5/5AF2A537" Ref="J34"  Part="1" 
AR Path="/5AE96312/5AF2A537" Ref="J51"  Part="1" 
AR Path="/5AE96ED4/5AF2A537" Ref="J68"  Part="1" 
F 0 "J14" H 1620 5075 50  0000 C CNN
F 1 "S12" H 1620 5166 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 5300 50  0001 C CNN
F 3 "~" H 1700 5300 50  0001 C CNN
	1    1700 5300
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J15
U 1 1 5AF2A569
P 1700 5600
AR Path="/5AE9ECEB/5AF2A569" Ref="J15"  Part="1" 
AR Path="/5AE958E5/5AF2A569" Ref="J35"  Part="1" 
AR Path="/5AE96312/5AF2A569" Ref="J52"  Part="1" 
AR Path="/5AE96ED4/5AF2A569" Ref="J69"  Part="1" 
F 0 "J15" H 1620 5375 50  0000 C CNN
F 1 "S13" H 1620 5466 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 5600 50  0001 C CNN
F 3 "~" H 1700 5600 50  0001 C CNN
	1    1700 5600
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J16
U 1 1 5AF2A59D
P 1700 5900
AR Path="/5AE9ECEB/5AF2A59D" Ref="J16"  Part="1" 
AR Path="/5AE958E5/5AF2A59D" Ref="J36"  Part="1" 
AR Path="/5AE96312/5AF2A59D" Ref="J53"  Part="1" 
AR Path="/5AE96ED4/5AF2A59D" Ref="J70"  Part="1" 
F 0 "J16" H 1620 5675 50  0000 C CNN
F 1 "S14" H 1620 5766 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 5900 50  0001 C CNN
F 3 "~" H 1700 5900 50  0001 C CNN
	1    1700 5900
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J17
U 1 1 5AF2A5D3
P 1700 6200
AR Path="/5AE9ECEB/5AF2A5D3" Ref="J17"  Part="1" 
AR Path="/5AE958E5/5AF2A5D3" Ref="J37"  Part="1" 
AR Path="/5AE96312/5AF2A5D3" Ref="J54"  Part="1" 
AR Path="/5AE96ED4/5AF2A5D3" Ref="J71"  Part="1" 
F 0 "J17" H 1620 5975 50  0000 C CNN
F 1 "S15" H 1620 6066 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 6200 50  0001 C CNN
F 3 "~" H 1700 6200 50  0001 C CNN
	1    1700 6200
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J18
U 1 1 5AF2A6A3
P 1700 6500
AR Path="/5AE9ECEB/5AF2A6A3" Ref="J18"  Part="1" 
AR Path="/5AE958E5/5AF2A6A3" Ref="J38"  Part="1" 
AR Path="/5AE96312/5AF2A6A3" Ref="J55"  Part="1" 
AR Path="/5AE96ED4/5AF2A6A3" Ref="J72"  Part="1" 
F 0 "J18" H 1620 6275 50  0000 C CNN
F 1 "S16" H 1620 6366 50  0000 C CNN
F 2 "Connector_Wire:SolderWirePad_single_0-8mmDrill" H 1700 6500 50  0001 C CNN
F 3 "~" H 1700 6500 50  0001 C CNN
	1    1700 6500
	-1   0    0    1   
$EndComp
Wire Wire Line
	1900 2900 2200 2900
Wire Wire Line
	2200 3200 1900 3200
Wire Wire Line
	1900 3500 2200 3500
Wire Wire Line
	1900 3800 2200 3800
Wire Wire Line
	2200 4100 1900 4100
Wire Wire Line
	1900 4400 2200 4400
Wire Wire Line
	2200 4700 1900 4700
Wire Wire Line
	1900 5000 2200 5000
Wire Wire Line
	2200 5300 1900 5300
Wire Wire Line
	1900 5600 2200 5600
Wire Wire Line
	2200 5900 1900 5900
Wire Wire Line
	1900 6200 2200 6200
Wire Wire Line
	2200 6500 1900 6500
$Comp
L Device:C_Small C14
U 1 1 5AF62C30
P 5600 2200
AR Path="/5AE9ECEB/5AF62C30" Ref="C14"  Part="1" 
AR Path="/5AE958E5/5AF62C30" Ref="C46"  Part="1" 
AR Path="/5AE96312/5AF62C30" Ref="C62"  Part="1" 
AR Path="/5AE96ED4/5AF62C30" Ref="C78"  Part="1" 
F 0 "C14" H 5692 2246 50  0000 L CNN
F 1 "0.1uF,X8R" H 5692 2155 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5600 2200 50  0001 C CNN
F 3 "~" H 5600 2200 50  0001 C CNN
	1    5600 2200
	1    0    0    -1  
$EndComp
Wire Wire Line
	5400 2300 5400 2000
Wire Wire Line
	5400 2000 5600 2000
Wire Wire Line
	5600 2000 5600 2100
Text HLabel 5900 2400 2    50   UnSpc ~ 0
AGND
Wire Wire Line
	5600 2400 5900 2400
Connection ~ 5600 2400
Wire Wire Line
	5600 2300 5600 2400
Wire Wire Line
	4400 2400 5600 2400
Wire Wire Line
	4400 2300 5400 2300
Wire Wire Line
	5600 2000 5900 2000
Connection ~ 5600 2000
Wire Wire Line
	3500 6900 3500 7000
Wire Wire Line
	3500 7000 3600 7000
$Comp
L Device:C_Small C5
U 1 1 5AE39B5F
P 3400 7200
AR Path="/5AE9ECEB/5AE39B5F" Ref="C5"  Part="1" 
AR Path="/5AE958E5/5AE39B5F" Ref="C37"  Part="1" 
AR Path="/5AE96312/5AE39B5F" Ref="C53"  Part="1" 
AR Path="/5AE96ED4/5AE39B5F" Ref="C69"  Part="1" 
F 0 "C5" V 3300 7200 50  0000 C CNN
F 1 "0.1uF" V 3500 7200 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3400 7200 50  0001 C CNN
F 3 "~" H 3400 7200 50  0001 C CNN
	1    3400 7200
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C6
U 1 1 5AE424D3
P 3400 7500
AR Path="/5AE9ECEB/5AE424D3" Ref="C6"  Part="1" 
AR Path="/5AE958E5/5AE424D3" Ref="C38"  Part="1" 
AR Path="/5AE96312/5AE424D3" Ref="C54"  Part="1" 
AR Path="/5AE96ED4/5AE424D3" Ref="C70"  Part="1" 
F 0 "C6" V 3300 7500 50  0000 C CNN
F 1 "10uF" V 3500 7500 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3400 7500 50  0001 C CNN
F 3 "~" H 3400 7500 50  0001 C CNN
	1    3400 7500
	0    1    1    0   
$EndComp
Wire Wire Line
	3600 7700 3700 7700
Wire Wire Line
	3600 7000 3600 7200
Wire Wire Line
	3400 6900 3400 7000
Wire Wire Line
	3400 7000 3200 7000
Wire Wire Line
	3200 7000 3200 7200
Wire Wire Line
	3200 7700 3100 7700
Wire Wire Line
	3200 7500 3300 7500
Connection ~ 3200 7500
Wire Wire Line
	3200 7500 3200 7700
Wire Wire Line
	3200 7200 3300 7200
Connection ~ 3200 7200
Wire Wire Line
	3200 7200 3200 7500
Wire Wire Line
	3500 7200 3600 7200
Connection ~ 3600 7200
Wire Wire Line
	3600 7200 3600 7500
Wire Wire Line
	3500 7500 3600 7500
Connection ~ 3600 7500
Wire Wire Line
	3600 7500 3600 7700
$Comp
L Device:C_Small C1
U 1 1 5AE748DC
P 2500 900
AR Path="/5AE9ECEB/5AE748DC" Ref="C1"  Part="1" 
AR Path="/5AE958E5/5AE748DC" Ref="C33"  Part="1" 
AR Path="/5AE96312/5AE748DC" Ref="C49"  Part="1" 
AR Path="/5AE96ED4/5AE748DC" Ref="C65"  Part="1" 
F 0 "C1" V 2400 900 50  0000 C CNN
F 1 "10uF" V 2600 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2500 900 50  0001 C CNN
F 3 "~" H 2500 900 50  0001 C CNN
	1    2500 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C2
U 1 1 5AE748E2
P 2500 1200
AR Path="/5AE9ECEB/5AE748E2" Ref="C2"  Part="1" 
AR Path="/5AE958E5/5AE748E2" Ref="C34"  Part="1" 
AR Path="/5AE96312/5AE748E2" Ref="C50"  Part="1" 
AR Path="/5AE96ED4/5AE748E2" Ref="C66"  Part="1" 
F 0 "C2" V 2400 1200 50  0000 C CNN
F 1 "0.1uF" V 2600 1200 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 2500 1200 50  0001 C CNN
F 3 "~" H 2500 1200 50  0001 C CNN
	1    2500 1200
	0    1    1    0   
$EndComp
Wire Wire Line
	2700 700  2700 900 
Wire Wire Line
	1900 600  2300 600 
Wire Wire Line
	2300 1200 2400 1200
Connection ~ 2300 1200
Wire Wire Line
	2300 900  2400 900 
Connection ~ 2300 900 
Wire Wire Line
	2300 900  2300 1200
Wire Wire Line
	2600 900  2700 900 
Connection ~ 2700 900 
Wire Wire Line
	2700 900  2700 1200
Wire Wire Line
	2600 1200 2700 1200
Connection ~ 2700 1200
Wire Wire Line
	2300 600  2300 900 
$Comp
L Device:C_Small C3
U 1 1 5AE9D928
P 3000 900
AR Path="/5AE9ECEB/5AE9D928" Ref="C3"  Part="1" 
AR Path="/5AE958E5/5AE9D928" Ref="C35"  Part="1" 
AR Path="/5AE96312/5AE9D928" Ref="C51"  Part="1" 
AR Path="/5AE96ED4/5AE9D928" Ref="C67"  Part="1" 
F 0 "C3" V 2900 900 50  0000 C CNN
F 1 "10uF" V 3100 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3000 900 50  0001 C CNN
F 3 "~" H 3000 900 50  0001 C CNN
	1    3000 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C4
U 1 1 5AE9D92E
P 3000 1200
AR Path="/5AE9ECEB/5AE9D92E" Ref="C4"  Part="1" 
AR Path="/5AE958E5/5AE9D92E" Ref="C36"  Part="1" 
AR Path="/5AE96312/5AE9D92E" Ref="C52"  Part="1" 
AR Path="/5AE96ED4/5AE9D92E" Ref="C68"  Part="1" 
F 0 "C4" V 2900 1200 50  0000 C CNN
F 1 "0.1uF" V 3100 1200 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3000 1200 50  0001 C CNN
F 3 "~" H 3000 1200 50  0001 C CNN
	1    3000 1200
	0    1    1    0   
$EndComp
Wire Wire Line
	3200 700  3200 900 
Wire Wire Line
	2800 1200 2900 1200
Connection ~ 2800 1200
Wire Wire Line
	2800 900  2900 900 
Connection ~ 2800 900 
Wire Wire Line
	2800 900  2800 1200
Wire Wire Line
	3100 900  3200 900 
Connection ~ 3200 900 
Wire Wire Line
	3200 900  3200 1200
Wire Wire Line
	3100 1200 3200 1200
Connection ~ 3200 1200
Wire Wire Line
	2800 600  2800 900 
$Comp
L Device:C_Small C7
U 1 1 5AEA3FF2
P 3600 900
AR Path="/5AE9ECEB/5AEA3FF2" Ref="C7"  Part="1" 
AR Path="/5AE958E5/5AEA3FF2" Ref="C39"  Part="1" 
AR Path="/5AE96312/5AEA3FF2" Ref="C55"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF2" Ref="C71"  Part="1" 
F 0 "C7" V 3500 900 50  0000 C CNN
F 1 "10uF" V 3700 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3600 900 50  0001 C CNN
F 3 "~" H 3600 900 50  0001 C CNN
	1    3600 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C8
U 1 1 5AEA3FF8
P 3600 1200
AR Path="/5AE9ECEB/5AEA3FF8" Ref="C8"  Part="1" 
AR Path="/5AE958E5/5AEA3FF8" Ref="C40"  Part="1" 
AR Path="/5AE96312/5AEA3FF8" Ref="C56"  Part="1" 
AR Path="/5AE96ED4/5AEA3FF8" Ref="C72"  Part="1" 
F 0 "C8" V 3500 1200 50  0000 C CNN
F 1 "0.1uF" V 3700 1200 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 3600 1200 50  0001 C CNN
F 3 "~" H 3600 1200 50  0001 C CNN
	1    3600 1200
	0    1    1    0   
$EndComp
Wire Wire Line
	3800 700  3800 900 
Wire Wire Line
	3400 1200 3500 1200
Connection ~ 3400 1200
Wire Wire Line
	3400 900  3500 900 
Connection ~ 3400 900 
Wire Wire Line
	3400 900  3400 1200
Wire Wire Line
	3700 900  3800 900 
Connection ~ 3800 900 
Wire Wire Line
	3800 900  3800 1200
Wire Wire Line
	3700 1200 3800 1200
Connection ~ 3800 1200
Wire Wire Line
	3800 1200 3800 1400
Wire Wire Line
	3400 600  3400 900 
$Comp
L Device:C_Small C9
U 1 1 5AEB23BE
P 4100 900
AR Path="/5AE9ECEB/5AEB23BE" Ref="C9"  Part="1" 
AR Path="/5AE958E5/5AEB23BE" Ref="C41"  Part="1" 
AR Path="/5AE96312/5AEB23BE" Ref="C57"  Part="1" 
AR Path="/5AE96ED4/5AEB23BE" Ref="C73"  Part="1" 
F 0 "C9" V 4000 900 50  0000 C CNN
F 1 "10uF" V 4200 900 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4100 900 50  0001 C CNN
F 3 "~" H 4100 900 50  0001 C CNN
	1    4100 900 
	0    1    1    0   
$EndComp
$Comp
L Device:C_Small C10
U 1 1 5AEB23C4
P 4100 1200
AR Path="/5AE9ECEB/5AEB23C4" Ref="C10"  Part="1" 
AR Path="/5AE958E5/5AEB23C4" Ref="C42"  Part="1" 
AR Path="/5AE96312/5AEB23C4" Ref="C58"  Part="1" 
AR Path="/5AE96ED4/5AEB23C4" Ref="C74"  Part="1" 
F 0 "C10" V 4000 1200 50  0000 C CNN
F 1 "0.1uF" V 4200 1200 50  0000 C CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 4100 1200 50  0001 C CNN
F 3 "~" H 4100 1200 50  0001 C CNN
	1    4100 1200
	0    1    1    0   
$EndComp
Wire Wire Line
	4300 700  4300 900 
Wire Wire Line
	3900 1200 4000 1200
Wire Wire Line
	3900 900  4000 900 
Wire Wire Line
	3900 900  3900 1200
Wire Wire Line
	4200 900  4300 900 
Connection ~ 4300 900 
Wire Wire Line
	4300 900  4300 1200
Wire Wire Line
	4200 1200 4300 1200
Connection ~ 4300 1200
Wire Wire Line
	4300 1200 4300 1400
Wire Wire Line
	2300 600  2800 600 
Connection ~ 2300 600 
Connection ~ 2800 600 
Wire Wire Line
	2800 600  3400 600 
Connection ~ 3400 600 
Wire Wire Line
	2700 700  3200 700 
Connection ~ 3200 700 
Wire Wire Line
	3200 700  3800 700 
Connection ~ 3800 700 
Wire Wire Line
	3800 700  4300 700 
Connection ~ 4300 700 
Wire Wire Line
	4300 700  4600 700 
Wire Wire Line
	2300 1200 2300 1600
Wire Wire Line
	2800 1200 2800 1400
Wire Wire Line
	2700 1200 2700 1500
Wire Wire Line
	4300 1400 4000 1400
$Comp
L Device:C_Small C13
U 1 1 5AF66660
P 5100 6700
AR Path="/5AE9ECEB/5AF66660" Ref="C13"  Part="1" 
AR Path="/5AE958E5/5AF66660" Ref="C45"  Part="1" 
AR Path="/5AE96312/5AF66660" Ref="C61"  Part="1" 
AR Path="/5AE96ED4/5AF66660" Ref="C77"  Part="1" 
F 0 "C13" H 5192 6746 50  0000 L CNN
F 1 "10uF" H 5192 6655 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 5100 6700 50  0001 C CNN
F 3 "~" H 5100 6700 50  0001 C CNN
	1    5100 6700
	1    0    0    -1  
$EndComp
Wire Wire Line
	4700 6900 5100 6900
Wire Wire Line
	5100 6900 5100 6800
Connection ~ 4700 6900
Wire Wire Line
	5100 6600 5100 6500
Connection ~ 5100 6500
Wire Wire Line
	5100 6500 5400 6500
$Comp
L Device:C_Small C15
U 1 1 5AFC3D5B
P 6000 6500
AR Path="/5AE9ECEB/5AFC3D5B" Ref="C15"  Part="1" 
AR Path="/5AE958E5/5AFC3D5B" Ref="C47"  Part="1" 
AR Path="/5AE96312/5AFC3D5B" Ref="C63"  Part="1" 
AR Path="/5AE96ED4/5AFC3D5B" Ref="C79"  Part="1" 
F 0 "C15" H 6092 6546 50  0000 L CNN
F 1 "0.1uF" H 6092 6455 50  0000 L CNN
F 2 "Capacitor_SMD:C_0603_1608Metric_Pad0.84x1.00mm_HandSolder" H 6000 6500 50  0001 C CNN
F 3 "~" H 6000 6500 50  0001 C CNN
	1    6000 6500
	1    0    0    -1  
$EndComp
Wire Wire Line
	6000 6700 6000 6600
Connection ~ 6000 6700
Wire Wire Line
	6000 6700 6400 6700
Wire Wire Line
	6000 6400 6000 6300
Connection ~ 6000 6300
Wire Wire Line
	6000 6300 6400 6300
Wire Wire Line
	3400 1200 3400 1700
Wire Wire Line
	3500 1700 3500 1400
Wire Wire Line
	3500 1400 3800 1400
Wire Wire Line
	3900 1200 3900 1500
Wire Wire Line
	3900 1500 3700 1500
Wire Wire Line
	3700 1500 3700 1700
Connection ~ 3900 1200
Wire Wire Line
	4000 1600 4000 1400
Wire Wire Line
	3900 900  3900 600 
Wire Wire Line
	3400 600  3900 600 
Connection ~ 3900 900 
Wire Wire Line
	3800 1700 3800 1600
Wire Wire Line
	3800 1600 4000 1600
Wire Wire Line
	3200 1200 3200 1700
Wire Wire Line
	2800 1400 3100 1400
Wire Wire Line
	3100 1400 3100 1700
Wire Wire Line
	2700 1500 2900 1500
Wire Wire Line
	2900 1500 2900 1700
Wire Wire Line
	2300 1600 2800 1600
Wire Wire Line
	2800 1600 2800 1700
$Comp
L Connector_Specialized:Test_Point TP1
U 1 1 5AE6BFCE
P 5900 2000
AR Path="/5AE9ECEB/5AE6BFCE" Ref="TP1"  Part="1" 
AR Path="/5AE958E5/5AE6BFCE" Ref="TP5"  Part="1" 
AR Path="/5AE96312/5AE6BFCE" Ref="TP6"  Part="1" 
AR Path="/5AE96ED4/5AE6BFCE" Ref="TP7"  Part="1" 
F 0 "TP1" V 5854 2187 50  0000 L CNN
F 1 "ADC_REF_OUT" V 5945 2187 50  0000 L CNN
F 2 "TestPoint:TestPoint_Pad_1.5x1.5mm" H 6100 2000 50  0001 C CNN
F 3 "~" H 6100 2000 50  0001 C CNN
	1    5900 2000
	0    1    1    0   
$EndComp
$EndSCHEMATC
