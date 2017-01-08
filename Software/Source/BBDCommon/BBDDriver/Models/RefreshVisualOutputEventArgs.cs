using BBDDriver.Models.Output;
using System;

namespace BBDDriver.Models
{
    public delegate void RefreshVisualOutputEventHandler(object sender, RefreshVisualOutputEventArgs e);

    public class RefreshVisualOutputEventArgs : EventArgs
    {
        public VisualOutputMode Mode;
        public int[] Dimensions;
        public float[,,] Values;
        public bool ChangedSinceLastRefresh;

        public RefreshVisualOutputEventArgs(VisualOutputMode mode, int[] dimensions, float[,,] values, bool changedSinceLastRefresh)
        {
            this.Mode = mode;
            this.Dimensions = dimensions;
            this.Values = values;
            this.ChangedSinceLastRefresh = changedSinceLastRefresh;
        }
    }
}
