namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090E6000</remarks>
[Class(0x090E6000)]
public partial class CPlugVehicleVisEmitterModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090E6000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehicleVisEmitterModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehicleVisEmitterModel() { }


    /// <summary>
    /// CPlugVehicleVisEmitterModel 0x002 chunk
    /// </summary>
    [Chunk(0x090E6002)]
    public partial class Chunk090E6002 : Chunk<CPlugVehicleVisEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E6002;

        public bool U01;

        public override void ReadWrite(CPlugVehicleVisEmitterModel n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleVisEmitterModel 0x003 chunk
    /// </summary>
    [Chunk(0x090E6003)]
    public partial class Chunk090E6003 : Chunk<CPlugVehicleVisEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E6003;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVehicleVisEmitterModel n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    /// <summary>
    /// CPlugVehicleVisEmitterModel 0x004 chunk
    /// </summary>
    [Chunk(0x090E6004)]
    public partial class Chunk090E6004 : Chunk<CPlugVehicleVisEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E6004;

        public int U01;
        public CPlugParticleEmitterModel? U02;
        public Components.GbxRefTableFile? U02File;
        public CPlugParticleEmitterModel? U03;
        public Components.GbxRefTableFile? U03File;
        public CPlugParticleEmitterModel? U04;
        public Components.GbxRefTableFile? U04File;
        public int U05;
        public int U06;
        public int U07;
        public bool U08;
        public bool U09;
        public bool U10;
        public Iso4 U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public float U17;
        public float U18;
        public float U19;
        public float U20;
        public float U21;
        public float U22;
        public float U23;
        public float U24;
        public float U25;

        public override void ReadWrite(CPlugVehicleVisEmitterModel n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U02, ref U02File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U03, ref U03File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U04, ref U04File);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Boolean(ref U08);
            rw.Boolean(ref U09);
            rw.Boolean(ref U10);
            rw.Iso4(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Single(ref U17);
            rw.Single(ref U18);
            rw.Single(ref U19);
            rw.Single(ref U20);
            rw.Single(ref U21);
            rw.Single(ref U22);
            rw.Single(ref U23);
            rw.Single(ref U24);
            rw.Single(ref U25);
        }
    }

    /// <summary>
    /// CPlugVehicleVisEmitterModel 0x005 chunk
    /// </summary>
    [Chunk(0x090E6005)]
    public partial class Chunk090E6005 : Chunk<CPlugVehicleVisEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E6005;

        public bool U01;

        public override void ReadWrite(CPlugVehicleVisEmitterModel n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleVisEmitterModel 0x006 chunk
    /// </summary>
    [Chunk(0x090E6006)]
    public partial class Chunk090E6006 : Chunk<CPlugVehicleVisEmitterModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E6006;

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugVehicleVisEmitterModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090E6002 => new Chunk090E6002(),
        0x090E6003 => new Chunk090E6003(),
        0x090E6004 => new Chunk090E6004(),
        0x090E6005 => new Chunk090E6005(),
        0x090E6006 => new Chunk090E6006(),
        _ => base.NewChunk(chunkId),
    };
}
