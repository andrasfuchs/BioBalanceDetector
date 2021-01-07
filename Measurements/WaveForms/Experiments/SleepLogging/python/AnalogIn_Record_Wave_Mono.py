"""
   DWF Python Example
   Author:  Digilent, Inc.
   Revision:  2018-07-19

   Requires:                       
       Python 2.7, 3
   Desciption:
   - generates sine on AWG1
   - records data on Scope 1
   - writes data to 16 bit WAV file
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
import array

if sys.platform.startswith("win"):
    dwf = cdll.dwf
elif sys.platform.startswith("darwin"):
    dwf = cdll.LoadLibrary("/Library/Frameworks/dwf.framework/dwf")
else:
    dwf = cdll.LoadLibrary("libdwf.so")

#declare ctype variables
hdwf = c_int()
sts = c_byte()
vOffset = c_double(0)
vAmplitude = c_double(2.45)
hzSignal = c_double(80)
hzAcq = c_double(8000)
nSamples = 12 * 60 * 60 * 8000
cAvailable = c_int()
cLost = c_int()
cCorrupted = c_int()
fLost = 0
fCorrupted = 0

#print(DWF version
version = create_string_buffer(16)
dwf.FDwfGetVersion(version)
print("DWF Version: "+str(version.value))

#open device
print("Opening first device")
dwf.FDwfDeviceOpen(c_int(-1), byref(hdwf))

if hdwf.value == hdwfNone.value:
    szerr = create_string_buffer(512)
    dwf.FDwfGetLastErrorMsg(szerr)
    print(str(szerr.value))
    print("failed to open device")
    quit()

print("Generating sine wave...")
dwf.FDwfAnalogOutNodeEnableSet(hdwf, c_int(0), AnalogOutNodeCarrier, c_bool(True))
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, c_int(0), AnalogOutNodeCarrier, funcSine)
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, c_int(0), AnalogOutNodeCarrier, hzSignal)
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, c_int(0), AnalogOutNodeCarrier, vAmplitude)
dwf.FDwfAnalogOutNodeOffsetSet(hdwf, c_int(0), AnalogOutNodeCarrier, vOffset)
dwf.FDwfAnalogOutConfigure(hdwf, c_int(0), c_bool(True))

#set up acquisition
dwf.FDwfAnalogInChannelEnableSet(hdwf, c_int(0), c_bool(True))
dwf.FDwfAnalogInChannelRangeSet(hdwf, c_int(0), c_double(2.0*vAmplitude.value))
dwf.FDwfAnalogInChannelOffsetSet(hdwf, c_int(0), vOffset)
dwf.FDwfAnalogInAcquisitionModeSet(hdwf, acqmodeRecord)
dwf.FDwfAnalogInFrequencySet(hdwf, hzAcq)
dwf.FDwfAnalogInRecordLengthSet(hdwf, c_double(-1)) # -1 infinite record length

#wait at least 2 seconds for the offset to stabilize
time.sleep(2)

print("Starting oscilloscope")
dwf.FDwfAnalogInConfigure(hdwf, c_int(0), c_int(1))

cSamples = 0

print("Generating "+str(hzSignal.value)+"Hz, recording "+str(hzAcq.value)+"Hz for "+"{:06.2f}".format(nSamples/hzAcq.value/60)+" minutes, press Ctrl+C to stop...");
#get the proper file name

#open WAV file
starttime = datetime.datetime.now();
startfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + ".wav";
print("Writing WAV file '" + startfilename + "'");
waveWrite = wave.open(startfilename, "wb");
waveWrite.setnchannels(1);				# 1 channels 
waveWrite.setsampwidth(2);				# 16 bit / sample
waveWrite.setframerate(hzAcq.value);
waveWrite.setcomptype("NONE","No compression");

try:
    while cSamples < nSamples:
        dwf.FDwfAnalogInStatus(hdwf, c_int(1), byref(sts))
        if cSamples == 0 and (sts == DwfStateConfig or sts == DwfStatePrefill or sts == DwfStateArmed) :
            # Acquisition not yet started.
            continue

        dwf.FDwfAnalogInStatusRecord(hdwf, byref(cAvailable), byref(cLost), byref(cCorrupted))
        
        cSamples += cLost.value

        if cLost.value :
            fLost = 1
        if cCorrupted.value :
            fCorrupted = 1

        if cAvailable.value==0 :
            continue

        if cSamples+cAvailable.value > nSamples :
            cAvailable = c_int(nSamples-cSamples)
        
        rgSamples = (c_int16*cAvailable.value)()

        dwf.FDwfAnalogInStatusData16(hdwf, c_int(0), rgSamples, c_int(0), cAvailable) # get channel 1 data chunk
        cSamples += cAvailable.value
        waveWrite.writeframes(rgSamples)
        
except KeyboardInterrupt:
    pass	

endtime = datetime.datetime.now();
dwf.FDwfAnalogOutReset(hdwf, c_int(0))
dwf.FDwfDeviceCloseAll()

print(" done")
if fLost:
    print("Samples were lost! Reduce frequency")
if fCorrupted:
    print("Samples could be corrupted! Reduce frequency")

waveWrite.close();

endfilename = "AD2_" + "{:04d}".format(starttime.year) + "{:02d}".format(starttime.month) + "{:02d}".format(starttime.day) + "_" + "{:02d}".format(starttime.hour) + "{:02d}".format(starttime.minute) + "{:02d}".format(starttime.second) + "-" + "{:02d}".format(endtime.hour) + "{:02d}".format(endtime.minute) + "{:02d}".format(endtime.second) + ".wav";
print("Renaming file from '" + startfilename + "' to '" + endfilename + "'");
os.rename(startfilename, endfilename);

print(" done")

