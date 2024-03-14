using GBX.NET.Managers;

namespace GBX.NET.Components;

public sealed class GbxHeader<T>(GbxHeaderBasic basic) : GbxHeader(basic) where T : IClass
{
#if !NET8_0_OR_GREATER
    private uint? classId;
#endif

    public override uint ClassId
    {
        get
        {
#if NET8_0_OR_GREATER
            return T.Id;
#else
            return classId ??= ClassManager.GetClassId<T>() ?? throw new Exception("Class ID not available");
#endif
        }
    }

    public override string ToString()
    {
        return $"GbxHeader ({ClassManager.GetName(ClassId)}, 0x{ClassId:X8}, known)";
    }
}