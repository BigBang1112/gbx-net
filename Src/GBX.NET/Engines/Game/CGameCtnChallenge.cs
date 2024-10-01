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
    private string authorLogin = string.Empty;
    private TimeInt32? bronzeTime; // Only used if ChallengeParameters is null
    private TimeInt32? silverTime; // Only used if ChallengeParameters is null
    private TimeInt32? goldTime; // Only used if ChallengeParameters is null
    private TimeInt32? authorTime; // Only used if ChallengeParameters is null
    private int authorScore; // Only used if ChallengeParameters is null
    private string? mapType; // Only used if ChallengeParameters is null
    private string? mapStyle; // Only used if ChallengeParameters is null

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
    /// Map type, the expected mode. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043003>(sinceVersion: 3)]
    public string? MapType
    {
        get => ChallengeParameters is null ? mapType : ChallengeParameters.MapType;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.MapType = value;
            }

            mapType = value;
        }
    }

    /// <summary>
    /// Map style. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043003>(sinceVersion: 3)]
    public string? MapStyle
    {
        get => ChallengeParameters is null ? mapStyle : ChallengeParameters.MapStyle;
        set
        {
            if (ChallengeParameters is not null)
            {
                ChallengeParameters.MapStyle = value;
            }

            mapStyle = value;
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
    public int? NbBlocks => Blocks?.Count;

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

    private bool hasLightmaps;
    [AppliedWithChunk<Chunk0304303D>]
    public bool HasLightmaps { get => hasLightmaps; set => hasLightmaps = value; }

    [AppliedWithChunk<HeaderChunk03043003>(sinceVersion: 9)]
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public int? LightmapVersion { get; set; }

    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public CHmsLightMapCache? LightmapCache { get; set; }

    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public LightmapFrame[]? LightmapFrames { get; set; }

    [ZLibData]
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
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
    public int? NbBakedBlocks => bakedBlocks?.Count;

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

    private bool hasCustomCamThumbnail;
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    public bool HasCustomCamThumbnail { get => hasCustomCamThumbnail; set => hasCustomCamThumbnail = value; }

    private Vec3 thumbnailPosition;
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public Vec3 ThumbnailPosition { get => thumbnailPosition; set => thumbnailPosition = value; }

    private float thumbnailFov;
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public float ThumbnailFov { get => thumbnailFov; set => thumbnailFov = value; }

    private float thumbnailNearClipPlane;
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public float ThumbnailNearClipPlane { get => thumbnailNearClipPlane; set => thumbnailNearClipPlane = value; }

    private float thumbnailFarClipPlane;
    [AppliedWithChunk<Chunk03043027>]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public float ThumbnailFarClipPlane { get => thumbnailFarClipPlane; set => thumbnailFarClipPlane = value; }

    private string? comments;
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public string? Comments { get => comments; set => comments = value; }

    private Vec3 thumbnailPitchYawRoll;
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public Vec3 ThumbnailPitchYawRoll { get => thumbnailPitchYawRoll; set => thumbnailPitchYawRoll = value; }


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

    public IEnumerable<CGameCtnBlock> GetBlocks()
    {
        return blocks ?? [];
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
    public IEnumerable<CGameCtnBlock> GetBlocks(Int3 pos) => GetBlocks().Where(x => x.Coord == pos);

    /// <summary>
    /// Retrieves ghost blocks on the map.
    /// </summary>
    /// <returns>An enumerable of ghost blocks.</returns>
    public IEnumerable<CGameCtnBlock> GetGhostBlocks() => GetBlocks().Where(x => x.IsGhost);

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

    /// <summary>
    /// Generates an approximate map UID using <see cref="MapUtils.GenerateMapUid()"/> that is applied to <see cref="MapUid"/> and returned.
    /// </summary>
    /// <returns>A random map UID.</returns>
    public string GenerateMapUid() => MapUid = MapUtils.GenerateMapUid();

    /// <summary>
    /// Generates an approximate map UID using <see cref="MapUtils.GenerateMapUid(int)"/> that is applied to <see cref="MapUid"/> and returned.
    /// </summary>
    /// <returns>A consistent map UID, based on the seed.</returns>
    public string GenerateMapUid(int seed) => MapUid = MapUtils.GenerateMapUid(seed);

    /// <summary>
    /// Generates an approximate map UID using <see cref="MapUtils.GenerateMapUid(Random)"/> that is applied to <see cref="MapUid"/> and returned.
    /// </summary>
    /// <returns>A random map UID.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="random"/> is null.</exception>
    public string GenerateMapUid(Random random) => MapUid = MapUtils.GenerateMapUid(random);

    IEnumerable<IGameCtnBlockTM10> IGameCtnChallengeTM10.GetBlocks() => GetBlocks();
    IEnumerable<IGameCtnBlockTMSX> IGameCtnChallengeTMSX.GetBlocks() => GetBlocks();
    IEnumerable<IGameCtnBlockTMF> IGameCtnChallengeTMF.GetBlocks() => GetBlocks();
    IEnumerable<IGameCtnBlockMP4> IGameCtnChallengeMP4.GetBlocks() => GetBlocks();
    IEnumerable<IGameCtnBlockTM2020> IGameCtnChallengeTM2020.GetBlocks() => GetBlocks();
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

        public bool NeedUnlock;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.mapInfo = r.ReadIdent();
            n.mapName = r.ReadString();
            n.decoration = r.ReadIdent();
            n.size = r.ReadInt3();
            NeedUnlock = r.ReadBoolean();
            Version = r.ReadInt32();

            var nbBlocks = r.ReadInt32();
            n.blocks = new List<CGameCtnBlock>(nbBlocks);

            for (var i = 0; i < nbBlocks; i++)
            {
                var block = r.ReadReadable<CGameCtnBlock>(Version);

                if (Version >= 6)
                {
                    block.Coord -= new Int3(1, 0, 1);
                }

                n.blocks.Add(block);
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.mapInfo);
            w.Write(n.mapName);
            w.Write(n.decoration);
            w.Write(n.size);
            w.Write(NeedUnlock);
            w.Write(Version);

            if (Version < 6)
            {
                w.WriteListWritable(n.blocks, version: Version);
                return;
            }

            w.Write(n.NbBlocks.GetValueOrDefault());

            if (n.blocks is null)
            {
                return;
            }

            foreach (var block in n.blocks)
            {
                try
                {
                    block.Coord += new Int3(1, 0, 1);
                    w.WriteWritable(block, Version);
                }
                finally
                {
                    block.Coord -= new Int3(1, 0, 1);
                }
            }
        }
    }

    public partial class Chunk0304303D
    {
        public int U01;
        public int? U02;
        public int? U03;
        public int? U04;
        public int? U05;
        public int? U06;
        public int? U07;
        public byte[]? U08;
        public int U09;
        public int U10;
        public float? U11;
        public int? U12;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.hasLightmaps); // true if SHmsLightMapCacheSmall is not empty

            if (!n.HasLightmaps)
            {
                return;
            }

            ReadWriteLightMapCacheSmall(n, rw);
        }

        internal void ReadWriteLightMapCacheSmall(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            if (rw.Reader is not null)
            {
                ReadLightMapCacheSmall(n, rw.Reader);
            }

            if (rw.Writer is not null)
            {
                WriteLightMapCacheSmall(n, rw.Writer);
            }
        }

        private void ReadLightMapCacheSmall(CGameCtnChallenge n, GbxReader r)
        {
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

            if (Gbx.ZLib is null)
            {
                return;
            }

            using var ms = n.LightmapCacheData.OpenDecompressedMemoryStream();
            var rBuffer = new GbxReader(ms);
            rBuffer.LoadFrom(r);

            n.LightmapCache = rBuffer.ReadNode<CHmsLightMapCache>();

            var rw = new GbxReaderWriter(rBuffer);
            ReadWriteTheRestOfCompressedData(n, rw);
        }

        private void WriteLightMapCacheSmall(CGameCtnChallenge n, GbxWriter w)
        {
            var lightmapVersion = n.LightmapVersion.GetValueOrDefault(8);

            w.Write(lightmapVersion);

            if (n.LightmapVersion < 2)
            {
                w.WriteNodeRef(n.LightmapCache);
                return;
            }

            if (n.LightmapVersion >= 5)
            {
                w.Write(n.LightmapFrames?.Length ?? 0);
            }

            if (n.LightmapFrames is null || n.LightmapFrames.Length == 0)
            {
                return;
            }

            foreach (var frame in n.LightmapFrames)
            {
                w.WriteWritable(frame, version: lightmapVersion);
            }

            if (Gbx.ZLib is null)
            {
                if (n.LightmapCacheData is null)
                {
                    throw new ZLibNotDefinedException();
                }

                w.Write(n.LightmapCacheData.UncompressedSize);
                w.WriteData(n.LightmapCacheData.Data);
                return;
            }

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            wBuffer.LoadFrom(w);

            wBuffer.WriteNode(n.LightmapCache);

            var rw = new GbxReaderWriter(wBuffer);
            ReadWriteTheRestOfCompressedData(n, rw);

            ms.Position = 0;
            using var compressedMs = new MemoryStream();
            Gbx.ZLib.Compress(ms, compressedMs);

            w.Write((int)ms.Length);
            w.Write((int)compressedMs.Length);
            compressedMs.WriteTo(w.BaseStream);
        }

        private void ReadWriteTheRestOfCompressedData(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);

            if (n.LightmapVersion >= 3)
            {
                rw.Int32(ref U02);
                rw.Int32(ref U03);
                rw.Int32(ref U04);
                rw.Int32(ref U05);
                rw.Int32(ref U06);

                if (n.LightmapVersion >= 4)
                {
                    rw.Int32(ref U07);

                    if (n.LightmapVersion < 5)
                    {
                        rw.Data(ref U08);
                    }
                    else if (n.LightmapFrames is not null)
                    {
                        foreach (var frame in n.LightmapFrames)
                        {
                            frame.U01 = rw.Data(frame.U01);
                            frame.U02 = rw.Single(frame.U02);

                            if (n.LightmapVersion >= 6)
                            {
                                // NHmsLightMapCache::ArchiveToZip
                                frame.Version = rw.Int32(frame.Version);

                                if (frame.Version < 2)
                                {
                                    frame.U03 = rw.Single(frame.U03);
                                    frame.U04 = rw.Int32(frame.U04);

                                    if (frame.Version != 0)
                                    {
                                        frame.U05 = rw.Int32(frame.U05);
                                    }

                                    frame.U06 = rw.Int32(frame.U06);
                                    frame.U07 = rw.Int32(frame.U07);
                                    frame.U08 = rw.Int32(frame.U08);
                                    frame.U09 = rw.Iso4(frame.U09);
                                    frame.U10 = rw.Array<short>(frame.U10);
                                }
                                else
                                {
                                    frame.U11 = rw.Single(frame.U11);
                                    frame.U12 = rw.Int32(frame.U12);

                                    if (frame.Version >= 5)
                                    {
                                        frame.U39 = rw.Single(frame.U39);
                                        frame.U40 = rw.Int32(frame.U40);

                                        if (frame.Version >= 6)
                                        {
                                            frame.U41 = rw.Single(frame.U41);
                                            frame.U42 = rw.Int32(frame.U42);
                                        }
                                    }

                                    frame.U13 = rw.Int32(frame.U13);
                                    frame.U14 = rw.Int32(frame.U14);
                                    frame.U15 = rw.Int32(frame.U15);

                                    if (frame.Version < 4)
                                    {
                                        frame.U16 = rw.ArrayReadableWritable<ProbeGridBoxOld>(frame.U16);
                                    }
                                    else
                                    {
                                        frame.U17 = rw.ArrayReadableWritable<ProbeGridBox>(frame.U17);
                                    }

                                    frame.U18 = rw.Array<Int2>(frame.U18);
                                    frame.U19 = rw.Array<short>(frame.U19);

                                    if (frame.Version >= 3)
                                    {
                                        frame.U20 = rw.Int32(frame.U20);
                                        frame.U21 = rw.Int32(frame.U21);
                                        frame.U22 = rw.Int32(frame.U22);
                                        frame.U23 = rw.Int32(frame.U23);
                                        frame.U24 = rw.Int32(frame.U24);
                                        frame.U25 = rw.Int32(frame.U25);

                                        if (frame.Version >= 4)
                                        {
                                            frame.U33 = rw.Int32(frame.U33);
                                            frame.U34 = rw.Int32(frame.U34);
                                            frame.U35 = rw.Int32(frame.U35);
                                        }

                                        frame.U26 = rw.Single(frame.U26);
                                        frame.U27 = rw.Single(frame.U27);
                                        frame.U28 = rw.Single(frame.U28);
                                        frame.U29 = rw.Single(frame.U29);
                                        frame.U30 = rw.Single(frame.U30);
                                        frame.U31 = rw.Single(frame.U31);
                                        frame.U32 = rw.Array<int>(frame.U32);
                                    }
                                }
                                //

                                if (n.LightmapVersion >= 8)
                                {
                                    frame.U36 = rw.Int32(frame.U36);
                                    frame.U37 = rw.Int32(frame.U37);

                                    // Unchecked
                                    if (n.LightmapVersion >= 10)
                                    {
                                        frame.U38 = rw.Int32(frame.U38);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            rw.Int32(ref U09);
            rw.Int32(ref U10);

            if (n.LightmapVersion < 5)
            {
                rw.Single(ref U11);
            }

            if (n.LightmapVersion >= 7)
            {
                rw.Int32(ref U12);
            }
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class LightmapFrame : IVersionable
    {
        public byte[]? U01 { get; set; }
        public float U02 { get; set; }
        public int Version { get; set; }
        public float? U03 { get; set; }
        public int? U04 { get; set; }
        public int? U05 { get; set; }
        public int? U06 { get; set; }
        public int? U07 { get; set; }
        public int? U08 { get; set; }
        public Iso4? U09 { get; set; }
        public short[]? U10 { get; set; }
        public float? U11 { get; set; }
        public int? U12 { get; set; }
        public int? U13 { get; set; }
        public int? U14 { get; set; }
        public int? U15 { get; set; }
        public ProbeGridBoxOld[]? U16 { get; set; }
        public ProbeGridBox[]? U17 { get; set; }
        public Int2[]? U18 { get; set; }
        public short[]? U19 { get; set; }
        public int? U20 { get; set; }
        public int? U21 { get; set; }
        public int? U22 { get; set; }
        public int? U23 { get; set; }
        public int? U24 { get; set; }
        public int? U25 { get; set; }
        public float? U26 { get; set; }
        public float? U27 { get; set; }
        public float? U28 { get; set; }
        public float? U29 { get; set; }
        public float? U30 { get; set; }
        public float? U31 { get; set; }
        public int[]? U32 { get; set; }
        public int? U33 { get; set; }
        public int? U34 { get; set; }
        public int? U35 { get; set; }
        public int? U36 { get; set; }
        public int? U37 { get; set; }
        public int? U38 { get; set; }
        public float? U39 { get; set; }
        public int? U40 { get; set; }
        public float? U41 { get; set; }
        public int? U42 { get; set; }
    }

    public partial class Chunk03043040 : IVersionable
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

            if (Version >= 1 && Version != 5 && Version < 8)
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
                if (Version >= 9)
                {
                    throw new ChunkVersionNotSupportedException(Version);
                }

                var blockIndexes = r.ReadArray<int>(); // block indexes, -1 means itemIndexes will have the value instead
                var usedBlocks = new CGameCtnBlock?[blockIndexes.Length];
                
                for (var i = 0; i < blockIndexes.Length; i++)
                {
                    var index = blockIndexes[i];

                    if (index > -1)
                    {
                        usedBlocks[i] = n.blocks![index];
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

                if (Version >= 8)
                {
                    return;
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
                block.Coord -= new Int3(1, 0, 1);
                n.bakedBlocks.Add(block);
            }

            U01 = r.ReadInt32();

            n.BakedClipsAdditionalData = r.ReadListReadable<SBakedClipsAdditionalData>(version: Version);
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);
            w.Write(BlocksVersion);

            if (BlocksVersion < 6)
            {
                w.WriteListWritable(n.bakedBlocks, version: BlocksVersion);
            }
            else
            {
                w.Write(n.NbBakedBlocks.GetValueOrDefault());

                if (n.bakedBlocks is not null)
                {
                    foreach (var block in n.bakedBlocks)
                    {
                        try
                        {
                            block.Coord += new Int3(1, 0, 1);
                            w.WriteWritable(block, BlocksVersion);
                        }
                        finally
                        {
                            block.Coord -= new Int3(1, 0, 1);
                        }
                    }
                }
            }

            w.Write(U01);

            w.WriteListWritable(n.BakedClipsAdditionalData, version: Version);
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SBakedClipsAdditionalData;


    public partial class Chunk0304304F : IVersionable
    {
        public int Version { get; set; } = 3;

        public int U01;
        public byte[]? U02;
        public byte U03;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version < 2)
            {
                U01 = r.ReadInt32(); // always 0
                var size = r.ReadInt32();

                using var _ = new Encapsulation(r);
                U02 = r.ReadData(size);
                return;
            }

            if (Version < 3)
            {
                if (r.ReadBoolean())
                {
                    U03 = 2;
                }

                return;
            }

            U03 = r.ReadByte();
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);

            if (Version < 2)
            {
                w.Write(U01);
                w.WriteData(U02);
                return;
            }

            if (Version < 3)
            {
                w.Write(U03 == 2);
                return;
            }

            w.Write(U03);
        }
    }

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
                wBuffer.WriteList(n.Textures);
            }

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    public partial class Chunk03043055
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            // empty, sets classic clips to true?
        }
    }

    public partial class Chunk0304305B : IVersionable
    {
        public int Version { get; set; }

        public bool U13;
        public bool U14;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            n.HasLightmaps = rw.Boolean(n.HasLightmaps);
            rw.Boolean(ref U13);
            rw.Boolean(ref U14);

            if (!n.HasLightmaps)
            {
                return;
            }

            ReadWriteLightMapCacheSmall(n, rw);
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
