using System.Drawing;

namespace GBX.NET.Engines.GameData;

/// <summary>
/// Collector. Something that can have an icon.
/// </summary>
/// <remarks>ID: 0x2E001000</remarks>
[Node(0x2E001000)]
public partial class CGameCtnCollector : CMwNod, CGameCtnCollector.IHeader
{
    #region Enums

    [Flags]
    public enum ECollectorFlags
    {
        None = 0,
        UnknownValue = 1,
        IsInternal = 2,
        IsAdvanced = 4
    }

    public enum EProdState
    {
        Aborted,
        GameBox,
        DevBuild,
        Release
    }

    #endregion

    #region Fields

    private Ident ident;
    private string pageName;
    private ECollectorFlags flags;
    private int catalogPosition;
    private string? name;
    private string? description;
    private bool iconUseAutoRender;
    private int iconQuarterRotationY;
    private EProdState? prodState;
    private string? skinDirectory;
    private bool isInternal;
    private bool isAdvanced;
    private ulong fileTime;
    private CMwNod? iconFid;
    private GameBoxRefTable.File? iconFidFile;

    #endregion

    #region Properties

    public HeaderChunkSet HeaderChunks { get; } = new();

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001002>]
    [AppliedWithChunk<Chunk2E001003>]
    [AppliedWithChunk<Chunk2E00100B>]
    public Ident Ident { get => ident; set => ident = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001003>]
    [AppliedWithChunk<Chunk2E001009>]
    public string PageName { get => pageName; set => pageName = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk2E001003>(sinceVersion: 3)]
    public ECollectorFlags Flags { get => flags; set => flags = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001003>(sinceVersion: 3)]
    [AppliedWithChunk<Chunk2E001007>]
    [AppliedWithChunk<Chunk2E001011>]
    public int CatalogPosition { get => catalogPosition; set => catalogPosition = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001003>(sinceVersion: 7)]
    [AppliedWithChunk<Chunk2E00100C>]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001003>(sinceVersion: 8)]
    [AppliedWithChunk<Chunk2E001011>(sinceVersion: 1)]
    public EProdState? ProdState { get => prodState; set => prodState = value; }

    /// <summary>
    /// Icon of the collector in 2D pixel array format from all versions except icons created after April 2022 in TM2020.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk2E001004>]
    public Color[,]? Icon { get; set; }

    /// <summary>
    /// Icon of the collector in WebP format from TM2020 icons since April 2022 update.
    /// </summary>
    [NodeMember]
    [WebpData]
    [AppliedWithChunk<Chunk2E001004>]
    public byte[]? IconWebP { get; set; }

    [NodeMember]
    [AppliedWithChunk<Chunk2E001006H>]
    public ulong FileTime { get => fileTime; set => fileTime = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001009>]
    public CMwNod? IconFid
    {
        get => iconFid = GetNodeFromRefTable(iconFid, iconFidFile) as CMwNod;
        set => iconFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E00100D>]
    public string? Description { get => description; set => description = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E00100E>]
    public bool IconUseAutoRender { get => iconUseAutoRender; set => iconUseAutoRender = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E00100E>]
    public int IconQuarterRotationY { get => iconQuarterRotationY; set => iconQuarterRotationY = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001008>]
    [AppliedWithChunk<Chunk2E001010>]
    public string? SkinDirectory { get => skinDirectory; set => skinDirectory = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001011>]
    public bool IsInternal { get => isInternal; set => isInternal = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E001011>]
    public bool IsAdvanced { get => isAdvanced; set => isAdvanced = value; }

    #endregion

    #region Constructors

    internal CGameCtnCollector()
    {
        ident = Ident.Empty;
        pageName = "";
    }

    #endregion

    #region Chunks

    #region 0x002 chunk (author)

    /// <summary>
    /// CGameCtnCollector 0x002 chunk (author)
    /// </summary>
    [Chunk(0x2E001002, "author")]
    public class Chunk2E001002 : Chunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.ident!);
        }
    }

    #endregion

    #region 0x003 header chunk (desc)

    /// <summary>
    /// CGameCtnCollector 0x003 header chunk (desc)
    /// </summary>
    [Chunk(0x2E001003, "desc")]
    public class Chunk2E001003 : HeaderChunk<CGameCtnCollector>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public string U01 = "";
        public string? U02;
        public int U03;
        public byte U04;
        public int U05;
        public short U06;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.ident!);
            rw.Int32(ref version); // Id but filtered, technically
            rw.String(ref n.pageName!);

            if (version == 5)
            {
                rw.Id(ref U01!);
            }

            if (version >= 4)
            {
                rw.Id(ref U02);
            }

            if (version >= 3)
            {
                rw.EnumInt32<ECollectorFlags>(ref n.flags);
                n.CatalogPosition = rw.Int16((short)n.CatalogPosition);

                if (version < 6)
                {
                    rw.Byte(ref U04);
                    rw.Int32(ref U05);
                    rw.Int16(ref U06);
                }

                if (version >= 7)
                {
                    rw.String(ref n.name);

                    if (version >= 8)
                    {
                        rw.EnumByte<EProdState>(ref n.prodState);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x004 header chunk (icon)

    /// <summary>
    /// CGameCtnCollector 0x004 header chunk (icon)
    /// </summary>
    [Chunk(0x2E001004, "icon")]
    public class Chunk2E001004 : HeaderChunk<CGameCtnCollector>
    {
        public short U01 = 1;

        public override void Read(CGameCtnCollector n, GameBoxReader r)
        {
            var width = r.ReadInt16();
            var height = r.ReadInt16();

            var flags1 = width & 0x8000;
            var flags2 = width & 0x8000;

            var isWebP = flags1 >> 15 != 0 && flags2 >> 15 != 0;

            if (isWebP)
            {
                // width &= 255;
                // height &= 255;
                // both seem to be unused so this operation is not useful

                U01 = r.ReadInt16();
                n.IconWebP = r.ReadBytes();

                return;
            }

            var iconData = r.ReadArray<int>(width * height);

            n.Icon = new Color[width, height];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    n.Icon[x, height - 1 - y] = Color.FromArgb(iconData[y * width + x]);
        }

        public override void Write(CGameCtnCollector n, GameBoxWriter w)
        {
            var width = (ushort)(n.Icon?.GetLength(0) ?? 64);
            var height = (ushort)(n.Icon?.GetLength(1) ?? 64);

            if (n.IconWebP is not null)
            {
                width |= 0x8000;
                height |= 0x8000;
            }

            w.Write(width);
            w.Write(height);

            if (n.IconWebP is not null)
            {
                w.Write(U01);
                w.Write(n.IconWebP.Length);
                w.Write(n.IconWebP);

                return;
            }

            if (n.Icon is null)
            {
                for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                        w.Write(int.MaxValue);

                return;
            }

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    w.Write(n.Icon[x, height - 1 - y].ToArgb());
        }
    }

    #endregion

    #region 0x006 header chunk (file time)

    /// <summary>
    /// CGameCtnCollector 0x006 header chunk (file time)
    /// </summary>
    [Chunk(0x2E001006, "file time")]
    public class Chunk2E001006H : HeaderChunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref n.fileTime); // SHeaderLightMap
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnCollector 0x006 chunk
    /// </summary>
    [Chunk(0x2E001006)]
    public class Chunk2E001006B : Chunk<CGameCtnCollector>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnCollector 0x007 chunk
    /// </summary>
    [Chunk(0x2E001007)]
    public class Chunk2E001007 : Chunk<CGameCtnCollector>
    {
        public bool U01;
        public bool U02;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Int32(ref n.catalogPosition);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CGameCtnCollector 0x008 chunk
    /// </summary>
    [Chunk(0x2E001008)]
    public class Chunk2E001008 : Chunk<CGameCtnCollector>
    {
        public Node? U01;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CGameCtnCollector 0x009 chunk
    /// </summary>
    [Chunk(0x2E001009)]
    public class Chunk2E001009 : Chunk<CGameCtnCollector>
    {
        public string? U01;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.pageName!);

            var hasIconFid = rw.Boolean(n.IconFid is not null);

            if (hasIconFid)
            {
                rw.NodeRef(ref n.iconFid, ref n.iconFidFile);
            }

            rw.Id(ref U01);
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CGameCtnCollector 0x00A chunk
    /// </summary>
    [Chunk(0x2E00100A)]
    public class Chunk2E00100A : Chunk<CGameCtnCollector>
    {
        public string? U01;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion

    #region 0x00B chunk (author)

    /// <summary>
    /// CGameCtnCollector 0x00B chunk (author)
    /// </summary>
    [Chunk(0x2E00100B, "author")]
    public class Chunk2E00100B : Chunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.ident!); // It may be called Ident
        }
    }

    #endregion

    #region 0x00C chunk (collector name)

    /// <summary>
    /// CGameCtnCollector 0x00C chunk (collector name)
    /// </summary>
    [Chunk(0x2E00100C, "collector name")]
    public class Chunk2E00100C : Chunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    #endregion

    #region 0x00D chunk (description)

    /// <summary>
    /// CGameCtnCollector 0x00D chunk (description)
    /// </summary>
    [Chunk(0x2E00100D, "description")]
    public class Chunk2E00100D : Chunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.description);
        }
    }

    #endregion

    #region 0x00E chunk (icon render)

    /// <summary>
    /// CGameCtnCollector 0x00E chunk (icon render)
    /// </summary>
    [Chunk(0x2E00100E, "icon render")]
    public class Chunk2E00100E : Chunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.iconUseAutoRender);
            rw.Int32(ref n.iconQuarterRotationY);
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CGameCtnCollector 0x010 chunk
    /// </summary>
    [Chunk(0x2E001010)]
    public class Chunk2E001010 : Chunk<CGameCtnCollector>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        private GameBoxRefTable.File? U01File;
        public CMwNod? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01, ref U01File); // skin direct reference inside pak
            rw.String(ref n.skinDirectory);

            if (version >= 2 && (n.skinDirectory is null || n.skinDirectory.Length == 0))
            {
                rw.NodeRef(ref U02);
            }
        }
    }

    #endregion

    #region 0x011 chunk

    /// <summary>
    /// CGameCtnCollector 0x011 chunk
    /// </summary>
    [Chunk(0x2E001011)]
    public class Chunk2E001011 : Chunk<CGameCtnCollector>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Boolean(ref n.isInternal);
            rw.Boolean(ref n.isAdvanced);
            rw.Int32(ref n.catalogPosition);

            if (version >= 1)
            {
                rw.EnumByte<EProdState>(ref n.prodState);
            }
        }
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CGameCtnCollector 0x012 chunk
    /// </summary>
    [Chunk(0x2E001012)]
    public class Chunk2E001012 : Chunk<CGameCtnCollector>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
        }
    }

    #endregion

    #endregion
}
