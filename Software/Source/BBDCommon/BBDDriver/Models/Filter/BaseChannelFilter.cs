using BBDDriver.Models.Source;
using System;

namespace BBDDriver.Models.Filter
{
    public class BaseChannelFilter<T> : IChannelFilter where T : ChannelFilterSettings, new()
    {
        public IDataChannel Input { get; private set; }
        public IDataChannel Output { get; private set; }

        protected T settings; 
        public ChannelFilterSettings Settings
        {
            get
            {
                return settings;
            }

            set
            {
                if (value is T)
                {
                    settings = value as T;
                    OnSettingsChanged(new SettingsChangedEventArgs(settings));
                }
            }
        }

        public event SettingsChangedEventHandler SettingsChanged;

        public BaseChannelFilter()
        {
            settings = new T();
        }

        public IDataChannel Apply(IDataChannel input)
        {
            FilteredDataChannel output = new FilteredDataChannel(input, input.SamplesPerSecond, input.BufferSize, this);

            this.Input = input;
            this.Output = output;

            this.Input.DataChanged += InputDataChanged;

            return output;
        }

        protected virtual void InputDataChanged(object sender, DataChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public IChannelFilter Copy()
        {
            IChannelFilter clone = (IChannelFilter)Activator.CreateInstance(this.GetType());
            clone.Settings = this.settings;
            return clone;
        }

        public T GetSettings()
        {
            return settings;
        }

        protected virtual void OnSettingsChanged(SettingsChangedEventArgs e)
        {
            SettingsChanged?.Invoke(this, e);
        }
    }
}
