# rpi_ad7606

Library for interfacing an Analog Devices AD7606 to a Raspberry Pi.

## Dependencies

  * pigpio (start it with `sudo pigpiod`)

## Installation

1. Log into Raspberry Pi

2. Clone from source and run the setup script

```
git clone https://github.com/bsumlin/rpi_ad7606.git
cd rpi_ad7606
sudo python setup.py install
```

3. At this point "rpi-ad7606" should be listed when `pip freeze` is executed.

## Test Script

You can find a simple test script [simple_spi_adc_read.py](Test%20and%20support%20files/simple_spi_adc_read.py) in "Test and support files"
