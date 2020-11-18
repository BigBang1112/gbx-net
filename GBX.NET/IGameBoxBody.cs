using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public interface IGameBoxBody
    {
        GameBox GBX { get; }
        SortedDictionary<int, Node> AuxilaryNodes { get; }
    }
}
