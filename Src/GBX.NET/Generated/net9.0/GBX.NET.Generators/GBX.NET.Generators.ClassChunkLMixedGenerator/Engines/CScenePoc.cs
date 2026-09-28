namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A009000</remarks>
[Class(0x0A009000)]
public abstract partial class CScenePoc : CSceneObject, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A009000;




    private bool isActive;
    [AppliedWithChunk<Chunk0A009000>]
    public bool IsActive { get => isActive; set => isActive = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CScenePoc"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CScenePoc() { }


    /// <summary>
    /// CScenePoc 0x000 chunk
    /// </summary>
    [Chunk(0x0A009000)]
    public partial class Chunk0A009000 : Chunk<CScenePoc>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A009000;


        public override void ReadWrite(CScenePoc n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isActive);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A009000 => new Chunk0A009000(),
        _ => base.NewChunk(chunkId),
    };
}
