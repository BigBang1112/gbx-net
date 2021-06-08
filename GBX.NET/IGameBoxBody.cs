using System.Collections.Generic;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public interface IGameBoxBody
    {
        GameBox GBX { get; }
        SortedDictionary<int, CMwNod> AuxilaryNodes { get; }
    }
}
