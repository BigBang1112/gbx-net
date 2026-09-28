namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F1000</remarks>
[Class(0x090F1000)]
public partial class CPlugVehicleMaterial : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F1000;




    private byte materialId;
    [AppliedWithChunk<Chunk090F1001>]
    [AppliedWithChunk<Chunk090F1009>]
    [AppliedWithChunk<Chunk090F100E>]
    public byte MaterialId { get => materialId; set => materialId = value; }

    private bool trailIsOnlyWhenSliding;
    [AppliedWithChunk<Chunk090F1001>]
    [AppliedWithChunk<Chunk090F1009>]
    public bool TrailIsOnlyWhenSliding { get => trailIsOnlyWhenSliding; set => trailIsOnlyWhenSliding = value; }

    private CPlugShader? trailShader;
    [AppliedWithChunk<Chunk090F1001>]
    public CPlugShader? TrailShader { get => trailShader; set => trailShader = value; }

    private float vibrationSpeedCoef;
    [AppliedWithChunk<Chunk090F1001>]
    [AppliedWithChunk<Chunk090F1009>]
    [AppliedWithChunk<Chunk090F100E>]
    public float VibrationSpeedCoef { get => vibrationSpeedCoef; set => vibrationSpeedCoef = value; }

    private float vibrationMax;
    [AppliedWithChunk<Chunk090F1001>]
    [AppliedWithChunk<Chunk090F1009>]
    [AppliedWithChunk<Chunk090F100E>]
    public float VibrationMax { get => vibrationMax; set => vibrationMax = value; }

    private CPlugBitmap? materialHeightDetailBitmap;
    [AppliedWithChunk<Chunk090F1004>]
    public CPlugBitmap? MaterialHeightDetailBitmap { get => materialHeightDetailBitmapFile?.GetNode(ref materialHeightDetailBitmap) ?? materialHeightDetailBitmap; set => materialHeightDetailBitmap = value; }
    private Components.GbxRefTableFile? materialHeightDetailBitmapFile;
    public Components.GbxRefTableFile? MaterialHeightDetailBitmapFile { get => materialHeightDetailBitmapFile; set => materialHeightDetailBitmapFile = value; }
    public CPlugBitmap? GetMaterialHeightDetailBitmap(GbxReadSettings settings = default, bool exceptions = false) => materialHeightDetailBitmapFile?.GetNode(ref materialHeightDetailBitmap, settings, exceptions) ?? materialHeightDetailBitmap;

    private Vec2 materialHeightDetailScale;
    [AppliedWithChunk<Chunk090F1004>]
    public Vec2 MaterialHeightDetailScale { get => materialHeightDetailScale; set => materialHeightDetailScale = value; }

    private float speed;
    [AppliedWithChunk<Chunk090F1005>]
    public float Speed { get => speed; set => speed = value; }

    private float grip;
    [AppliedWithChunk<Chunk090F1005>]
    public float Grip { get => grip; set => grip = value; }

    private float accelerationCoef;
    [AppliedWithChunk<Chunk090F1005>]
    public float AccelerationCoef { get => accelerationCoef; set => accelerationCoef = value; }

    private float brakeCoef;
    [AppliedWithChunk<Chunk090F1005>]
    public float BrakeCoef { get => brakeCoef; set => brakeCoef = value; }

    private CPlugFlockModel? trailManagerModel;
    [AppliedWithChunk<Chunk090F1006>]
    public CPlugFlockModel? TrailManagerModel { get => trailManagerModelFile?.GetNode(ref trailManagerModel) ?? trailManagerModel; set => trailManagerModel = value; }
    private Components.GbxRefTableFile? trailManagerModelFile;
    public Components.GbxRefTableFile? TrailManagerModelFile { get => trailManagerModelFile; set => trailManagerModelFile = value; }
    public CPlugFlockModel? GetTrailManagerModel(GbxReadSettings settings = default, bool exceptions = false) => trailManagerModelFile?.GetNode(ref trailManagerModel, settings, exceptions) ?? trailManagerModel;


    /// <summary>
    /// CPlugVehicleMaterial 0x001 chunk
    /// </summary>
    [Chunk(0x090F1001)]
    public partial class Chunk090F1001 : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F1001;


        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.Byte(ref n.materialId);
            rw.Boolean(ref n.trailIsOnlyWhenSliding);
            rw.NodeRef<CPlugShader>(ref n.trailShader);
            rw.Single(ref n.vibrationSpeedCoef);
            rw.Single(ref n.vibrationMax);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x004 chunk
    /// </summary>
    [Chunk(0x090F1004)]
    public partial class Chunk090F1004 : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F1004;


        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.materialHeightDetailBitmap, ref n.materialHeightDetailBitmapFile);
            rw.Vec2(ref n.materialHeightDetailScale);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x005 chunk
    /// </summary>
    [Chunk(0x090F1005)]
    public partial class Chunk090F1005 : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F1005;


        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.Single(ref n.speed);
            rw.Single(ref n.grip);
            rw.Single(ref n.accelerationCoef);
            rw.Single(ref n.brakeCoef);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x006 chunk
    /// </summary>
    [Chunk(0x090F1006)]
    public partial class Chunk090F1006 : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F1006;


        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFlockModel>(ref n.trailManagerModel, ref n.trailManagerModelFile);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x009 chunk
    /// </summary>
    [Chunk(0x090F1009)]
    public partial class Chunk090F1009 : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F1009;

        public CMwNod? U01;
        public Components.GbxRefTableFile? U01File;
        public CPlugParticleEmitterModel? U02;
        public Components.GbxRefTableFile? U02File;
        public CPlugParticleEmitterModel? U03;
        public Components.GbxRefTableFile? U03File;
        public CPlugParticleEmitterModel? U04;
        public Components.GbxRefTableFile? U04File;
        public CPlugParticleEmitterModel? U05;
        public Components.GbxRefTableFile? U05File;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.Byte(ref n.materialId);
            rw.Single(ref n.vibrationSpeedCoef);
            rw.Single(ref n.vibrationMax);
            rw.Boolean(ref n.trailIsOnlyWhenSliding);
            rw.NodeRef<CMwNod>(ref U01, ref U01File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U02, ref U02File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U03, ref U03File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U04, ref U04File);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U05, ref U05File);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x00A chunk
    /// </summary>
    [Chunk(0x090F100A)]
    public partial class Chunk090F100A : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F100A;

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x00B chunk
    /// </summary>
    [Chunk(0x090F100B)]
    public partial class Chunk090F100B : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F100B;

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x00C chunk
    /// </summary>
    [Chunk(0x090F100C)]
    public partial class Chunk090F100C : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F100C;

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x00D chunk
    /// </summary>
    [Chunk(0x090F100D)]
    public partial class Chunk090F100D : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F100D;

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x00E chunk
    /// </summary>
    [Chunk(0x090F100E)]
    public partial class Chunk090F100E : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F100E;


        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.Byte(ref n.materialId);
            rw.Single(ref n.vibrationSpeedCoef);
            rw.Single(ref n.vibrationMax);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x00F chunk
    /// </summary>
    [Chunk(0x090F100F)]
    public partial class Chunk090F100F : Chunk<CPlugVehicleMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x090F100F;

        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleMaterial 0x010 chunk
    /// </summary>
    [Chunk(0x090F1010)]
    public partial class Chunk090F1010 : Chunk<CPlugVehicleMaterial>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F1010;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public byte U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public bool U17;
        public Vec2 U18;
        public float U19;
        public float U20;
        public float U21;
        public float U22;
        public Keys? U23;
        public Keys? U24;
        public float U25;
        public Keys? U26;
        public Keys? U27;
        public float U28;
        public float U29;
        public float U30;
        public Keys? U31;
        public int U32;

        public override void ReadWrite(CPlugVehicleMaterial n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Byte(ref U05);
            if (Version == 1)
            {
                rw.Single(ref U06);
                rw.Single(ref U07);
            }
            if (Version >= 7)
            {
                rw.Single(ref U08);
                rw.Single(ref U09);
            }
            if (Version >= 1)
            {
                rw.Single(ref U10);
                rw.Single(ref U11);
                rw.Single(ref U12);
            }
            if (Version >= 2)
            {
                rw.Single(ref U13);
            }
            if (Version >= 3)
            {
                rw.Single(ref U14);
                rw.Single(ref U15);
                rw.Single(ref U16);
            }
            if (Version >= 4)
            {
                rw.Boolean(ref U17);
                rw.Vec2(ref U18);
            }
            if (Version >= 5)
            {
                rw.Single(ref U19);
            }
            if (Version >= 6)
            {
                rw.Single(ref U20);
            }
            if (Version >= 8)
            {
                rw.Single(ref U21);
                if (Version <= 13)
                {
                    rw.Single(ref U22);
                }
            }
            if (Version >= 9)
            {
                rw.ReadableWritable<Keys>(ref U23, version: Version - 12);
            }
            if (Version >= 10)
            {
                rw.ReadableWritable<Keys>(ref U24, version: Version - 12);
            }
            if (Version >= 11)
            {
                rw.Single(ref U25);
            }
            if (Version >= 13)
            {
                rw.ReadableWritable<Keys>(ref U26, version: Version - 12);
                rw.ReadableWritable<Keys>(ref U27, version: Version - 12);
                rw.Single(ref U28);
                rw.Single(ref U29);
                rw.Single(ref U30);
            }
            if (Version >= 14)
            {
                rw.ReadableWritable<Keys>(ref U31, version: Version - 12);
                rw.Int32(ref U32);
            }
        }
    }


    public sealed partial class Keys : IReadableWritable
    {
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F1001 => new Chunk090F1001(),
        0x090F1004 => new Chunk090F1004(),
        0x090F1005 => new Chunk090F1005(),
        0x090F1006 => new Chunk090F1006(),
        0x090F1009 => new Chunk090F1009(),
        0x090F100A => new Chunk090F100A(),
        0x090F100B => new Chunk090F100B(),
        0x090F100C => new Chunk090F100C(),
        0x090F100D => new Chunk090F100D(),
        0x090F100E => new Chunk090F100E(),
        0x090F100F => new Chunk090F100F(),
        0x090F1010 => new Chunk090F1010(),
        _ => base.NewChunk(chunkId),
    };
}
