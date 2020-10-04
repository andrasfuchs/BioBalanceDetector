from threading import Thread
import time
import pigpio as pg

RPi = pg.pi()
laserClockPin = 12

def main_loop():
    laserthread = LaserSim()
    laserthread.start()

    time.sleep(20) # execute while loop for 20 seconds
    laserthread.stop()

class LaserSim(Thread):

    def __init__(self):
        super(LaserSim, self).__init__()
        self.pins = [17,22,27,24]
        self._keepgoing = True

    def run(self):
        RPi.set_pull_up_down(laserClockPin,pg.PUD_DOWN)
        RPi.hardware_PWM(laserClockPin,1250,500000)
        while (self._keepgoing):
            for pin in self.pins:
                RPi.write(pin,1)
                time.sleep(0.25)
                RPi.write(pin,0)


    def stop(self):
        self._keepgoing = False
        RPi.write(laserClockPin,0)
        for pin in self.pins:
            RPi.write(pin,0)

main_loop()
