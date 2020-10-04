print("|-----------------------------------------------------------------------------------------------------------------");
print("| Sleep Logging v1.2 (2020-10-04)");
print("|");
print("| This script generates audio feddback based on the measured EEG signals in the 0-45 Hz and 55-95 Hz range.");
print("| The bigger the change was in the last 5 seconds, the higher the volume will be.");
print("| If your EEG signal becomes higher frequency then the pitch rises, if it becomes lower then the pitch falls. ");
print("| The audio signal is a 432 Hz sine wave, modified by the difference between the long and short measurements.");
print("|");
print("| Please note that the long average is only updated once, when you start the script, so every change will be");
print("| measured against it during its run.");
print("| Because of this I recommend to start the script and wait until the short averaging runs. At this point the");
print("| frequency must be around 432 Hz and the volume around 0.1 V.");
print("| If you start to hear low volume-low pitch sounds then you're brain is more relaxed than before, so going");
print("| to sleep soon.");
print("|");
print("| Short averaging: 5 seconds");
print("| Long averaging: 60 seconds");
print("|");
print("| Plot - green line: audio frequency");
print("|-----------------------------------------------------------------------------------------------------------------");

const dataPointCount = 512;            // FFT datapoint count = Time.Samples / 2
                                   
var avgSamplesCountShort = 240;        // 240 samples ~ 5 second @ 1024 samples and 800 Hz
var avgSamplesCountLong = 10000;	     // 10000 samples ~ 30 seconds @ 1024 samples and 800 Hz

var amplificationFactor = 5000.0;      // the higher the amplification to more sensitive the device's is voltagewise
var audioAmplificationFactor = 50.0;   // the higher the amplification to more sensitive the device's audio output is

{ // configure scope
    Scope1.Buffer.value = 1;
    Scope1.Time.Mode.text = "Shift";
    Scope1.Time.Rate.value = 800;
    Scope1.Time.Samples = dataPointCount * 2;
    Scope1.Channel1.checked = true;
    Scope1.Channel2.checked = false;
}
{ // configure wavegen
    Wavegen1.Channel1.Mode.text = "Simple";
    Wavegen1.Channel1.Simple.Type.text = "Sine";
}

var datapoints = new Array(4); // datapoint array (short average values, long average values, difference between the two)
datapoints[0] = new Array(dataPointCount);
datapoints[1] = new Array(dataPointCount);
datapoints[2] = new Array(dataPointCount);
datapoints[3] = new Array(dataPointCount);

var avgSamplesShort = new Array(dataPointCount+1);
var avgSamplesLong = new Array(dataPointCount+1);
for(var i = 0; i <= dataPointCount; i++)
{
    avgSamplesShort[i] = new Array(avgSamplesCountShort);
    avgSamplesLong[i] = new Array(avgSamplesCountLong);
}

{ // configure plot
    plot1.X.Units.text = "";
    plot1.X.Offset.value = -dataPointCount/2
    plot1.X.Range.value = dataPointCount;
    plot1.Y1.AutoScale.checked = false;
    plot1.Y2.AutoScale.checked = false;
    plot1.Y3.AutoScale.checked = false;
    plot1.Y4.AutoScale.checked = false;

    const fftMax = 10;
    const fftMin = 0;
    var offset = -(fftMin + (fftMax - fftMin)/2);
    var range = (fftMax - fftMin);

    plot1.Y1.Offset.value = offset;
    plot1.Y1.Range.value = range;
    plot1.Y2.Offset.value = offset;
    plot1.Y2.Range.value = range;

    const fftDifMax = +1;
    const fftDifMin = -1;
    var offset = -(fftDifMin + (fftDifMax - fftDifMin)/2);
    var range = (fftDifMax - fftDifMin);

    plot1.Y3.Offset.value = offset;
    plot1.Y3.Range.value = range;

    const audioHzMax = 814;
    const audioHzMin = 50;
    var offset = -(audioHzMin + (audioHzMax - audioHzMin)/2);
    var range = (audioHzMax - audioHzMin);

    plot1.Y4.Offset.value = offset;
    plot1.Y4.Range.value = range;
}

Wavegen1.run();
Scope1.run();

wait(3);

var ignored = 0;
var m = 0;
print(new Date().toISOString() + " | " + "===== Long averaging was started ====");

