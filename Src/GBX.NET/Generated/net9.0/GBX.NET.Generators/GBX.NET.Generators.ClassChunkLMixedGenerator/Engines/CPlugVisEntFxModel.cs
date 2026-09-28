namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09115000</remarks>
[Class(0x09115000)]
public partial class CPlugVisEntFxModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09115000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugVisEntFxModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVisEntFxModel() { }


    /// <summary>
    /// CPlugVisEntFxModel 0x000 chunk
    /// </summary>
    [Chunk(0x09115000)]
    public partial class Chunk09115000 : Chunk<CPlugVisEntFxModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09115000;

        public int Version { get; set; }

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;
        public CPlugParticleEmitterModel? U02;
        public Components.GbxRefTableFile? U02File;
        public CPlugParticleEmitterModel? U03;
        public Components.GbxRefTableFile? U03File;
        public CPlugParticleEmitterModel? U04;
        public Components.GbxRefTableFile? U04File;
        public CFuncKeysReal? U05;
        public Components.GbxRefTableFile? U05File;
        public CFuncKeysReal? U06;
        public Components.GbxRefTableFile? U06File;
        public float U07;
        public float U08;
        public float U09;
        public GxLightBall? U10;

        public override void ReadWrite(CPlugVisEntFxModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U02, ref U02File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U03, ref U03File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U04, ref U04File);
            rw.NodeRef<CFuncKeysReal>(ref U05, ref U05File);
            rw.NodeRef<CFuncKeysReal>(ref U06, ref U06File);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.NodeRef<GxLightBall>(ref U10);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09115000 => new Chunk09115000(),
        _ => base.NewChunk(chunkId),
    };
}
