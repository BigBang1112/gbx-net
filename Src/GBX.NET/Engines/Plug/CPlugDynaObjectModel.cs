using GBX.NET.Components;

namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaObjectModel
{
    /// <summary>
    /// Collision shape used by the Maniaplanet version 0 archive.
    /// </summary>
    public CPlugSurface? Shape { get => DynaShape; set => DynaShape = value; }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
