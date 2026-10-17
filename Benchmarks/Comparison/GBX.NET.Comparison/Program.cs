using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.Serialization.Chunking;

var outputPath = args[1];
var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
try
{
    var bytes = File.ReadAllBytes(args[0]);
    var sampleCount = int.Parse(args[2], CultureInfo.InvariantCulture);
    var warmupSeconds = double.Parse(args[3], CultureInfo.InvariantCulture);
    var sampleMs = double.Parse(args[4], CultureInfo.InvariantCulture);
    Gbx.LZO = new Lzo();
    var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
    Gbx? lastResult = null;

    void Parse()
    {
        using var stream = new MemoryStream(bytes, writable: false);
        lastResult = Gbx.Parse(stream, settings);
    }

    Parse();
    if (lastResult!.Node is not CGameCtnChallenge map || lastResult.Body.Exception is not null || map.Blocks is null)
    {
        throw new InvalidOperationException("Parsing did not produce a map body with blocks.");
    }
    var problems = map.Chunks.Where(chunk => chunk.GetType() == typeof(SkippableChunk))
        .Select(chunk => $"Unparsed map chunk 0x{chunk.Id:X8}").ToArray();
    var validation = new { map.MapUid, BlockCount = map.Blocks.Count, ItemCount = map.AnchoredObjects?.Count, Problems = problems };
    var warmup = Stopwatch.StartNew();
    var warmupOperations = 0;
    do
    {
        Parse();
        warmupOperations++;
    }
    while (warmup.Elapsed.TotalSeconds < warmupSeconds);
    var operations = Math.Clamp((int)Math.Ceiling(sampleMs * warmupOperations / warmup.Elapsed.TotalMilliseconds), 1, 10000);
    var samples = new double[sampleCount];
    for (var sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
    {
        var start = Stopwatch.GetTimestamp();
        for (var operationIndex = 0; operationIndex < operations; operationIndex++)
        {
            Parse();
        }
        samples[sampleIndex] = Stopwatch.GetElapsedTime(start).TotalMilliseconds / operations;
    }
    GC.KeepAlive(lastResult);
    File.WriteAllText(outputPath, JsonSerializer.Serialize(new
    {
        Status = problems.Length > 0 ? "partial" : "ok",
        Version = typeof(Gbx).Assembly.GetName().Version?.ToString(),
        LzoVersion = typeof(Lzo).Assembly.GetName().Version?.ToString(),
        Runtime = RuntimeInformation.FrameworkDescription,
        Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
        Validation = validation,
        WarmupOperations = warmupOperations,
        OperationsPerSample = operations,
        SamplesMs = samples
    }, options));
}
catch (Exception exception)
{
    File.WriteAllText(outputPath, JsonSerializer.Serialize(new { Status = "error", Error = exception.ToString() }, options));
    Environment.ExitCode = 1;
}
