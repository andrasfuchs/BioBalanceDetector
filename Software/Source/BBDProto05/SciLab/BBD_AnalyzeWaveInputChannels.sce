// Press F5 to execute, select Control/Abort to stop

[wavfilepath]="c:\Work\BioBalanceDetector\Recordings\";
[wavfilename]="BBD_20161219_064719_E1LA.wav";

// FFTs are most efficient if the number of samples, N, is a power of 2.
[points]=8192;
[rate]=8000;
[fmax]=60;
[fmin]=20;
[channelcount]=8;
[inspectedfrequency]=40;


function [fftvalues] = analyzeWav (w,fmin,fmax,rate,points)
    // Make a frequency plot of the signal w with sampling rate.
    // The data must be at least points long.
    // The maximal frequency plotted will be fmax, the minimal fmin.
    [lhs,rhs]=argn(0);
    if ( rhs <= 4 ) then points=8192 ; end ;
    if ( rhs <= 3 ) then rate=0; end ;
    if ( rhs <= 2 ) then fmax=1500; end ;
    if ( rhs <= 1 ) then fmin=10;  end ;
    defaultrate=22050;
    if rate==0; rate=defaultrate; end;
    v=w(1:points);              // v contains the selected portion of the data
    f=abs(fft(v,1));            // f is the FFT-d v (using CPU)
    //fGPU=gpuFFT(v,1);           // f is the FFT-d v (using OpenCL)
    //f=abs(gpuGetData(fGPU));    // we need to get the data from the GPU's memory
    i=fmin/rate*points:fmax/rate*points;    // i is the range of the x-axis
    fr=i/points*rate;           // convert i to Hz for the x-axis
    plot2d(fr',f(i)',rect=[fmin,0,fmax,0.5]);  // display Hz on the x-axis, and the FFT value on the y-axis
    
    [fftvalues]=return(f);
endfunction



function mainloop()
    [counter]=0;
    [startpos]=0;
    [lastsize]=0;
    
    da=gda();
    da.auto_clear = 'on';
    
    while counter<10, 
    
        counter = counter + 1;
        [wavsize]=wavread([wavfilepath]+[wavfilename],"size")(2);
        //[wavinfo]=wavread([wavfilepath]+[wavfilename],"info")
        //xstring(0, 0, "Bio Balance Detector - "+[wavfilename]+" with "+string([wavsize])+" samples - SciLab");
        //disp("lastsize:"+string([lastsize])+" wavsize:"+string([wavsize]));
        
        if ([wavsize] > [lastsize]) then            
            [lastsize] = [wavsize];
            [counter] = 0;
            [startpos]=[wavsize]-8192;
            [y,Fs,bits]=wavread([wavfilepath]+[wavfilename],[startpos startpos+8192]);Fs,bits
                        
            for i = 1:[channelcount]
                subplot(3,[channelcount],[channelcount]*0+i);
                plot2d(y(i,:));                             // display the waveform
                xtitle("channel "+string(i), boxed = 1);    // add a title on the top

                subplot(3,[channelcount],[channelcount]*1+i);
                [fftvalues]=analyzeWav(y(i,:), fmin, fmax, rate, points);   // display the FFT

                subplot(3,[channelcount],[channelcount]*2+i);
                [intensity]=255-([fftvalues]([inspectedfrequency])*512);
                plot2d(0,0,-1,"010"," ",[-2,-2,2,2]);
                xset("color",color([intensity],[intensity],[intensity]));
                xfrect(-2,-2,4,4);
            end;
        else
            sleep(500);
        end
    end
endfunction

mainloop();
