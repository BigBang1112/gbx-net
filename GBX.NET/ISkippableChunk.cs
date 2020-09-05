using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET
{
    public interface ISkippableChunk
    {
        bool Discovered { get; set; }
        MemoryStream Stream { get; set; }

        void Write(GameBoxWriter w);
        void Discover();
    }
}
