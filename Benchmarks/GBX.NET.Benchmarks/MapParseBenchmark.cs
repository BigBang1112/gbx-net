using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Benchmarks;

public class MapParseBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
{
    public MapParseBenchmark() : base(folder: "Maps")
    {

    }

    [Benchmark]
    public CGameCtnChallenge ParseMap()
    {
        return GameBox.ParseNode<CGameCtnChallenge>(stream);
    }
}
