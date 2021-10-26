using System;
using System.Collections.Generic;
using System.Text;
using TmEssentials;

namespace GBX.NET
{
    /// <summary>
    /// Input from an input device.
    /// </summary>
    public class ControlEntry
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public uint Data { get; set; }
        public bool IsEnabled => Data != 0;

        public ControlEntry(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"[{Time.ToTmString()}] {Name}: {((Data == 128 || Data == 1 || Data == 0) ? IsEnabled.ToString() : Data.ToString())}";
        }
    }
}
