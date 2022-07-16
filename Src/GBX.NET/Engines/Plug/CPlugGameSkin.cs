namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F4000</remarks>
[Node(0x090F4000)]
public class CPlugGameSkin : CMwNod
{
    protected CPlugGameSkin()
    {

    }

    /// <summary>
    /// CPlugGameSkin 0x000 header chunk
    /// </summary>
    [Chunk(0x090F4000)]
    public class Chunk090F4000 : HeaderChunk<CPlugGameSkin>, IVersionable
    {
        private int version;

        public string? U01;
        public string? U02;
        public string? U03;
        public byte U04;
        public UnknownClass[]? U05;

        public int Version { get => version; set => version = value; }

        public override void Read(CPlugGameSkin n, GameBoxReader r)
        {
            
        }

        public override void Write(CPlugGameSkin n, GameBoxWriter w)
        {
            
        }

        public override void ReadWrite(GameBoxReaderWriter rw)
        {
            rw.Byte(ref version);
            rw.String(ref U01);

            if (version >= 1)
            {
                rw.String(ref U02);
                rw.String(ref U03);
            }

            if (rw.Reader is GameBoxReader r)
            {
                var count = r.ReadByte();
                
                U05 = r.ReadArray(count, r =>
                {
                    var u01 = r.ReadUInt32();
                    var u02 = r.ReadString();
                    var u05 = false;

                    if (version >= 3)
                    {
                        //u05 = r.ReadBoolean();
                    }

                    var u03 = r.ReadString();
                    var u04 = r.ReadBoolean();

                    return new UnknownClass(u01, u02, u03, u04, u05);
                });
            }

            if (rw.Writer is GameBoxWriter w)
            {
                if (U05 is null)
                {
                    w.Write((byte)0);
                }
                else
                {
                    w.Write((byte)U05.Length);

                    for (var i = 0; i < U05.Length; i++)
                    {
                        w.Write(U05[i].U01);
                        w.Write(U05[i].U02);

                        if (version >= 3)
                        {
                            w.Write(U05[i].U05);
                        }
                        
                        w.Write(U05[i].U03);
                        w.Write(U05[i].U04);
                    }
                }
            }
        }

        public class UnknownClass
        {
            public uint U01 { get; init; }
            public string U02 { get; init; }
            public string U03 { get; init; }
            public bool U04 { get; init; }
            public bool U05 { get; init; }

            public UnknownClass(uint u01, string u02, string u03, bool u04, bool u05)
            {
                U01 = u01;
                U02 = u02;
                U03 = u03;
                U04 = u04;
                U05 = u05;
            }
        }
    }
}
