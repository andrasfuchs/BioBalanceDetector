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
    public class VisualOutput
    {
        private int[] dimensions;
        private float[,,] values;
        private bool changedSinceLastRefresh;

        private Timer refreshVisualOutputTimer;

        public event RefreshVisualOutputEventHandler RefreshVisualOutput;

        public VisualOutput(MultiChannelInput<IDataChannel> mci, int maxFramesPerSecond, int sizeX, int sizeY = 1, int sizeZ = 1)
        {
            values = new float[sizeX, sizeY, sizeZ];
            dimensions = new int[3] { sizeX, sizeY, sizeZ };

            mci.DataChanged += DataChanged;

            refreshVisualOutputTimer = new Timer(UpdateVisualOutput, this, 0, 1000 / maxFramesPerSecond);
        }

        private void DataChanged(object sender, DataChangedEventArgs e)
        {
            PhysicalPosition pp = ChannelMapper.GetChannelPosition(e.Channel);

            float value = e.Channel.GetData(1)[0];
            //if (values[pp.X, pp[1], pp[2]] != value)
            //{
            //    values[pp[0], pp[1], pp[2]] = value;
            //    changedSinceLastRefresh = true;
            //}
            changedSinceLastRefresh = true;
        }

        private void UpdateVisualOutput(object stateInfo)
        {
            RefreshVisualOutput?.Invoke(this, new RefreshVisualOutputEventArgs(this.dimensions, this.values, this.changedSinceLastRefresh));
        }
    }
}
