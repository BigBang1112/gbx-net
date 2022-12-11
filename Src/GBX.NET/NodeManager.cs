using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

public static partial class NodeManager
{
    public sealed record ChunkAttributes(string Description, bool ProcessSync, bool Ignore, bool AutoReadWrite);

    public static IReadOnlyDictionary<Type, uint> ClassIdsByType { get; }
    public static IReadOnlyDictionary<Type, uint> ChunkIdsByType { get; }
    public static IReadOnlyCollection<Type> WritingNotSupportedClassTypes { get; }
    public static IReadOnlyDictionary<uint, ChunkAttributes> ChunkAttributesById { get; }
    public static IReadOnlyDictionary<uint, ChunkAttributes> HeaderChunkAttributesById { get; }

    public static partial Type? GetClassTypeById(uint classId);
    public static partial IEnumerable<string> GetGbxExtensions(uint classId);
    public static partial Type? GetChunkTypeById(uint chunkId);
    public static partial Type? GetHeaderChunkTypeById(uint chunkId);
    public static partial Chunk? GetNewChunk(uint chunkId);
    public static partial Node? GetNewNode(uint classId);
    public static partial IHeaderChunk? GetNewHeaderChunk(uint chunkId);
    public static partial bool IsSkippableChunk(uint chunkId);
    public static partial bool IsAsyncChunk(uint chunkId);
    public static partial bool IsReadAsyncChunk(uint chunkId);
    public static partial bool IsWriteAsyncChunk(uint chunkId);
    public static partial bool IsReadWriteAsyncChunk(uint chunkId);

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public static bool TryGetName(uint classId, [NotNullWhen(true)] out string? name)
#else
    public static bool TryGetName(uint classId, out string? name)
#endif
    {
        name = GetName(classId);
        return name is not null;
    }
    
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public static bool TryGetExtension(uint classId, [NotNullWhen(true)] out string? extension)
#else
    public static bool TryGetExtension(uint classId, out string? extension)
#endif
    {
        extension = GetExtension(classId);
        return extension is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public static bool TryGetCollectionName(int collectionId, [NotNullWhen(true)] out string? name)
#else
    public static bool TryGetCollectionName(int collectionId, out string? name)
#endif
    {
        name = GetCollectionName(collectionId);
        return name is not null;
    }

    public static bool TryGetMapping(uint classId, out uint mappedClassId)
    {
        var mapped = GetMapping(classId);
        mappedClassId = mapped.GetValueOrDefault();
        return mapped.HasValue;
    }

    public static bool TryGetReverseMapping(uint classId, out uint mappedClassId)
    {
        var mapped = GetReverseMapping(classId);
        mappedClassId = mapped.GetValueOrDefault();
        return mapped.HasValue;
    }
}
