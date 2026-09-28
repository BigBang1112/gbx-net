namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03093000</remarks>
[Class(0x03093000)]
public partial class CGameCtnReplayRecord : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03093000;




    /// <summary>
    /// [SHeaderVersion] CGameCtnReplayRecord 0x000 header chunk (basic)
    /// </summary>
    [Chunk(0x03093000, "basic")]
    [ChunkGameVersion(GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020, 1, 4, 4, 6, 7, 8, 8, 8, 8)]
    public partial class HeaderChunk03093000 : HeaderChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// [SHeaderCommunity] CGameCtnReplayRecord 0x001 header chunk (xml)
    /// </summary>
    [Chunk(0x03093001, "xml")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk03093001 : HeaderChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// [SAuthorInfo] CGameCtnReplayRecord 0x002 header chunk (author)
    /// </summary>
    [Chunk(0x03093002, "author")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class HeaderChunk03093002 : HeaderChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }


    /// <summary>
    /// CGameCtnReplayRecord 0x002 chunk (track)
    /// </summary>
    [Chunk(0x03093002, "track")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093002 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x003 chunk (validation TM1.0)
    /// </summary>
    [Chunk(0x03093003, "validation TM1.0")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk03093003 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x004 chunk (ghosts)
    /// </summary>
    [Chunk(0x03093004, "ghosts")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk03093004 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x005 chunk
    /// </summary>
    [Chunk(0x03093005)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk03093005 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x007 skippable chunk
    /// </summary>
    [Chunk(0x03093007)]
    [ChunkGameVersion(GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT)]
    public partial class Chunk03093007 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x008 skippable chunk (game)
    /// </summary>
    [Chunk(0x03093008, "game")]
    [ChunkGameVersion(GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk03093008 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x00C chunk (clip)
    /// </summary>
    [Chunk(0x0309300C, "clip")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk0309300C : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309300C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x00D chunk (validation)
    /// </summary>
    [Chunk(0x0309300D, "validation")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0309300D : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309300D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x00E chunk (events)
    /// </summary>
    [Chunk(0x0309300E, "events")]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk0309300E : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309300E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0309300F)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0309300F : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309300F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x010 chunk (simple events display)
    /// </summary>
    [Chunk(0x03093010, "simple events display")]
    public partial class Chunk03093010 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093010;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x011 chunk
    /// </summary>
    [Chunk(0x03093011)]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMU)]
    public partial class Chunk03093011 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMU;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x013 skippable chunk
    /// </summary>
    [Chunk(0x03093013)]
    [ChunkGameVersion(GameVersion.TMU)]
    public partial class Chunk03093013 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093013;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x014 chunk (ghosts)
    /// </summary>
    [Chunk(0x03093014, "ghosts")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093014 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093014;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x015 chunk (clip)
    /// </summary>
    [Chunk(0x03093015, "clip")]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093015 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093015;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x018 skippable chunk (author)
    /// </summary>
    [Chunk(0x03093018, "author")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093018 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093018;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x01A skippable chunk (scenery vortex key)
    /// </summary>
    [Chunk(0x0309301A, "scenery vortex key")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309301A : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309301A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x01B skippable chunk (player of interest)
    /// </summary>
    [Chunk(0x0309301B, "player of interest")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309301B : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309301B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x01C skippable chunk
    /// </summary>
    [Chunk(0x0309301C)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309301C : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309301C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x01D skippable chunk (InterfaceScriptInfos)
    /// </summary>
    [Chunk(0x0309301D, "InterfaceScriptInfos")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309301D : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309301D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x01E skippable chunk (actions)
    /// </summary>
    [Chunk(0x0309301E, "actions")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309301E : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309301E;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x01F skippable chunk (AnchoredObjectInfos)
    /// </summary>
    [Chunk(0x0309301F, "AnchoredObjectInfos")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0309301F : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x0309301F;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x020 skippable chunk (item skins and names)
    /// </summary>
    [Chunk(0x03093020, "item skins and names")]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093020 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093020;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x021 skippable chunk
    /// </summary>
    [Chunk(0x03093021)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093021 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093021;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x022 skippable chunk (TimedCamVal)
    /// </summary>
    [Chunk(0x03093022, "TimedCamVal")]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk03093022 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093022;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x023 skippable chunk (BonusBumpKey)
    /// </summary>
    [Chunk(0x03093023, "BonusBumpKey")]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk03093023 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093023;

        /// <inheritdoc />
        public override bool Ignore => true;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x024 chunk (record data)
    /// </summary>
    [Chunk(0x03093024, "record data")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 1, 1)]
    public partial class Chunk03093024 : Chunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093024;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x025 skippable chunk
    /// </summary>
    [Chunk(0x03093025)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093025 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093025;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x026 skippable chunk (EntDataSceneUIdsToGhost)
    /// </summary>
    [Chunk(0x03093026, "EntDataSceneUIdsToGhost")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk03093026 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093026;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x027 skippable chunk
    /// </summary>
    [Chunk(0x03093027)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03093027 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093027;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x028 skippable chunk
    /// </summary>
    [Chunk(0x03093028)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03093028 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093028;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnReplayRecord 0x029 skippable chunk
    /// </summary>
    [Chunk(0x03093029)]
    [ChunkGameVersion(GameVersion.TM2020)]
    public partial class Chunk03093029 : SkippableChunk<CGameCtnReplayRecord>
    {
        /// <inheritdoc />
        public override uint Id => 0x03093029;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM2020;

    }


    public sealed partial class EntDataSceneUIdsToGhost : IReadable, IWritable
    {
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            U02 = r.ReadInt32();
            U03 = r.ReadInt32();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
            w.Write(U03);
        }
    }

    public sealed partial class InterfaceScriptInfo : IReadable, IWritable
    {
        public string[]? U01 { get; set; }
        public int U02 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadArrayString();
            U02 = r.ReadInt32();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.WriteArray(U01);
            w.Write(U02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03093002 => new Chunk03093002(),
        0x03093003 => new Chunk03093003(),
        0x03093004 => new Chunk03093004(),
        0x03093005 => new Chunk03093005(),
        0x03093007 => new Chunk03093007(),
        0x03093008 => new Chunk03093008(),
        0x0309300C => new Chunk0309300C(),
        0x0309300D => new Chunk0309300D(),
        0x0309300E => new Chunk0309300E(),
        0x0309300F => new Chunk0309300F(),
        0x03093010 => new Chunk03093010(),
        0x03093011 => new Chunk03093011(),
        0x03093013 => new Chunk03093013(),
        0x03093014 => new Chunk03093014(),
        0x03093015 => new Chunk03093015(),
        0x03093018 => new Chunk03093018(),
        0x0309301A => new Chunk0309301A(),
        0x0309301B => new Chunk0309301B(),
        0x0309301C => new Chunk0309301C(),
        0x0309301D => new Chunk0309301D(),
        0x0309301E => new Chunk0309301E(),
        0x0309301F => new Chunk0309301F(),
        0x03093020 => new Chunk03093020(),
        0x03093021 => new Chunk03093021(),
        0x03093022 => new Chunk03093022(),
        0x03093023 => new Chunk03093023(),
        0x03093024 => new Chunk03093024(),
        0x03093025 => new Chunk03093025(),
        0x03093026 => new Chunk03093026(),
        0x03093027 => new Chunk03093027(),
        0x03093028 => new Chunk03093028(),
        0x03093029 => new Chunk03093029(),
        _ => base.NewChunk(chunkId),
    };
}
