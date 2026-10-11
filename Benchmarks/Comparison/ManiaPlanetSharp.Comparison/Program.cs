using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using ManiaPlanetSharp.GameBox;
using ManiaPlanetSharp.GameBox.Parsing.Chunks;

var outputPath = args[1];
var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
try
{
    var bytes = File.ReadAllBytes(args[0]);
    var sampleCount = int.Parse(args[2], CultureInfo.InvariantCulture);
    var warmupSeconds = double.Parse(args[3], CultureInfo.InvariantCulture);
    var sampleMs = double.Parse(args[4], CultureInfo.InvariantCulture);
    GameBoxFile? lastResult = null;
    IEnumerable<Node>? lastChunks = null;

    void Parse()
    {
        using var stream = new MemoryStream(bytes, writable: false);
        lastResult = GameBoxFile.Parse(stream);
        // Parse() only reads the container. ParseBody() decompresses and scans its body.
        lastChunks = lastResult.ParseBody();
    }

    var diagnostics = new StringWriter(CultureInfo.InvariantCulture);
    Console.SetOut(diagnostics);
    Console.SetError(diagnostics);
    Parse();
    var problems = new List<string>
    {
        "Body parser scans for recognized chunks and suppresses parsing failures; complete body coverage is not verified."
    };
    problems.AddRange(diagnostics.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Distinct());
    problems.AddRange(lastResult!.HeaderChunks.OfType<UnknownChunk>().Select(chunk => $"Unparsed header chunk 0x{chunk.Id:X8}"));
    var map = lastChunks!.OfType<MapChunk>().FirstOrDefault();
    if (map?.Blocks is null) problems.Add("No parsed map block list.");
    else if (map.Blocks.Any(block => block is null)) problems.Add("Map block list contains unparsed entries.");
    var validation = new
    {
        MapUid = map?.Uid,
        BlockCount = map?.Blocks?.Count(block => block is not null && block.FlagsU != uint.MaxValue),
        ParsedChunkIds = lastChunks!.Select(chunk => $"0x{chunk.Id:X8}").Distinct().Order().ToArray(),
        Problems = problems
    };
    Console.SetOut(TextWriter.Null);
    Console.SetError(TextWriter.Null);
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
        for (var operationIndex = 0; operationIndex < operations; operationIndex++) Parse();
        samples[sampleIndex] = Stopwatch.GetElapsedTime(start).TotalMilliseconds / operations;
    }
    GC.KeepAlive(lastResult);
    GC.KeepAlive(lastChunks);
    File.WriteAllText(outputPath, JsonSerializer.Serialize(new
    {
        Status = "partial",
        Version = typeof(GameBoxFile).Assembly.GetName().Version?.ToString(),
        ParserMode = "runtime-generated",
        LzoImplementation = "upstream MiniLZO",
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
