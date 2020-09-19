using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public interface IChunk
    {
        Node Node { get; set; }
        GameBoxPart Part { get; set; }
        int Progress { get; set; }
        void OnLoad();
        void ReadWrite(Node n, GameBoxReaderWriter rw);
    }
}
