import time
import pigpio as pg
import rpi_ad7606 as adc

RPi = pg.pi()

def main():
    adc_pins = {'standby':13, 'convsta':19, 'reset':26, 'busy':21, '1stData':20}
    adc1 = adc.AD7606_SPI(10,'simultaneous',adc_pins)
   
    adc1.ADCreset()

    i = 0
    while True:
        i = i + 1
        r = adc1.ADCread()
        _ch = 1
        print("loop #{n}".format(n=i))
        for _r in r:
            print("Channel #{n}: {v:>6.3f} Volts.".format(n=_ch,v=_r))            
            _ch += 1
        print("");
        time.sleep(1)

if __name__ == "__main__":
    main()
