using System;

namespace Assistment.form
{
    public class WertEventArgs : EventArgs
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public WertEventArgs(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
