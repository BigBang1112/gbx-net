using GBX.NET.Components;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Managers;

[ExcludeFromCodeCoverage]
public static partial class ClassManager
{
#if DEBUG
    public static partial string? GetName(uint classId, bool all = true);
    public static partial uint? GetId(string className, bool all = true);
#else
    public static partial string? GetName(uint classId, bool all);
    public static partial string? GetName(uint classId);
    public static partial uint? GetId(string className, bool all);
    public static partial uint? GetId(string className);
#endif

    /// <summary>
    /// Get the class ID when a type is provided. Slower and heavier on older .NET versions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static uint? GetClassId<T>() where T : IClass
    {
#if NET8_0_OR_GREATER
        return T.Id;
#else
        return ClassIds.TryGetValue(typeof(T), out var classId) ? classId : null;
#endif
    }

    public static partial Type? GetType(uint classId);

    internal static partial IClass? New(uint classId);
    internal static partial GbxHeader? NewHeader(GbxHeaderBasic basic, uint classId);
    internal static partial Gbx? NewGbx(GbxHeader header, IClass node);

    internal static uint GetChunkId(Type type) => throw new NotImplementedException();

    internal static partial IHeaderChunk? NewHeaderChunk(uint chunkId);
    internal static partial IChunk? NewChunk(uint chunkId);

    internal static partial uint Wrap(uint classId);
    internal static partial uint Unwrap(uint classId);
}
