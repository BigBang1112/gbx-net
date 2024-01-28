namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlock : IReadable, IWritable
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

    public void Read(IGbxReader r, int version = 0)
    {
        throw new NotImplementedException();
    }

    public void Write(IGbxWriter w, int version = 0)
    {
        throw new NotImplementedException();
    }
}
