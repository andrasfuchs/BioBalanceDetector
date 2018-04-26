EESchema Schematic File Version 4
LIBS:CellSlave-cache
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title "Venus-256 / Cell Slave"
Date "2018-04-26"
Rev "A"
Comp "Bio Balance Detector"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Analog_ADC:AD7616 U?
U 1 1 5AE20D71
P 4300 4300
F 0 "U?" H 4300 7078 50  0000 C CNN
F 1 "AD7616" H 4300 6987 50  0000 C CNN
F 2 "Package_QFP:LQFP-80_14x14mm_P0.65mm" H 4300 4300 50  0001 C CIN
F 3 "http://www.analog.com/media/en/technical-documentation/data-sheets/AD7616.pdf" H 4400 1300 50  0001 C CNN
	1    4300 4300
	1    0    0    -1  
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J100
U 1 1 5AE20EA6
P 2700 2000
F 0 "J100" H 2620 1775 50  0000 C CNN
F 1 "CH00" H 2620 1866 50  0000 C CNN
F 2 "" H 2700 2000 50  0001 C CNN
F 3 "~" H 2700 2000 50  0001 C CNN
	1    2700 2000
	-1   0    0    1   
$EndComp
$Comp
L Connector_Generic:Conn_01x01 J101
U 1 1 5AE21024
P 2400 2100
F 0 "J101" H 2320 1875 50  0000 C CNN
F 1 "CH01" H 2320 1966 50  0000 C CNN
F 2 "" H 2400 2100 50  0001 C CNN
F 3 "~" H 2400 2100 50  0001 C CNN
	1    2400 2100
	-1   0    0    1   
$EndComp
$EndSCHEMATC
