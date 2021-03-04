using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E001000)]
    public class CGameCtnCollector : Node
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

        private Ident ident;
        private string pageName;
        private int catalogPosition;
        private string name;
        private string description;
        private bool iconUseAutoRender;
        private int iconQuarterRotationY;
        private EProdState prodState;
        private string skinDirectory;
        private string collectorName;
        private bool isInternal;
        private bool isAdvanced;

        #endregion

        #region Properties

        [NodeMember]
        public Ident Ident
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
        public string Name
        {
            get => name;
            set => name = value;
        }

        [NodeMember]
        public Task<Bitmap> Icon { get; set; }

        [NodeMember]
        public Node IconFid { get; set; }

        [NodeMember]
        public string CollectorName
        {
            get => collectorName;
            set => collectorName = value;
        }

        [NodeMember]
        public string Description
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
        public string SkinDirectory
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

        #endregion

        #region Methods

        public void ExportIcon(Stream stream, ImageFormat format)
        {
            Icon.Result.Save(stream, format);
        }

        public void ExportIcon(string fileName, ImageFormat format)
        {
            Icon.Result.Save(fileName, format);
        }

        #endregion

        #region Chunks

        #region 0x003 header chunk

        /// <summary>
        /// CGameCtnCollector 0x003 header chunk
        /// </summary>
        [Chunk(0x2E001003)]
        public class Chunk2E001003 : HeaderChunk<CGameCtnCollector>
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Ident(ref n.ident);
                rw.Int32(ref version);
                rw.String(ref n.pageName);

                if (version == 5)
                    rw.Int32(Unknown);
                if (version >= 4)
                    rw.Int32(Unknown);

                if (version >= 3)
                {
                    rw.Int32(Unknown);
                    n.CatalogPosition = rw.Int16((short)n.CatalogPosition);
                }

                if (version >= 7)
                    rw.String(ref n.name);

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
            public override void Read(CGameCtnCollector n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var width = r.ReadInt16();
                var height = r.ReadInt16();

                var iconData = r.ReadBytes(width * height * 4);

                n.Icon = Task.Run(() =>
                {
                    var bitmap = new Bitmap(width, height);
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                    Marshal.Copy(iconData, 0, bitmapData.Scan0, iconData.Length);
                    bitmap.UnlockBits(bitmapData);
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    return bitmap;
                });
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
            public long FileTime { get; set; }

            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                FileTime = rw.Int64(FileTime);
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
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
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
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
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
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown); // 0
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
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.String(ref n.pageName);

                var hasIconFid = rw.Boolean(n.IconFid != null);
                if (hasIconFid)
                    n.IconFid = rw.NodeRef(n.IconFid);

                rw.Id(Unknown);
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
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(Unknown);
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
                rw.String(ref n.collectorName);
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
        public class Chunk2E001010 : Chunk<CGameCtnCollector>
        {
            private int version;
            private Node u01;
            private int u02;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public Node U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version); // 2
                rw.NodeRef(ref u01); // -1
                rw.String(ref n.skinDirectory);
                if(n.skinDirectory.Length == 0)
                    rw.Int32(ref u02); // -1
            }
        }

        #endregion

        #region 0x011 chunk

        /// <summary>
        /// CGameCtnCollector 0x011 chunk
        /// </summary>
        [Chunk(0x2E001011)]
        public class Chunk2E001011 : Chunk<CGameCtnCollector>
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

        #endregion
    }
}
