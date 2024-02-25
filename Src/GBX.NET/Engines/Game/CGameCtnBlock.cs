namespace GBX.NET.Engines.Game;

[ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
public partial class CGameCtnBlock
{
    private string? name;
    /// <summary>
    /// Name of the block.
    /// </summary>
    public string Name
    {
        get => name ?? blockModel.Id;
        set => name = value;
    }

    private Ident blockModel = Ident.Empty;
    public Ident BlockModel
    {
        get
        {
            if (name is null)
            {
                return blockModel;
            }

            if (blockModel.Id == name)
            {
                return blockModel;
            }

            return blockModel = blockModel with { Id = name };
        }
        set
        {
            blockModel = value;
            name = value.Id;
        }
    }

    private Int3 coord;
    /// <summary>
    /// Position of the block on the map in block coordination.
    /// </summary>
    public Int3 Coord { get => coord; set => coord = value; }

    public override string ToString()
    {
        return $"{nameof(CGameCtnBlock)}: {Name} {coord}";
    }
}
