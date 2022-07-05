namespace GBX.NET.Engines.Hms;

/// <summary>
/// Lightmap cache.
/// </summary>
/// <remarks>ID: 0x06022000</remarks>
[Node(0x06022000)]
[NodeExtension("LightMapCache")]
public partial class CHmsLightMapCache : CMwNod
{
    #region Fields

    private int[]? mapT3s;
    private EQuality quality;
    private Id? collection;
    private string? decoration;
    private EVersion? version;
    private int? decal2d;
    private int? decal3d;
    private EQualityVer? qualityVer;
    private int? ambSamples;
    private int? dirSamples;
    private int? pntSamples;
    private ESortMode? sortMode;
    private EAllocMode? allocMode;
    private ECompressMode? compressMode;
    private EBump? bump;
    private bool spriteOriginYWasWronglyTop;
    private SMap[]? maps;
    private SHmsLightMapCacheMapping? mapping;
    private EPlugGpuPlatform? gpuPlatform;
    private float? allocatedTexelByMeter;

    #endregion

    #region Properties

    [NodeMember]
    public int[]? MapT3s
    {
        get
        {
            DiscoverChunk<Chunk0602200B>();
            return mapT3s;
        }
        set
        {
            DiscoverChunk<Chunk0602200B>();
            mapT3s = value;
        }
    }

    /// <summary>
    /// Quality of the calculated shadows.
    /// </summary>
    /// <remarks>Exact name of this member is m_Quality.</remarks>
    [NodeMember(ExactName = "m_Quality")]
    public EQuality Quality
    {
        get
        {
            DiscoverChunk<Chunk0602200F>();
            return quality;
        }
        set
        {
            DiscoverChunk<Chunk0602200F>();
            quality = value;
        }
    }

    /// <remarks>Exact name of this member is m_Id_IdCollection.</remarks>
    [NodeMember(ExactName = "m_Id_IdCollection")]
    public Id? Collection
    {
        get
        {
            DiscoverChunk<Chunk06022015>();
            return collection;
        }
        set
        {
            DiscoverChunk<Chunk06022015>();
            collection = value;
        }
    }

    /// <remarks>Exact name of this member is m_Id_IdDecoration.</remarks>
    [NodeMember(ExactName = "m_Id_IdDecoration")]
    public string? Decoration
    {
        get
        {
            DiscoverChunk<Chunk06022015>();
            return decoration;
        }
        set
        {
            DiscoverChunk<Chunk06022015>();
            decoration = value;
        }
    }

    /// <remarks>Exact name of this member is m_Version.</remarks>
    [NodeMember(ExactName = "m_Version")]
    public EVersion? Version
    {
        get
        {
            DiscoverChunk<Chunk06022016>();
            return version;
        }
        set
        {
            DiscoverChunk<Chunk06022016>();
            version = value;
        }
    }

    /// <remarks>Exact name of this member is cDecal2d.</remarks>
    [NodeMember(ExactName = "cDecal2d")]
    public int? Decal2d
    {
        get
        {
            DiscoverChunk<Chunk06022017>();
            return decal2d;
        }
        set
        {
            DiscoverChunk<Chunk06022017>();
            decal2d = value;
        }
    }

    /// <remarks>Exact name of this member is cDecal3d.</remarks>
    [NodeMember(ExactName = "cDecal3d")]
    public int? Decal3d
    {
        get
        {
            DiscoverChunk<Chunk06022017>();
            return decal3d;
        }
        set
        {
            DiscoverChunk<Chunk06022017>();
            decal3d = value;
        }
    }

    /// <remarks>Exact name of this member is m_QualityVer.</remarks>
    [NodeMember(ExactName = "m_QualityVer")]
    public EQualityVer? QualityVer
    {
        get
        {
            DiscoverChunk<Chunk06022019>();
            return qualityVer;
        }
        set
        {
            DiscoverChunk<Chunk06022019>();
            qualityVer = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public int? AmbSamples
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return ambSamples;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            ambSamples = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public int? DirSamples
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return dirSamples;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            dirSamples = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public int? PntSamples
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return pntSamples;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            pntSamples = value;
        }
    }

