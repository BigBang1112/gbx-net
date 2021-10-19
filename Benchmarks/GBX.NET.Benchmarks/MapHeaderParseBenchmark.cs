using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Benchmarks
{
    public class MapHeaderParseBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
    {
        public MapHeaderParseBenchmark() : base(folder: "Maps")
        {

        }

        [Benchmark]
        public CGameCtnChallenge ParseMapHeader()
        {
            return GameBox.ParseNodeHeader<CGameCtnChallenge>(stream);
        }
    }
}
