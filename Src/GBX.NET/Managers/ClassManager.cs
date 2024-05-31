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
    public static string? GetName(Type type) => GetName(GetId(type) ?? 0);

    /// <summary>
    /// Get the class ID when a type is provided. Slower and heavier on older .NET versions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static uint? GetId<T>() where T : IClass
    {
#if NET8_0_OR_GREATER
        return T.Id;
#else
        return GetId(typeof(T));
#endif
    }

    public static uint? GetId(Type type)
    {
        return ClassIds.TryGetValue(type, out var classId) ? classId : null;
    }

    public static partial Type? GetType(uint classId);

    internal static partial IClass? New(uint classId);
    internal static partial GbxHeader? NewHeader(GbxHeaderBasic basic, uint classId);
    internal static partial Gbx? NewGbx(GbxHeader header, GbxBody body, IClass node);

    internal static partial IHeaderChunk? NewHeaderChunk(uint chunkId);
    internal static partial IChunk? NewChunk(uint chunkId);

    internal static partial uint Wrap(uint classId);
    internal static partial uint Unwrap(uint classId);

    internal static partial bool IsChunkIdRemapped(uint chunkId);
    public static partial bool IsClassWriteSupported(uint classId);

    public static IEnumerable<string> GetGbxExtensions(uint classId) => [];
}