    /// <remarks>Exact name of this member is m_SortMode.</remarks>
    [NodeMember(ExactName = "m_SortMode")]
    public ESortMode? SortMode
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return sortMode;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            sortMode = value;
        }
    }

    /// <remarks>Exact name of this member is m_AllocMode.</remarks>
    [NodeMember(ExactName = "m_AllocMode")]
    public EAllocMode? AllocMode
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return allocMode;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            allocMode = value;
        }
    }

    /// <remarks>Exact name of this member is m_CompressMode.</remarks>
    [NodeMember(ExactName = "m_CompressMode")]
    public ECompressMode? CompressMode
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return compressMode;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            compressMode = value;
        }
    }

    /// <remarks>Exact name of this member is m_Bump.</remarks>
    [NodeMember(ExactName = "m_Bump")]
    public EBump? Bump
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return bump;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            bump = value;
        }
    }

    public SMap[]? Maps
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return maps;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            maps = value;
        }
    }

    /// <remarks>Exact name of this member is m_SpriteOriginY_WasWronglyTop.</remarks>
    [NodeMember(ExactName = "m_SpriteOriginY_WasWronglyTop")]
    public bool SpriteOriginYWasWronglyTop
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return spriteOriginYWasWronglyTop;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            spriteOriginYWasWronglyTop = value;
        }
    }

    public SHmsLightMapCacheMapping? Mapping
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return mapping;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            mapping = value;
        }
    }

    /// <remarks>Exact name of this member is m_GpuPlatform.</remarks>
    [NodeMember(ExactName = "m_GpuPlatform")]
    public EPlugGpuPlatform? GpuPlatform
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return gpuPlatform;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            gpuPlatform = value;
        }
    }

    /// <remarks>Exact name of this member is m_AllocatedTexelByMeter.</remarks>
    [NodeMember(ExactName = "m_AllocatedTexelByMeter")]
    public float? AllocatedTexelByMeter
    {
        get
        {
            DiscoverChunk<Chunk0602201A>();
            return allocatedTexelByMeter;
        }
        set
        {
            DiscoverChunk<Chunk0602201A>();
            allocatedTexelByMeter = value;
        }
    }

    #endregion

    #region Constructors

    protected CHmsLightMapCache()
    {

    }

    #endregion

    #region Chunks

    #region 0x00B skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x00B skippable chunk
    /// </summary>
    [Chunk(0x0602200B)]
    public class Chunk0602200B : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Array<int>(ref n.mapT3s);
        }
    }

    #endregion

    #region 0x00F skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0602200F)]
    public class Chunk0602200F : SkippableChunk<CHmsLightMapCache>
    {
        public int U01;

        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.EnumInt32<EQuality>(ref n.quality);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x013 skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x013 skippable chunk
    /// </summary>
    [Chunk(0x06022013)]
    public class Chunk06022013 : SkippableChunk<CHmsLightMapCache>
    {
        public bool U01;
        public bool U02;
        public ulong U03;

        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.UInt64(ref U03);
        }
    }

    #endregion

    #region 0x015 skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x015 skippable chunk
    /// </summary>
    [Chunk(0x06022015, processSync: true)] // does processSync need to be set?
    public class Chunk06022015 : SkippableChunk<CHmsLightMapCache>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public ulong U01;
        public int U02;
        public bool? U03;
        public TimeSpan? U04;
        public int? U05;
        public string? U06;

        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref version);
            rw.UInt64(ref U01);
            rw.Collection(ref n.collection);
            rw.Id(ref n.decoration);
            rw.Int32(ref U02);

            if (version < 4)
            {
                rw.Boolean(ref U03);
            }

            if (version >= 2)
            {
                rw.Int32(0);
                rw.TimeOfDay(ref U04);

                if (version >= 3)
                {
                    rw.Int32(0);
                    rw.Int32(ref U05);

                    if (version >= 5)
                    {
                        rw.String(ref U06);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x016 skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x016 skippable chunk
    /// </summary>
    [Chunk(0x06022016)]
    public class Chunk06022016 : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.EnumInt32<EVersion>(ref n.version);
        }
    }

    #endregion

    #region 0x017 skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x017 skippable chunk
    /// </summary>
    [Chunk(0x06022017)]
    public class Chunk06022017 : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref n.decal2d);
            rw.Int32(ref n.decal3d);
        }
    }

    #endregion

    #region 0x018 skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x018 skippable chunk
    /// </summary>
    [Chunk(0x06022018)]
    public class Chunk06022018 : SkippableChunk<CHmsLightMapCache>
    {
        public ulong U01;

        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.UInt64(ref U01);
        }
    }

    #endregion

    #region 0x019 skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x019 skippable chunk
    /// </summary>
    [Chunk(0x06022019)]
    public class Chunk06022019 : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.EnumInt32<EQualityVer>(ref n.qualityVer);
        }
    }

    #endregion

    #region 0x01A skippable chunk

    /// <summary>
    /// CHmsLightMapCache 0x01A skippable chunk
    /// </summary>
    [Chunk(0x0602201A)]
    public class Chunk0602201A : SkippableChunk<CHmsLightMapCache>, IVersionable
    {
        private int version = 13;
        private int countSMap;

        public int Version { get => version; set => version = value; }

        public byte[]? U01;
        public int U02;
        public int U03;
        public bool U04;
        public int? U05;
        public short[]? U06;
        public float? U07;
        public int U08;
        public int U09;

        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref version);

            rw.Int32(ref countSMap);
            rw.Bytes(ref U01, countSMap * 5 * 4);

            rw.Int32(ref n.ambSamples);
            rw.Int32(ref n.dirSamples);
            rw.Int32(ref n.pntSamples);
            rw.EnumInt32<ESortMode>(ref n.sortMode);
            rw.EnumInt32<EAllocMode>(ref n.allocMode);
            rw.Int32(ref U02);
            rw.EnumInt32<ECompressMode>(ref n.compressMode);
            rw.Int32(ref U03);

            if (version >= 6)
            {
                rw.EnumInt32<EBump>(ref n.bump);
            }

            if (version < 3)
            {
                throw new ChunkVersionNotSupportedException(version);
            }

            rw.Array<SMap>(ref n.maps, (rw, x) => x.ReadWrite(rw, version));

            rw.Boolean(ref U04);
            rw.Boolean(ref n.spriteOriginYWasWronglyTop);

            // SHmsLightMapCacheMapping::Archive

            rw.Archive<SHmsLightMapCacheMapping>(ref n.mapping);

            if (version != 0)
            {
                rw.EnumInt32<EPlugGpuPlatform>(ref n.gpuPlatform);
            }

            if (version == 2)
            {
                // GmVec3_ArchiveAsReal16
                rw.Array<short>(ref U06, count: 3);
            }

            if (version >= 5)
            {
                rw.Single(ref n.allocatedTexelByMeter);
            }

            if (version >= 11)
            {
                rw.Int32(ref U08);
                rw.Int32(ref U09);
            }
        }
    }

    #endregion

    #endregion
}
