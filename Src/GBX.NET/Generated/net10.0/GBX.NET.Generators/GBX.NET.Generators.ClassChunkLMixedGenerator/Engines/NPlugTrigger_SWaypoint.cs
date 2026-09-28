namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09178000</remarks>
[Class(0x09178000)]
public partial class NPlugTrigger_SWaypoint : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x09178000;




    private EGameItemWaypointType type;
    public EGameItemWaypointType Type { get => type; set => type = value; }

    private CPlugSurface? triggerShape;
    public CPlugSurface? TriggerShape { get => triggerShapeFile?.GetNode(ref triggerShape) ?? triggerShape; set => triggerShape = value; }
    private Components.GbxRefTableFile? triggerShapeFile;
    public Components.GbxRefTableFile? TriggerShapeFile { get => triggerShapeFile; set => triggerShapeFile = value; }
    public CPlugSurface? GetTriggerShape(GbxReadSettings settings = default, bool exceptions = false) => triggerShapeFile?.GetNode(ref triggerShape, settings, exceptions) ?? triggerShape;

    private bool noRespawn;
    public bool NoRespawn { get => noRespawn; set => noRespawn = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.EnumInt32<EGameItemWaypointType>(ref type);
        rw.NodeRef<CPlugSurface>(ref triggerShape, ref triggerShapeFile);
        rw.Boolean(ref noRespawn);
    }




    public enum EGameItemWaypointType
    {
        Start,
        Finish,
        Checkpoint,
        None,
        StartFinish,
        Dispenser,
    }

}
