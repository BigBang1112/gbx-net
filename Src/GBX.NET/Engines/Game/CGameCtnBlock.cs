namespace GBX.NET.Engines.Game;

[ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
public partial class CGameCtnBlock
{
    /// <summary>
    /// Name of the block.
    /// </summary>
    public string Name
    {
        get => blockModel.Id;
        set => blockModel = blockModel with { Id = value };
    }

    private Int3 coord;
    /// <summary>
    /// Position of the block on the map in block coordination.
    /// </summary>
    public Int3 Coord { get => coord; set => coord = value; }

    public void Read(GbxReader r, int version = 0)
    {
        throw new NotImplementedException();
    }

    public void Write(GbxWriter w, int version = 0)
    {
        throw new NotImplementedException();
    }
}
