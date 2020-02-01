<p align="center">
  <img src="https://github.com/andrasfuchs/BioBalanceDetector/blob/master/Business/Branding/Logos/BioBalanceDetectorLogo_nobg_540x709.png"/>
</p>

Technically speaking Bio Balance Detector is a software and hardware combination (a data acquisition system - DAS) to detect, measure, display and analyze the electromagnetic energy field around living beings and indicate any potential health imbalance.  
  
These very weak, but measurable electromagnetic fields are the same what our doctors use while using an EEG (electroencephalogram) and ECG (electrocardiogram) devices. We use the information gathered there mainly to detect if we are sleeping, awake or alert, or if there is any problem with our heart's. My goal is to extend this phenomenon's measurements to our hands, or even our whole body.  
  
Technically I will need to connect an electromagnetic sensor matrix which is connected to a PC where the rendering would take place. It will look similar to a Wacom tablet, just like this:
![Wacom Tablet](https://github.com/andrasfuchs/BioBalanceDetector/blob/master/Hardware/Design/Wacom-Intuos-Art-Medium-CTH690AK.jpg)  
  
Right now, with my [Mercury-16 prototype](https://github.com/andrasfuchs/BioBalanceDetector/wiki/Proto-%235---Mercury-16)  I reached a point where I have a working sensor matrix connected to a microcontroller. Every microcontroller is connected to 8 sensors and 1 emitter. The sensors detect electromagnetic fields, the emitters generate those fields. This part (1 microcontroller, 8 sensors, 1 emitter) is called the cell. These cells are daisy-chained and they all are connected to another microcontroller called the organizer. The cells digitize the analog signals (at 1kHz, 12bit) from the sensors (antennas) and forward them to the organizer. The organizer's job is to format the raw data and send it to the PC trough USB. The PC has a driver which further processes the data, and applies the filters if necessary. There are now WAV file writer, VTS file writer, FFT (Fast Fourier Transformation), averager and offsetter filters available. The driver is capable to work with [SciLab](http://www.scilab.org/) and [ParaView](https://www.paraview.org/), and it also has a very basic console application UI.  
  
You can browse the technical challenges I worked and I am working on on the [implementation wiki page](https://github.com/andrasfuchs/BioBalanceDetector/wiki/Implementation). Future plans include, but not limited to: fractal antenna design, analog low-pass Butterworth filters, Goertzel algorithm implementation on the cells, emitters, USB 3.0, Unity3D GUI and many more.  
  
I realize at this point that I need experienced, passionate people to more forward. I'm currently looking for people with the following expertise, so don't hesitate to contact me:  
* Hardware designer: PCB with [KiCad](https://www.kicad-pcb.org/), antenna and analog low pass filter design  
* Microcontroller firmware programmer: Atmel XMEGA programming with AVR C  
* PC driver developer: .NET, C#, signal processing and WebSockets  
* GUI: Unity3D, C# and WebSockets  
  
If you would like to get know the project more, check out the [wiki page](http://bit.ly/29OBsJ2) or contact me at <andras.fuchs@gmail.com>.  
