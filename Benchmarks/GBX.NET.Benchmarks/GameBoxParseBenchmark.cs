using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Benchmarks
{
    [IterationCount(50)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MarkdownExporterAttribute.GitHub]
    public partial class GameBoxParseBenchmark<T> where T : CMwNod
    {
        private readonly string folder;

        protected MemoryStream stream = null!;

        [ParamsSource(nameof(GetFileNames))]
        public string FileName { get; set; } = string.Empty;

        public GameBoxParseBenchmark(string folder)
        {
            this.folder = folder;
        }

        public IEnumerable<string> GetFileNames()
        {
            return Directory.GetFiles(folder, "*.Gbx", SearchOption.AllDirectories)
                .Select(path => Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), folder), path));
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            stream = new MemoryStream(File.ReadAllBytes(Path.Combine(folder, FileName)));
        }

        [IterationSetup]
        public void IterationSetup()
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            stream.Dispose();
        }
    }
}
