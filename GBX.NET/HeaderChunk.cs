using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public class HeaderChunk<T> : SkippableChunk<T> where T : Node
    {
        public bool IsHeavy { get; set; }

        public HeaderChunk()
        {

        }

        public HeaderChunk(T node, byte[] data) : base(node, data)
        {

        }

        public HeaderChunk(T node, uint id, byte[] data) : base(node, id, data)
        {

        }
    }
}
