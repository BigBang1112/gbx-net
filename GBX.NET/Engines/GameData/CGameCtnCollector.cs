using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.GameData
{
    [Node(0x2E001000)]
    public class CGameCtnCollector : Node
    {
        #region Properties

        [NodeMember]
        public Ident Ident { get; set; }

        [NodeMember]
        public string PageName { get; set; }

        [NodeMember]
        public short CatalogPosition { get; set; }

        [NodeMember]
        public string Name { get; set; }

        [NodeMember]
        public Task<Bitmap> Icon { get; set; }

        [NodeMember]
        public string SkinFile { get; set; }

        [NodeMember]
        public Node IconFid { get; set; }

        [NodeMember]
        public string CollectorName { get; set; }

        [NodeMember]
        public string Description { get; set; }

        [NodeMember]
        public bool IconUseAutoRender { get; set; }

        [NodeMember]
        public int IconQuarterRotationY { get; set; }

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

        [Chunk(0x2E001003)]
        public class Chunk2E001003 : HeaderChunk<CGameCtnCollector>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                n.Ident = rw.Ident(n.Ident);
                Version = rw.Int32(Version);
                n.PageName = rw.String(n.PageName);

                if (Version == 5)
                    rw.Int32(Unknown);
                if (Version >= 4)
                    rw.Int32(Unknown);

                if (Version >= 3)
                {
                    rw.Int32(Unknown);
                    n.CatalogPosition = rw.Int16(n.CatalogPosition);
                }

                if (Version >= 7)
                    n.Name = rw.String(n.Name);

                // prodState
            }
        }

        #endregion

        #region 0x004 header chunk

        [Chunk(0x2E001004)]
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

        #region 0x006 header chunk

        [Chunk(0x2E001006)]
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

        [Chunk(0x2E001008)]
        public class Chunk2E001008 : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown); // 0
                n.SkinFile = rw.String(n.SkinFile);
            }
        }

        #endregion

        #region 0x009 chunk

        [Chunk(0x2E001009)]
        public class Chunk2E001009 : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                n.PageName = rw.String(n.PageName);

                var hasIconFid = rw.Boolean(n.IconFid != null);
                if (hasIconFid)
                    n.IconFid = rw.NodeRef(n.IconFid);

                rw.LookbackString(Unknown);
            }
        }

        #endregion

        #region 0x00A chunk

        [Chunk(0x2E00100A)]
        public class Chunk2E00100A : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(Unknown);
            }
        }

        #endregion

        #region 0x00B chunk

        [Chunk(0x2E00100B)]
        public class Chunk2E00100B : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                n.Ident = rw.Ident(n.Ident);
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x2E00100C)]
        public class Chunk2E00100C : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                n.CollectorName = rw.String(n.CollectorName);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x2E00100D)]
        public class Chunk2E00100D : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                n.Description = rw.String(n.Description);
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x2E00100E)]
        public class Chunk2E00100E : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                n.IconUseAutoRender = rw.Boolean(n.IconUseAutoRender);
                n.IconQuarterRotationY = rw.Int32(n.IconQuarterRotationY);
            }
        }

        #endregion

        #region 0x010 chunk

        [Chunk(0x2E001010)]
        public class Chunk2E001010 : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown); // 2
                rw.Int32(Unknown); // -1
                rw.Int32(Unknown); // 0
                rw.Int32(Unknown); // -1
            }
        }

        #endregion

        #region 0x011 chunk

        [Chunk(0x2E001011)]
        public class Chunk2E001011 : Chunk<CGameCtnCollector>
        {
            public override void ReadWrite(CGameCtnCollector n, GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #endregion
    }
}
