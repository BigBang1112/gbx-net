using GBX.NET.Managers;
#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.Components;

public sealed class GbxHeaderUnknown(GbxHeaderBasic basic, uint classId) : GbxHeader(basic)
{
    public override uint ClassId => classId;
    public ISet<IHeaderChunk> UserData { get; } = new SortedSet<IHeaderChunk>(ChunkIdComparer.Default);

    public override string ToString()
    {
        return $"GbxHeader ({ClassManager.GetName(ClassId)}, 0x{ClassId:X8}, unknown)";
    }

#if NET8_0_OR_GREATER
    [Experimental("GBXNET10001")]
#endif
#if NETSTANDARD2_0
    public override GbxHeader DeepClone()
#else
    public override GbxHeaderUnknown DeepClone()
#endif
    {
        var clone = new GbxHeaderUnknown(Basic, ClassId);

        foreach (var chunk in UserData)
        {
            clone.UserData.Add((IHeaderChunk)chunk.DeepClone());
        }

        return clone;
    }
}
