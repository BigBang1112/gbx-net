using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public class ControlEntry
    {
        public int Time { get; }
        public byte ControlNameIndex { get; }
        public bool Enable { get; }

        public ControlEntry(int time, byte controlNameIndex, bool enable)
        {
            Time = time;
            ControlNameIndex = controlNameIndex;
            Enable = enable;
        }
    }
}
