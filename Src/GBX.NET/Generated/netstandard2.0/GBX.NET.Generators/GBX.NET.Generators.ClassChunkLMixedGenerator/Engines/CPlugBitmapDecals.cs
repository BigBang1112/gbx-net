namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09078000</remarks>
[Class(0x09078000)]
public partial class CPlugBitmapDecals : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09078000;




    private External<CPlugDecalModel>[]? decalModels;
    [AppliedWithChunk<Chunk09078000>]
    public External<CPlugDecalModel>[]? DecalModels { get => decalModels; set => decalModels = value; }

    private DecalSet[]? decalSets;
    [AppliedWithChunk<Chunk09078000>]
    public DecalSet[]? DecalSets { get => decalSets; set => decalSets = value; }

    private string? matterId;
    [AppliedWithChunk<Chunk09078000>]
    public string? MatterId { get => matterId; set => matterId = value; }


    /// <summary>
    /// CPlugBitmapDecals 0x000 chunk
    /// </summary>
    [Chunk(0x09078000)]
    public partial class Chunk09078000 : Chunk<CPlugBitmapDecals>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09078000;

        public int Version { get; set; }


        public override void Read(CPlugBitmapDecals n, GbxReader r)
        {
            Version = r.ReadInt32();
            n.DecalModels = r.ReadArrayExternalNodeRef_deprec<CPlugDecalModel>();
            n.DecalSets = r.ReadArrayReadable<DecalSet>(version: Version);
            if (Version >= 4)
            {
                n.MatterId = r.ReadId();
            }
        }

        public override void Write(CPlugBitmapDecals n, GbxWriter w)
        {
            w.Write(Version);
            w.WriteArrayExternalNodeRef_deprec<CPlugDecalModel>(n.DecalModels);
            w.WriteArrayWritable<DecalSet>(n.DecalSets, version: Version);
            if (Version >= 4)
            {
                w.WriteIdAsString(n.MatterId);
            }
        }
    }

    /// <summary>
    /// CPlugBitmapDecals 0x001 chunk
    /// </summary>
    [Chunk(0x09078001)]
    public partial class Chunk09078001 : Chunk<CPlugBitmapDecals>
    {
        /// <inheritdoc />
        public override uint Id => 0x09078001;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CPlugBitmapDecals n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }


    public sealed partial class Decal : IReadable, IWritable
    {
        public int U01 { get; set; }
        public string? U02 { get; set; }
        public float U03 { get; set; }
        public float U04 { get; set; }
        public float U05 { get; set; }
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
        public bool U18 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            U02 = r.ReadId();
            U03 = r.ReadSingle();
            U04 = r.ReadSingle();
            U05 = r.ReadSingle();
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
            if (v >= 2)
            {
                U18 = r.ReadBoolean();
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.WriteIdAsString(U02);
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
            if (v >= 2)
            {
                w.Write(U18);
            }
        }
    }

    public sealed partial class DecalSet : IReadable, IWritable
    {
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09078000 => new Chunk09078000(),
        0x09078001 => new Chunk09078001(),
        _ => base.NewChunk(chunkId),
    };
}