while (m < avgSamplesCountLong)
{
    Scope1.wait();
    var rghz = Scope1.Channel1.fftfrequency;
    var rgmag = Scope1.Channel1.fftmagnitude;

    ignored = 0;
    for(var j = 0; j < dataPointCount; j++)
    {
        // filter the 50Hz out
        if (IsFiltered(rghz[j]))
        {
            avgSamplesLong[j][m%avgSamplesCountLong] = 0;
            ignored += 1;
            continue;
        } 
                
        avgSamplesLong[j][m%avgSamplesCountLong] = rgmag[j];
        avgSamplesShort[j][m%avgSamplesCountShort] = rgmag[j];
    }
    
    m++;
}

for(var j = 0; j < dataPointCount; j++)
{
    datapoints[1][j] = ArrayAverage(avgSamplesLong[j]) * amplificationFactor;
}

var longTermAudioFrequencyValue = CalculateAudioFrequency(datapoints[1], rghz, dataPointCount)

print(new Date().toISOString() + " | " + "===== Long averaging was completed ====");

print(new Date().toISOString() + " | " + "===== Short averaging was started ====");

while (true)
{
    Scope1.wait();

    // check out the FFT values
    var rgmag = Scope1.Channel1.fftmagnitude;
    var rghz = Scope1.Channel1.fftfrequency;
    
    // full the short-term buffer
    var avgrgmag = 0;
    for(var j = 0; j < dataPointCount; j++)
    {
        // filter the 50Hz out
        if (IsFiltered(rghz[j]))
        {
            avgSamplesShort[j][m%avgSamplesCountShort] = 0;
            continue;
        } 
                
        avgSamplesShort[j][m%avgSamplesCountShort] = rgmag[j];
        
        avgrgmag += rgmag[j];
    }


    avgrgmag /= (dataPointCount - ignored);

    // calculate the amplified, averaged short-term values
    for(var j = 0; j < dataPointCount; j++)
    {
        datapoints[0][j] = ArrayAverage(avgSamplesShort[j]) * amplificationFactor;
        datapoints[2][j] = datapoints[0][j] - datapoints[1][j];
    }

    // the bigger the change in the values the higher the volume will be
    var volumeChange = 0;
    for(var j = 0; j < dataPointCount; j++)
    {
        volumeChange += abs(datapoints[2][j]);
    }
    volumeChange /= (dataPointCount - ignored);

    var audioVolume = 0.0;
    audioVolume += volumeChange * 2;

    // change the frequency depending on the dominating freqeuncies    
    var audioFrequency = 0;
    audioFrequency = CalculateAudioFrequency(datapoints[0], rghz, dataPointCount) - longTermAudioFrequencyValue;
    audioFrequency = 432 + (audioFrequency *  audioAmplificationFactor);
    datapoints[3][m%dataPointCount] = audioFrequency;

    // plot the most recent values on the chart
    plot1.Y1.data = datapoints[0];// yellow - short averaging
    plot1.Y2.data = datapoints[1];// blue - long averaging
    plot1.Y3.data = datapoints[2];// red - (short - long)
    plot1.Y4.data = datapoints[3];// green - audio frequency
    


    if (!isNaN(audioVolume) && !isNaN(audioFrequency))
    {
        if (audioVolume < 0.0)
        {
            audioVolume = 0.0;
        }
        
        if (audioVolume > 1.0)
        {
            audioVolume = 1.0;
        }

        if (audioFrequency < 50.0)
        {
            audioFrequency = 50.0;
        }

        Wavegen1.Channel1.Simple.Amplitude.text = (audioVolume / 2) + " V";
        Wavegen1.Channel1.Simple.Frequency.text = audioFrequency;

        if (m % 48  == 0)
        {
            print(new Date().toISOString() + " | " + "Frequency: " + audioFrequency.toFixed(2) + " Hz | volume: " + audioVolume.toFixed(3) + " V");                      
        }
    }

    m++;
}

print(new Date().toISOString() + " | " + "===== Short averaging was completed ====");

Wavegen1.stop();
Scope1.stop();





function ArrayAverage(array)
{
    var result = 0;

    for (k=0; k<array.length; k++)
    {
        result += array[k];
    }
    result /= k;

    return result;
}

function CalculateAudioFrequency(rgmag, rghz, fftCount)
{
    var result = 0;
    var rgmagsum = 0;

    for (k=0; k<fftCount; k++)
    {
        rgmagsum += rgmag[k];
        result += rgmag[k] * rghz[k];
    }
    result /= rgmagsum;
    
    return result;
}

function IsFiltered(hz)
{
    if (((hz > 45) && (hz < 55)) || (hz > 95))
    {
        return true;
    }

    if (hz > 30)
    {
        return true;
    }

    return false;
}