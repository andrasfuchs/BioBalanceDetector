from threading import Thread
import time
import pigpio as pg
import rpi_ad7606 as adc

RPi = pg.pi()
laserClockPin = 12

def main():
    laser_pins = [17,22,27,24]
    adc_pins = {'standby':13, 'convsta':19, 'reset':26, 'busy':21, '1stData':20}
    adc1 = adc.AD7606('SPI',10,'simultaneous',adc_pins)

    laserLoop = laserSim(1250,laser_pins)
    adcLoop = adcSim(adc1)

    laserLoop.start()
    adcLoop.start()
    time.sleep(10)
    laserLoop.stop()
    adcLoop.stop()
    # blah blah

def laserSim(Thread):
    def __init__(self,frequency,pins):
        super(laserSim, self).__init__()
        self._keepgoing = True
        self.frequency = frequency
        self.pins = pins

    def run(self):
        RPi.set_pull_up_down(laserClockPin,pg.PUD_DOWN)
        RPi.hardware_PWM(laserClockPin,self.frequency,500000)
        while(self._keepgoing):
            for pin in self.pins:
                RPi.write(pin,1)
                time.sleep(0.25)
                Rpi.write(pin,0)

    def stop(self):
        RPi.write(laserClockPin,0)
        for pin in self.pins:
            RPi.write(pin,0)
        self._keepgoing = False

def adcSim(Thread):
    def __init__(self,_adc):
        super(adcSim,self).__init__()
        self._keepgoing = true
        self._adc = _adc
        self._adc.ADCreset()

    def run(self):
        while(self._keepgoing):
            r = self._adc.ADCread()
            _ch = 1
            for _r in r:
                print("Channel {n}: {v:1.3f} volts.".format(n=_ch,v=_r))
                _ch += 1
            time.sleep(1)

    def stop(self):
        self._adc.ADCreset()
        self._keepgoing = False

if __name__ == '__main__':
    main()
