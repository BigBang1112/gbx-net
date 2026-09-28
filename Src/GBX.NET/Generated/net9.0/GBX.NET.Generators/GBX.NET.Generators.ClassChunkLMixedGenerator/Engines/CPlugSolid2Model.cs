namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BB000</remarks>
[Class(0x090BB000)]
public partial class CPlugSolid2Model : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090BB000;




    private int fileVersion;
    [AppliedWithChunk<HeaderChunk090BB000>]
    public int FileVersion { get => fileVersion; set => fileVersion = value; }

    private FakeOccProj[]? fakeOccProjs;
    [AppliedWithChunk<Chunk090BB002>]
    public FakeOccProj[]? FakeOccProjs { get => fakeOccProjs; set => fakeOccProjs = value; }

    /// <summary>
    /// [SHeaderFileVersion] CPlugSolid2Model 0x000 header chunk (file version)
    /// </summary>
    [Chunk(0x090BB000, "file version")]
    public partial class HeaderChunk090BB000 : HeaderChunk<CPlugSolid2Model>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BB000;


        public override void ReadWrite(CPlugSolid2Model n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.fileVersion);
        }
    }


    /// <summary>
    /// CPlugSolid2Model 0x000 chunk
    /// </summary>
    [Chunk(0x090BB000)]
    public partial class Chunk090BB000 : Chunk<CPlugSolid2Model>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BB000;

    }

    /// <summary>
    /// CPlugSolid2Model 0x002 skippable chunk (fake occlusion)
    /// </summary>
    [Chunk(0x090BB002, "fake occlusion")]
    public partial class Chunk090BB002 : SkippableChunk<CPlugSolid2Model>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BB002;


        public override void ReadWrite(CPlugSolid2Model n, GbxReaderWriter rw)
        {
            rw.Data(ref n.fileImageBytes);
            rw.ArrayReadableWritable<FakeOccProj>(ref n.fakeOccProjs!);
        }
    }


    public sealed partial class PreLightGen : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private Int2 spriteCount;
        public Int2 SpriteCount { get => spriteCount; set => spriteCount = value; }

        private BoxAligned[]? u12;
        public BoxAligned[]? U12 { get => u12; set => u12 = value; }

        private Int4[]? uvGroups;
        public Int4[]? UvGroups { get => uvGroups; set => uvGroups = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Int32(ref u01);
            rw.Single(ref u02);
            rw.Boolean(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Int2(ref spriteCount);
            rw.Array<BoxAligned>(ref u12!);
            if (Version>=1)
            {
                rw.Array<Int4>(ref uvGroups!);
            }
        }
    }

    public sealed partial class Material : IReadableWritable
    {

        private string? materialName;
        public string? MaterialName { get => materialName; set => materialName = value; }

        private CPlugMaterialUserInst? materialUserInst;
        public CPlugMaterialUserInst? MaterialUserInst { get => materialUserInst; set => materialUserInst = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref materialName);
            if (MaterialName==null||MaterialName=="")
            {
                rw.NodeRef<CPlugMaterialUserInst>(ref materialUserInst);
            }
        }
    }

    public sealed partial class ShadedGeom : IReadableWritable
    {

        private int visualIndex;
        public int VisualIndex { get => visualIndex; set => visualIndex = value; }

        private int materialIndex;
        public int MaterialIndex { get => materialIndex; set => materialIndex = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int lodMask;
        public int LodMask { get => lodMask; set => lodMask = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref visualIndex);
            rw.Int32(ref materialIndex);
            rw.Int32(ref u01);
            if (v >= 1)
            {
                rw.Int32(ref lodMask);
                if (v >= 32)
                {
                    rw.Int32(ref u02);
                }
            }
        }
    }

    public sealed partial class LightInst : IReadableWritable
    {

        private int modelIndex;
        public int ModelIndex { get => modelIndex; set => modelIndex = value; }

        private int socketIndex;
        public int SocketIndex { get => socketIndex; set => socketIndex = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref modelIndex);
            rw.Int32(ref socketIndex);
        }
    }

    public sealed partial class Light : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private CPlugLight? u03;
        public CPlugLight? U03 { get => u03File?.GetNode(ref u03) ?? u03; set => u03 = value; }
        private Components.GbxRefTableFile? u03File;
        public Components.GbxRefTableFile? U03File { get => u03File; set => u03File = value; }
        public CPlugLight? GetU03(GbxReadSettings settings = default, bool exceptions = false) => u03File?.GetNode(ref u03, settings, exceptions) ?? u03;

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        private Iso4 u05;
        public Iso4 U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        private int u11;
        public int U11 { get => u11; set => u11 = value; }

        private int u12;
        public int U12 { get => u12; set => u12 = value; }

        private int u13;
        public int U13 { get => u13; set => u13 = value; }

        private int u14;
        public int U14 { get => u14; set => u14 = value; }

        private bool u15;
        public bool U15 { get => u15; set => u15 = value; }

        private float u16;
        public float U16 { get => u16; set => u16 = value; }

        private float u17;
        public float U17 { get => u17; set => u17 = value; }

        private float u18;
        public float U18 { get => u18; set => u18 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Boolean(ref u02);
            if (U02)
            {
                rw.NodeRef<CPlugLight>(ref u03, ref u03File);
            }
            if (!U02)
            {
                rw.String(ref u04);
            }
            rw.Iso4(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Int32(ref u10);
            rw.Int32(ref u11);
            if (v >= 26)
            {
                rw.Int32(ref u12);
                rw.Int32(ref u13);
                rw.Int32(ref u14);
            }
            rw.Boolean(ref u15);
            if (U15)
            {
                rw.Single(ref u16);
                rw.Single(ref u17);
                rw.Single(ref u18);
            }
        }
    }

    public sealed partial class FakeOccProj : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private Quat u04;
        public Quat U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private float u09;
        public float U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Quat(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090BB000 => new Chunk090BB000(),
        0x090BB002 => new Chunk090BB002(),
        _ => base.NewChunk(chunkId),
    };
}
