namespace GBX.NET.Engines.Game;

/// <summary>
/// Player profile (0x0308C000)
/// </summary>
[Node(0x0308C000), WritingNotSupported]
public sealed class CGamePlayerProfile : CMwNod
{
    private string? description;
    private CGameNetOnlineMessage[]? messages;
    private CInputBindingsConfig? inputBindingsConfig;
    private CGameLeague[]? leagues;

    public string? OnlineLogin { get; set; }
    public string? OnlineSupportKey { get; set; }

    public CGameNetOnlineMessage[]? Messages
    {
        get => messages;
        set => messages = value;
    }

    public CInputBindingsConfig? InputBindingsConfig
    {
        get => inputBindingsConfig;
        set => inputBindingsConfig = value;
    }

    public CGameLeague[]? Leagues
    {
        get => leagues;
        set => leagues = value;
    }

    public string? Description
    {
        get
        {
            DiscoverChunk<Chunk0308C029>();
            return description;
        }
        set
        {
            DiscoverChunk<Chunk0308C029>();
            description = value;
        }
    }

    #region Constructors

    private CGamePlayerProfile()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x0308C000)]
    public class Chunk0308C000 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            n.OnlineLogin = rw.String(n.OnlineLogin);
            n.OnlineSupportKey = rw.String(n.OnlineSupportKey);
        }
    }

    #endregion

    #region 0x003 chunk

    [Chunk(0x0308C003)]
    public class Chunk0308C003 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Array<int>(count: 16);
        }
    }

    #endregion

    #region 0x004 chunk

    [Chunk(0x0308C004)]
    public class Chunk0308C004 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Array<int>();
        }
    }

    #endregion

    #region 0x006 chunk

    [Chunk(0x0308C006)]
    public class Chunk0308C006 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var array = rw.Array(null, r => new
            {
                u01 = r.ReadInt32(),
                u02 = r.ReadByte(),
                u03 = r.ReadString(),
                u04 = r.ReadId(),
                u05 = r.ReadId(),
                u06 = r.ReadArray<int>()
            }, (x, w) => { });
        }
    }

    #endregion

    #region 0x007 chunk

    [Chunk(0x0308C007)]
    public class Chunk0308C007 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x008 chunk

    [Chunk(0x0308C008)]
    public class Chunk0308C008 : Chunk<CGamePlayerProfile>
    {
        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x009 chunk

    [Chunk(0x0308C009)]
    public class Chunk0308C009 : Chunk<CGamePlayerProfile>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    #endregion

    #region 0x00A chunk

    [Chunk(0x0308C00A)]
    public class Chunk0308C00A : Chunk<CGamePlayerProfile>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x011 chunk

    [Chunk(0x0308C011)]
    public class Chunk0308C011 : Chunk<CGamePlayerProfile>
    {
        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x014 chunk

    [Chunk(0x0308C014)]
    public class Chunk0308C014 : Chunk<CGamePlayerProfile>
    {
        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x01B chunk

    [Chunk(0x0308C01B)]
    public class Chunk0308C01B : Chunk<CGamePlayerProfile>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var cars = rw.Reader!.ReadArray(r => r.ReadId());
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
        }
    }

    #endregion

    #region 0x01E chunk

    [Chunk(0x0308C01E)]
    public class Chunk0308C01E : Chunk<CGamePlayerProfile>
    {
        public override void Read(CGamePlayerProfile n, GameBoxReader r)
        {
            var skins = r.ReadArray(r1 => new Skin
            {
                PlayerModel = r1.ReadIdent(),
                SkinFile = r1.ReadString(),
                Checksum = r1.ReadUInt32()
            });
        }
    }

    #endregion

    #region 0x01F chunk

    [Chunk(0x0308C01F)]
    public class Chunk0308C01F : Chunk<CGamePlayerProfile>
    {
        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var profileName = rw.Reader!.ReadString();
            var displayProfileName = rw.Reader!.ReadString();
            var deviceGuid = rw.Reader!.ReadId();
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x023 chunk

    [Chunk(0x0308C023)]
    public class Chunk0308C023 : Chunk<CGamePlayerProfile>
    {
        public int U01;

        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x029 chunk

    [Chunk(0x0308C029)]
    public class Chunk0308C029 : SkippableChunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.description);
        }
    }

    #endregion

    #region 0x038 chunk

    [Chunk(0x0308C038)]
    public class Chunk0308C038 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Int32();
            var u02 = rw.Int32();
            var u03 = rw.Int32();
            var u04 = rw.Bytes(count: 66);
            var u05 = rw.Array<int>(count: 16);
            var profileName = rw.String();
            var u06 = rw.Int32();
        }
    }

    #endregion

    #region 0x040 chunk

    [Chunk(0x0308C040)]
    public class Chunk0308C040 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Array<int>(count: 2);
        }
    }

    #endregion

    #region 0x041 chunk

    [Chunk(0x0308C041)]
    public class Chunk0308C041 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Int32();
        }
    }

    #endregion

    #region 0x043 chunk

    [Chunk(0x0308C043)]
    public class Chunk0308C043 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var vehicleSkins = rw.Array(null, r => new
            {
                vehicle = rw.Ident(),
                skin = rw.String(),
                u01 = rw.Bytes(count: 16),
                u02 = rw.Int32(),
                u03 = rw.Array<float>(count: 2)
            }, (x, w) => { });
        }
    }

    #endregion

    #region 0x044 chunk

    [Chunk(0x0308C044)]
    public class Chunk0308C044 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x047 chunk

    [Chunk(0x0308C047)]
    public class Chunk0308C047 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.ArrayNode<CGameNetOnlineMessage>(ref n.messages!);
            rw.Int32();
            rw.Int32();
        }
    }

    #endregion

    #region 0x04A chunk

    [Chunk(0x0308C04A)]
    public class Chunk0308C04A : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
        }
    }

    #endregion

    #region 0x04B chunk

    [Chunk(0x0308C04B)]
    public class Chunk0308C04B : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CInputBindingsConfig>(ref n.inputBindingsConfig);
        }
    }

    #endregion

    #region 0x04C chunk

    [Chunk(0x0308C04C)]
    public class Chunk0308C04C : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Byte();
            var u01 = rw.Array<int>(count: 5);
            rw.ArrayNode<CGameLeague>(ref n.leagues!);
        }
    }

    #endregion

    #region 0x04D chunk

    [Chunk(0x0308C04D)]
    public class Chunk0308C04D : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
        }
    }

    #endregion

    #region 0x050 chunk

    [Chunk(0x0308C050)]
    public class Chunk0308C050 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
        }
    }

    #endregion

    #region 0x053 chunk

    [Chunk(0x0308C053)]
    public class Chunk0308C053 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var myZone = rw.String();
        }
    }

    #endregion

    #region 0x054 chunk

    [Chunk(0x0308C054)]
    public class Chunk0308C054 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x056 chunk

    [Chunk(0x0308C056)]
    public class Chunk0308C056 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Int32();
            var friendList = rw.Array(null, r => new
            {
                u01 = r.ReadInt32(),
                login = r.ReadString(),
                nickname = r.ReadString(),
                u03 = r.ReadArray<int>(9),
                u04 = r.ReadByte(),
                u05 = r.ReadArray(r => new
                {
                    id = r.ReadId(),
                    u01 = r.ReadInt32(),
                    u02 = r.ReadInt32()
                }),
                zone = r.ReadString(),
                u07 = r.ReadInt32()
            }, (x, w) => { });
        }
    }

    #endregion

    #region 0x058 chunk

    [Chunk(0x0308C058)]
    public class Chunk0308C058 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x059 chunk

    [Chunk(0x0308C059)]
    public class Chunk0308C059 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x05B chunk

    [Chunk(0x0308C05B)]
    public class Chunk0308C05B : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var profileName = rw.String();
            var nickname = rw.String();
            var guid = rw.Id();
            rw.Int32();
            rw.Int32();
        }
    }

    #endregion

    #region 0x05E chunk

    [Chunk(0x0308C05E)]
    public class Chunk0308C05E : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var shootParams = rw.NodeRef<CGameCtnMediaShootParams>();
        }
    }

    #endregion

    #region 0x05F chunk

    [Chunk(0x0308C05F)]
    public class Chunk0308C05F : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x061 chunk

    [Chunk(0x0308C061)]
    public class Chunk0308C061 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var country = rw.String();
        }
    }

    #endregion

    #region 0x062 chunk

    [Chunk(0x0308C062)]
    public class Chunk0308C062 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var version = rw.Int32();
            var nod = rw.NodeRef<CInputBindingsConfig>();
        }
    }

    #endregion

    #region 0x064 chunk

    [Chunk(0x0308C064)]
    public class Chunk0308C064 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }

    #endregion

    #region 0x065 chunk

    [Chunk(0x0308C065)]
    public class Chunk0308C065 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x068 chunk

    [Chunk(0x0308C068)]
    public class Chunk0308C068 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Array(null, r => new
            {
                u01 = r.ReadId(),
                u02 = r.ReadInt32()
            }, (x, w) => { });
        }
    }

    #endregion

    #region 0x069 chunk

    [Chunk(0x0308C069)]
    public class Chunk0308C069 : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Int32();
            var description = rw.String();
            var u02 = rw.Int32();
            var u03 = rw.Int32();
            var login = rw.String();
            var u04 = rw.Int32();
            var u05 = rw.Bytes(count: 26);
            var hash = rw.String();
            var u06 = rw.String();
            var u07 = rw.Int32();
            var u08 = rw.Int32();
            var serverUrl = rw.String();
            var u09 = rw.String();
        }
    }

    #endregion

    #region 0x06A chunk

    [Chunk(0x0308C06A)]
    public class Chunk0308C06A : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x06B chunk

    [Chunk(0x0308C06B)]
    public class Chunk0308C06B : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x06C chunk

    [Chunk(0x0308C06C)]
    public class Chunk0308C06C : Chunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    #endregion

    #region 0x07C chunk

    [Chunk(0x0308C07C)]
    public class Chunk0308C07C : SkippableChunk<CGamePlayerProfile>
    {
        public override void ReadWrite(CGamePlayerProfile n, GameBoxReaderWriter rw)
        {
            var keyboardGuid = rw.Id();
            var profileName = rw.String();
        }
    }

    #endregion

    #endregion

    public class Skin
    {
        public Ident PlayerModel { get; set; } = new Ident();
        public string? SkinFile { get; set; }
        public uint Checksum { get; set; }
    }
}
