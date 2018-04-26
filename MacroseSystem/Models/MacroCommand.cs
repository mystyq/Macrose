using Macrose.Enums;
using System;

namespace Macrose.Models
{
    [Serializable]
    public class MacroCommand
    {
        public object Data { get; set; }
        public int Delay { get; set; }
        public MacroEvent Event { get; set; }
        public string CommandString
        {
            get { return $"{this.Event.ToString()}: {this.Data}"; }
        }
    }
}
