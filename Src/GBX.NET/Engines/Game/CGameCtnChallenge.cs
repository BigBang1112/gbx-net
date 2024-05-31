using GBX.NET.Extensions;
using GBX.NET.Interfaces.Game;
using System.IO.Compression;
using System.Text;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge :
    IGameCtnChallengeTM10,
    IGameCtnChallengeTMSX,
    IGameCtnChallengeTMF,
    IGameCtnChallengeMP4,
    IGameCtnChallengeTM2020
{
    private string authorLogin;
    private TimeInt32? bronzeTime; // Only used if ChallengeParameters is null
    private TimeInt32? silverTime; // Only used if ChallengeParameters is null
    private TimeInt32? goldTime; // Only used if ChallengeParameters is null
    private TimeInt32? authorTime; // Only used if ChallengeParameters is null
    private int authorScore; // Only used if ChallengeParameters is null

    private Ident mapInfo = Ident.Empty;
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Ident MapInfo { get => mapInfo; set => mapInfo = value; }

    private string mapName = string.Empty;
    [SupportsFormatting]
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk03043012>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public string MapName { get => mapName; set => mapName = value; }


    private Int3 size;
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Int3 Size { get => size; set => size = value; }

    [AppliedWithChunk<HeaderChunk03043008>]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    [AppliedWithChunk<Chunk03043042>]
    public string AuthorLogin
    {
        get => authorLogin is null ? mapInfo.Author : authorLogin;
        set
        {
            authorLogin = value;
            mapInfo = new Ident(mapInfo.Id, mapInfo.Collection, value);
        }
    }

    /// <summary>
    /// Time of the bronze medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>(sinceVersion: 1)]
    public TimeInt32? BronzeTime
    {
        get => ChallengeParameters is null ? bronzeTime : ChallengeParameters.BronzeTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.BronzeTime = value;
            }

            bronzeTime = value;
        }
    }

    /// <summary>
    /// Time of the silver medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>(sinceVersion: 1)]
    public TimeInt32? SilverTime
    {
        get => ChallengeParameters is null ? silverTime : ChallengeParameters.SilverTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.SilverTime = value;
            }

            silverTime = value;
        }
    }

    /// <summary>
    /// Time of the gold medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>(sinceVersion: 1)]
    public TimeInt32? GoldTime
    {
        get => ChallengeParameters is null ? goldTime : ChallengeParameters.GoldTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.GoldTime = value;
            }

            goldTime = value;
        }
    }

    /// <summary>
    /// Time of the author medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>(sinceVersion: 1)]
    public TimeInt32? AuthorTime
    {
        get => ChallengeParameters is null ? authorTime : ChallengeParameters.AuthorTime;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.AuthorTime = value;
            }

            authorTime = value;
        }
    }

    /// <summary>
    /// Usually author time or stunts score. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>(sinceVersion: 10)]
    public int AuthorScore
    {
        get => ChallengeParameters is null ? authorScore : ChallengeParameters.AuthorScore;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.AuthorScore = value;
            }

            authorScore = value;
        }
    }

    /// <summary>
    /// The map's UID.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public string MapUid
    {
        get => mapInfo.Id;
        set
        {
            mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);

            if (Gbx.CRC32 is not null)
            {
                ComputeCrc32();
            }
        }
    }

    /// <summary>
    /// If the map was made using the simple editor.
    /// </summary>
    public bool CreatedWithSimpleEditor
    {
        get => (editor & EditorMode.Simple) != 0;
        set => editor = value ? editor | EditorMode.Simple : editor & ~EditorMode.Simple;
    }

    /// <summary>
    /// If the map uses ghost blocks.
    /// </summary>
    public bool HasGhostBlocks
    {
        get => (editor & EditorMode.HasGhostBlocks) != 0;
        set => editor = value ? editor | EditorMode.HasGhostBlocks : editor & ~EditorMode.HasGhostBlocks;
    }

    /// <summary>
    /// If the map was made using the gamepad editor.
    /// </summary>
    public bool CreatedWithGamepadEditor
    {
        get => (editor & EditorMode.Gamepad) != 0;
        set => editor = value ? editor | EditorMode.Gamepad : editor & ~EditorMode.Gamepad;
    }

    private byte[]? thumbnail;
    [JpegData]
    [AppliedWithChunk<HeaderChunk03043007>]
    public byte[]? Thumbnail { get => thumbnail; set => thumbnail = value; }

    [AppliedWithChunk<HeaderChunk03043002>]
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Id? Collection => mapInfo?.Collection;

    private IList<CGameCtnBlock>? blocks;
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public IList<CGameCtnBlock>? Blocks { get => blocks; set => blocks = value; }

    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public int? NbBlocks => Blocks?.Count(x => x.Name != "Unassigned1");

    private UInt128? hashedPassword;
    [AppliedWithChunk<Chunk03043029>]
    public UInt128? HashedPassword
    {
        get => hashedPassword;
        set
        {
            hashedPassword = value;

            if (Gbx.CRC32 is not null)
            {
                ComputeCrc32();
            }
        }
    }

    [AppliedWithChunk<Chunk0304303D>]
    public bool HasLightmaps { get; set; }

    [AppliedWithChunk<Chunk0304303D>]
    public int? LightmapVersion { get; set; }

    [AppliedWithChunk<Chunk0304303D>]
    public CHmsLightMapCache? LightmapCache { get; set; }

    [AppliedWithChunk<Chunk0304303D>]
    public LightmapFrame[] LightmapFrames { get; set; }

    [ZLibData]
    [AppliedWithChunk<Chunk0304303D>]
    public CompressedData? LightmapCacheData { get; set; }

    private IList<CGameCtnAnchoredObject>? anchoredObjects;
    [AppliedWithChunk<Chunk03043040>]
    public IList<CGameCtnAnchoredObject>? AnchoredObjects { get => anchoredObjects; set => anchoredObjects = value; }

    private IList<CGameCtnZoneGenealogy>? zoneGenealogy;
    [AppliedWithChunk<Chunk03043043>]
    public IList<CGameCtnZoneGenealogy>? ZoneGenealogy { get => zoneGenealogy; set => zoneGenealogy = value; }

    private CScriptTraitsMetadata? scriptMetadata;
    [AppliedWithChunk<Chunk03043044>]
    public CScriptTraitsMetadata? ScriptMetadata { get => scriptMetadata; set => scriptMetadata = value; }

    [AppliedWithChunk<Chunk03043048>]
    public int? NbBakedBlocks => bakedBlocks?.Count(x => x.Name != "Unassigned1");

    private IList<CGameCtnBlock>? bakedBlocks;
    [AppliedWithChunk<Chunk03043048>]
    public IList<CGameCtnBlock>? BakedBlocks { get => bakedBlocks; set => bakedBlocks = value; }

    [AppliedWithChunk<Chunk03043048>]
    public IList<SBakedClipsAdditionalData>? BakedClipsAdditionalData { get; set; }

    [ZipData]
    [AppliedWithChunk<Chunk03043054>]
    public byte[]? EmbeddedZipData { get; set; }

    [AppliedWithChunk<Chunk03043054>]
    private IList<string>? Textures { get; set; }

    [AppliedWithChunk<Chunk03043069>]
    public IList<MacroblockInstance>? MacroblockInstances { get; set; }

    // poss to generate
    string IGameCtnChallenge.MapUid
    {
        get => MapUid ?? throw new MemberNullException(nameof(MapUid));
        set => MapUid = value;
    }

    // poss to generate
    IList<CGameCtnBlock> IGameCtnChallenge.Blocks
    {
        get => Blocks ?? throw new MemberNullException(nameof(Blocks));
        set => Blocks = value;
    }

    public string GetEnvironment()
    {
        return Collection ?? throw new Exception("Environment not available");
    }

    public IEnumerable<CGameCtnBlock> GetBlocks(bool includeUnassigned1 = true)
    {
        if (includeUnassigned1)
        {
            return blocks ?? [];
        }

        return blocks?.Where(x => x.Name != "Unassigned1") ?? [];
    }

    public IEnumerable<CGameCtnAnchoredObject> GetAnchoredObjects()
    {
        return anchoredObjects ?? [];
    }

    public IEnumerable<CGameCtnBlock> GetBakedBlocks()
    {
        return bakedBlocks ?? [];
    }

    public ZipArchive OpenReadEmbeddedZipData()
    {
        if (EmbeddedZipData is null || EmbeddedZipData.Length == 0)
        {
            throw new Exception("Embedded data zip is not available and cannot be read.");
        }

        var ms = new MemoryStream(EmbeddedZipData);
        return new ZipArchive(ms);
    }

    public void UpdateEmbeddedZipData(Action<ZipArchive> update)
    {
        EmbeddedZipData ??= [];

        using var ms = new MemoryStream(EmbeddedZipData.Length);

        if (EmbeddedZipData.Length > 0)
        {
            ms.Write(EmbeddedZipData, 0, EmbeddedZipData.Length);
        }

        using (var zip = new ZipArchive(ms, ZipArchiveMode.Update))
        {
            update(zip);
        }

        EmbeddedZipData = ms.ToArray();
    }

    public async Task UpdateEmbeddedZipDataAsync(Func<ZipArchive, Task> update, CancellationToken cancellationToken = default)
    {
        EmbeddedZipData ??= [];

        using var ms = new MemoryStream(EmbeddedZipData.Length);

        if (EmbeddedZipData.Length > 0)
        {
#if NET6_0_OR_GREATER
            await ms.WriteAsync(EmbeddedZipData, cancellationToken);
#else
            await ms.WriteAsync(EmbeddedZipData, 0, EmbeddedZipData.Length, cancellationToken);
#endif
        }

        using (var zip = new ZipArchive(ms, ZipArchiveMode.Update))
        {
            await update(zip);
        }

        EmbeddedZipData = ms.ToArray();
    }

    public async Task UpdateEmbeddedZipDataAsync(Func<ZipArchive, CancellationToken, Task> update, CancellationToken cancellationToken = default)
    {
        EmbeddedZipData ??= [];

        using var ms = new MemoryStream(EmbeddedZipData.Length);

        if (EmbeddedZipData.Length > 0)
        {
#if NET6_0_OR_GREATER
            await ms.WriteAsync(EmbeddedZipData, cancellationToken);
#else
            await ms.WriteAsync(EmbeddedZipData, 0, EmbeddedZipData.Length, cancellationToken);
#endif
        }

        using (var zip = new ZipArchive(ms, ZipArchiveMode.Update))
        {
            await update(zip, cancellationToken);
        }

        EmbeddedZipData = ms.ToArray();
    }

    /// <summary>
    /// Calculates the CRC32 of the map.
    /// </summary>
    public void ComputeCrc32()
    {
        string toHash;

        if (hashedPassword is null)
        {
            toHash = $"0x00000000000000000000000000000000???{MapUid}";
        }
        else
        {
            Span<byte> bytes = stackalloc byte[16];
            hashedPassword.Value.WriteLittleEndian(bytes);
            Span<char> hex = stackalloc char[32];
            TryHex(bytes, hex);
            toHash = $"0x{hex.ToString()}???{MapUid}";
        }

        Crc32 = Gbx.CRC32?.Hash(Encoding.ASCII.GetBytes(toHash)) ?? throw new Exception("CRC32 is not imported (ICrc32).");

        static void TryHex(ReadOnlySpan<byte> value, Span<char> chars)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var hex1 = HexIntToChar((byte)(value[value.Length - 1 - i] % 16));
                var hex2 = HexIntToChar((byte)(value[value.Length - 1 - i] / 16));

                chars[i * 2 + 1] = hex1;
                chars[i * 2] = hex2;
            }

            return;

            static char HexIntToChar(byte v)
            {
                if (v < 10)
                {
                    return (char)(v + 48);
                }

                return (char)(v + 55);
            }
        }
    }
    
    /// <summary>
    /// Removes the map password.
    /// </summary>
    public void RemovePassword()
    {
        Password = null;

        if (HashedPassword is not null)
        {
            HashedPassword = new UInt128();
        }

        Chunks.Remove<Chunk03043029>();
    }

    /// <summary>
    /// Gets the first block at this position.
    /// </summary>
    /// <param name="pos">Position of the block.</param>
    /// <returns>The first available block.</returns>
    public CGameCtnBlock? GetBlock(Int3 pos) => blocks?.FirstOrDefault(x => x.Coord == pos);

    /// <summary>
    /// Retrieves blocks at this position.
    /// </summary>
    /// <param name="pos">Position of the block.</param>
    /// <returns>An enumerable of blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetBlocks(Int3 pos) => GetBlocks(includeUnassigned1: false).Where(x => x.Coord == pos);

    /// <summary>
    /// Retrieves ghost blocks on the map.
    /// </summary>
    /// <returns>An enumerable of ghost blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetGhostBlocks() => GetBlocks(includeUnassigned1: false).Where(x => x.IsGhost);

    public CGameCtnBlock PlaceBlock(
        Ident blockModel,
        Int3 coord,
        Direction direction,
        bool isGround = false,
        byte variant = 0,
        byte subVariant = 0)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        var block = new CGameCtnBlock
        {
            BlockModel = blockModel,
            Coord = coord,
            Direction = direction,
            IsGround = isGround,
            Variant = variant,
            SubVariant = subVariant
        };

        block.CreateChunk<CGameCtnBlock.Chunk03057002>();

        Blocks.Add(block);

        return block;
    }

    public CGameCtnBlock PlaceBlock(
        string blockModel,
        Int3 coord,
        Direction direction,
        bool isGround = false,
        byte variant = 0,
        byte subVariant = 0)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        var block = new CGameCtnBlock
        {
            Name = blockModel,
            Coord = coord,
            Direction = direction,
            IsGround = isGround,
            Variant = variant,
            SubVariant = subVariant
        };

        Blocks.Add(block);

        return block;
    }

    public void PlaceBlock(CGameCtnBlock block)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        Blocks.Add(block);
    }

    public void RemoveAllBlocks()
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        Blocks.Clear();
    }

    public int RemoveBlocks(Predicate<CGameCtnBlock> match)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        return Blocks.RemoveAll(match);
    }

    public bool RemoveBlock(Predicate<CGameCtnBlock> match)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        foreach (var block in Blocks)
        {
            if (match(block))
            {
                Blocks.Remove(block);
                return true;
            }
        }

        return false;
    }

    public bool RemoveBlock(CGameCtnBlock block)
    {
        _ = Blocks ?? throw new MemberNullException(nameof(Blocks));

        return Blocks.Remove(block);
    }

    public CGameCtnAnchoredObject PlaceAnchoredObject(Ident itemModel, Vec3 absolutePosition, Vec3 pitchYawRoll, Vec3 offsetPivot = default)
    {
        _ = AnchoredObjects ?? throw new MemberNullException(nameof(AnchoredObjects));

        CreateChunk<Chunk03043040>();

        var anchoredObject = new CGameCtnAnchoredObject
        {
            ItemModel = itemModel,
            AbsolutePositionInMap = absolutePosition,
            PitchYawRoll = pitchYawRoll,
            PivotPosition = offsetPivot
        };

        anchoredObject.CreateChunk<CGameCtnAnchoredObject.Chunk03101002>().Version = 7;

        AnchoredObjects.Add(anchoredObject);

        return anchoredObject;
    }

    public bool RemoveAnchoredObject(CGameCtnAnchoredObject anchoredObject)
    {
        _ = AnchoredObjects ?? throw new MemberNullException(nameof(AnchoredObjects));

        return AnchoredObjects.Remove(anchoredObject);
    }

    public int RemoveAnchoredObjects(Predicate<CGameCtnAnchoredObject> match)
    {
        _ = AnchoredObjects ?? throw new MemberNullException(nameof(AnchoredObjects));

        return AnchoredObjects.RemoveAll(match);
    }

    public void RemoveAllAnchoredObjects()
    {
        _ = AnchoredObjects ?? throw new MemberNullException(nameof(AnchoredObjects));

        AnchoredObjects.Clear();
    }

    public void RemoveAllOffZone()
    {
        _ = Offzones ?? throw new MemberNullException(nameof(Offzones));

        Offzones.Clear();
    }

    public void RemoveAll()
    {
        RemoveAllBlocks();
        RemoveAllAnchoredObjects();
        RemoveAllOffZone();
    }

    IEnumerable<IGameCtnBlockTM10> IGameCtnChallengeTM10.GetBlocks() => GetBlocks(includeUnassigned1: true);
    IEnumerable<IGameCtnBlockTMSX> IGameCtnChallengeTMSX.GetBlocks() => GetBlocks(includeUnassigned1: true);
    IEnumerable<IGameCtnBlockTMF> IGameCtnChallengeTMF.GetBlocks() => GetBlocks(includeUnassigned1: true);
    IEnumerable<IGameCtnBlockMP4> IGameCtnChallengeMP4.GetBlocks(bool includeUnassigned1) => GetBlocks(includeUnassigned1);
    IEnumerable<IGameCtnBlockTM2020> IGameCtnChallengeTM2020.GetBlocks() => GetBlocks(includeUnassigned1: true);
    IEnumerable<IGameCtnBlockMP4> IGameCtnChallengeMP4.GetBakedBlocks() => GetBakedBlocks();
    IEnumerable<IGameCtnBlockTM2020> IGameCtnChallengeTM2020.GetBakedBlocks() => GetBakedBlocks();
    IGameCtnBlockTM10? IGameCtnChallengeTM10.GetBlock(Int3 pos) => GetBlock(pos);
    IGameCtnBlockTMSX? IGameCtnChallengeTMSX.GetBlock(Int3 pos) => GetBlock(pos);
    IGameCtnBlockTMF? IGameCtnChallengeTMF.GetBlock(Int3 pos) => GetBlock(pos);
    IGameCtnBlockMP4? IGameCtnChallengeMP4.GetBlock(Int3 pos) => GetBlock(pos);
    IGameCtnBlockTM2020? IGameCtnChallengeTM2020.GetBlock(Int3 pos) => GetBlock(pos);
    IEnumerable<IGameCtnBlockTM10> IGameCtnChallengeTM10.GetBlocks(Int3 pos) => GetBlocks(pos);
    IEnumerable<IGameCtnBlockTMSX> IGameCtnChallengeTMSX.GetBlocks(Int3 pos) => GetBlocks(pos);
    IEnumerable<IGameCtnBlockTMF> IGameCtnChallengeTMF.GetBlocks(Int3 pos) => GetBlocks(pos);
    IEnumerable<IGameCtnBlockMP4> IGameCtnChallengeMP4.GetBlocks(Int3 pos) => GetBlocks(pos);
    IEnumerable<IGameCtnBlockTM2020> IGameCtnChallengeTM2020.GetBlocks(Int3 pos) => GetBlocks(pos);
    IEnumerable<IGameCtnBlockMP4> IGameCtnChallengeMP4.GetGhostBlocks() => GetGhostBlocks();
    IEnumerable<IGameCtnBlockTM2020> IGameCtnChallengeTM2020.GetGhostBlocks() => GetGhostBlocks();
    IGameCtnBlockTM10 IGameCtnChallengeTM10.PlaceBlock(Ident blockModel, Int3 coord, Direction direction, bool isGround, byte variant, byte subVariant) => PlaceBlock(blockModel, coord, direction, isGround, variant, subVariant);
    IGameCtnBlockTMSX IGameCtnChallengeTMSX.PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround, byte variant, byte subVariant) => PlaceBlock(blockModel, coord, direction, isGround, variant, subVariant);
    IGameCtnBlockTMF IGameCtnChallengeTMF.PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround, byte variant, byte subVariant) => PlaceBlock(blockModel, coord, direction, isGround, variant, subVariant);
    IGameCtnBlockMP4 IGameCtnChallengeMP4.PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround, byte variant, byte subVariant) => PlaceBlock(blockModel, coord, direction, isGround, variant, subVariant);
    IGameCtnBlockTM2020 IGameCtnChallengeTM2020.PlaceBlock(string blockModel, Int3 coord, Direction direction, bool isGround, byte variant, byte subVariant) => PlaceBlock(blockModel, coord, direction, isGround, variant, subVariant);
    int IGameCtnChallengeTM10.RemoveBlocks(Predicate<IGameCtnBlockTM10> match) => RemoveBlocks(match);
    int IGameCtnChallengeTMSX.RemoveBlocks(Predicate<IGameCtnBlockTMSX> match) => RemoveBlocks(match);
    int IGameCtnChallengeTMF.RemoveBlocks(Predicate<IGameCtnBlockTMF> match) => RemoveBlocks(match);
    int IGameCtnChallengeMP4.RemoveBlocks(Predicate<IGameCtnBlockMP4> match) => RemoveBlocks(match);
    int IGameCtnChallengeTM2020.RemoveBlocks(Predicate<IGameCtnBlockTM2020> match) => RemoveBlocks(match);
    int IGameCtnChallengeTM10.RemoveBlock(Predicate<IGameCtnBlockTM10> match) => RemoveBlocks(match);
    int IGameCtnChallengeTMSX.RemoveBlock(Predicate<IGameCtnBlockTMSX> match) => RemoveBlocks(match);
    int IGameCtnChallengeTMF.RemoveBlock(Predicate<IGameCtnBlockTMF> match) => RemoveBlocks(match);
    int IGameCtnChallengeMP4.RemoveBlock(Predicate<IGameCtnBlockMP4> match) => RemoveBlocks(match);
    int IGameCtnChallengeTM2020.RemoveBlock(Predicate<IGameCtnBlockTM2020> match) => RemoveBlocks(match);

    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class HeaderChunk03043007 : IVersionable
    {
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version == 0)
            {
                return;
            }

            var thumbnailSize = rw.Int32(n.thumbnail?.Length ?? 0);
            rw.Marker("<Thumbnail.jpg>");
            rw.Data(ref n.thumbnail, thumbnailSize);
            rw.Marker("</Thumbnail.jpg>");
            rw.Marker("<Comments>");
            rw.String(ref n.comments);
            rw.Marker("</Comments>");
        }
    }

    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Chunk03043013;

    public partial class Chunk0304301F : IVersionable
    {
        public int Version { get; set; } = 6;

        public bool U01;
        public ulong? U02;

        public bool IsUnlimiter2 { get; set; }

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.mapInfo = r.ReadIdent();
            n.mapName = r.ReadString();
            n.decoration = r.ReadIdent();
            n.size = r.ReadInt3();
            U01 = r.ReadBoolean();
            Version = r.ReadInt32();

            var nbBlocks = r.ReadInt32();
            n.blocks = new List<CGameCtnBlock>(nbBlocks);

            var isUnlimiter = default(bool?);

            for (var i = 0; i < nbBlocks; i++)
            {
                var block = r.ReadReadable<CGameCtnBlock>(Version);
                n.blocks.Add(block);

                if (block.Flags != -1)
                {
                    continue;
                }

                isUnlimiter ??= block.Name.StartsWith("TMUnlimiter 2");

                if (isUnlimiter.Value)
                {
                    IsUnlimiter2 = isUnlimiter.Value;
                }
                else
                {
                    i--;
                }
            }

            if (IsUnlimiter2)
            {
                U02 = r.ReadUInt64();
                return;
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                n.blocks.Add(r.ReadReadable<CGameCtnBlock>(Version));
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.mapInfo);
            w.Write(n.mapName);
            w.Write(n.decoration);
            w.Write(n.size);
            w.Write(U01);
            w.Write(Version);

            w.Write(n.NbBlocks.GetValueOrDefault());

            if (n.blocks is null)
            {
                return;
            }

            foreach (var block in n.blocks)
            {
                w.WriteWritable(block, Version);
            }

            if (IsUnlimiter2)
            {
                w.Write(U02.GetValueOrDefault());
            }
        }
    }

    public partial class Chunk0304303D
    {
        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.HasLightmaps = r.ReadBoolean(); // true is SHmsLightMapCacheSmall is not empty

            if (!n.HasLightmaps)
            {
                return;
            }

            n.LightmapVersion = r.ReadInt32();

            if (n.LightmapVersion < 2)
            {
                n.LightmapCache = r.ReadNodeRef<CHmsLightMapCache>();
                throw new NotSupportedException("Lightmap version <2 is not supported.");
            }

            var frameCount = n.LightmapVersion >= 5 ? r.ReadInt32() : 1;

            n.LightmapFrames = r.ReadArrayReadable<LightmapFrame>(frameCount, n.LightmapVersion.GetValueOrDefault(8));

            if (!n.LightmapFrames.Any(x => x.Data?.Length > 0 || x.Data2?.Length > 0 || x.Data3?.Length > 0))
            {
                return;
            }

            n.LightmapCacheData = new CompressedData(r.ReadInt32(), r.ReadData());

            using var ms = n.LightmapCacheData.OpenDecompressedMemoryStream();
            using var rBuffer = new GbxReader(ms);
            rBuffer.LoadFrom(r);

            n.LightmapCache = rBuffer.ReadNode<CHmsLightMapCache>();
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.HasLightmaps);

            if (!n.HasLightmaps)
            {
                return;
            }

            w.Write(n.LightmapVersion.GetValueOrDefault());

            if (n.LightmapVersion < 2)
            {
                w.WriteNodeRef(n.LightmapCache);
                return;
            }

            w.Write(n.LightmapFrames.Length);

            w.WriteArrayWritable(n.LightmapFrames, version: n.LightmapVersion.GetValueOrDefault(8));

            if (n.LightmapCacheData is null)
            {
                throw new Exception("Lightmap cache data is not available.");
            }

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            wBuffer.LoadFrom(w);

            wBuffer.WriteNode(n.LightmapCache);

            if (Gbx.ZLib is null)
            {
                throw new Exception("ZLib is not imported (IZLib).");
            }

            ms.Position = 0;
            using var compressedMs = new MemoryStream();

            Gbx.ZLib.Compress(ms, compressedMs);

            w.Write(compressedMs.Length);
            compressedMs.CopyTo(w.BaseStream);
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class LightmapFrame;

    public partial class Chunk03043040
    {
        public int U01;
        public int[]? U02;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 4;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.anchoredObjects = r.ReadListNodeRef_deprec<CGameCtnAnchoredObject>()!;

            if (Version >= 1 && Version != 5)
            {
                // defines which (second element) items are deleted together with other (first element) item?
                var itemsOnItem = r.ReadArray<Int2>();

                foreach (var item in itemsOnItem)
                {
                    n.anchoredObjects[item.Y].PlacedOnItem = n.anchoredObjects[item.X];
                }
            }

            if (Version >= 5)
            {
                var blockIndexes = r.ReadArray<int>(); // block indexes, -1 means itemIndexes will have the value instead
                var usedBlocks = new CGameCtnBlock?[blockIndexes.Length];
                var blocksWithoutUnassigned = n.blocks!.Where(x => x.Flags != -1).ToArray();

                for (var i = 0; i < blockIndexes.Length; i++)
                {
                    var index = blockIndexes[i];

                    if (index > -1)
                    {
                        usedBlocks[i] = blocksWithoutUnassigned[index];
                    }
                }

                var snapItemGroups = Version < 7 ? r.ReadArray<int>() : null; // snap item group - only some snapped items will delete on a block. they are consistent numbers

                var usedItems = default(CGameCtnAnchoredObject[]);

                if (Version >= 6)
                {
                    var itemIndexes = r.ReadArray<int>(); // item indexes
                    usedItems = new CGameCtnAnchoredObject[itemIndexes.Length];

                    for (var i = 0; i < itemIndexes.Length; i++)
                    {
                        var index = itemIndexes[i];

                        if (index > -1)
                        {
                            usedItems[i] = n.anchoredObjects![index];
                        }
                    }
                }

                snapItemGroups ??= Version >= 7 ? r.ReadArray<int>() : null;

                if (Version != 6)
                {
                    var U07 = r.ReadArray<int>();

                    if (U07.Any(x => x != -1))
                    {
                        throw new NotSupportedException("U07 has something else than -1");
                    }
                }

                // always the same count as anchoredObjects
                var snappedIndexes = r.ReadArray<int>(); // "snapped onto block/item" indexes

                for (var i = 0; i < snappedIndexes.Length; i++)
                {
                    var snappedIndex = snappedIndexes[i];

                    if (snappedIndex <= -1)
                    {
                        continue;
                    }

                    var usedBlock = usedBlocks[snappedIndex];

                    if (usedBlock is not null)
                    {
                        n.anchoredObjects[i].SnappedOnBlock = usedBlock;
                    }

                    var usedItem = usedItems?[snappedIndex];

                    if (usedItem is not null)
                    {
                        n.anchoredObjects[i].SnappedOnItem = usedItem;
                    }

                    n.anchoredObjects[i].SnappedOnGroup = snapItemGroups?[snappedIndex] ?? 0;
                }

                if (Version >= 8)
                {
                    throw new ChunkVersionNotSupportedException(Version);
                }
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);
            w.Write(U01);

            using var itemMs = new MemoryStream();
            using var itemW = new GbxWriter(itemMs);
            using var _ = new Encapsulation(itemW);

            itemW.WriteListNodeRef_deprec((n.anchoredObjects ?? [])!);

            var itemDict = new Dictionary<CGameCtnAnchoredObject, int>();

            if (n.anchoredObjects is not null)
            {
                for (var i = 0; i < n.anchoredObjects.Count; i++)
                {
                    itemDict[n.anchoredObjects[i]] = i;
                }
            }

            if (Version >= 1 && Version != 5)
            {
                var pairs = new List<Int2>();

                if (n.anchoredObjects is not null)
                {
                    for (var i = 0; i < n.anchoredObjects.Count; i++)
                    {
                        var placedOnItem = n.anchoredObjects[i].PlacedOnItem;

                        if (placedOnItem is not null && itemDict.TryGetValue(placedOnItem, out int index))
                        {
                            pairs.Add((index, i));
                        }
                    }
                }

                itemW.WriteList(pairs);
            }

            if (Version >= 5)
            {
                var blockDict = new Dictionary<CGameCtnBlock, int>();

                if (n.blocks is not null)
                {
                    for (var i = 0; i < n.blocks.Count; i++)
                    {
                        blockDict[n.blocks[i]] = i;
                    }
                }

                var usedBlockIndexHashSet = new HashSet<(int blockIndex, int group)>();
                var usedBlockIndexList = new List<(int blockIndex, int group)>();
                var usedItemIndexHashSet = new HashSet<(int itemIndex, int group)>();
                var usedItemIndexList = new List<(int itemIndex, int group)>();

                var indicesOnUsedBlocksAndItems = new Dictionary<(int index, int group), int>();
                var snappedOnIndices = new List<int>(n.anchoredObjects?.Count ?? 0);

                foreach (var item in n.GetAnchoredObjects())
                {
                    var isItemNotSnappedOnBlock = item.SnappedOnBlock is null || !blockDict.ContainsKey(item.SnappedOnBlock);
                    var isItemNotSnappedOnItem = item.SnappedOnItem is null || !itemDict.ContainsKey(item.SnappedOnItem);

                    if (isItemNotSnappedOnBlock && isItemNotSnappedOnItem)
                    {
                        snappedOnIndices.Add(-1);
                        continue;
                    }

                    var groupIndex = item.SnappedOnGroup ?? 0;
                    var unique = (-1, groupIndex);

                    if (item.SnappedOnBlock is not null)
                    {
                        var blockIndex = blockDict[item.SnappedOnBlock];

                        unique = (blockIndex, groupIndex);

                        if (!usedBlockIndexHashSet.Contains(unique))
                        {
                            usedBlockIndexList.Add(unique);
                            usedBlockIndexHashSet.Add(unique);

                            if (isItemNotSnappedOnItem)
                            {
                                usedItemIndexList.Add((-1, groupIndex));
                            }
                        }
                    }

                    if (item.SnappedOnItem is not null && itemDict.TryGetValue(item.SnappedOnItem, out var itemIndex))
                    {
                        unique = (itemIndex, groupIndex);

                        if (!usedItemIndexHashSet.Contains(unique))
                        {
                            usedItemIndexList.Add(unique);
                            usedItemIndexHashSet.Add(unique);

                            if (item.SnappedOnBlock is null)
                            {
                                usedBlockIndexList.Add((-1, groupIndex));
                            }
                        }
                    }

                    if (indicesOnUsedBlocksAndItems.TryGetValue(unique, out int indexOfBlockOrItemIndex))
                    {
                        snappedOnIndices.Add(indexOfBlockOrItemIndex);
                    }
                    else
                    {
                        indicesOnUsedBlocksAndItems[unique] = indicesOnUsedBlocksAndItems.Count;
                        snappedOnIndices.Add(indicesOnUsedBlocksAndItems.Count - 1);
                    }
                }

                itemW.WriteArray(usedBlockIndexList.Select(x => x.blockIndex).ToArray());

                if (Version < 7)
                {
                    itemW.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                }

                if (Version >= 6)
                {
                    itemW.WriteArray(usedItemIndexList.Select(x => x.itemIndex).ToArray());
                }

                if (Version >= 7)
                {
                    itemW.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                }

                if (Version != 6)
                {
                    itemW.WriteArray(Enumerable.Repeat(-1, usedBlockIndexList.Count).ToArray());
                }

                itemW.WriteArray(snappedOnIndices.ToArray());
            }

            w.Write((int)itemMs.Length);
            w.Write(itemMs.ToArray());
        }
    }

    public partial class Chunk03043043
    {
        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.zoneGenealogy = r.ReadListNodeRef<CGameCtnZoneGenealogy>()!;
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(U01);
            
            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            wBuffer.WriteListNodeRef((n.zoneGenealogy ?? [])!);

            w.Write((int)ms.Length);
            ms.WriteTo(w.BaseStream);
        }
    }

    public partial class Chunk03043044
    {
        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.scriptMetadata = r.ReadNode<CScriptTraitsMetadata>()!;
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            wBuffer.WriteNode(n.scriptMetadata!);

            w.Write((int)ms.Length);
            ms.WriteTo(w.BaseStream);
        }
    }

    public partial class Chunk03043048 : IVersionable
    {
        public int Version { get; set; }
        public int BlocksVersion { get; set; } = 6;
        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            BlocksVersion = r.ReadInt32();

            var nbBakedBlocks = r.ReadInt32();
            n.bakedBlocks = new List<CGameCtnBlock>(nbBakedBlocks);

            for (var i = 0; i < nbBakedBlocks; i++)
            {
                var block = r.ReadReadable<CGameCtnBlock>(BlocksVersion);
                n.bakedBlocks.Add(block);

                if (block.Flags == -1)
                {
                    i--;
                }
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                n.bakedBlocks.Add(r.ReadReadable<CGameCtnBlock>(BlocksVersion));
            }

            U01 = r.ReadInt32();

            n.BakedClipsAdditionalData = r.ReadListReadable<SBakedClipsAdditionalData>(version: Version);
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);
            w.Write(BlocksVersion);

            w.Write(n.NbBakedBlocks.GetValueOrDefault());

            if (n.bakedBlocks is not null)
            {
                foreach (var block in n.bakedBlocks)
                {
                    w.WriteWritable(block, BlocksVersion);
                }
            }

            w.Write(U01);

            w.WriteListWritable(n.BakedClipsAdditionalData, version: Version);
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SBakedClipsAdditionalData;

    public partial class Chunk03043054 : IVersionable
    {
        public int Version { get; set; }

        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            var embeddedItemModels = r.ReadArrayIdent(); // ignored, could be used for validation

            n.EmbeddedZipData = r.ReadData();

            if (Version >= 1)
            {
                n.Textures = r.ReadListString();
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(wBuffer);

            if (n.EmbeddedZipData is null || n.EmbeddedZipData.Length == 0)
            {
                wBuffer.Write(0);
                wBuffer.Write(0);
            }
            else
            {
                using var embeddedMs = new MemoryStream(n.EmbeddedZipData);
                using var zip = new ZipArchive(embeddedMs, ZipArchiveMode.Read);

                var itemModelList = new List<Ident>();

                foreach (var entry in zip.Entries)
                {
                    using var entryStream = entry.Open();

                    try
                    {
                        var nodeHeader = Gbx.ParseHeaderNode(entryStream);

                        if (nodeHeader is CGameItemModel { Ident: not null } itemModel)
                        {
                            itemModelList.Add(itemModel.Ident);
                        }
                    }
                    catch
                    {

                    }
                }

                wBuffer.WriteList(itemModelList);
                wBuffer.WriteData(n.EmbeddedZipData!);
            }

            if (Version >= 1)
            {
                wBuffer.WriteList(n.Textures!);
            }

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    public partial class Chunk0304305F : IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks().Concat(n.GetBakedBlocks()).Where(x => x.IsFree))
            {
                block.AbsolutePositionInMap = rw.Vec3(block.AbsolutePositionInMap);
                block.PitchYawRoll = rw.Vec3(block.PitchYawRoll);
            }
        }
    }

    public partial class Chunk03043062 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks())
            {
                block.Color = (DifficultyColor)rw.Byte((byte)block.Color);
            }

            foreach (var block in n.GetBakedBlocks())
            {
                block.Color = (DifficultyColor)rw.Byte((byte)block.Color);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.Color = (DifficultyColor)rw.Byte((byte)item.Color);
            }
        }
    }

    public partial class Chunk03043063 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.AnimPhaseOffset = (CGameCtnAnchoredObject.EPhaseOffset)rw.Byte((byte)item.AnimPhaseOffset);
            }
        }
    }

    public partial class Chunk03043065 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                var hasForegroundPackDesc = rw.Boolean(item.ForegroundPackDesc is not null, asByte: true);

                if (hasForegroundPackDesc)
                {
                    item.ForegroundPackDesc = rw.PackDesc(item.ForegroundPackDesc);
                }
            }
        }
    }

    public partial class Chunk03043068 : IVersionable
    {
        // It has not been 100% validated if this is lightmap quality per block/object or not, but a lot of things hint towards it

        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks())
            {
                block.LightmapQuality = (LightmapQuality)rw.Byte((byte)block.LightmapQuality);
            }

            foreach (var block in n.GetBakedBlocks())
            {
                block.LightmapQuality = (LightmapQuality)rw.Byte((byte)block.LightmapQuality);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.LightmapQuality = (LightmapQuality)rw.Byte((byte)item.LightmapQuality);
            }
        }
    }

    public partial class Chunk03043069
    {
        public int Version { get; set; }

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            var dict = new Dictionary<int, MacroblockInstance>();

            foreach (var block in n.GetBlocks())
            {
                var macroblockId = r.ReadInt32();

                if (macroblockId == -1)
                {
                    continue;
                }

                if (!dict.TryGetValue(macroblockId, out var instance))
                {
                    instance = new MacroblockInstance();
                    dict[macroblockId] = instance;
                }

                block.MacroblockReference = instance;
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                var macroblockId = r.ReadInt32();

                if (macroblockId == -1)
                {
                    continue;
                }

                if (!dict.TryGetValue(macroblockId, out var instance))
                {
                    instance = new MacroblockInstance();
                    dict[macroblockId] = instance;
                }

                item.MacroblockReference = instance;
            }

            var idFlagsPair = r.ReadArray<Int2>();

            foreach (var (id, flags) in idFlagsPair)
            {
                if (dict.TryGetValue(id, out var instance))
                {
                    instance.Flags = flags;
                }
            }

            n.MacroblockInstances = dict.Values.ToList();
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);

            var dict = new Dictionary<MacroblockInstance, int>();

            if (n.MacroblockInstances is not null)
            {
                for (var i = 0; i < n.MacroblockInstances.Count; i++)
                {
                    dict[n.MacroblockInstances[i]] = i;
                }
            }

            foreach (var block in n.GetBlocks())
            {
                if (block.MacroblockReference is not null && dict.TryGetValue(block.MacroblockReference, out int index))
                {
                    w.Write(index);
                }
                else
                {
                    w.Write(-1);
                }
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                if (item.MacroblockReference is not null && dict.TryGetValue(item.MacroblockReference, out int index))
                {
                    w.Write(index);
                }
                else
                {
                    w.Write(-1);
                }
            }

            if (n.MacroblockInstances is null)
            {
                w.Write(0);
                return;
            }

            w.Write(n.MacroblockInstances.Count);
            for (var i = 0; i < n.MacroblockInstances.Count; i++)
            {
                w.Write(i);
                w.Write(n.MacroblockInstances[i].Flags);
            }
        }
    }
}
