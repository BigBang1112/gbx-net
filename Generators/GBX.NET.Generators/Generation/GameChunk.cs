namespace GBX.NET.Generators.Generation;

internal sealed class GameChunk(LayoutModel owner, ChunkModel chunk)
{
    public LayoutModel Owner { get; } = owner;
    public ChunkModel Chunk { get; } = chunk;
    public bool HasVersion => Payloads().Any(static c => c.Scope.HasVersion);

    private readonly Dictionary<string, int> versions = new(StringComparer.Ordinal);

    public int Version(string game)
    {
        if (!versions.TryGetValue(game, out var version)) versions.Add(game, version = FindVersion(game));
        return version;
    }

    private int FindVersion(string game)
    {
        var qualifier = Chunk.Declaration.VersionQualifiers.FirstOrDefault(q => q.Label == game);
        if (qualifier?.MaxVersion is { } version) return version;
        foreach (var payload in Payloads())
        {
            var fallback = payload.Scope.Fields.FirstOrDefault(static f => f.IsVersion)?.Declaration.DefaultValue;
            if (fallback is not null && int.TryParse(ChunkL.ChunkLParser.WriteExpression(fallback), out var value)) return value;
        }
        return 0;
    }

    private IEnumerable<ChunkModel> Payloads()
    {
        var visited = new HashSet<uint>();
        for (var current = Chunk; visited.Add(current.Id);)
        {
            yield return current;
            var inherited = LayoutModel.Attribute(current.Declaration.Attributes, "base");
            if (inherited is null) yield break;
            var id = LayoutModel.Hex(inherited);
            if (id <= 0xFFF) id |= Owner.Id;
            current = Owner.Chunks.FirstOrDefault(c => c.Id == id);
            if (current is null) yield break;
        }
    }
}
