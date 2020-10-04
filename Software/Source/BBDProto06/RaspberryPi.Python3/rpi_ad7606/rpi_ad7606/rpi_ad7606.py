import pigpio as pio
from time import sleep
from numpy import zeros, int16

class AD7606_SPI:
    def __init__(self,inputRange,conversionABmode,pins,dataFreq=1000000,returnRaw=False):
        # pins should be a dict mapping pin names to RPi numbers. If using SPI, only need to specify CE channel. Unwrap pins to individual self.whatevers
        # inputRange should be 5 or 10, nothing else. Default it to 5.
        # ABmode should be 'simultaneous' or 'grouped'
        self.inputRange = inputRange
        self.xferFactor = inputRange/(2**15) # hardcoded for now, but in the future range could change dynamically
        self.ABmode = None#ABmode
        self.p_standby = pins['standby']
        self.p_convsta = pins['convsta']
        self.p_dataA = None#pins['DoutA'] # None for SPI
        if conversionABmode == 'simultaneous': # all on one line
            self.p_convstb = None
            self.p_dataB = None
        elif conversionABmode == 'grouped': # all on A and B lines - update later to separate conversion and reading
            self.p_convstb = pins['convstb']
            self.p_dataB = pins['DoutB']
        self.p_reset = pins['reset']
        self.p_busy = pins['busy']
        self.p_1stData = pins['1stData']
        self.returnRaw = returnRaw

        self.io = pio.pi() # initialize pigpio
        # initialize pins as appropriate
        for _p in [self.p_standby, self.p_convsta, self.p_convstb, self.p_reset]:
            if _p is not None:
                self.io.set_mode(_p,pio.OUTPUT)
        for _p in [self.p_dataA,self.p_dataB,self.p_busy,self.p_1stData]:
            if _p is not None:
                self.io.set_mode(_p,pio.INPUT)
        for _p in [self.p_convsta,self.p_convstb]:
            if _p is not None:
                self.io.set_pull_up_down(_p,pio.PUD_UP)
        for _p in [self.p_reset,self.p_busy]:
            if _p is not None:
                self.io.set_pull_up_down(_p,pio.PUD_DOWN)
        self.adc = self.io.spi_open(0,dataFreq,0) # initialize spi on channel 0 at 1mhz. self.adc is now the "handle"

    def transferFunction(self,reading):
        def _twosComp(_val):
            if (_val & (1 << 15)) != 0:
                _val = _val - (1 << 16)
            return _val
        return self.xferFactor*_twosComp(reading) # returns the voltage of the sensor

    def ADCreset(self):
        self.io.write(self.p_reset,1)
        self.io.write(self.p_reset,0)

    #def standby(self):
        # do this later, mine's never in standby

    def ADCread(self):
        self.io.write(self.p_convsta,0)
        self.io.write(self.p_convsta,1)
        #while self.io.read(self.p_busy) == 1:
            #continue
        _c, _d = self.io.spi_read(self.adc,12) # two bytes per each of the 6 channels. _c is number of bytes read back and _d is a bytearray of said bytes.

        if self.returnRaw:
            return _d
        else:
            _channel1 = self.transferFunction(_d[0] << 8 | _d[1])
            _channel2 = self.transferFunction(_d[2] << 8 | _d[3])
            _channel3 = self.transferFunction(_d[4] << 8 | _d[5])
            _channel4 = self.transferFunction(_d[6] << 8 | _d[7])
            _channel5 = self.transferFunction(_d[8] << 8 | _d[9])
            _channel6 = self.transferFunction(_d[10] << 8 | _d[11])

            return [_channel1,_channel2,_channel3,_channel4,_channel5,_channel6]

class AD7606_AB:
    def __init__(self,inputRange,pins,returnRaw=False):
        # pins should be a dict mapping pin names to RPi numbers.
        # inputRange should be 5 or 10, nothing else. Default it to 5.
        self.inputRange = inputRange
        self.xferFactor = inputRange/(2**15) # hardcoded for now, but in the future range could change dynamically
        self.p_standby = pins['standby']
        self.p_cs = pins['cs']
        self.p_clock = pins['clock']
        self.p_convsta = pins['convsta']
        self.p_convstb = None#pins['convstb']
        self.p_dataA = pins['DoutA']
        self.p_dataB = pins['DoutB']
        self.p_reset = pins['reset']
        self.p_busy = pins['busy']
        self.p_1stData = pins['1stData'] # not really using this...may be for parallel stuff
        self.returnRaw = returnRaw
        self.containerA = np.zeros(3,dtype=np.int16)
        self.containerB = np.zeros(3,dtype=np.int16)

        self.io = pio.pi() # initialize pigpio
        # initialize pins as appropriate
        for _p in [self.p_standby, self.p_convsta, self.p_convstb, self.p_reset, self.p_cs, self.p_clock]:
            if _p is not None:
                self.io.set_mode(_p,pio.OUTPUT)
        for _p in [self.p_dataA,self.p_dataB,self.p_busy,self.p_1stData]:
            if _p is not None:
                self.io.set_mode(_p,pio.INPUT)
        for _p in [self.p_convsta,self.p_convstb,self.p_cs,self.p_clock]:
            if _p is not None:
                self.io.set_pull_up_down(_p,pio.PUD_UP)
        for _p in [self.p_reset,self.p_busy]:
            if _p is not None:
                self.io.set_pull_up_down(_p,pio.PUD_DOWN)

    def transferFunction(self,reading):
        def _twosComp(_val):
            if (_val & (1 << 15)) != 0:
                _val = _val - (1 << 16)
            return _val
        return self.xferFactor*_twosComp(reading) # returns the voltage of the sensor

    def ADCreset(self):
        self.io.write(self.p_reset,1)
        self.io.write(self.p_reset,0)

    #def standby(self):
        # do this later, mine's never in standby

    def ADCread(self):
        self.io.write(self.p_convsta,0)
        self.io.write(self.p_convsta,1)
        #while self.io.read(self.p_busy) == 1:
            #continue
        self.io.write(self.cs,0)

        for _c in range(3):
            _A = 0
            _B = 0
            for _i in range(16):
                self.io.write(self.clock,0)
                _A += self.io.read(self.p_dataA)<<_i
                _B += self.io.read(self.p_dataB)<<_i
                self.io.write(self.clock,1)
            self.containerA[_c] = _A
            self.containerB[_c] = _B
        self.io.write(self.cs,1)

        if returnRaw:
            return [self.containerA,self.containerB]
        else:
            return [self.transferFunction(x) for x in self.containerA]+[self.transferFunction(x) for x in self.containerB]
