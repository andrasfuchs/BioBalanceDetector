"""
   DWF Python Example
   Author:  Digilent, Inc.
   Revision:  2018-07-19

   Requires:                       
       Python 2.7, 3
"""

from ctypes import *
from dwfconstants import *
import math
import time
import matplotlib.pyplot as plt
import sys
import numpy
import wave
import datetime
import os

buffersize = 4096;			# samples / buffer
samplerate = 8000;			# samples / second
signalgenhz = 80;

if sys.platform.startswith("win"):
    dwf = cdll.dwf
elif sys.platform.startswith("darwin"):
    dwf = cdll.LoadLibrary("/Library/Frameworks/dwf.framework/dwf")
else:
    dwf = cdll.LoadLibrary("libdwf.so")

#declare ctype variables
hdwf = c_int()
sts = c_byte()
rgdSamples = (c_double*buffersize)()

version = create_string_buffer(16)
dwf.FDwfGetVersion(version)
print("DWF Version: "+str(version.value))

#open device
print("Opening first device")
dwf.FDwfDeviceOpen(c_int(-1), byref(hdwf))

if hdwf.value == hdwfNone.value:
    szerr = create_string_buffer(512)
    dwf.FDwfGetLastErrorMsg(szerr)
    print(szerr.value)
    print("failed to open device")
    quit()

cBufMax = c_int()
dwf.FDwfAnalogInBufferSizeInfo(hdwf, 0, byref(cBufMax))
print("Device buffer size: "+str(cBufMax.value)+" samples") 

#set up acquisition
dwf.FDwfAnalogInFrequencySet(hdwf, c_double(samplerate))
dwf.FDwfAnalogInBufferSizeSet(hdwf, c_int(buffersize)) 
dwf.FDwfAnalogInChannelEnableSet(hdwf, c_int(0), c_bool(True))
dwf.FDwfAnalogInChannelRangeSet(hdwf, c_int(0), c_double(5))

# set up signal generation
channel = c_int(0)																				# use W1
dwf.FDwfAnalogOutNodeEnableSet(hdwf, channel, AnalogOutNodeCarrier, c_bool(True))
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, channel, AnalogOutNodeCarrier, funcTriangle)					# ! this looks like a square wave
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, channel, AnalogOutNodeCarrier, c_double(signalgenhz))
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, channel, AnalogOutNodeCarrier, c_double(1.41))			# ! this doesn't really do anything
dwf.FDwfAnalogOutNodeOffsetSet(hdwf, channel, AnalogOutNodeCarrier, c_double(1.41))

print("Generating sine wave @"+str(signalgenhz)+"Hz...")
dwf.FDwfAnalogOutConfigure(hdwf, channel, c_bool(True))

#wait at least 2 seconds for the offset to stabilize
time.sleep(2)


#get the proper file name
starttime = datetime.datetime.now();
startfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + ".wav";


#open WAV file
print("Opening WAV file '" + startfilename + "'");
waveWrite = wave.open(startfilename, "wb");
waveWrite.setnchannels(2);				# 2 channels for the testing (1 channel would be enough if FDwfAnalogInStatusData returned only 1 channel's data
waveWrite.setsampwidth(4);				# 32 bit / sample
waveWrite.setframerate(samplerate);
waveWrite.setcomptype("NONE","No compression");


#start aquisition
print("Starting oscilloscope")
dwf.FDwfAnalogInConfigure(hdwf, c_bool(False), c_bool(True))

print("Recording data @"+str(samplerate)+"Hz, press Ctrl+C to stop...");

bufferCounter = 0;
try:
	while True:
		while True:
			dwf.FDwfAnalogInStatus(hdwf, c_int(1), byref(sts))
			if sts.value == DwfStateDone.value :
				break
			time.sleep(0.1)
			
		dwf.FDwfAnalogInStatusData(hdwf, 0, rgdSamples, buffersize) 	# get channel 1 data CH1 - ! it looks like 2 channels get read here and only the second is the data of CH1
		#dwf.FDwfAnalogInStatusData(hdwf, 1, rgdSamples, buffersize) 	# get channel 2 data CH2

		waveWrite.writeframes(rgdSamples);
		bufferCounter += 1;
		
		if ((bufferCounter % 1) == 0):
			print(str(waveWrite.tell() * 4) + " bytes were written");
		
except KeyboardInterrupt:
    pass	

print("Acquisition done")

print("Closing WAV file")
waveWrite.close();

dwf.FDwfDeviceCloseAll()

#rename the file so that we know both the start and end times from the filename
endtime = datetime.datetime.now();
endfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + "-" + "{:02d}".format(endtime.hour) + "{:02d}".format(endtime.minute) + "{:02d}".format(endtime.second) + ".wav";

print("Renaming file from '" + startfilename + "' to '" + endfilename + "'");
os.rename(startfilename, endfilename);


#plot window
#dc = sum(rgdSamples)/len(rgdSamples)
#print("DC: "+str(dc)+"V")

#plt.plot(numpy.fromiter(rgdSamples, dtype = numpy.float))
#plt.show()


