using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.PAK
{
    public class NadeoPakListItem
    {
        public string Name { get; set; }
        public byte Flags { get; set; }
        public byte[] Key { get; set; }

        public override string ToString()
        {
            return $"{Name} ({BitConverter.ToString(Key)})";
        }
    }
}
