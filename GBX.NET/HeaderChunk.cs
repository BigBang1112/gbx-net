using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public class HeaderChunk : SkippableChunk
    {
        public bool IsHeavy { get; set; }

        public HeaderChunk(Node node, byte[] data) : base(node, data)
        {

        }

        public HeaderChunk(Node node, uint id, byte[] data) : base(node, id, data)
        {

        }
    }
}
