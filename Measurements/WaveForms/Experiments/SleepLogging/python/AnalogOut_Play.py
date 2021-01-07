"""
   DWF Python Example
   Author:  Digilent, Inc.
   Revision:  2018-07-19

   Requires:                       
       Python 2.7, 3
"""

import numpy as np
import scipy.io.wavfile
import matplotlib.pyplot as plt
import ctypes
from ctypes import *
import sys

print("Load audio.WAV file")
rate, data = scipy.io.wavfile.read('audio.wav')
print("Rate: "+str(rate))
print("Size: "+str(data.size))
print("Type: " +str(np.dtype(data[0])))
# AnalogOut expects double normalized to +/-1 value
dataf = data.astype(np.float64)
if np.dtype(data[0]) == np.int8 or np.dtype(data[0]) == np.uint8 :
    print("Scaling: UINT8")
    dataf /= 128.0
    dataf -= 1.0
elif np.dtype(data[0]) == np.int16 :
    print("Scaling: INT16")
    dataf /= 32768.0
elif np.dtype(data[0]) == np.int32 :
    print("Scaling: INT32")
    dataf /= 2147483648.0
data_c = (ctypes.c_double * len(dataf))(*dataf)
plt.plot(data)
plt.show()

if sys.platform.startswith("win"):
    dwf = cdll.dwf
elif sys.platform.startswith("darwin"):
    dwf = cdll.LoadLibrary("/Library/Frameworks/dwf.framework/dwf")
else:
    dwf = cdll.LoadLibrary("libdwf.so")

# declare ctype variables
hdwf = c_int()
channel = c_int(0) # AWG 1

# print(DWF version
version = create_string_buffer(16)
dwf.FDwfGetVersion(version)
print("DWF Version: "+str(version.value))

# open device
print("Opening first device...")
dwf.FDwfDeviceOpen(c_int(-1), byref(hdwf))

if hdwf.value == 0:
    print("Failed to open device")
    szerr = create_string_buffer(512)
    dwf.FDwfGetLastErrorMsg(szerr)
    print(str(szerr.value))
    quit()

print("Playing audio...")
iPlay = 0
dwf.FDwfAnalogOutNodeEnableSet(hdwf, channel, 0, c_bool(True))
dwf.FDwfAnalogOutNodeFunctionSet(hdwf, channel, 0, c_int(31)) #funcPlay
dwf.FDwfAnalogOutRepeatSet(hdwf, channel, c_int(1))
sRun = 1.0*data.size/rate
print("Length: "+str(sRun))
dwf.FDwfAnalogOutRunSet(hdwf, channel, c_double(sRun))
dwf.FDwfAnalogOutNodeFrequencySet(hdwf, channel, 0, c_double(rate))
dwf.FDwfAnalogOutNodeAmplitudeSet(hdwf, channel, 0, c_double(1.0))
# prime the buffer with the first chunk of data
cBuffer = c_int(0)
dwf.FDwfAnalogOutNodeDataInfo(hdwf, channel, 0, 0, byref(cBuffer))
if cBuffer.value > data.size : cBuffer.value = data.size
dwf.FDwfAnalogOutNodeDataSet(hdwf, channel, 0, data_c, cBuffer)
iPlay += cBuffer.value
dwf.FDwfAnalogOutConfigure(hdwf, channel, c_bool(True))

dataLost = c_int(0)
dataFree = c_int(0)
dataCorrupted = c_int(0)
sts = c_ubyte(0)
totalLost = 0
totalCorrupted = 0

while True :
    # fetch analog in info for the channel
    if dwf.FDwfAnalogOutStatus(hdwf, channel, byref(sts)) != 1:
        print("Error")
        szerr = create_string_buffer(512)
        dwf.FDwfGetLastErrorMsg(szerr)
        print(szerr.value)
        break
    
    if sts.value != 3: break # not running !DwfStateRunning
    if iPlay >= data.size : continue # no more data to stream

    dwf.FDwfAnalogOutNodePlayStatus(hdwf, channel, 0, byref(dataFree), byref(dataLost), byref(dataCorrupted))
    totalLost += dataLost.value
    totalCorrupted += dataCorrupted.value

    if iPlay + dataFree.value > data.size : # last chunk might be less than the free buffer size
        dataFree.value = data.size - iPlay
    if dataFree.value == 0 : continue
    if dwf.FDwfAnalogOutNodePlayData(hdwf, channel, 0, byref(data_c, iPlay*8), dataFree) != 1: # offset for double is *8 (bytes) 
        print("Error")
        break
    iPlay += dataFree.value

print("Lost: "+str(totalLost))
print("Corrupted: "+str(totalCorrupted))

print("done")
dwf.FDwfAnalogOutReset(hdwf, channel)
dwf.FDwfDeviceClose(hdwf)