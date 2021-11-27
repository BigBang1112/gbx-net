using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Benchmarks;

[IterationCount(50)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MarkdownExporterAttribute.GitHub]
public abstract class GameBoxParseBenchmark<T> : Benchmark where T : CMwNod
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
        OnGlobalSetup();
    }

    public virtual void OnGlobalSetup()
    {

    }

    [IterationSetup]
    public void IterationSetup()
    {
        stream.Seek(0, SeekOrigin.Begin);
        OnIterationSetup();
    }

    public virtual void OnIterationSetup()
    {

    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        stream.Dispose();
    }
}
