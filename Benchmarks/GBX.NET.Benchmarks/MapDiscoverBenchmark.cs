using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Benchmarks
{
    public class MapDiscoverBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
    {
        private CGameCtnChallenge map;

        public MapDiscoverBenchmark() : base(folder: "Maps")
        {
            map = null!;
        }

        public override void OnGlobalSetup()
        {
            map = GameBox.ParseNode<CGameCtnChallenge>(stream);
        }

        [Benchmark]
        public void DiscoverMap()
        {
            try
            {
                map.DiscoverAllChunks();
            }
            catch
            {

            }
        }
    }
}
