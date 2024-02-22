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
}
