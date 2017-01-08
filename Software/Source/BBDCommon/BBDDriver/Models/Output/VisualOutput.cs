using BBDDriver.Models.Source;
using BBDDriver.Models.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBDDriver.Models.Output
{
    public enum VisualOutputMode { NotSet, Waveform, Matrix, Spectrum };

    public class VisualOutput
    {
        private VisualOutputMode mode;
        private int[] dimensions;
        private IDataChannel[,] channels;
        private float[,,] values;
        private bool changedSinceLastRefresh;

        private Timer refreshVisualOutputTimer;

        public event RefreshVisualOutputEventHandler RefreshVisualOutput;

        public VisualOutput(MultiChannelInput<IDataChannel> mci, int maxFramesPerSecond, VisualOutputMode mode)
        {
            this.mode = mode;
            if (mode == VisualOutputMode.Matrix)
            {
                channels = ChannelMapper.Get2DChannelMatrix(mci);
                dimensions = new int[3] { channels.GetLength(0), channels.GetLength(1), 1 };
                values = new float[channels.GetLength(0), channels.GetLength(1), 1];
            } else if (mode == VisualOutputMode.Waveform)
            {
                channels = new IDataChannel[mci.ChannelCount, 1];
                for (int i = 0; i < mci.ChannelCount; i++)
                {
                    channels[i, 0] = mci.GetChannel(i);
                }
                dimensions = new int[3] { mci.ChannelCount, mci.SamplesPerSecond, 1 };
                values = new float[mci.ChannelCount, mci.SamplesPerSecond, 1];
            } else if (mode == VisualOutputMode.Spectrum)
            {
                channels = new IDataChannel[mci.ChannelCount, 1];
                for (int i = 0; i<mci.ChannelCount; i++)
                {
                    channels[i, 0] = mci.GetChannel(i);
                }

                int fftSize = 1024;

                dimensions = new int[3] { mci.ChannelCount, fftSize, 1 };
                values = new float[mci.ChannelCount, fftSize, 1];
            }

            mci.DataChanged += DataChanged;

            refreshVisualOutputTimer = new Timer(UpdateVisualOutput, this, 0, 1000 / maxFramesPerSecond);
        }

        private void DataChanged(object sender, DataChangedEventArgs e)
        {
            if (mode == VisualOutputMode.Matrix)
            {
                for (int x = 0; x < dimensions[0]; x++)
                {
                    for (int y = 0; y < dimensions[1]; y++)
                    {
                        for (int z = 0; z < dimensions[2]; z++)
                        {
                            if (channels[x, y] != e.Channel) continue;

                            float value = e.Channel.GetData(1)[0];
                            if (values[x, y, z] != value)
                            {
                                values[x, y, z] = value;
                                changedSinceLastRefresh = true;
                            }
                        }
                    }
                }
            }
            else if (mode == VisualOutputMode.Waveform)
            {
                for (int x = 0; x < dimensions[0]; x++)
                {
                    if (channels[x, 0] != e.Channel) continue;

                    float[] chValues = e.Channel.GetData(e.Channel.SamplesPerSecond);
                    for (int i = 0; i < chValues.Length; i++)
                    {
                        values[x, i, 0] = chValues[i];
                    }

                    changedSinceLastRefresh = true;
                }
            }
            else if (mode == VisualOutputMode.Spectrum)
            {
                for (int x = 0; x < dimensions[0]; x++)
                {
                    if (channels[x, 0] != e.Channel) continue;

                    float[] chValues = e.Channel.GetData(1024 * 2);
                    for (int i = 0; i < chValues.Length / 2; i++)
                    {
                        float real = chValues[i * 2 + 0];
                        float im = chValues[i * 2 + 1];

                        //  Get the magnitude of the complex number sqrt((real * real) + (im * im))
                        double magnitude = Math.Sqrt(real * real + im * im);
                        magnitude /= 1024;

                        values[x, i, 0] = (float)magnitude;
                    }

                    changedSinceLastRefresh = true;
                }
            }

        }

        private void UpdateVisualOutput(object stateInfo)
        {
            RefreshVisualOutput?.Invoke(this, new RefreshVisualOutputEventArgs(this.mode, this.dimensions, this.values, this.changedSinceLastRefresh));
        }
    }
}
