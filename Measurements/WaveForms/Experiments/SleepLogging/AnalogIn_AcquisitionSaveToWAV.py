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
print("Device buffer size: "+str(cBufMax.value)) 

#set up acquisition
dwf.FDwfAnalogInFrequencySet(hdwf, c_double(samplerate/2))		# is this a bug?
dwf.FDwfAnalogInBufferSizeSet(hdwf, c_int(buffersize)) 
dwf.FDwfAnalogInChannelEnableSet(hdwf, c_int(0), c_bool(True))
dwf.FDwfAnalogInChannelRangeSet(hdwf, c_int(0), c_double(5))

#wait at least 2 seconds for the offset to stabilize
time.sleep(2)

starttime = datetime.datetime.now();
startfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + ".wav";

print("Opening WAV file '" + startfilename + "'");

waveWrite = wave.open(startfilename, "wb");
waveWrite.setnchannels(1);				# mono
waveWrite.setsampwidth(4);				# 32 bit / sample
waveWrite.setframerate(samplerate);
waveWrite.setcomptype("NONE","No compression");


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
			
		dwf.FDwfAnalogInStatusData(hdwf, 0, rgdSamples, buffersize) # get channel 1 data
		#dwf.FDwfAnalogInStatusData(hdwf, 1, rgdSamples, buffersize) # get channel 2 data

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

endtime = datetime.datetime.now();
endfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + "-" + "{:02d}".format(endtime.hour) + "{:02d}".format(endtime.minute) + "{:02d}".format(endtime.second) + ".wav";

print("Renaming file from '" + startfilename + "' to '" + endfilename + "'");
os.rename(startfilename, endfilename);


#plot window
#dc = sum(rgdSamples)/len(rgdSamples)
#print("DC: "+str(dc)+"V")

#plt.plot(numpy.fromiter(rgdSamples, dtype = numpy.float))
#plt.show()


