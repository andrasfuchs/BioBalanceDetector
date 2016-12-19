using System;

namespace BBDDriver.Models
{
    public delegate void RefreshVisualOutputEventHandler(object sender, RefreshVisualOutputEventArgs e);

    public class RefreshVisualOutputEventArgs : EventArgs
    {
        public int[] Dimensions;
        public float[,,] Values;
        public bool ChangedSinceLastRefresh;

        public RefreshVisualOutputEventArgs(int[] dimensions, float[,,] values, bool changedSinceLastRefresh)
        {
            this.Dimensions = dimensions;
            this.Values = values;
            this.ChangedSinceLastRefresh = changedSinceLastRefresh;
        }
    }
}
