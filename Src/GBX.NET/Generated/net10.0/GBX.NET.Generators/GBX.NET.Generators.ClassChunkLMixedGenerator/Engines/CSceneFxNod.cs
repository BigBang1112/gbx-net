namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A03A000</remarks>
[Class(0x0A03A000)]
public partial class CSceneFxNod : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A03A000;




    private CSceneFx? fx;
    [AppliedWithChunk<Chunk0A03A000>]
    public CSceneFx? Fx { get => fxFile?.GetNode(ref fx) ?? fx; set => fx = value; }
    private Components.GbxRefTableFile? fxFile;
    public Components.GbxRefTableFile? FxFile { get => fxFile; set => fxFile = value; }
    public CSceneFx? GetFx(GbxReadSettings settings = default, bool exceptions = false) => fxFile?.GetNode(ref fx, settings, exceptions) ?? fx;

    private CSceneFxNod[]? nodInputs;
    [AppliedWithChunk<Chunk0A03A001>]
    public CSceneFxNod[]? NodInputs { get => nodInputs; set => nodInputs = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneFxNod"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneFxNod() { }


    /// <summary>
    /// CSceneFxNod 0x000 chunk
    /// </summary>
    [Chunk(0x0A03A000)]
    public partial class Chunk0A03A000 : Chunk<CSceneFxNod>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A03A000;


        public override void ReadWrite(CSceneFxNod n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneFx>(ref n.fx, ref n.fxFile);
        }
    }

    /// <summary>
    /// CSceneFxNod 0x001 chunk
    /// </summary>
    [Chunk(0x0A03A001)]
    public partial class Chunk0A03A001 : Chunk<CSceneFxNod>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A03A001;

        public int U01;

        public override void ReadWrite(CSceneFxNod n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CSceneFxNod>(ref n.nodInputs!);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSceneFxNod 0x002 chunk
    /// </summary>
    [Chunk(0x0A03A002)]
    public partial class Chunk0A03A002 : Chunk<CSceneFxNod>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0A03A002;

        public int Version { get; set; }

        public CMwNod? U01;
        public CMwNod? U02;
        public Components.GbxRefTableFile? U02File;

        public override void ReadWrite(CSceneFxNod n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CMwNod>(ref U01);
            rw.NodeRef<CMwNod>(ref U02, ref U02File);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A03A000 => new Chunk0A03A000(),
        0x0A03A001 => new Chunk0A03A001(),
        0x0A03A002 => new Chunk0A03A002(),
        _ => base.NewChunk(chunkId),
    };
}
