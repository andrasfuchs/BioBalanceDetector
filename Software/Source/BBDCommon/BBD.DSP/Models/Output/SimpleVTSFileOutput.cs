using BBD.DSP.Models.Source;
using BBD.DSP.Models.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BBD.DSP.Helpers;
using BBD.DSP.Models.Filter;

namespace BBD.DSP.Models.Output
{
    public class SimpleVTSFileOutput : FileOutput
    {
        private int zSize = 0;
        private int fftSize = 0;

        private string header;
        private string footer;
        private string pointData;
        private string cellData;
        private string points;

        private int channelCount;

        private StringBuilder sb;
        private bool dataChanged;

        private bool createFileSeries = false;
        private int fileSeriesIndex = 1;
        private float maxFilesPerMinute = 300;
        private DateTime lastFileWriteUtc = DateTime.UtcNow;

        public SimpleVTSFileOutput(MultiChannelInput<IDataChannel> mci, string path, bool createFileSeries = false) : base(mci, path)
        {
            this.fileWriteThreshold = 0.0f;
            this.createFileSeries = createFileSeries;

            PhysicalBoundaries boundaries = ChannelMapper.GetChannelInputBoundaries(mci);
            var filter = FilterManager.FindFilter<FFTWFilter>(mci.GetChannel(0));
            if (filter == null) throw new Exception("SimpleVTSFileOutput supports FFTWFilter only.");
            var filterSettings = filter.GetSettings();
            if (filterSettings.OutputFormat != FFTOutputFormat.Magnitude) throw new InvalidDataException($"SimpleVTSFileOutput needs and FFT output format of 'Magnitude'. Your FFT filter has '{filterSettings.OutputFormat}'.");

            this.fftSize = filterSettings.FFTSampleCount;
            zSize = fftSize / 2;

            channelCount = mci.ChannelCount;

            header = $"<VTKFile type=\"StructuredGrid\" version=\"1.0\" byte_order=\"LittleEndian\" header_type=\"UInt64\">{Environment.NewLine}<StructuredGrid WholeExtent=\"0 {channelCount-1} 0 0 0 {zSize-1}\">{Environment.NewLine}<Piece Extent=\"0 {channelCount - 1} 0 0 0 {zSize - 1}\">{Environment.NewLine}";
            footer = $"</Piece>{Environment.NewLine}</StructuredGrid>{Environment.NewLine}</VTKFile>{Environment.NewLine}";
            cellData = $"<CellData>{Environment.NewLine}</CellData>{Environment.NewLine}";
            
            sb = new StringBuilder();
            lock (sb)
            {
                sb.AppendLine("<Points>");
                sb.AppendLine("<DataArray type=\"Float32\" Name=\"Points\" NumberOfComponents=\"3\" format=\"ascii\">");
                for (int z = 0; z < zSize; z++)
                {
                    for (int chIndex = 0; chIndex < channelCount; chIndex++)
                    {
                        PhysicalPosition pp = ChannelMapper.GetChannelPosition(mci.GetChannel(chIndex));

                        sb.AppendLine(pp.X.ToString("0.0000000000") + " " + pp.Y.ToString("0.0000000000") + " " + (pp.Z + (z * filter.FrequencyStep)).ToString("0.0000000000"));
                    }
                }
                sb.AppendLine("</DataArray>");
                sb.AppendLine("</Points>");
                points = sb.ToString();
            }
        }

        protected override void WriteDataToBuffer(float[][] dataToWrite, int dataCount)
        {
            lock (sb)
            {
                sb.Clear();
                sb.AppendLine("<PointData Scalars=\"Energy\" Vectors=\"Momentum\">");

                sb.Append("<DataArray type=\"Float32\" Name=\"Energy\" format=\"ascii\">");
                long valuesWritten = 0;
                for (int z = 0; z < zSize; z++)
                {
                    for (int chIndex = 0; chIndex < channelCount; chIndex++)
                    {
                        //float real = dataToWrite[chIndex][z * 2 + 0];
                        //float im = dataToWrite[chIndex][z * 2 + 1];

                        ////  Get the magnitude of the complex number sqrt((real * real) + (im * im))
                        //double magnitude = Math.Sqrt(real * real + im * im);
                        //magnitude /= fftSize;
                        float magnitude = dataToWrite[chIndex][z];

                        if (valuesWritten % 6 == 0) sb.AppendLine();
                        sb.Append(magnitude.ToString("0.0000000000"));
                        sb.Append(' ');

                        valuesWritten++;
                    }
                }
                sb.AppendLine();
                sb.AppendLine("</DataArray>");

                sb.Append("<DataArray type=\"Float32\" Name=\"Momentum\" NumberOfComponents=\"3\" format=\"ascii\">");
                string fixedMomentum = "0.0000000000 0.0000000000 0.0000000000";
                valuesWritten = 0;
                for (int z = 0; z < zSize; z++)
                {
                    for (int chIndex = 0; chIndex < channelCount; chIndex++)
                    {
                        if (valuesWritten % 6 == 0) sb.AppendLine();
                        sb.Append(fixedMomentum);
                        sb.Append(' ');

                        valuesWritten += 3;
                    }
                }
                sb.AppendLine();
                sb.AppendLine("</DataArray>");

                sb.AppendLine("</PointData>");

                pointData = sb.ToString();

                dataProcessed += dataCount;
                dataChanged = true;
            }
        }

        protected override void WriteDataFromBuffer(object stateInfo)
        {
            if ((DateTime.UtcNow - lastFileWriteUtc).TotalMilliseconds < ((1000.0f * 60.0f) / maxFilesPerMinute)) return;

            if (!dataChanged) return;
            dataChanged = false;

            int contentLength = 0;
            lock (filename)
            {
                string content = header + pointData + cellData + points + footer;
                contentLength = content.Length;

                File.WriteAllText(Path.Combine(directory, filename + (createFileSeries ? "_" + fileSeriesIndex.ToString("00000") : "") + ".vts"), content);
                fileSeriesIndex++;

                lastFileWriteUtc = DateTime.UtcNow;
                bytesWritten += content.Length;
            }

            OnDataWritten(this, new DataWrittenEventArgs(filename, contentLength, bytesWritten, this.DataOverflowWarningCount));
        }
    }
}
