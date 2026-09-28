namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A001000</remarks>
[Class(0x0A001000)]
public abstract partial class CScene : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A001000;




    private CSceneConfig? sceneConfig;
    [AppliedWithChunk<Chunk0A001003>]
    public CSceneConfig? SceneConfig { get => sceneConfig; set => sceneConfig = value; }

    private External<CMotionManager>[]? motionManagerModels;
    [AppliedWithChunk<Chunk0A001005>]
    public External<CMotionManager>[]? MotionManagerModels { get => motionManagerModels; set => motionManagerModels = value; }

    private OldSceneMobil[]? oldMobils;
    [AppliedWithChunk<Chunk0A001006>]
    public OldSceneMobil[]? OldMobils { get => oldMobils; set => oldMobils = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CScene"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CScene() { }


    /// <summary>
    /// CScene 0x003 chunk
    /// </summary>
    [Chunk(0x0A001003)]
    public partial class Chunk0A001003 : Chunk<CScene>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A001003;


        public override void ReadWrite(CScene n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneConfig>(ref n.sceneConfig);
        }
    }

    /// <summary>
    /// CScene 0x004 chunk
    /// </summary>
    [Chunk(0x0A001004)]
    public partial class Chunk0A001004 : Chunk<CScene>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A001004;

        public uint[]? U01;

        public override void ReadWrite(CScene n, GbxReaderWriter rw)
        {
            rw.Array<uint>(ref U01!);
        }
    }

    /// <summary>
    /// CScene 0x005 chunk
    /// </summary>
    [Chunk(0x0A001005)]
    public partial class Chunk0A001005 : Chunk<CScene>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A001005;


        public override void ReadWrite(CScene n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CMotionManager>(ref n.motionManagerModels!);
        }
    }

    /// <summary>
    /// CScene 0x006 chunk
    /// </summary>
    [Chunk(0x0A001006)]
    public partial class Chunk0A001006 : Chunk<CScene>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A001006;

        /// <inheritdoc />
        public override bool Ignore => true;


        public override void ReadWrite(CScene n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<OldSceneMobil>(ref n.oldMobils!);
        }
    }


    public sealed partial class OldSceneMobil : IReadableWritable
    {

        private CMwNod? u01;
        public CMwNod? U01 { get => u01File?.GetNode(ref u01) ?? u01; set => u01 = value; }
        private Components.GbxRefTableFile? u01File;
        public Components.GbxRefTableFile? U01File { get => u01File; set => u01File = value; }
        public CMwNod? GetU01(GbxReadSettings settings = default, bool exceptions = false) => u01File?.GetNode(ref u01, settings, exceptions) ?? u01;

        private CMwNod? u02;
        public CMwNod? U02 { get => u02File?.GetNode(ref u02) ?? u02; set => u02 = value; }
        private Components.GbxRefTableFile? u02File;
        public Components.GbxRefTableFile? U02File { get => u02File; set => u02File = value; }
        public CMwNod? GetU02(GbxReadSettings settings = default, bool exceptions = false) => u02File?.GetNode(ref u02, settings, exceptions) ?? u02;

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef<CMwNod>(ref u01, ref u01File);
            rw.NodeRef<CMwNod>(ref u02, ref u02File);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A001003 => new Chunk0A001003(),
        0x0A001004 => new Chunk0A001004(),
        0x0A001005 => new Chunk0A001005(),
        0x0A001006 => new Chunk0A001006(),
        _ => base.NewChunk(chunkId),
    };
}
