using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public interface IGameBoxBody : ILookbackable
    {
        IGameBox GBX { get; }
        List<Node> AuxilaryNodes { get; }

        void RemoveAllChunks();
        bool RemoveChunk<TChunk>() where TChunk : Chunk;
    }
}
