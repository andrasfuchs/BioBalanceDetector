import time
import busio
import board

import adafruit_mlx90393

I2C_BUS = busio.I2C(board.SCL, board.SDA)
SENSOR = adafruit_mlx90393.MLX90393(I2C_BUS, gain=adafruit_mlx90393.GAIN_5X)


COUNTER = 0

while (COUNTER < 30):
    MX, MY, MZ = SENSOR.magnetic
    print("[{time:>10.4f}]  X: {mx:>+12.2f} uT | Y: {my:>+12.2f} uT | Z: {mz:>+12.2f} uT".format(time=time.monotonic(), mx=MX, my=MY, mz=MZ))
    # Display the status field if an error occured, etc.
    if SENSOR.last_status > adafruit_mlx90393.STATUS_OK:
        SENSOR.display_status()
    time.sleep(0.2)
    COUNTER = COUNTER + 1