using GBX.NET.Managers;

namespace GBX.NET.Components;

public sealed class GbxHeader<T>(GbxHeaderBasic basic) : GbxHeader(basic) where T : IClass
{
#if !NET8_0_OR_GREATER
    private uint? classId;
#endif

    public override uint ClassId =>
#if NET8_0_OR_GREATER
        T.Id;
#else
        classId ??= ClassManager.GetId<T>() ?? throw new InvalidOperationException($"Class ID of {typeof(T).Name} is not available.");
#endif

#if NETSTANDARD2_0
    public override GbxHeader DeepClone() => new GbxHeader<T>(Basic) { NumNodes = NumNodes };
#else
    public override GbxHeader<T> DeepClone() => new(Basic) { NumNodes = NumNodes };
#endif

    public override string ToString()
    {
        return $"GbxHeader ({ClassManager.GetName(ClassId)}, 0x{ClassId:X8}, known)";
    }
}
