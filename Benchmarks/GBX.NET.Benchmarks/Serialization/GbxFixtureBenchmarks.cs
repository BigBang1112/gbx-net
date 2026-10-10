using BenchmarkDotNet.Attributes;
using GBX.NET.LZO;

namespace GBX.NET.Benchmarks.Serialization;

[MemoryDiagnoser]
[BenchmarkCategory("Serialization")]
public class GbxFixtureBenchmarks
{
    private static readonly GbxReadSettings readSettings = new()
    {
        SafeSkippableChunks = false,
        IgnoreExceptionsInBody = false
    };

    private MemoryStream input = null!;
    private MemoryStream output = null!;
    private Gbx gbx = null!;

    public IEnumerable<GbxFixture> Fixtures => FixtureCatalog.All;

    [ParamsSource(nameof(Fixtures))]
    public GbxFixture Fixture { get; set; } = null!;

    [Params(GbxCompression.Uncompressed, GbxCompression.Compressed)]
    public GbxCompression Compression { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Gbx.LZO = new Lzo();
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gbx", Fixture.RelativePath);
        var original = Gbx.Parse(path, readSettings);
        original.BodyCompression = Compression;

        using var prepared = new MemoryStream();
        original.Save(prepared);
        input = new MemoryStream(prepared.ToArray(), writable: false);
        gbx = Gbx.Parse(input, readSettings);
        output = new MemoryStream(checked((int)prepared.Length));

        // Prime serialization caches and verify that the selected fixture can be saved
        // and fully parsed before measuring repeated operations on the prepared object.
        gbx.Save(output);
        output.Position = 0;
        var restored = Gbx.Parse(output, readSettings);
        if (restored.Node is null || restored.Body.Exception is not null
            || restored.Header.ClassId != gbx.Header.ClassId || restored.BodyCompression != Compression)
        {
            throw new InvalidOperationException($"Fixture '{Fixture}' failed benchmark round-trip validation.");
        }
    }

    [Benchmark]
    public Gbx Read()
    {
        input.Position = 0;
        return Gbx.Parse(input, readSettings);
    }

    [Benchmark]
    public long Write()
    {
        output.Position = 0;
        output.SetLength(0);
        gbx.Save(output);
        return output.Length;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        input?.Dispose();
        output?.Dispose();
    }
}
