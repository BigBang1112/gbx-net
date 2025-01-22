using GBX.NET.Components;

namespace GBX.NET.Engines.GameData;

public partial class CGameCtnCollector
{
    private int catalogPosition;
    public int CatalogPosition { get => catalogPosition; set => catalogPosition = value; }

    /// <summary>
    /// Icon of the collector in 2D pixel array format from all versions except icons created after April 2022 in TM2020.
    /// </summary>
    public Color[,]? Icon { get; set; }

    /// <summary>
    /// Icon of the collector in WebP format from TM2020 icons since April 2022 update.
    /// </summary>
    [WebpData]
    public byte[]? IconWebP { get; set; }

    private CMwNod? iconFid;
    private GbxRefTableFile? iconFidFile;
    public CMwNod? IconFid { get => iconFid; set => iconFid = value; }

    private string? skinDirectory;
    public string? SkinDirectory { get => skinDirectory; set => skinDirectory = value; }

    public partial class HeaderChunk2E001004
    {
        public short U01 = 1;

        public override void Read(CGameCtnCollector n, GbxReader r)
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
                n.IconWebP = r.ReadData();

                return;
            }

            var iconData = r.ReadArray<int>(width * height);

            n.Icon = new Color[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    n.Icon[x, height - 1 - y] = new Color(iconData[y * width + x]);
                }
            }
        }

        public override void Write(CGameCtnCollector n, GbxWriter w)
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
                {
                    for (var x = 0; x < width; x++)
                    {
                        w.Write(int.MaxValue);
                    }
                }

                return;
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    w.Write(n.Icon[x, height - 1 - y].ToArgb());
                }
            }
        }
    }

    public partial class Chunk2E001009
    {
        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.String(ref n.pageName);

            if (rw.Boolean(n.IconFid is not null))
            {
                rw.NodeRef(ref n.iconFid, ref n.iconFidFile);
            }

            rw.Id(ref n.parentCollectorId);
        }
    }

    public partial class Chunk2E001010 : IVersionable
    {
        public CMwNod? U01;
        private GbxRefTableFile? U01File;
        public CMwNod? U02;

        public int Version { get; set; }

        public override void ReadWrite(CGameCtnCollector n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef(ref U01, ref U01File); // skin direct reference inside pak
            rw.String(ref n.skinDirectory);

            if (Version >= 2 && string.IsNullOrEmpty(n.skinDirectory))
            {
                rw.NodeRef(ref U02);
            }
        }
    }
}
