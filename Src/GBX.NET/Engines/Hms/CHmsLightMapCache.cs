namespace GBX.NET.Engines.Hms;

/// <summary>
/// Lightmap cache (0x06022000)
/// </summary>
[Node(0x06022000)]
public partial class CHmsLightMapCache : CMwNod
{
    private int[]? mapT3s;
    private EQuality quality;
    private Id? collection;
    private string? decoration;
    private EVersion? version;
    private int? decal2d;
    private int? decal3d;
    private EQualityVer? qualityVer;

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

    protected CHmsLightMapCache()
    {

    }

    [Chunk(0x0602200B)]
    public class Chunk0602200B : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Array<int>(ref n.mapT3s);
        }
    }

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

    [Chunk(0x06022015, processSync: true)]
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

    [Chunk(0x06022016)]
    public class Chunk06022016 : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.EnumInt32<EVersion>(ref n.version);
        }
    }

    [Chunk(0x06022017)]
    public class Chunk06022017 : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref n.decal2d);
            rw.Int32(ref n.decal3d);
        }
    }

    [Chunk(0x06022018)]
    public class Chunk06022018 : SkippableChunk<CHmsLightMapCache>
    {
        public ulong U01;

        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.UInt64(ref U01);
        }
    }

    [Chunk(0x06022019)]
    public class Chunk06022019 : SkippableChunk<CHmsLightMapCache>
    {
        public override void ReadWrite(CHmsLightMapCache n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.EnumInt32<EQualityVer>(ref n.qualityVer);
        }
    }
}
