using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET;

public interface IChunk
{
    uint ID { get; }
    void Read(CMwNod n, GameBoxReader r);
    void Write(CMwNod n, GameBoxWriter w);
    void ReadWrite(CMwNod n, GameBoxReaderWriter rw);
}