rem Fuse information is on page 282 of Atmel-8271-8-bit-AVR-Microcontroller-ATmega48A-48PA-88A-88PA-168A-168PA-328-328P_datasheet_Complete.pdf
rem avrdude's valid memtype parameters are: hfuse (high fuse), lfuse (low fuse), or efuse (extended fuse)
rem http://electronics.stackexchange.com/a/32958

rem default lfuse value is 0b01100010 / 0x62 (internal RC oscillator @ 8Mhz)
rem the new lfuse value is 0b01100000 / 0x60 (external clock @ 0-20Mhz) [this one bricked my ATmega16]
rem the new lfuse value is 0b01101111 / 0x6F (external lower power crystal oscillator @ 8-16Mhz [works with 20Mhz too])

rem avrdude.exe -c usbasp -p atmega168 -U lfuse:w:0x62:m %1 %2 %3 %4 %5 %6
avrdude.exe -c usbasp -p atmega168 -U lfuse:w:0x6F:m %1 %2 %3 %4 %5 %6

rem avrdude.exe -c usbasp -p atmega168 -U flash:w:emptygccapp.hex %1 %2 %3 %4 %5 %6