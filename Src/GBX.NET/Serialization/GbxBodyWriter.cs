using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxBodyWriter(GbxBody refTable, GbxWriter writer, GbxWriteSettings settings, GbxCompression compression)
{
    internal bool Write(IClass? node)
    {
        return false;
    }
}