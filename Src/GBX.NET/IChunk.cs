using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET;

public interface IChunk
{
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    void Read(Node n, GameBoxReader r);
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void Write(Node n, GameBoxWriter w);
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void ReadWrite(Node n, GameBoxReaderWriter rw);
}