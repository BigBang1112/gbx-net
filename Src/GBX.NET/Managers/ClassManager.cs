using GBX.NET.Components;

namespace GBX.NET.Managers;

public static partial class ClassManager
{
#if DEBUG
    public static partial string? GetName(uint classId, bool all = true);
#else
    public static partial string? GetName(uint classId, bool all);
    public static partial string? GetName(uint classId);
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

    internal static IHeaderChunk? NewHeaderChunk(uint chunkId) => chunkId switch
    {
        0x03043002 => new CGameCtnChallenge.HeaderChunk03043002(),
        0x03043003 => new CGameCtnChallenge.HeaderChunk03043003(),
        0x03043004 => new CGameCtnChallenge.HeaderChunk03043004(),
        0x03043005 => new CGameCtnChallenge.HeaderChunk03043005(),
        0x03043007 => new CGameCtnChallenge.HeaderChunk03043007(),
        0x03043008 => new CGameCtnChallenge.HeaderChunk03043008(),
        _ => null
    };

    internal static IChunk? NewChunk(uint chunkId) => null;

    internal static uint Remap(uint classId, ClassIdRemapMode remapMode)
    {
        return classId;
    }
}
