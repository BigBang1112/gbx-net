namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09179000</remarks>
[Class(0x09179000)]
public partial class NPlugTrigger_SSpecial : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09179000;




    private CPlugSurface? triggerShape;
    public CPlugSurface? TriggerShape { get => triggerShapeFile?.GetNode(ref triggerShape) ?? triggerShape; set => triggerShape = value; }
    private Components.GbxRefTableFile? triggerShapeFile;
    public Components.GbxRefTableFile? TriggerShapeFile { get => triggerShapeFile; set => triggerShapeFile = value; }
    public CPlugSurface? GetTriggerShape(GbxReadSettings settings = default, bool exceptions = false) => triggerShapeFile?.GetNode(ref triggerShape, settings, exceptions) ?? triggerShape;

    private bool isMergeable;
    public bool IsMergeable { get => isMergeable; set => isMergeable = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.NodeRef<CPlugSurface>(ref triggerShape, ref triggerShapeFile);
        if (Version>=2)
        {
            rw.Boolean(ref isMergeable);
        }
    }




}
