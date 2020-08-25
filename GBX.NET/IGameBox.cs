using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public interface IGameBox
    {
        ClassIDRemap Game { get; set; }
        uint? ClassID { get; }

        TChunk CreateBodyChunk<TChunk>() where TChunk : Chunk => CreateBodyChunk<TChunk>(new byte[0]);
        TChunk CreateBodyChunk<TChunk>(byte[] data) where TChunk : Chunk;
        void RemoveAllBodyChunks();
        bool RemoveBodyChunk<TChunk>() where TChunk : Chunk;
    }
}
