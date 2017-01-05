using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BBDDriver.Models.Output
{
    public class SimpleVTSFileOutput : FileOutput
    {
        private int xSize = 0;
        private int ySize = 0;
        private int zSize = 0;

        private string header;
        private string footer;
        private string pointData;
        private string cellData;
        private string points;

        private int channelCount;
        private int[,] channelMapper;

        private StringBuilder sb;

        private bool createFileSeries = false;
        private int fileSeriesIndex = 1;

        public SimpleVTSFileOutput(MultiChannelInput<IDataChannel> mci, string path, int fftSize, bool createFileSeries = false) : base(mci, path)
        {
            this.createFileSeries = createFileSeries;

            int cellCount = ((mci.ChannelCount - 1) / 8) + 1;
            double frequencyStep = ((double)mci.SamplesPerSecond / 2) / fftSize;

            xSize = cellCount * 3;
            ySize = cellCount * 3;
            zSize = (mci.SamplesPerSecond / 2);

            channelCount = mci.ChannelCount;
            channelMapper = new int[4, 4];
            channelMapper[0, 0] = 0;
            channelMapper[1, 0] = 1;
            channelMapper[2, 0] = 2;
            channelMapper[2, 1] = 3;
            channelMapper[2, 2] = 4;
            channelMapper[1, 2] = 5;
            channelMapper[0, 2] = 6;
            channelMapper[0, 1] = 7;

            header = $"<VTKFile type=\"StructuredGrid\" version=\"1.0\" byte_order=\"LittleEndian\" header_type=\"UInt64\">{Environment.NewLine}<StructuredGrid WholeExtent=\"0 {xSize - 1} 0 {ySize - 1} 0 {zSize - 1}\">{Environment.NewLine}<Piece Extent=\"0 {xSize - 1} 0 {ySize - 1} 0 {zSize - 1}\">{Environment.NewLine}";
            footer = $"</Piece>{Environment.NewLine}</StructuredGrid>{Environment.NewLine}</VTKFile>{Environment.NewLine}";
            cellData = $"<CellData>{Environment.NewLine}</CellData>{Environment.NewLine}";
            
            sb = new StringBuilder();
            lock (sb)
            {
                sb.AppendLine("<Points>");
                sb.AppendLine("<DataArray type=\"Float32\" Name=\"Points\" NumberOfComponents=\"3\" format=\"ascii\">");
                for (int z = 0; z < zSize; z++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        for (int x = 0; x < xSize; x++)
                        {
                            sb.AppendLine(x.ToString("0.0000000000") + " " + y.ToString("0.0000000000") + " " + (z * frequencyStep).ToString("0.0000000000"));
                        }
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
                sb.AppendLine("<PointData Scalars=\"Density\" Vectors=\"Momentum\">");

                sb.Append("<DataArray type=\"Float32\" Name=\"Density\" format=\"ascii\">");
                long valuesWritten = 0;
                for (int z = 0; z < zSize; z++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        for (int x = 0; x < xSize; x++)
                        {
                            if (z % 2 == 1)
                            {
                                sb.Append("0.0000000000");
                                continue;
                            }

                            int chIndex = channelMapper[x%4,y%4];
                            if (chIndex >= channelCount) chIndex %= channelCount;

                            float real = dataToWrite[chIndex][z];
                            float im = dataToWrite[chIndex][z + 1];

                            //  Get the magnitude of the complex number sqrt((real * real) + (im * im))
                            double magnitude = Math.Sqrt(real * real + im * im);
                            double energy = magnitude * magnitude;

                            // energy /= fftSize


                            if (valuesWritten % 6 == 0) sb.AppendLine();
                            sb.Append(magnitude.ToString("0.0000000000"));
                            sb.Append(' ');

                            valuesWritten++;
                        }
                    }
                }
                sb.AppendLine();
                sb.AppendLine("</DataArray>");

                sb.Append("<DataArray type=\"Float32\" Name=\"Momentum\" NumberOfComponents=\"3\" format=\"ascii\">");
                string fixedMomentum = "0.0000000000 0.0000000000 0.0000000000";
                valuesWritten = 0;
                for (int z = 0; z < zSize; z++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        for (int x = 0; x < xSize; x++)
                        {
                            if (valuesWritten % 6 == 0) sb.AppendLine();
                            sb.Append(fixedMomentum);
                            sb.Append(' ');

                            valuesWritten += 3;
                        }
                    }
                }
                sb.AppendLine();
                sb.AppendLine("</DataArray>");

                sb.AppendLine("</PointData>");

                pointData = sb.ToString();
            }

            dataProcessed += dataCount;
        }

        protected override void WriteDataFromBuffer(object stateInfo)
        {
            if (dataProcessed == 0) return;

            lock (filename)
            {
                string content = header + pointData + cellData + points + footer;

                File.WriteAllText(Path.Combine(directory, filename + (createFileSeries ? "_" + fileSeriesIndex : "") + ".vts"), content);
                fileSeriesIndex++;

                bytesWritten += content.Length;
            }
        }
    }
}
