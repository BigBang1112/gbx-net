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
        public CGameCtnCollector(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x003 chunk

        [Chunk(0x2E001003)]
        public class Chunk003 : SkippableChunk
        {
            public Meta Metadata { get; set; }
            public int Version { get; set; }
            public string PageName { get; set; }
            public short CatalogPosition { get; set; }
            public string Name { get; set; }

            public Chunk003(CGameCtnCollector node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Metadata = rw.Meta(Metadata);
                Version = rw.Int32(Version);
                PageName = rw.String(PageName);

                if (Version == 5) rw.LookbackString(Unknown);
                if (Version >= 4) rw.LookbackString(Unknown);

                if (Version >= 3)
                {
                    rw.Int32(Unknown);

                    CatalogPosition = rw.Int16(CatalogPosition);
                }

                if (Version >= 7)
                    Name = rw.String(Name);

                // prodState
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x2E001004)]
        public class Chunk004 : SkippableChunk
        {
            public Task<Bitmap> Icon { get; set; }

            public Chunk004(CGameCtnCollector node, byte[] data) : base(node, data)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var width = r.ReadInt16();
                var height = r.ReadInt16();

                var iconData = r.ReadBytes(width * height * 4);

                Icon = Task.Run(() =>
                {
                    var bitmap = new Bitmap(width, height);
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                    Marshal.Copy(iconData, 0, bitmapData.Scan0, iconData.Length);
                    bitmap.UnlockBits(bitmapData);
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    return bitmap;
                });
            }

            public void ExportIcon(Stream stream, ImageFormat format)
            {
                Icon.Result.Save(stream, format);
            }

            public void ExportIcon(string fileName, ImageFormat format)
            {
                Icon.Result.Save(fileName, format);
            }
        }

        #endregion

        #region 0x006 chunk

        [Chunk(0x2E001006)]
        public class Chunk006 : SkippableChunk
        {
            public long FileTime { get; set; }

            public Chunk006(CGameCtnCollector node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                FileTime = rw.Int64(FileTime);
            }
        }

        #endregion

        #region 0x008 chunk

        [Chunk(0x2E001008)]
        public class Chunk008 : Chunk
        {
            public string SkinFile { get; set; }

            public Chunk008(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown); // 0
                SkinFile = rw.String(SkinFile);
            }
        }

        #endregion

        #region 0x009 chunk

        [Chunk(0x2E001009)]
        public class Chunk009 : Chunk
        {
            public string PageName { get; set; }
            public Node IconFid { get; set; }

            public Chunk009(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                PageName = rw.String(PageName);

                var hasIconFid = rw.Boolean(IconFid != null);
                if (hasIconFid)
                    IconFid = rw.NodeRef(IconFid);

                rw.LookbackString(Unknown);
            }
        }

        #endregion

        #region 0x00B chunk

        [Chunk(0x2E00100B)]
        public class Chunk00B : Chunk
        {
            public Meta Decoration { get; set; }

            public Chunk00B(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Decoration = rw.Meta(Decoration);
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x2E00100C)]
        public class Chunk00C : Chunk
        {
            public string CollectorName { get; set; }

            public Chunk00C(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                CollectorName = rw.String(CollectorName);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x2E00100D)]
        public class Chunk00D : Chunk
        {
            public string Description { get; set; }

            public Chunk00D(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Description = rw.String(Description);
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x2E00100E)]
        public class Chunk00E : Chunk
        {
            public bool IconUseAutoRender { get; set; }
            public int IconQuarterRotationY { get; set; }

            public Chunk00E(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                IconUseAutoRender = rw.Boolean(IconUseAutoRender);
                IconQuarterRotationY = rw.Int32(IconQuarterRotationY);
            }
        }

        #endregion

        #region 0x010 chunk

        [Chunk(0x2E001010)]
        public class Chunk010 : Chunk
        {
            public Chunk010(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
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
        public class Chunk011 : Chunk
        {
            public Chunk011(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
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
