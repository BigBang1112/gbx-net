using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Game;
using System.Linq.Expressions;

namespace GBX.NET.Benchmarks
{
    [MemoryDiagnoser]
    public class Program
    {
        private static Dictionary<string, MemoryStream> filesInMemory = new();

        private static MemoryStream maniaPlanetMap;
        private static MemoryStream maniaPlanetMapUncompressed = new MemoryStream();
        private static MemoryStream summer2021Map;
        private static MemoryStream summer2021MapUncompressed = new MemoryStream();

        static void Main(string[] args)
        {
            var results = BenchmarkRunner.Run<Program>();
        }

        [GlobalSetup]
        public void Setup()
        {
            foreach (var mapFile in Directory.GetFiles("Maps", "*.Gbx", SearchOption.AllDirectories))
            {
                filesInMemory[mapFile] = new MemoryStream(File.ReadAllBytes(mapFile));
            }

            maniaPlanetMap = filesInMemory["Maps\\CCP#04 - ODYSSEY.Map.Gbx"];
            var parsedManiaPlanetMap = GameBox.ParseNode(maniaPlanetMap);
            parsedManiaPlanetMap.GBX.Header.CompressionOfBody = GameBoxCompression.Uncompressed;
            parsedManiaPlanetMap.Save(maniaPlanetMapUncompressed);

            summer2021Map = filesInMemory["Maps\\8_Trackmania 2020\\20210701\\Summer 2021 - 25.Map.Gbx"];
            var parsed2021Map = GameBox.ParseNode(summer2021Map);
            parsed2021Map.GBX.Header.CompressionOfBody = GameBoxCompression.Uncompressed;
            parsed2021Map.Save(summer2021MapUncompressed);
        }

        [Benchmark]
        public CMwNod ParseManiaPlanetMap()
        {
            maniaPlanetMap.Position = 0;
            return GameBox.ParseNode(maniaPlanetMap);
        }

        [Benchmark]
        public CMwNod ParseManiaPlanetMapNoCompression()
        {
            maniaPlanetMapUncompressed.Position = 0;
            return GameBox.ParseNode(maniaPlanetMapUncompressed);
        }

        [Benchmark]
        public CMwNod ParseSummer2021()
        {
            summer2021Map.Position = 0;
            return GameBox.ParseNode(summer2021Map);
        }

        [Benchmark]
        public CMwNod ParseSummer2021NoCompression()
        {
            summer2021MapUncompressed.Position = 0;
            return GameBox.ParseNode(summer2021MapUncompressed);
        }

        [Benchmark]
        public CMwNod ParseSummer2021Header()
        {
            summer2021Map.Position = 0;
            return GameBox.ParseNodeHeader(summer2021Map);
        }

        /*[Benchmark]
        public CGameCtnChallenge Noda()
        {
            return new CGameCtnChallenge();
        }

        [Benchmark]
        public CGameCtnChallenge.Chunk03043040 NodaChunko()
        {
            return new CGameCtnChallenge.Chunk03043040();
        }*/

        [Benchmark]
        public CMwNod ParseSummer2020Header()
        {
            var stream = filesInMemory["Maps\\8_Trackmania 2020\\20200701\\Summer 2020 - 11.Map.Gbx"];
            stream.Position = 0;
            return GameBox.ParseNodeHeader(stream);
        }

        [Benchmark]
        public CMwNod ParseSummer2020()
        {
            var stream = filesInMemory["Maps\\8_Trackmania 2020\\20200701\\Summer 2020 - 11.Map.Gbx"];
            stream.Position = 0;
            return GameBox.ParseNode(stream);
        }
    }
}
