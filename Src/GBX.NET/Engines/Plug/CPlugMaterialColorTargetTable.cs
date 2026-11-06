namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterialColorTargetTable
{
    public CPlugMaterialColorTargetTable()
    {
        ColorTargets = CreateDefaultColorTargets();
    }

    private static ColorPair[] CreateDefaultColorTargets()
    {
        var targets = new ColorPair[13];
        for (var i = 0; i < targets.Length; i++) targets[i] = new ColorPair();
        return targets;
    }

    public override void ReadWrite(GbxReaderWriter rw) => ReadWrite(rw, v: 0);
}
