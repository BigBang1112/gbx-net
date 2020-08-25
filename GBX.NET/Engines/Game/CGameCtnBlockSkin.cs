using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03059000)]
    public class CGameCtnBlockSkin : Node
    {
        public string Text
        {
            get => GetValue<Chunk000, Chunk001, Chunk002>(x => x.Text, x=> x.Text, x => x.Text) as string;
            set => SetValue<Chunk000, Chunk001, Chunk002>(x => x.Text = value, x => x.Text = value, x => x.Text = value);
        }

        public FileRef PackDesc
        {
            get => GetValue<Chunk001, Chunk002>(x => x.PackDesc, x => x.PackDesc) as FileRef;
            set => SetValue<Chunk001, Chunk002>(x => x.PackDesc = value, x => x.PackDesc = value);
        }

        public FileRef ParentPackDesc
        {
            get => GetValue<Chunk002>(x => x.ParentPackDesc) as FileRef;
            set => SetValue<Chunk002>(x => x.ParentPackDesc = value);
        }

        public FileRef SecondaryPackDesc
        {
            get => GetValue<Chunk003>(x => x.SecondaryPackDesc) as FileRef;
            set => SetValue<Chunk003>(x => x.SecondaryPackDesc = value);
        }

        public CGameCtnBlockSkin(ILookbackable lookbackable) : this(lookbackable, 0x03059000)
        {

        }

        public CGameCtnBlockSkin(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03059000)]
        public class Chunk000 : Chunk
        {
            public string Text { get; set; }
            public string Ignored { get; set; }

            public Chunk000(CGameCtnBlockSkin node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Text = rw.String(Text);
                Ignored = rw.String(Ignored);
            }
        }

        #endregion

        #region 0x001 chunk

        [Chunk(0x03059001)]
        public class Chunk001 : Chunk
        {
            public string Text { get; set; }
            public FileRef PackDesc { get; set; } = new FileRef(3, new byte[32], "", "");

            public Chunk001(CGameCtnBlockSkin node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Text = rw.String(Text);
                PackDesc = rw.FileRef(PackDesc);
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x03059002)]
        public class Chunk002 : Chunk
        {
            public string Text { get; set; }
            public FileRef PackDesc { get; set; } = new FileRef(3, new byte[32], "", "");
            public FileRef ParentPackDesc { get; set; } = new FileRef(3, new byte[32], "", "");

            public Chunk002(CGameCtnBlockSkin node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Text = rw.String(Text);
                PackDesc = rw.FileRef(PackDesc);
                ParentPackDesc = rw.FileRef(ParentPackDesc);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03059003)]
        public class Chunk003 : Chunk
        {
            public int Version { get; set; }
            public FileRef SecondaryPackDesc { get; set; } = new FileRef();

            public Chunk003(CGameCtnBlockSkin node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                SecondaryPackDesc = rw.FileRef(SecondaryPackDesc);
            }
        }

        #endregion

        #endregion
    }
}
