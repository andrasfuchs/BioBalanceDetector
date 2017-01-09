using BBDDriver.Models.Source;
using BBDDriver.Models.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BBDDriver.Models.Filter;

namespace BBDDriver.Models.Output
{
    public enum VisualOutputMode { NotSet, Waveform, Matrix, Spectrum, DominanceMatrix };

    public struct FrequencyValue
    {
        public float Frequency;
        public float Value;
    }

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
                int? blockSize = null;

                for (int i = 0; i<mci.ChannelCount; i++)
                {
                    channels[i, 0] = mci.GetChannel(i);

                    if (!(channels[i, 0] is FilteredDataChannel)) throw new Exception("Spectrum visual output mode is only available for frequency-domain channels.");
                    var filter = ((FilteredDataChannel)(channels[i, 0])).Filter as FFTWFilter;
                    if (filter == null) throw new Exception("Spectrum visual output mode supports FFTWFilter only.");

                    if (!blockSize.HasValue)
                    {
                        blockSize = filter.OutputBlockSize;
                    } if (blockSize != filter.OutputBlockSize) throw new Exception("All of the OutputBlockSizes of the filters of the multichannel input must be the same.");
                }

                dimensions = new int[3] { mci.ChannelCount, blockSize.Value, 1 };
                values = new float[mci.ChannelCount, blockSize.Value, 1];
            }
            else if (mode == VisualOutputMode.DominanceMatrix)
            {
                channels = ChannelMapper.Get2DChannelMatrix(mci);
                dimensions = new int[3] { channels.GetLength(0), channels.GetLength(1), 5 * 2 };    // will containg the top 5 frequencies and their strength
                values = new float[channels.GetLength(0), channels.GetLength(1), 5 * 2];
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

                    float[] chValues = e.Channel.GetData(dimensions[1]);
                    for (int i = 0; i < chValues.Length / 4; i++)
                    {
                        values[x, i, 0] = chValues[i];
                    }

                    changedSinceLastRefresh = true;
                }
            }
            else if (mode == VisualOutputMode.DominanceMatrix)
            {
                for (int x = 0; x < dimensions[0]; x++)
                {
                    for (int y = 0; y < dimensions[1]; y++)
                    {
                        if (channels[x, y] != e.Channel) continue;

                        var filter = ((FilteredDataChannel)(e.Channel)).Filter as FFTWFilter;

                        float[] frequencyValues = e.Channel.GetData(filter.OutputBlockSize);

                        // order freuencies by their values
                        FrequencyValue[] orderedByValue = new FrequencyValue[frequencyValues.Length / 2];
                        for (int i = 0; i< orderedByValue.Length; i++)
                        {
                            orderedByValue[i] = new FrequencyValue() { Frequency = frequencyValues[i * 2 + 0], Value = frequencyValues[i * 2 + 1] };
                        }
                        orderedByValue = orderedByValue.OrderByDescending(obv => obv.Value).ToArray();

                        for (int z = 0; z < dimensions[2] / 2; z++)
                        {
                            if (values[x, y, z * 2 + 1] != orderedByValue[z].Value)
                            {
                                values[x, y, z * 2 + 0] = orderedByValue[z].Frequency;
                                values[x, y, z * 2 + 1] = orderedByValue[z].Value;
                                changedSinceLastRefresh = true;
                            }
                        }
                    }
                }
            }

        }

        private void UpdateVisualOutput(object stateInfo)
        {
            RefreshVisualOutput?.Invoke(this, new RefreshVisualOutputEventArgs(this.mode, this.dimensions, this.values, this.changedSinceLastRefresh));
        }
    }
}
