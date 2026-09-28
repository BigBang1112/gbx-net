namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A033000</remarks>
[Class(0x0A033000)]
public partial class CSceneVehicleEnvironment : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A033000;




    private External<CPlugMaterial>[]? materials;
    [AppliedWithChunk<Chunk0A033002>]
    public External<CPlugMaterial>[]? Materials { get => materials; set => materials = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneVehicleEnvironment"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneVehicleEnvironment() { }


    /// <summary>
    /// CSceneVehicleEnvironment 0x000 chunk
    /// </summary>
    [Chunk(0x0A033000)]
    public partial class Chunk0A033000 : Chunk<CSceneVehicleEnvironment>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A033000;

        public bool U01;

        public override void ReadWrite(CSceneVehicleEnvironment n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSceneVehicleEnvironment 0x002 chunk
    /// </summary>
    [Chunk(0x0A033002)]
    public partial class Chunk0A033002 : Chunk<CSceneVehicleEnvironment>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A033002;


        public override void ReadWrite(CSceneVehicleEnvironment n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CPlugMaterial>(ref n.materials!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A033000 => new Chunk0A033000(),
        0x0A033002 => new Chunk0A033002(),
        _ => base.NewChunk(chunkId),
    };
}
