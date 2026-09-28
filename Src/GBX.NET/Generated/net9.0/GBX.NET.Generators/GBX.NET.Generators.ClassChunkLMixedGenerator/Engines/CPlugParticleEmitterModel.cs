namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090B3000</remarks>
[Class(0x090B3000)]
public partial class CPlugParticleEmitterModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090B3000;




    private External<CPlugParticleEmitterSubModel>[]? particleEmitterSubModels;
    [AppliedWithChunk<Chunk090B3000>]
    public External<CPlugParticleEmitterSubModel>[]? ParticleEmitterSubModels { get => particleEmitterSubModels; set => particleEmitterSubModels = value; }

    private bool isSplashMode;
    [AppliedWithChunk<Chunk090B3002>]
    public bool IsSplashMode { get => isSplashMode; set => isSplashMode = value; }

    private float shadowMapTexelSize;
    [AppliedWithChunk<Chunk090B3002>]
    public float ShadowMapTexelSize { get => shadowMapTexelSize; set => shadowMapTexelSize = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugParticleEmitterModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugParticleEmitterModel() { }


    /// <summary>
    /// CPlugParticleEmitterModel 0x000 chunk
    /// </summary>
    [Chunk(0x090B3000)]
    public partial class Chunk090B3000 : Chunk<CPlugParticleEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090B3000;


        public override void ReadWrite(CPlugParticleEmitterModel n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugParticleEmitterSubModel>(ref n.particleEmitterSubModels!);
        }
    }

    /// <summary>
    /// CPlugParticleEmitterModel 0x001 chunk
    /// </summary>
    [Chunk(0x090B3001)]
    public partial class Chunk090B3001 : Chunk<CPlugParticleEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090B3001;

        public string? U01;

        public override void ReadWrite(CPlugParticleEmitterModel n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CPlugParticleEmitterModel 0x002 chunk
    /// </summary>
    [Chunk(0x090B3002)]
    public partial class Chunk090B3002 : Chunk<CPlugParticleEmitterModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090B3002;

        public int Version { get; set; }


        public override void ReadWrite(CPlugParticleEmitterModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.isSplashMode);
            if (Version >= 3)
            {
                rw.Single(ref n.shadowMapTexelSize);
            }
        }
    }

    /// <summary>
    /// CPlugParticleEmitterModel 0x003 chunk
    /// </summary>
    [Chunk(0x090B3003)]
    public partial class Chunk090B3003 : Chunk<CPlugParticleEmitterModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090B3003;

        public int Version { get; set; }

        public string[]? U01;

        public override void ReadWrite(CPlugParticleEmitterModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayString(ref U01!);
        }
    }

    /// <summary>
    /// CPlugParticleEmitterModel 0x004 chunk
    /// </summary>
    [Chunk(0x090B3004)]
    public partial class Chunk090B3004 : Chunk<CPlugParticleEmitterModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090B3004;

        public int Version { get; set; }

        public bool U01;
        public byte[]? U02;
        public byte[]? U03;
        public float U04;
        public int U05;
        public float U06;
        public int U07;
        public float U08;
        public float U09;
        public bool U10;
        public int U11;
        public bool U12;

        public override void ReadWrite(CPlugParticleEmitterModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                if (Version == 1)
                {
                    rw.Boolean(ref U01);
                    if (U01)
                    {
                        rw.Data(ref U02!, 40);
                    }
                }
            }
            if (Version >= 2)
            {
                if (Version == 2)
                {
                    rw.Data(ref U03!, 12);
                }
                rw.Single(ref U04);
                rw.Int32(ref U05);
                rw.Single(ref U06);
                rw.Int32(ref U07);
                rw.Single(ref U08);
                rw.Single(ref U09);
                rw.Boolean(ref U10);
                rw.Int32(ref U11);
                rw.Boolean(ref U12);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090B3000 => new Chunk090B3000(),
        0x090B3001 => new Chunk090B3001(),
        0x090B3002 => new Chunk090B3002(),
        0x090B3003 => new Chunk090B3003(),
        0x090B3004 => new Chunk090B3004(),
        _ => base.NewChunk(chunkId),
    };
}
