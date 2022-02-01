namespace GBX.NET.Engines.Game;

[Node(0x03033000)]
[NodeExtension("TMCollection")]
[NodeExtension("Collection")]
public class CGameCtnCollection : CMwNod
{
    public byte CollectionID { get; set; }
    public byte CollectionPackMask { get; set; }
    public string? DisplayName { get; set; }
    public string? CollectionIcon { get; set; }
    public string? BlockInfoFlat { get; set; }
    public Ident? Vehicle { get; set; }
    public string? LoadingScreen { get; set; }

    #region Constructors

    protected CGameCtnCollection()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    [Chunk(0x03033001)]
    [ChunkWithOwnIdState]
    public class Chunk03033001 : Chunk<CGameCtnCollection>
    {
        public int Version { get; set; }

        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            Version = r.ReadByte();
            n.CollectionID = r.ReadByte();
            _ = r.ReadBytes(6);
            n.CollectionPackMask = r.ReadByte();
            n.DisplayName = r.ReadString();
            _ = r.ReadInt32();
            n.CollectionIcon = r.ReadString();
            _ = r.ReadArray<int>(2);
            n.BlockInfoFlat = r.ReadId();
            n.Vehicle = r.ReadIdent();
            _ = r.ReadInt32();
            _ = r.ReadArray<float>(4);
            n.LoadingScreen = r.ReadString();
            _ = r.ReadArray<int>(7);
            _ = r.ReadString();
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x03033002)]
    public class Chunk03033002 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadByte();
            _ = r.ReadString();
            _ = r.ReadInt32();
            _ = r.ReadString();
            _ = r.ReadArray<int>(3);
        }
    }

    #endregion

    #region 0x003 chunk

    [Chunk(0x03033003)]
    public class Chunk03033003 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadByte();
            _ = r.ReadString();
        }
    }

    #endregion

    #region 0x009 chunk

    [Chunk(0x03033009)]
    public class Chunk03033009 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadId();
            _ = r.ReadInt32();
            _ = r.ReadArray(r1 => r1.ReadInt32());
            _ = r.ReadInt32();
            _ = r.ReadArray<float>(3);
            n.Vehicle = r.ReadIdent();
        }
    }

    #endregion

    #region 0x00C chunk

    [Chunk(0x0303300C)]
    public class Chunk0303300C : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            r.ReadInt32();
            r.ReadInt32();
        }
    }

    #endregion

    #region 0x00D chunk

    [Chunk(0x0303300D)]
    public class Chunk0303300D : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            r.ReadInt32();
            r.ReadInt32();
            r.ReadInt32();
        }
    }

    #endregion

    #region 0x00E chunk

    [Chunk(0x0303300E)]
    public class Chunk0303300E : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            r.ReadInt32();
        }
    }

    #endregion

    #region 0x011 chunk

    [Chunk(0x03033011)]
    public class Chunk03033011 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            r.ReadInt32();
        }
    }

    #endregion

    #region 0x019 chunk

    [Chunk(0x03033019)]
    public class Chunk03033019 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            r.ReadInt32();
        }
    }

    #endregion

    #region 0x01A chunk

    [Chunk(0x0303301A)]
    public class Chunk0303301A : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32(); // -1
            _ = r.ReadArray<float>(11);
        }
    }

    #endregion

    #region 0x01D chunk

    [Chunk(0x0303301D)]
    public class Chunk0303301D : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
            _ = r.ReadIdent();
            _ = r.ReadInt32();
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x01F chunk

    [Chunk(0x0303301F)]
    public class Chunk0303301F : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x020 chunk

    [Chunk(0x03033020)]
    public class Chunk03033020 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadString();
            _ = r.ReadInt32();
            _ = r.ReadString();
            _ = r.ReadString();
        }
    }

    #endregion

    #region 0x021 chunk

    [Chunk(0x03033021)]
    public class Chunk03033021 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadString();
        }
    }

    #endregion

    #region 0x027 chunk

    [Chunk(0x03033027)]
    public class Chunk03033027 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x028 chunk

    [Chunk(0x03033028)]
    public class Chunk03033028 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x029 chunk

    [Chunk(0x03033029)]
    public class Chunk03033029 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x02A chunk

    [Chunk(0x0303302A)]
    public class Chunk0303302A : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x02C chunk

    [Chunk(0x0303302C)]
    public class Chunk0303302C : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadArray<int>(4);
        }
    }

    #endregion

    #region 0x02F chunk

    [Chunk(0x0303302F)]
    public class Chunk0303302F : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadVec3();
        }
    }

    #endregion

    #region 0x030 chunk

    [Chunk(0x03033030)]
    public class Chunk03033030 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x031 chunk

    [Chunk(0x03033031)]
    public class Chunk03033031 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #region 0x033 chunk

    [Chunk(0x03033033)]
    public class Chunk03033033 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
            _ = r.ReadArray(r1 => r1.ReadId());
        }
    }

    #endregion

    #region 0x034 chunk

    [Chunk(0x03033034)]
    public class Chunk03033034 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadArray<int>(3);
        }
    }

    #endregion

    #region 0x036 chunk

    [Chunk(0x03033036)]
    public class Chunk03033036 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadArray<int>(2);
        }
    }

    #endregion

    #region 0x037 chunk

    [Chunk(0x03033037)]
    public class Chunk03033037 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadSingle();
        }
    }

    #endregion

    #region 0x038 chunk

    [Chunk(0x03033038)]
    public class Chunk03033038 : Chunk<CGameCtnCollection>
    {
        public override void Read(CGameCtnCollection n, GameBoxReader r)
        {
            _ = r.ReadInt32();
        }
    }

    #endregion

    #endregion
}
