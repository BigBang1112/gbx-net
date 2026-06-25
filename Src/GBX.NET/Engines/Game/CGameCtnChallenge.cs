using GBX.NET.Interfaces.Game;
using System.Collections.Immutable;
using System.IO.Compression;
using System.Security;
using System.Text;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge :
    IGameCtnChallengeTM10,
    IGameCtnChallengeTMSX,
    IGameCtnChallengeTMF,
    IGameCtnChallengeMP4,
    IGameCtnChallengeTM2020
{
    private string? authorLogin;
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

    private string? authorNickname;
    [SupportsFormatting]
    [AppliedWithChunk<HeaderChunk03043008>]
    [AppliedWithChunk<Chunk03043042>]
    public string? AuthorNickname { get => authorNickname; set => authorNickname = value; }

    /// <summary>
    /// Time of the bronze medal. If <see cref="ChallengeParameters"/> is available, it uses the value from there instead.
    /// </summary>
    [AppliedWithChunk<HeaderChunk03043002>(sinceVersion: 1)]
    public TimeInt32? BronzeTime
    {
        get => ChallengeParameters is null ? bronzeTime : ChallengeParameters.BronzeTime;
        set
        {
            ChallengeParameters?.BronzeTime = value;

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
            ChallengeParameters?.SilverTime = value;

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
            ChallengeParameters?.GoldTime = value;

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
            ChallengeParameters?.AuthorTime = value;

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
            ChallengeParameters?.AuthorScore = value;

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
            ChallengeParameters?.MapType = value;

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
            ChallengeParameters?.MapStyle = value;

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

    private Ident decoration = Ident.Empty;
    [AppliedWithChunk<HeaderChunk03043003>]
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public Ident Decoration { get => decoration; set => decoration = value; }

    private List<CGameCtnBlock>? blocks;
    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public List<CGameCtnBlock>? Blocks { get => blocks; set => blocks = value; }

    [AppliedWithChunk<Chunk0304300F>]
    [AppliedWithChunk<Chunk03043013>]
    [AppliedWithChunk<Chunk0304301F>]
    public int? NbBlocks => Blocks?.Count;

    private Checksum128? hashedPassword;
    [AppliedWithChunk<Chunk03043029>]
    public Checksum128? HashedPassword
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
    [AppliedWithChunk<Chunk0304305B>]
    public bool HasLightmaps { get => hasLightmaps; set => hasLightmaps = value; }

    [AppliedWithChunk<HeaderChunk03043003>(sinceVersion: 9)]
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public int? LightmapVersion { get; set; }

    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public ZlibData? LightmapCacheData { get; set; }

#if NET9_0_OR_GREATER
    private readonly Lock LightmapCacheDataLock = new();
#else
    private readonly object LightmapCacheDataLock = new();
#endif

    private CHmsLightMapCache? lightmapCache;
    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public CHmsLightMapCache? LightmapCache
    {
        get
        {
            // Lightmap cache is not compressed in version 1 and below
            if (lightmapCache is not null || LightmapVersion < 2) return lightmapCache;
            if (LightmapCacheData is null || LightmapCacheData.Parsed) return lightmapCache;

            lock (LightmapCacheDataLock)
            {
                if (lightmapCache is not null) return lightmapCache;
                ParseLightMapCacheSmall(); // sets lightmapCache and LightmapCacheData.Parsed to true
                return lightmapCache;
            }
        }
        set
        {
            lock (LightmapCacheDataLock)
            {
                lightmapCache = value;
            }
        }
    }

    private CHmsLightMapCache.Frame[]? lightmapFrames;
    [AppliedWithChunk<Chunk0304303D>]
    [AppliedWithChunk<Chunk0304305B>]
    public CHmsLightMapCache.Frame[]? LightmapFrames
    {
        get
        {
            if (lightmapFrames is not null) return lightmapFrames;
            if (LightmapCacheData is null || LightmapCacheData.Parsed) return lightmapFrames;

            lock (LightmapCacheDataLock)
            {
                if (lightmapFrames is not null) return lightmapFrames;
                ParseLightMapCacheSmall(); // updates lightmapFrames and LightmapCacheData.Parsed to true
                return lightmapFrames;
            }
        }
        set
        {
            lock (LightmapCacheDataLock)
            {
                lightmapFrames = value;
            }
        }
    }

    private CHmsLightMapCache.Small? lightmapCacheSmall;
    public CHmsLightMapCache.Small? LightmapCacheSmall
    {
        get
        {
            if (LightmapCacheData is null || LightmapCacheData.Parsed) return lightmapCacheSmall;

            lock (LightmapCacheDataLock)
            {
                if (LightmapCacheData is null || LightmapCacheData.Parsed) return lightmapCacheSmall;
                ParseLightMapCacheSmall(); // updates lightmapCacheSmall and LightmapCacheData.Parsed to true
                return lightmapCacheSmall;
            }
        }
        set
        {
            lock (LightmapCacheDataLock)
            {
                lightmapCacheSmall = value;
            }
        }
    }

    private List<CGameCtnAnchoredObject>? anchoredObjects;
    [AppliedWithChunk<Chunk03043040>]
    public List<CGameCtnAnchoredObject>? AnchoredObjects { get => anchoredObjects; set => anchoredObjects = value; }

#if NET9_0_OR_GREATER
    private readonly Lock ZoneGenealogyDataLock = new();
#else
    private readonly object ZoneGenealogyDataLock = new();
#endif

    public RawData? ZoneGenealogyData { get; set; }

    private List<CGameCtnZoneGenealogy>? zoneGenealogy;
    [AppliedWithChunk<Chunk03043043>]
    public List<CGameCtnZoneGenealogy>? ZoneGenealogy
    {
        get
        {
            if (ZoneGenealogyData is null || ZoneGenealogyData.Parsed) return zoneGenealogy;

            lock (ZoneGenealogyDataLock)
            {
                if (ZoneGenealogyData is null || ZoneGenealogyData.Parsed) return zoneGenealogy;

                try
                {
                    using var ms = new MemoryStream(ZoneGenealogyData.Data);
                    using var r = new GbxReader(ms);
                    using var _ = new Encapsulation(r);
                    zoneGenealogy = r.ReadListNodeRef<CGameCtnZoneGenealogy>()!;

                    ZoneGenealogyData.Parsed = true;
                }
                catch (Exception ex)
                {
                    ZoneGenealogyData.Exception = ex;
                    throw;
                }
                return zoneGenealogy;
            }
        }
        set
        {
            lock (ZoneGenealogyDataLock)
            {
                zoneGenealogy = value;
            }
        }
    }

    private CScriptTraitsMetadata? scriptMetadata;
    [AppliedWithChunk<Chunk03043044>]
    public CScriptTraitsMetadata? ScriptMetadata { get => scriptMetadata; set => scriptMetadata = value; }

    [AppliedWithChunk<Chunk03043048>]
    public int? NbBakedBlocks => bakedBlocks?.Count;

    private List<CGameCtnBlock>? bakedBlocks;
    [AppliedWithChunk<Chunk03043048>]
    public List<CGameCtnBlock>? BakedBlocks { get => bakedBlocks; set => bakedBlocks = value; }

    [AppliedWithChunk<Chunk03043048>]
    public List<SBakedClipsAdditionalData>? BakedClipsAdditionalData { get; set; }

    [ZipData]
    [AppliedWithChunk<Chunk03043054>]
    public byte[]? EmbeddedZipData { get; set; }

    [AppliedWithChunk<Chunk03043054>]
    private List<string>? Textures { get; set; }

    [AppliedWithChunk<Chunk03043069>]
    public List<MacroblockInstance>? MacroblockInstances { get; set; }

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
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03043028>]
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public string? Comments { get => comments; set => comments = value; }

    private Vec3 thumbnailPitchYawRoll;
    [AppliedWithChunk<Chunk0304302D>]
    [AppliedWithChunk<Chunk03043036>]
    public Vec3 ThumbnailPitchYawRoll { get => thumbnailPitchYawRoll; set => thumbnailPitchYawRoll = value; }

    /// <summary>
    /// List of embedded item models (includes items and blocks) that are expected in the original embedded data ZIP (will not match if modified!). This is used by the game to verify availability of item models without having to look into the ZIP directly. Upon serialization, this list is constructed from scratch again using the actual ZIP data.
    /// </summary>
    public ImmutableList<Ident>? ExpectedEmbeddedItemModels { get; private set; }

    public TMUnlimiter? TMUnlimiterData { get; set; }

    // poss to generate
    string IGameCtnChallenge.MapUid
    {
        get => MapUid ?? throw new MemberNullException(nameof(MapUid));
        set => MapUid = value;
    }

    // poss to generate
    List<CGameCtnBlock> IGameCtnChallenge.Blocks
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

#if NET8_0_OR_GREATER
    public void ExtractEmbeddedZipData(string destinationDirectoryName)
    {
        if (EmbeddedZipData is null || EmbeddedZipData.Length == 0)
        {
            throw new Exception("Embedded data zip is not available and cannot be read.");
        }

        using var ms = new MemoryStream(EmbeddedZipData);
        ZipFile.ExtractToDirectory(ms, destinationDirectoryName);
    }
#else
    public void ExtractEmbeddedZipData(string destinationDirectoryName)
    {
        if (EmbeddedZipData is null || EmbeddedZipData.Length == 0)
        {
            throw new Exception("Embedded data zip is not available and cannot be read.");
        }

        using var ms = new MemoryStream(EmbeddedZipData);
        using var zip = new ZipArchive(ms, ZipArchiveMode.Read);
        
        Directory.CreateDirectory(destinationDirectoryName);

        foreach (var entry in zip.Entries)
        {
            if (string.IsNullOrEmpty(entry.Name))
            {
                continue;
            }

            var destinationPath = Path.Combine(destinationDirectoryName, entry.FullName);
            
            var directoryPath = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Extract the file
            using var entryStream = entry.Open();
            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
            entryStream.CopyTo(fileStream);
        }
    }
#endif

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
            HashedPassword = new Checksum128();
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

    public CGameCtnAnchoredObject PlaceAnchoredObject(Ident itemModel, Vec3 absolutePosition, Vec3 yawPitchRoll, Vec3 offsetPivot = default)
    {
        _ = AnchoredObjects ?? throw new MemberNullException(nameof(AnchoredObjects));

        CreateChunk<Chunk03043040>();

        var anchoredObject = new CGameCtnAnchoredObject
        {
            ItemModel = itemModel,
            AbsolutePositionInMap = absolutePosition,
            YawPitchRoll = yawPitchRoll,
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

    public string CreateHeaderXml()
    {
        if (IsGameVersion(GameVersion.TMSX, strict: true)) return CreateHeaderXml(GameVersion.TMSX);
        if (IsGameVersion(GameVersion.TMU, strict: true)) return CreateHeaderXml(GameVersion.TMU);
        if (IsGameVersion(GameVersion.TMF, strict: true)) return CreateHeaderXml(GameVersion.TMF);
        if (IsGameVersion(GameVersion.MP3, strict: true)) return CreateHeaderXml(GameVersion.MP3);
        if (IsGameVersion(GameVersion.TMT, strict: true)) return CreateHeaderXml(GameVersion.TMT);
        if (IsGameVersion(GameVersion.MP4, strict: true)) return CreateHeaderXml(GameVersion.MP4);
        if (IsGameVersion(GameVersion.TM2020, strict: true)) return CreateHeaderXml(GameVersion.TM2020);
        throw new NotSupportedException($"Game version {GameVersion} cannot be used to automatically create an XML header. Use the CreateHeaderXml(GameVersion gameVersion) overload and specify a concrete GameVersion.");
    }

    public string CreateHeaderXml(GameVersion gameVersion)
    {
        switch (gameVersion)
        {
            case GameVersion.TMSX:
            case GameVersion.TMU:
            case GameVersion.TMF:
                {
                    var version = gameVersion switch
                    {
                        GameVersion.TMSX => "TMc.6",
                        GameVersion.TMU or GameVersion.TMF => "TMc.6",
                        _ => throw new NotSupportedException($"Game version {gameVersion} cannot be used to automatically assign 'version'.")
                    };

                    var exever = gameVersion switch
                    {
                        GameVersion.TMSX => "0.1.5.0",
                        GameVersion.TMU => "0.2.1.0",
                        GameVersion.TMF => "2.11.25", // also often 2.11.11
                        _ => throw new NotSupportedException($"Game version {gameVersion} cannot be used to automatically assign 'exever'.")
                    };

                    var sb = new StringBuilder("<header type=\"challenge\" version=\"");
                    sb.Append(version);
                    sb.Append("\" exever=\"");
                    sb.Append(exever);
                    sb.Append("\"><ident uid=\"");
                    sb.Append(SecurityElement.Escape(MapUid));
                    sb.Append("\" name=\"");
                    sb.Append(SecurityElement.Escape(MapName));
                    sb.Append("\" author=\"");
                    sb.Append(SecurityElement.Escape(AuthorLogin));
                    sb.Append("\"/><desc envir=\"");
                    sb.Append(SecurityElement.Escape(GetEnvironment())); // some collections might be called wrong
                    sb.Append("\" mood=\"");
                    sb.Append(SecurityElement.Escape(Decoration.Id));
                    sb.Append("type=\"");
                    sb.Append(Mode.ToString());
                    sb.Append("\" nblaps=\"");
                    sb.Append(IsLapRace ? NbLaps : 0);
                    sb.Append("\" price=\"");
                    sb.Append(Cost);
                    sb.Append("\" ");

                    if (ModPackDesc is not null && !string.IsNullOrEmpty(ModPackDesc.FilePath))
                    {
                        sb.Append("mod=\"");
                        sb.Append(SecurityElement.Escape(Path.GetFileNameWithoutExtension(ModPackDesc.FilePath)));
                        sb.Append('"');
                    }

                    sb.Append("/><times bronze=\"");
                    sb.Append(BronzeTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" silver=\"");
                    sb.Append(SilverTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" gold=\"");
                    sb.Append(GoldTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" authortime=\"");
                    sb.Append(AuthorTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" authorscore=\"");
                    sb.Append(AuthorScore);
                    sb.Append("\"/><deps>");

                    foreach (var dep in GetBlocks()
                        .Select(x => x.Skin?.PackDesc)
                        .OfType<PackDesc>()
                        .Where(x => !string.IsNullOrEmpty(x.FilePath)))
                    {
                        AppendDep(sb, dep);
                    }

                    if (ModPackDesc is not null && !string.IsNullOrEmpty(ModPackDesc.FilePath))
                    {
                        AppendDep(sb, ModPackDesc);
                    }

                    AppendMediaTrackerDeps(sb);

                    if (CustomMusicPackDesc is not null && !string.IsNullOrEmpty(CustomMusicPackDesc.FilePath))
                    {
                        AppendDep(sb, CustomMusicPackDesc);
                    }

                    sb.Append("</deps></header>");
                    return sb.ToString();
                }
            case GameVersion.MP3:
            case GameVersion.TMT:
            case GameVersion.MP4:
            case GameVersion.TM2020:
                {
                    var exever = "3.3.0";
                    var exebuild = default(string);
                    var lighmapVersion = 0;

                    if (lightmapFrames?.Length > 0)
                    {
                        lighmapVersion = gameVersion switch
                        {
                            GameVersion.MP3 or GameVersion.TMT => 6,
                            GameVersion.MP4 => 7,
                            GameVersion.TM2020 => 8, // somehow, LightmapVersion 10 is still being written as 8 here
                            _ => throw new NotSupportedException($"Game version {gameVersion} cannot be used to automatically assign 'lightmap'.")
                        };
                    }

                    foreach (var param in BuildVersion?.Split(' ') ?? [])
                    {
                        if (param.StartsWith("date=", StringComparison.OrdinalIgnoreCase))
                        {
                            exebuild = param.Substring(5);
                        }
                        else if (param.StartsWith("GameVersion=", StringComparison.OrdinalIgnoreCase))
                        {
                            exever = param.Substring(12);
                        }
                    }

                    exebuild ??= gameVersion switch
                    {
                        GameVersion.MP3 => "2015-06-18_15_10",
                        GameVersion.TMT => "2016-11-07_16_15",
                        GameVersion.MP4 => "2019-11-19_18_50",
                        GameVersion.TM2020 => "2024-12-12_15_15",
                        _ => throw new NotSupportedException($"Game version {gameVersion} cannot be used to automatically assign 'exebuild'.")
                    };

                    var sb = new StringBuilder("<header type=\"map\" exever=\"");
                    sb.Append(exever);
                    sb.Append("\" exebuild=\"");
                    sb.Append(exebuild);
                    sb.Append("\" title=\"");
                    sb.Append(SecurityElement.Escape(TitleId));
                    sb.Append("\" lightmap=\"");
                    sb.Append(lighmapVersion);
                    sb.Append("\"><ident uid=\"");
                    sb.Append(SecurityElement.Escape(MapUid));
                    sb.Append("\" name=\"");
                    sb.Append(SecurityElement.Escape(MapName));
                    sb.Append("\" author=\"");
                    sb.Append(SecurityElement.Escape(AuthorLogin));
                    sb.Append("\" authorzone=\"");
                    sb.Append(SecurityElement.Escape(AuthorZone));
                    sb.Append("\"/><desc envir=\"");
                    sb.Append(SecurityElement.Escape(GetEnvironment()));
                    sb.Append("\" mood=\"");
                    sb.Append(SecurityElement.Escape(Decoration.Id));
                    sb.Append("\" type=\"");
                    sb.Append(Mode.ToString());
                    sb.Append("\" maptype=\"");
                    sb.Append(SecurityElement.Escape(MapType ?? ""));
                    sb.Append("\" mapstyle=\"");
                    sb.Append(SecurityElement.Escape(MapStyle ?? ""));
                    sb.Append("\" validated=\"");
                    sb.Append(AuthorTime.HasValue ? "1" : "0"); // It's a bit different logic, but here it's simplified
                    sb.Append("\" nblaps=\"");
                    sb.Append(IsLapRace ? NbLaps : 0);
                    sb.Append("\" displaycost=\"");
                    sb.Append(Cost);
                    sb.Append("\" mod=\"");
                    sb.Append(SecurityElement.Escape(Path.GetFileNameWithoutExtension(ModPackDesc?.FilePath ?? "")));
                    sb.Append("\" hasghostblocks=\"");
                    sb.Append(HasGhostBlocks ? "1" : "0");
                    sb.Append("\"/><playermodel id=\"");
                    sb.Append(SecurityElement.Escape(PlayerModel?.Id ?? ""));
                    sb.Append("\"/><times bronze=\"");
                    sb.Append(BronzeTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" silver=\"");
                    sb.Append(SilverTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" gold=\"");
                    sb.Append(GoldTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" authortime=\"");
                    sb.Append(AuthorTime?.TotalMilliseconds ?? -1);
                    sb.Append("\" authorscore=\"");
                    sb.Append(AuthorScore);

                    if (gameVersion is GameVersion.TM2020)
                    {
                        sb.Append("\" hasclones=\"");
                        sb.Append(HasClones ? "1" : "0");
                    }

                    sb.Append("\"/><deps>");

                    if (ModPackDesc is not null && !string.IsNullOrEmpty(ModPackDesc.FilePath))
                    {
                        AppendDep(sb, ModPackDesc);
                    }

                    if (CustomMusicPackDesc is not null && !string.IsNullOrEmpty(CustomMusicPackDesc.FilePath))
                    {
                        AppendDep(sb, CustomMusicPackDesc);
                    }

                    AppendMediaTrackerDeps(sb);

                    sb.Append("</deps></header>");
                    return sb.ToString();
                }
            default:
                throw new NotSupportedException($"Game version {gameVersion} is not supported for header XML generation.");
        }

        void AppendMediaTrackerDeps(StringBuilder sb)
        {
            foreach (var block in ClipIntro?.Tracks
                .Concat(ClipGroupInGame?.Clips.SelectMany(x => x.Clip.Tracks) ?? [])
                .Concat(ClipGroupEndRace?.Clips.SelectMany(x => x.Clip.Tracks) ?? [])
                .Concat(ClipGlobal?.Tracks ?? [])
                .Concat(ClipAmbiance?.Tracks ?? [])
                .SelectMany(x => x.Blocks) ?? [])
            {
                switch (block)
                {
                    case CGameCtnMediaBlockImage imageBlock:
                        if (imageBlock.Image is not null && !string.IsNullOrEmpty(imageBlock.Image.FilePath))
                        {
                            AppendDep(sb, imageBlock.Image);
                        }
                        break;
                    case CGameCtnMediaBlockSound soundBlock:
                        if (soundBlock.Sound is not null && !string.IsNullOrEmpty(soundBlock.Sound.FilePath))
                        {
                            AppendDep(sb, soundBlock.Sound);
                        }
                        break;
                }
            }
        }

        static void AppendDep(StringBuilder sb, PackDesc dep)
        {
            sb.Append("<dep file=\"");
            sb.Append(SecurityElement.Escape(dep.FilePath));
            sb.Append('"');

            if (!string.IsNullOrEmpty(dep.LocatorUrl))
            {
                sb.Append(" url=\"");
                sb.Append(dep.LocatorUrl);
                sb.Append('"');
            }

            sb.Append("/>");
        }
    }

    public string UpdateHeaderXml(GameVersion version)
    {
        return Xml = CreateHeaderXml(version);
    }

    public string UpdateHeaderXml()
    {
        return Xml = CreateHeaderXml();
    }

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

    public partial class HeaderChunk03043002
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            if (Version <= 2)
            {
                rw.Ident(ref n.mapInfo);
                rw.String(ref n.mapName);
            }
            rw.Boolean(ref n.needUnlock);
            if (Version >= 1)
            {
                rw.TimeInt32Nullable(ref n.bronzeTime);
                rw.TimeInt32Nullable(ref n.silverTime);
                rw.TimeInt32Nullable(ref n.goldTime);
                rw.TimeInt32Nullable(ref n.authorTime);
                if (Version == 2)
                {
                    rw.Byte(ref U02);
                }
                if (Version >= 4)
                {
                    rw.Int32(ref n.cost);
                    if (Version >= 5)
                    {
                        rw.Boolean(ref n.isLapRace);
                        if (Version >= 6)
                        {
                            rw.EnumInt32<PlayMode>(ref n.mode);
                            if (Version >= 9)
                            {
                                rw.Boolean(ref n.hasClones);
                                if (Version >= 10)
                                {
                                    rw.Int32(ref n.authorScore);
                                    if (Version >= 11)
                                    {
                                        rw.EnumInt32<EditorMode>(ref n.editor);
                                        if (Version >= 12)
                                        {
                                            rw.Int32(ref U05);
                                            if (Version >= 13)
                                            {
                                                rw.Int32(ref n.nbCheckpoints);
                                                if (n.isLapRace || rw.Reader is not null)
                                                {
                                                    rw.Int32(ref n.nbLaps);
                                                }
                                                else
                                                {
                                                    rw.Int32(1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

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

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.mapInfo = r.ReadIdent();
            n.mapName = r.ReadString();
            n.decoration = r.ReadIdent();
            n.size = r.ReadInt3();
            n.needUnlock = r.ReadBoolean();
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
            w.Write(n.needUnlock);
            w.Write(Version);

            if (Version < 6)
            {
                var tmUnlimiterChunk = n.GetChunk<Chunk3F001001>() ?? (Chunk3F001001?)n.GetChunk<Chunk3F001002>() ?? (Chunk3F001001?)n.GetChunk<Chunk3F001003>();

                if (tmUnlimiterChunk?.Version > 0)
                {
                    w.WriteListWritable(n.TMUnlimiterData?.FakeBlocks, version: Version);
                    return;
                }

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

    private void ParseLightMapCacheSmall()
    {
        if (LightmapCacheData is null) throw new InvalidOperationException("LightmapCacheData not available");

        try
        {
            if (lightmapFrames is null) throw new InvalidOperationException("LightmapFrames not available");
            if (LightmapVersion is null) throw new InvalidOperationException("LightmapVersion not available");

            using var r = LightmapCacheData.OpenDecompressedReader();
            using var rw = new GbxReaderWriter(r);

            lightmapCacheSmall = new CHmsLightMapCache.Small();
            lightmapCacheSmall.ReadWrite(LightmapVersion.Value, ref lightmapCache, lightmapFrames, rw);

            LightmapCacheData.Parsed = true;
        }
        catch (Exception ex)
        {
            LightmapCacheData.Exception = ex;
            throw;
        }
    }

    public partial class Chunk0304303D
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.hasLightmaps); // true if SHmsLightMapCacheSmall is not empty

            if (!n.HasLightmaps)
            {
                return;
            }

            ReadWriteLightMapCacheSmall(n, rw);
        }

        protected static void ReadWriteLightMapCacheSmall(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            n.LightmapVersion = rw.Int32(n.LightmapVersion.GetValueOrDefault(8));

            if (n.LightmapVersion < 2)
            {
                rw.NodeRef<CHmsLightMapCache>(ref n.lightmapCache);
                throw new NotSupportedException("Lightmap version <2 is not supported.");
            }

            if (rw.Reader is not null)
            {
                var r = rw.Reader;

                var frameCount = n.LightmapVersion >= 5 ? r.ReadInt32() : 1;

                n.lightmapFrames = r.ReadArrayReadable<CHmsLightMapCache.Frame>(frameCount, n.LightmapVersion.GetValueOrDefault(8));

                if (!n.lightmapFrames.Any(x => x.Data?.Length > 0 || x.Data2?.Length > 0 || x.Data3?.Length > 0))
                {
                    return;
                }

                n.LightmapCacheData = r.ReadZlibData();
            }

            if (rw.Writer is not null)
            {
                var w = rw.Writer;

                if (n.LightmapVersion >= 5)
                {
                    w.Write(n.lightmapFrames?.Length ?? 0);
                }

                if (n.lightmapFrames is null or { Length: 0 })
                {
                    return;
                }

                foreach (var frame in n.lightmapFrames)
                {
                    w.WriteWritable(frame, version: n.LightmapVersion.Value);
                }

                w.WriteZlibData(n.LightmapCacheData, w =>
                {
                    using var rw = new GbxReaderWriter(w);
                    (n.LightmapCacheSmall ?? new()).ReadWrite(n.LightmapVersion.Value, ref n.lightmapCache, n.lightmapFrames, rw);
                });
            }
        }
    }

    public partial class Chunk03043040 : IVersionable
    {
        public int[]? U02;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 4;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            r.ReadEncapsulated(r =>
            {
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
            });
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);

            w.WriteEncapsulated(w =>
            {
                w.WriteListNodeRef_deprec((n.anchoredObjects ?? [])!);

                var itemDict = new Dictionary<CGameCtnAnchoredObject, int>();

                if (n.anchoredObjects is not null)
                {
                    for (var i = 0; i < n.anchoredObjects.Count; i++)
                    {
                        itemDict[n.anchoredObjects[i]] = i;
                    }
                }

                if (Version >= 1 && Version != 5 && Version < 8)
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

                    w.WriteList(pairs);
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

                    w.WriteArray(usedBlockIndexList.Select(x => x.blockIndex).ToArray());

                    if (Version < 7)
                    {
                        w.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                    }

                    if (Version >= 6)
                    {
                        w.WriteArray(usedItemIndexList.Select(x => x.itemIndex).ToArray());
                    }

                    if (Version >= 7)
                    {
                        w.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                    }

                    if (Version != 6)
                    {
                        w.WriteArray(Enumerable.Repeat(-1, usedBlockIndexList.Count).ToArray());
                    }

                    w.WriteArray(snappedOnIndices.ToArray());
                }
            });
        }
    }

    public partial class Chunk03043043
    {
        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.ZoneGenealogyData = r.ReadEncapsulated();
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.WriteEncapsulated(n.ZoneGenealogyData, w =>
            {
                w.WriteListNodeRef<CGameCtnZoneGenealogy>(n.ZoneGenealogy!);
            });
        }
    }

    public partial class Chunk03043044
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.Encapsulated(rw =>
            {
                rw.Node<CScriptTraitsMetadata>(ref n.scriptMetadata!);
            });
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

        public RawData? U02;
        public byte U03;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version < 2)
            {
                U02 = r.ReadEncapsulated();
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
                w.WriteEncapsulated(U02 ?? throw new InvalidOperationException("U02 must be set for version <2"));
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

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            r.ReadEncapsulated(r =>
            {
                n.ExpectedEmbeddedItemModels = r.ReadArrayIdent().ToImmutableList();

                n.EmbeddedZipData = r.ReadData();

                if (Version >= 1)
                {
                    n.Textures = r.ReadListString();
                }
            });
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);

            w.WriteEncapsulated(w =>
            {
                if (n.EmbeddedZipData is null || n.EmbeddedZipData.Length == 0)
                {
                    w.Write(0);
                    w.Write(0);
                }
                else
                {
                    using var embeddedMs = new MemoryStream(n.EmbeddedZipData);
                    using var zip = new ZipArchive(embeddedMs, ZipArchiveMode.Read);

                    var itemModelList = new List<Ident>();

                    foreach (var entry in zip.Entries)
                    {
                        const string itemsPrefix = "Items\\";
                        const string blocksPrefix = "Blocks\\";
                        const string clubItemsPrefix = "ClubItems\\";

                        using var entryStream = entry.Open();

                        try
                        {
                            var nodeHeader = Gbx.ParseHeaderNode(entryStream);

                            if (nodeHeader is not CGameItemModel itemModel)
                            {
                                continue;
                            }

                            if (itemModel.Ident is null)
                            {
                                continue;
                            }

                            var ident = itemModel.Ident;

                            // sometimes, when the items are placed in incorrect or different folders, the ident won't match the file name
                            // this will cause a popup on opening, but the items will still be loaded. needs more investigation if the ident
                            // should come entirely from the file name or not
                            var fullName = entry.FullName.Replace('/', '\\');
                            if (fullName.StartsWith(itemsPrefix))
                            {
                                ident = ident with { Id = fullName.Substring(itemsPrefix.Length) };
                            }
                            else if (fullName.StartsWith(blocksPrefix))
                            {
                                ident = ident with { Id = fullName.Substring(blocksPrefix.Length) };
                            }
                            else if (fullName.StartsWith(clubItemsPrefix))
                            {
                                ident = ident with
                                {
#if NET6_0_OR_GREATER
                                    Id = string.Concat("club:", fullName.AsSpan(clubItemsPrefix.Length))
#else
                                    Id = "club:" + fullName.Substring(clubItemsPrefix.Length)
#endif
                                };
                            }

                            itemModelList.Add(ident);

                            // CGameItemModel.Ident is also often renamed inside the Gbx file
                            // so if this is an issue to match the Ident, read the gbx fully, change Ident, and save
                            // do so only if it doesn't match with entry file name, to optimize the process
                        }
                        catch
                        {
                            // TODO: log
                        }
                    }

                    w.WriteList(itemModelList);
                    w.WriteData(n.EmbeddedZipData!);
                }

                if (Version >= 1)
                {
                    w.WriteList(n.Textures);
                }
            });
        }
    }

    public partial class Chunk0304305B : IVersionable
    {
        public int Version { get; set; }

        public bool U01;
        public bool U02;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            n.HasLightmaps = rw.Boolean(n.HasLightmaps);
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);

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
                block.YawPitchRoll = rw.Vec3(block.YawPitchRoll);
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
