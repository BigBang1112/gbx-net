namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090E8000</remarks>
[Class(0x090E8000)]
public partial class CPlugVehicleVisModelShared : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090E8000;




    private SimulationWheel[]? simulationWheels;
    [AppliedWithChunk<Chunk090E8005>]
    public SimulationWheel[]? SimulationWheels { get => simulationWheels; set => simulationWheels = value; }

    private CPlugVehicleMaterialGroup[]? vehicleMaterialGroups;
    [AppliedWithChunk<Chunk090E8012>]
    public CPlugVehicleMaterialGroup[]? VehicleMaterialGroups { get => vehicleMaterialGroups; set => vehicleMaterialGroups = value; }

    private CPlugVehicleVisEmitterModel[]? vehicleEmitters;
    [AppliedWithChunk<Chunk090E8014>]
    public CPlugVehicleVisEmitterModel[]? VehicleEmitters { get => vehicleEmitters; set => vehicleEmitters = value; }


    /// <summary>
    /// CPlugVehicleVisModelShared 0x005 chunk
    /// </summary>
    [Chunk(0x090E8005)]
    public partial class Chunk090E8005 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8005;


        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<SimulationWheel>(ref n.simulationWheels!);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x006 chunk
    /// </summary>
    [Chunk(0x090E8006)]
    public partial class Chunk090E8006 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8006;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x009 chunk
    /// </summary>
    [Chunk(0x090E8009)]
    public partial class Chunk090E8009 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8009;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x00A chunk
    /// </summary>
    [Chunk(0x090E800A)]
    public partial class Chunk090E800A : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E800A;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x00C chunk
    /// </summary>
    [Chunk(0x090E800C)]
    public partial class Chunk090E800C : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E800C;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x00D chunk
    /// </summary>
    [Chunk(0x090E800D)]
    public partial class Chunk090E800D : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E800D;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x00F chunk
    /// </summary>
    [Chunk(0x090E800F)]
    public partial class Chunk090E800F : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E800F;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x010 chunk
    /// </summary>
    [Chunk(0x090E8010)]
    public partial class Chunk090E8010 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8010;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x012 chunk
    /// </summary>
    [Chunk(0x090E8012)]
    public partial class Chunk090E8012 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8012;


        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugVehicleMaterialGroup>(ref n.vehicleMaterialGroups!);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x013 chunk
    /// </summary>
    [Chunk(0x090E8013)]
    public partial class Chunk090E8013 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8013;

        public CFuncKeysReal? U01;
        public CFuncKeysReal? U02;
        public CFuncKeysReal? U03;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
            rw.NodeRef<CFuncKeysReal>(ref U03);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x014 chunk
    /// </summary>
    [Chunk(0x090E8014)]
    public partial class Chunk090E8014 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8014;


        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugVehicleVisEmitterModel>(ref n.vehicleEmitters!);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x015 chunk
    /// </summary>
    [Chunk(0x090E8015)]
    public partial class Chunk090E8015 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8015;

    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x016 chunk
    /// </summary>
    [Chunk(0x090E8016)]
    public partial class Chunk090E8016 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8016;

        public CFuncKeysReal? U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysReal>(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x018 chunk
    /// </summary>
    [Chunk(0x090E8018)]
    public partial class Chunk090E8018 : Chunk<CPlugVehicleVisModelShared>
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8018;

        public CPlugParticleEmitterModel? U01;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x01E chunk
    /// </summary>
    [Chunk(0x090E801E)]
    public partial class Chunk090E801E : Chunk<CPlugVehicleVisModelShared>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E801E;

        public int Version { get; set; }

        public CPlugMaterial? U01;
        public Components.GbxRefTableFile? U01File;
        public CPlugMaterial? U02;
        public Components.GbxRefTableFile? U02File;
        public External<CPlugMaterial>[]? U03;
        public Components.GbxRefTableFile? U03File;
        public CPlugMaterial? U04;
        public CPlugMaterial? U05;
        public CPlugMaterial? U06;
        public CPlugMaterial? U07;
        public CPlugMaterial? U08;
        public CPlugMaterial? U09;
        public CPlugMaterial? U10;
        public CPlugLight? U11;
        public Components.GbxRefTableFile? U11File;
        public CPlugLight? U12;
        public Components.GbxRefTableFile? U12File;
        public CPlugLight? U13;
        public Components.GbxRefTableFile? U13File;
        public CPlugLight? U14;
        public Components.GbxRefTableFile? U14File;
        public CPlugLight? U15;
        public Components.GbxRefTableFile? U15File;
        public CPlugLight? U16;
        public Components.GbxRefTableFile? U16File;
        public float U17;
        public float U18;
        public CPlugSolid2Model? U19;
        public Components.GbxRefTableFile? U19File;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugMaterial>(ref U01, ref U01File);
            if (Version >= 1)
            {
                if (Version <= 2)
                {
                    rw.NodeRef<CPlugMaterial>(ref U02, ref U02File);
                }
                if (Version >= 3)
                {
                    rw.ArrayNodeRef<CPlugMaterial>(ref U03!);
                }
                if (Version <= 1)
                {
                    rw.NodeRef<CPlugMaterial>(ref U04);
                    rw.NodeRef<CPlugMaterial>(ref U05);
                    rw.NodeRef<CPlugMaterial>(ref U06);
                    rw.NodeRef<CPlugMaterial>(ref U07);
                    rw.NodeRef<CPlugMaterial>(ref U08);
                    rw.NodeRef<CPlugMaterial>(ref U09);
                    rw.NodeRef<CPlugMaterial>(ref U10);
                }
                rw.NodeRef<CPlugLight>(ref U11, ref U11File);
                rw.NodeRef<CPlugLight>(ref U12, ref U12File);
                rw.NodeRef<CPlugLight>(ref U13, ref U13File);
                rw.NodeRef<CPlugLight>(ref U14, ref U14File);
                rw.NodeRef<CPlugLight>(ref U15, ref U15File);
                rw.NodeRef<CPlugLight>(ref U16, ref U16File);
                rw.Single(ref U17);
                rw.Single(ref U18);
                if (Version >= 4)
                {
                    rw.NodeRef<CPlugSolid2Model>(ref U19, ref U19File);
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x01F chunk
    /// </summary>
    [Chunk(0x090E801F)]
    public partial class Chunk090E801F : Chunk<CPlugVehicleVisModelShared>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E801F;

        public int Version { get; set; }

        public CPlugParticleEmitterModel? U01;
        public Components.GbxRefTableFile? U01File;
        public CPlugParticleEmitterModel? U02;
        public CPlugParticleEmitterModel? U03;
        public Components.GbxRefTableFile? U03File;
        public CPlugParticleEmitterModel? U04;
        public Components.GbxRefTableFile? U04File;
        public CPlugParticleEmitterModel? U05;
        public Components.GbxRefTableFile? U05File;
        public CPlugVehicleVisEmitterModel[]? U06;
        public CPlugParticleEmitterModel? U07;
        public Components.GbxRefTableFile? U07File;
        public CPlugParticleEmitterModel? U08;
        public Components.GbxRefTableFile? U08File;
        public CPlugVisEntFxModel? U09;
        public CPlugParticleEmitterModel? U10;
        public CPlugParticleEmitterModel? U11;
        public CPlugParticleEmitterModel? U12;
        public CPlugParticleEmitterModel? U13;
        public CPlugVisEntFxModel? U14;
        public CPlugParticleEmitterModel? U15;
        public Components.GbxRefTableFile? U15File;
        public CPlugParticleEmitterModel? U16;
        public Components.GbxRefTableFile? U16File;
        public CPlugParticleEmitterModel? U17;
        public Components.GbxRefTableFile? U17File;
        public CPlugParticleEmitterModel? U18;
        public Components.GbxRefTableFile? U18File;
        public CPlugParticleEmitterModel? U19;
        public Components.GbxRefTableFile? U19File;
        public CPlugParticleEmitterModel? U20;
        public Components.GbxRefTableFile? U20File;
        public CPlugParticleEmitterModel? U21;
        public Components.GbxRefTableFile? U21File;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U01, ref U01File);
            if (Version >= 1)
            {
                rw.NodeRef<CPlugParticleEmitterModel>(ref U02);
                if (Version >= 2)
                {
                    rw.NodeRef<CPlugParticleEmitterModel>(ref U03, ref U03File);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CPlugParticleEmitterModel>(ref U04, ref U04File);
                        if (Version >= 4)
                        {
                            rw.NodeRef<CPlugParticleEmitterModel>(ref U05, ref U05File);
                            if (Version >= 5)
                            {
                                rw.ArrayNodeRef_deprec<CPlugVehicleVisEmitterModel>(ref U06!);
                                rw.NodeRef<CPlugParticleEmitterModel>(ref U07, ref U07File);
                                rw.NodeRef<CPlugParticleEmitterModel>(ref U08, ref U08File);
                                if (Version == 6)
                                {
                                    rw.NodeRef<CPlugVisEntFxModel>(ref U09);
                                    rw.NodeRef<CPlugParticleEmitterModel>(ref U10);
                                    rw.NodeRef<CPlugParticleEmitterModel>(ref U11);
                                    rw.NodeRef<CPlugParticleEmitterModel>(ref U12);
                                    rw.NodeRef<CPlugParticleEmitterModel>(ref U13);
                                }
                                if (Version >= 7)
                                {
                                    rw.NodeRef<CPlugVisEntFxModel>(ref U14);
                                    if (Version >= 8)
                                    {
                                        rw.NodeRef<CPlugParticleEmitterModel>(ref U15, ref U15File);
                                        if (Version >= 9)
                                        {
                                            rw.NodeRef<CPlugParticleEmitterModel>(ref U16, ref U16File);
                                            if (Version >= 10)
                                            {
                                                rw.NodeRef<CPlugParticleEmitterModel>(ref U17, ref U17File);
                                                if (Version >= 11)
                                                {
                                                    rw.NodeRef<CPlugParticleEmitterModel>(ref U18, ref U18File);
                                                    if (Version >= 12)
                                                    {
                                                        rw.NodeRef<CPlugParticleEmitterModel>(ref U19, ref U19File);
                                                        if (Version >= 13)
                                                        {
                                                            rw.NodeRef<CPlugParticleEmitterModel>(ref U20, ref U20File);
                                                            if (Version >= 14)
                                                            {
                                                                rw.NodeRef<CPlugParticleEmitterModel>(ref U21, ref U21File);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x020 chunk
    /// </summary>
    [Chunk(0x090E8020)]
    public partial class Chunk090E8020 : Chunk<CPlugVehicleVisModelShared>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8020;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            if (Version >= 1)
            {
                rw.Single(ref U03);
                rw.Single(ref U04);
            }
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x021 chunk
    /// </summary>
    [Chunk(0x090E8021)]
    public partial class Chunk090E8021 : Chunk<CPlugVehicleVisModelShared>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8021;

        public int Version { get; set; }

        public CFuncKeysReal? U01;
        public CFuncKeysReal? U02;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CFuncKeysReal>(ref U01);
            rw.NodeRef<CFuncKeysReal>(ref U02);
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x022 chunk
    /// </summary>
    [Chunk(0x090E8022)]
    public partial class Chunk090E8022 : Chunk<CPlugVehicleVisModelShared>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8022;

        public int Version { get; set; }

        public CPlugLocatedSound? U01;
        public CPlugLocatedSound? U02;
        public CPlugLocatedSound? U03;
        public CPlugLocatedSound? U04;
        public CPlugLocatedSound? U05;
        public CPlugLocatedSound? U06;
        public CPlugLocatedSound? U07;
        public CPlugLocatedSound? U08;
        public CPlugLocatedSound? U09;
        public CPlugLocatedSound? U10;
        public CPlugLocatedSound? U11;
        public CPlugLocatedSound? U12;
        public CPlugLocatedSound? U13;
        public CPlugLocatedSound? U14;
        public CPlugLocatedSound? U15;
        public CPlugLocatedSound? U16;
        public CPlugLocatedSound? U17;
        public CPlugLocatedSound? U18;
        public CPlugLocatedSound? U19;
        public CPlugLocatedSound? U20;
        public CPlugLocatedSound? U21;
        public CPlugLocatedSound? U22;
        public CPlugLocatedSound? U23;
        public CPlugLocatedSound? U24;
        public CPlugLocatedSound? U25;
        public CPlugLocatedSound? U26;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugLocatedSound>(ref U01);
            rw.NodeRef<CPlugLocatedSound>(ref U02);
            rw.NodeRef<CPlugLocatedSound>(ref U03);
            if (Version >= 2)
            {
                rw.NodeRef<CPlugLocatedSound>(ref U04);
                if (Version <= 2)
                {
                    rw.NodeRef<CPlugLocatedSound>(ref U05);
                }
                if (Version >= 3)
                {
                    rw.NodeRef<CPlugLocatedSound>(ref U06);
                    rw.NodeRef<CPlugLocatedSound>(ref U07);
                    rw.NodeRef<CPlugLocatedSound>(ref U08);
                    if (Version >= 4)
                    {
                        rw.NodeRef<CPlugLocatedSound>(ref U09);
                        rw.NodeRef<CPlugLocatedSound>(ref U10);
                        if (Version >= 5)
                        {
                            rw.NodeRef<CPlugLocatedSound>(ref U11);
                            rw.NodeRef<CPlugLocatedSound>(ref U12);
                            rw.NodeRef<CPlugLocatedSound>(ref U13);
                            rw.NodeRef<CPlugLocatedSound>(ref U14);
                            rw.NodeRef<CPlugLocatedSound>(ref U15);
                            rw.NodeRef<CPlugLocatedSound>(ref U16);
                            rw.NodeRef<CPlugLocatedSound>(ref U17);
                            rw.NodeRef<CPlugLocatedSound>(ref U18);
                            rw.NodeRef<CPlugLocatedSound>(ref U19);
                            rw.NodeRef<CPlugLocatedSound>(ref U20);
                            rw.NodeRef<CPlugLocatedSound>(ref U21);
                            rw.NodeRef<CPlugLocatedSound>(ref U22);
                            rw.NodeRef<CPlugLocatedSound>(ref U23);
                            rw.NodeRef<CPlugLocatedSound>(ref U24);
                            if (Version >= 6)
                            {
                                rw.NodeRef<CPlugLocatedSound>(ref U25);
                                if (Version >= 7)
                                {
                                    rw.NodeRef<CPlugLocatedSound>(ref U26);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehicleVisModelShared 0x023 chunk
    /// </summary>
    [Chunk(0x090E8023)]
    public partial class Chunk090E8023 : Chunk<CPlugVehicleVisModelShared>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090E8023;

        public int Version { get; set; }

        public CPlugParticleMaterialImpactModel? U01;
        public Components.GbxRefTableFile? U01File;
        public CPlugParticleMaterialImpactModel? U02;
        public Components.GbxRefTableFile? U02File;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CPlugVehicleVisModelShared n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugParticleMaterialImpactModel>(ref U01, ref U01File);
            rw.NodeRef<CPlugParticleMaterialImpactModel>(ref U02, ref U02File);
            if (Version >= 1)
            {
                rw.Int32(ref U03);
                rw.Int32(ref U04);
                rw.Int32(ref U05);
                rw.Int32(ref U06);
            }
        }
    }


    public sealed partial class VisualArm : IReadable, IWritable
    {
        public VisualId? U01 { get; set; }
        public VisualId? U02 { get; set; }
        public VisualId? U03 { get; set; }
        public bool U04 { get; set; }
        public bool U05 { get; set; }
        public int U06 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadReadable<VisualId>();
            U02 = r.ReadReadable<VisualId>();
            U03 = r.ReadReadable<VisualId>();
            U04 = r.ReadBoolean();
            U05 = r.ReadBoolean();
            U06 = r.ReadInt32();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.WriteWritable<VisualId>(U01);
            w.WriteWritable<VisualId>(U02);
            w.WriteWritable<VisualId>(U03);
            w.Write(U04);
            w.Write(U05);
            w.Write(U06);
        }
    }

    public sealed partial class VisualId : IReadable, IWritable
    {
        public string? Name { get; set; }
        public bool U01 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            Name = r.ReadId();
            U01 = r.ReadBoolean();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.WriteIdAsString(Name);
            w.Write(U01);
        }
    }

    public sealed partial class Emitter : IReadable, IWritable
    {
        public int U01 { get; set; }

        private CPlugParticleEmitterModel? emitterModel;
        public CPlugParticleEmitterModel? EmitterModel { get => emitterModelFile?.GetNode(ref emitterModel) ?? emitterModel; set => emitterModel = value; }
        private Components.GbxRefTableFile? emitterModelFile;
        public Components.GbxRefTableFile? EmitterModelFile { get => emitterModelFile; set => emitterModelFile = value; }
        public CPlugParticleEmitterModel? GetEmitterModel(GbxReadSettings settings = default, bool exceptions = false) => emitterModelFile?.GetNode(ref emitterModel, settings, exceptions) ?? emitterModel;
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public bool U05 { get; set; }
        public float U06 { get; set; }
        public float U07 { get; set; }
        public float U08 { get; set; }
        public float U09 { get; set; }
        public float U10 { get; set; }
        public float U11 { get; set; }
        public float U12 { get; set; }
        public float U13 { get; set; }
        public float U14 { get; set; }
        public float U15 { get; set; }
        public float U16 { get; set; }
        public float U17 { get; set; }
        public float U18 { get; set; }
        public float U19 { get; set; }
        public float U20 { get; set; }
        public float U21 { get; set; }
        public float U22 { get; set; }
        public float U23 { get; set; }
        public float U24 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            EmitterModel = r.ReadNodeRef<CPlugParticleEmitterModel>(out emitterModelFile);
            U02 = r.ReadInt32();
            U03 = r.ReadInt32();
            U04 = r.ReadInt32();
            U05 = r.ReadBoolean();
            U06 = r.ReadSingle();
            U07 = r.ReadSingle();
            U08 = r.ReadSingle();
            U09 = r.ReadSingle();
            U10 = r.ReadSingle();
            U11 = r.ReadSingle();
            U12 = r.ReadSingle();
            U13 = r.ReadSingle();
            U14 = r.ReadSingle();
            U15 = r.ReadSingle();
            U16 = r.ReadSingle();
            U17 = r.ReadSingle();
            U18 = r.ReadSingle();
            U19 = r.ReadSingle();
            U20 = r.ReadSingle();
            U21 = r.ReadSingle();
            U22 = r.ReadSingle();
            U23 = r.ReadSingle();
            U24 = r.ReadSingle();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.WriteNodeRef<CPlugParticleEmitterModel>(EmitterModel, EmitterModelFile);
            w.Write(U02);
            w.Write(U03);
            w.Write(U04);
            w.Write(U05);
            w.Write(U06);
            w.Write(U07);
            w.Write(U08);
            w.Write(U09);
            w.Write(U10);
            w.Write(U11);
            w.Write(U12);
            w.Write(U13);
            w.Write(U14);
            w.Write(U15);
            w.Write(U16);
            w.Write(U17);
            w.Write(U18);
            w.Write(U19);
            w.Write(U20);
            w.Write(U21);
            w.Write(U22);
            w.Write(U23);
            w.Write(U24);
        }
    }

    public sealed partial class SimulationWheel : IReadableWritable
    {

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private string? name;
        public string? Name { get => name; set => name = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Boolean(ref u01);
            rw.Boolean(ref u02);
            rw.Id(ref name);
        }
    }

    public sealed partial class VisualLight : IReadable, IWritable
    {
        public VisualId? U01 { get; set; }
        public bool U02 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadReadable<VisualId>();
            U02 = r.ReadBoolean();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.WriteWritable<VisualId>(U01);
            w.Write(U02);
        }
    }

    public sealed partial class VisualWheel : IReadable, IWritable
    {
        public VisualId? U01 { get; set; }
        public VisualId? U02 { get; set; }
        public VisualId? U03 { get; set; }
        public VisualId? U04 { get; set; }
        public VisualId? U05 { get; set; }
        public int U06 { get; set; }
        public bool U07 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadReadable<VisualId>();
            U02 = r.ReadReadable<VisualId>();
            U03 = r.ReadReadable<VisualId>();
            U04 = r.ReadReadable<VisualId>();
            if (v >= 1)
            {
                U05 = r.ReadReadable<VisualId>();
            }
            U06 = r.ReadInt32();
            U07 = r.ReadBoolean();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.WriteWritable<VisualId>(U01);
            w.WriteWritable<VisualId>(U02);
            w.WriteWritable<VisualId>(U03);
            w.WriteWritable<VisualId>(U04);
            if (v >= 1)
            {
                w.WriteWritable<VisualId>(U05);
            }
            w.Write(U06);
            w.Write(U07);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090E8005 => new Chunk090E8005(),
        0x090E8006 => new Chunk090E8006(),
        0x090E8009 => new Chunk090E8009(),
        0x090E800A => new Chunk090E800A(),
        0x090E800C => new Chunk090E800C(),
        0x090E800D => new Chunk090E800D(),
        0x090E800F => new Chunk090E800F(),
        0x090E8010 => new Chunk090E8010(),
        0x090E8012 => new Chunk090E8012(),
        0x090E8013 => new Chunk090E8013(),
        0x090E8014 => new Chunk090E8014(),
        0x090E8015 => new Chunk090E8015(),
        0x090E8016 => new Chunk090E8016(),
        0x090E8018 => new Chunk090E8018(),
        0x090E801E => new Chunk090E801E(),
        0x090E801F => new Chunk090E801F(),
        0x090E8020 => new Chunk090E8020(),
        0x090E8021 => new Chunk090E8021(),
        0x090E8022 => new Chunk090E8022(),
        0x090E8023 => new Chunk090E8023(),
        _ => base.NewChunk(chunkId),
    };
}
