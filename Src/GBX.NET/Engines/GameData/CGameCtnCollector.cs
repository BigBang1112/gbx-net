using System.Drawing;

namespace GBX.NET.Engines.GameData;

/// <summary>
/// Collector (0x2E001000)
/// </summary>
[Node(0x2E001000)]
public class CGameCtnCollector : CMwNod
{
    #region Enums

    public enum EProdState
    {
        Aborted,
        GameBox,
        DevBuild,
        Release
    }

    #endregion

    #region Fields

    private Ident? author;
    private Ident? ident;
    private string pageName;
    private int catalogPosition;
    private string? name;
    private string? description;
    private bool iconUseAutoRender;
    private int iconQuarterRotationY;
    private EProdState prodState;
    private string? skinDirectory;
    private bool isInternal;
    private bool isAdvanced;
    private long fileTime;

    #endregion

    #region Properties

    [NodeMember]
    public Ident? Author
    {
        get => author;
        set => author = value;
    }

    [NodeMember]
    public Ident? Ident
    {
        get => ident;
        set => ident = value;
    }

    [NodeMember]
    public string PageName
    {
        get => pageName;
        set => pageName = value;
    }

    [NodeMember]
    public int CatalogPosition
    {
        get => catalogPosition;
        set => catalogPosition = value;
    }

    [NodeMember]
    public EProdState ProdState
    {
        get => prodState;
        set => prodState = value;
    }

    [NodeMember]
    public string? Name
    {
        get => name;
        set => name = value;
    }

    [NodeMember]
    public Color[,]? Icon { get; set; }

    [NodeMember]
    public byte[]? IconWebP { get; set; }

    [NodeMember]
    public CMwNod? IconFid { get; set; }

    [NodeMember]
    public string? Description
    {
        get => description;
        set => description = value;
    }

    [NodeMember]
    public bool IconUseAutoRender
    {
        get => iconUseAutoRender;
        set => iconUseAutoRender = value;
    }

    [NodeMember]
    public int IconQuarterRotationY
    {
        get => iconQuarterRotationY;
        set => iconQuarterRotationY = value;
    }

    [NodeMember]
    public string? SkinDirectory
    {
        get => skinDirectory;
        set => skinDirectory = value;
    }

    [NodeMember]
    public bool IsInternal
    {
        get => isInternal;
        set => isInternal = value;
    }

    [NodeMember]
    public bool IsAdvanced
    {
        get => isAdvanced;
        set => isAdvanced = value;
    }

    [NodeMember]
    public long FileTime
    {
        get => fileTime;
        set => fileTime = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnCollector()
    {
        pageName = null!;
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
            rw.Ident(ref n.author);
        }
    }

    #endregion

    #region 0x003 header chunk

    /// <summary>
    /// CGameCtnCollector 0x003 header chunk
    /// </summary>
    [Chunk(0x2E001003)]
    public class Chunk2E001003 : HeaderChunk<CGameCtnCollector>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.ident);
            rw.Int32(ref version);
            rw.String(ref n.pageName!);

            if (version == 5)
            {
                rw.Int32(ref U01);
            }

            if (version >= 4)
            {
                rw.Int32(ref U02);
            }

            if (version >= 3)
            {
                rw.Int32(ref U03);
                n.CatalogPosition = rw.Int16((short)n.CatalogPosition);
            }

            if (version >= 7)
            {
                rw.String(ref n.name);
            }

            rw.EnumByte<EProdState>(ref n.prodState);
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
            {
                for (var x = 0; x < width; x++)
                {
                    n.Icon[width - 1 - x, height - 1 - y] = Color.FromArgb(iconData[y * width + x]);
                }
            }
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
                    w.Write(n.Icon[width - 1 - x, height - 1 - y].ToArgb());
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
            rw.Int64(ref n.fileTime);
        }
    }

    #endregion

    #region 0x006 body chunk

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
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Int32(ref U03);
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
        public byte U01;

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref U01); // 0
            rw.String(ref n.skinDirectory);
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
                n.IconFid = rw.NodeRef(n.IconFid);
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

    #region 0x00B chunk (ident)

    /// <summary>
    /// CGameCtnCollector 0x00B chunk (ident)
    /// </summary>
    [Chunk(0x2E00100B, "ident")]
    public class Chunk2E00100B : Chunk<CGameCtnCollector>
    {
        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.ident);
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
        public int U02;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version); // 2
            
            // Needs more checking

            rw.NodeRef(ref U01); // -1
            rw.String(ref n.skinDirectory);
            
            if (n.skinDirectory!.Length == 0)
            {
                rw.Int32(ref U02); // -1
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

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Boolean(ref n.isInternal);
            rw.Boolean(ref n.isAdvanced);
            rw.Int32(ref n.catalogPosition);
            rw.EnumByte<EProdState>(ref n.prodState);
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
