using GBX.NET.Utils;
using System.Globalization;

namespace GBX.NET.Engines.Plug;

/// <summary>
/// CPlugCrystal (0x09003000)
/// </summary>
/// <remarks>A custom mesh or model.</remarks>
[Node(0x09003000), WritingNotSupported]
[NodeExtension("Crystal")]
public class CPlugCrystal : CPlugTreeGenerator
{
    #region Enums

    public enum ELayerType
    {
        Geometry,
        Smooth,
        Translation,
        Rotation,
        Scale,
        Mirror,
        U07,
        U08,
        Subdivide,
        Chaos,
        U11,
        U12,
        Deformation,
        Cubes,
        Trigger,
        SpawnPosition
    }

    public enum EAxis
    {
        X, Y, Z
    }

    #endregion

    #region Fields

    private CPlugMaterialUserInst?[]? materials;

    #endregion

    #region Properties

    [NodeMember]
    public CPlugMaterialUserInst?[]? Materials
    {
        get => materials;
        set => materials = value;
    }

    [NodeMember]
    public Layer[] Layers { get; set; }

    #endregion

    #region Constructors

    protected CPlugCrystal()
    {
        Layers = null!;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Exports the crystal to .obj file.
    /// </summary>
    /// <param name="objStream">Stream to write OBJ content into.</param>
    /// <param name="mtlStream">Stream to write MTL content into.</param>
    /// <param name="gameDataFolderPath">Folder for the Material.Gbx, Texture.Gbx, and .dds lookup.</param>
    public void ExportToObj(Stream objStream, Stream mtlStream, string gameDataFolderPath)
    {
        using var exporter = new ObjFileExporter(objStream, mtlStream, gameDataFolderPath);

        exporter.Export(this);
    }

    private static GeometryLayer ReadGeometryLayer(GameBoxReader r,
                                                   CPlugCrystal n,
                                                   string layerId,
                                                   string layerName,
                                                   bool isEnabled,
                                                   int typeVersion)
    {
        var crystal = ReadCrystal(r, n);

        var countup = r.ReadArray<int>();

        var collidable = true;
        var isVisible = true;

        if (typeVersion >= 1)
        {
            isVisible = r.ReadBoolean();
            collidable = r.ReadBoolean();
        }

        return new GeometryLayer(crystal)
        {
            LayerID = layerId,
            LayerName = layerName,
            IsEnabled = isEnabled,
            IsVisible = isVisible,
            Collidable = collidable
        };
    }

    private static TriggerLayer ReadTriggerLayer(GameBoxReader r,
                                                 CPlugCrystal n,
                                                 string layerId,
                                                 string layerName,
                                                 bool isEnabled,
                                                 int typeVersion)
    {
        var crystal = ReadCrystal(r, n);

        if (typeVersion >= 1)
        {
            var countup = r.ReadArray<int>();
        }

        return new TriggerLayer(crystal)
        {
            LayerID = layerId,
            LayerName = layerName,
            IsEnabled = isEnabled
        };
    }

    private static Crystal ReadCrystal(GameBoxReader r, CPlugCrystal n)
    {
        var crystalVersion = r.ReadInt32(); // up to 32 supported
        var u06 = r.ReadInt32(); // 4
        var u07 = r.ReadInt32(); // 3
        var u08 = r.ReadInt32(); // 4
        var u09 = r.ReadSingle(); // 64
        var u10 = r.ReadInt32(); // 2
        var u11 = r.ReadSingle(); // 128
        var u12 = r.ReadInt32(); // 1
        var u13 = r.ReadSingle(); // 192
        var u14 = r.ReadInt32(); // 0 - SAnchorInfo array?

        // SCrystalPart array
        var groups = r.ReadArray(r => new Group()
        {
            U01 = crystalVersion >= 31 ? r.ReadInt32() : 0,
            U02 = crystalVersion >= 36 ? r.ReadByte() : r.ReadInt32(), // maybe bool
            U03 = r.ReadInt32(),
            Name = r.ReadString(),
            U04 = r.ReadInt32(),
            U05 = r.ReadArray<int>()
        });

        if (crystalVersion < 21)
        {
            // some other values
            throw new Exception("crystalVersion < 21 not supported");
        }

        var isEmbeddedCrystal = false;

        if (crystalVersion >= 25)
        {
            if (crystalVersion < 29)
            {
                isEmbeddedCrystal = r.ReadBoolean();
                isEmbeddedCrystal = r.ReadBoolean();
            }

            isEmbeddedCrystal = r.ReadBoolean(asByte: crystalVersion >= 34);

            if (crystalVersion >= 33)
            {
                var u30 = r.ReadInt32(); // local_378
                var u31 = r.ReadInt32(); // local_374
            }
        }

        var vertices = default(Vec3[]);
        var edges = default(Int2[]);
        var faces = default(Face[]);

        if (isEmbeddedCrystal)
        {
            vertices = r.ReadArray<Vec3>();

            var edgesCount = r.ReadInt32();

            if (crystalVersion >= 35)
            {
                var unfacedEdgesCount = r.ReadInt32();
                var unfacedEdges = r.ReadOptimizedIntArray(unfacedEdgesCount * 2); // unfaced edges
            }

            edges = r.ReadArray<Int2>(crystalVersion >= 35 ? 0 : edgesCount);

            var facesCount = r.ReadInt32();

            var uvs = default(Vec2[]);
            var faceIndicies = default(int[]);

            if (crystalVersion >= 37)
            {
                uvs = r.ReadArray<Vec2>(); // unique uv values
                faceIndicies = r.ReadOptimizedIntArray();
            }

            var indiciesCounter = 0;

            faces = r.ReadArray(facesCount, r =>
            {
                var uvVertices = crystalVersion >= 35 ? (r.ReadByte() + 3) : r.ReadInt32();
                var inds = crystalVersion >= 34 ? r.ReadOptimizedIntArray(uvVertices, vertices.Length) : r.ReadArray<int>(uvVertices);

                var uv = default(Vec2[]);

                if (crystalVersion < 27)
                {
                    uv = r.ReadArray<Vec2>();
                    var niceVec = r.ReadVec3();
                }
                else if (crystalVersion < 37)
                {
                    uv = r.ReadArray<Vec2>(uvVertices);
                }
                else if (uvs is not null && faceIndicies is not null)
                {
                    uv = new Vec2[inds.Length];

                    for (var i = 0; i < uv.Length; i++)
                    {
                        uv[i] = uvs[faceIndicies[indiciesCounter]];
                        indiciesCounter++;
                    }
                }

                var materialIndex = default(int);

                if (crystalVersion >= 25)
                {
                    if (crystalVersion >= 33)
                    {
                        materialIndex = n.Materials is null
                            ? r.ReadInt32()
                            : r.ReadOptimizedInt(n.Materials.Length);
                    }
                    else
                    {
                        materialIndex = r.ReadInt32();
                    }
                }

                var groupIndex = crystalVersion >= 33 ? r.ReadOptimizedInt(groups.Length) : r.ReadInt32(); // optimized by amount of groups?

                var material = materialIndex != -1 ? n.Materials?[materialIndex] : null;
                
                return new Face()
                {
                    VertCount = uvVertices,
                    Indices = inds,
                    UV = uv ?? throw new Exception("No UVs"),
                    Material = material,
                    Group = groups[groupIndex]
                };
            });
        }
        else
        {
            var handleVerts = r.ReadArray<(bool, int, int)>(r => new(
                r.ReadBoolean(),
                r.ReadInt32(),
                r.ReadInt32()
            ));
            var handleVertsU01 = r.ReadInt32();
            var handleVertsU02 = r.ReadInt32();
            var handleEdges = r.ReadArray<(bool, int, int)>(r => new(
                r.ReadBoolean(),
                r.ReadInt32(),
                r.ReadInt32()
            ));
            var handleEdgesU01 = r.ReadInt32();
            var handleEdgesU02 = r.ReadInt32();
            var handleFaces = r.ReadArray<(bool, int, int)>(r => new(
                r.ReadBoolean(),
                r.ReadInt32(),
                r.ReadInt32()
            ));
            var handleFacesU01 = r.ReadInt32();
            var handleFacesU02 = r.ReadInt32();

            throw new NotSupportedException("Unsupported crystal.");

            var wtf = r.ReadArray<int>(15007);

            var verts = r.ReadArray<Vec3>();
        }

        foreach (var face in faces)
        {
            if (!isEmbeddedCrystal)
            {
                var u18 = r.ReadInt32();
            }

            if (crystalVersion < 30 || !isEmbeddedCrystal)
            {
                var u19 = r.ReadInt32();
            }

            if (crystalVersion >= 22 && !isEmbeddedCrystal)
            {
                var u20 = r.ReadInt32();
            }
        }

        foreach (var vert in vertices)
        {
            if (crystalVersion < 29)
            {
                var u21 = r.ReadSingle();
            }
        }

        var u22 = r.ReadInt32();

        if (crystalVersion >= 7 && crystalVersion < 32)
        {
            var u23 = r.ReadInt32(); // crystal link array

            if (crystalVersion >= 10)
            {
                var u24 = r.ReadInt32();
                var u25 = r.ReadString();

                if (crystalVersion < 30)
                {
                    var u26 = r.ReadArray<float>(); // SCrystalSmoothingGroup array
                }
            }
        }

        if (crystalVersion < 36)
        {
            var numFaces = r.ReadInt32();
            var numEdges = r.ReadInt32();
            var numVerts = r.ReadInt32();

            var u27 = r.ReadArray<int>(numFaces);
            var u28 = r.ReadArray<int>(numEdges);
            var u29 = r.ReadArray<int>(numVerts);

            var u17 = r.ReadInt32();
        }

        return new Crystal()
        {
            Vertices = vertices,
            Edges = edges,
            Faces = faces,
            Groups = groups
        };
    }

    #endregion

    #region Chunks

    #region 0x003 chunk

    /// <summary>
    /// CPlugCrystal 0x000 chunk
    /// </summary>
    [Chunk(0x09003000)]
    public class Chunk09003000 : Chunk<CPlugCrystal>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.Layers = new[]
            {
                new GeometryLayer(ReadCrystal(r, n))
                {
                    LayerID = "Layer0",
                    LayerName = "Geometry",
                    IsEnabled = true
                }
            };
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CPlugCrystal 0x003 chunk (materials)
    /// </summary>
    [Chunk(0x09003003, "materials")]
    public class Chunk09003003 : Chunk<CPlugCrystal>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.materials = r.ReadArray(r =>
            {
                var name = r.ReadString();

                if (name.Length == 0)  // If the material file exists (name != ""), it references the file instead
                {
                    var material = r.ReadNodeRef<CPlugMaterialUserInst>();

                    // more stuff when version <=1

                    return material;
                }

                return null;
            });
        }
    }

    #endregion

    #region 0x004 skippable chunk

    /// <summary>
    /// CPlugCrystal 0x004 skippable chunk
    /// </summary>
    [Chunk(0x09003004)]
    public class Chunk09003004 : SkippableChunk<CPlugCrystal>, IVersionable
    {
        private int version;
        private byte[]? U01;
        private int? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Bytes(ref U01);

            if (version >= 1)
            {
                rw.Int32(ref U02);
            }
        }
    }

    #endregion

    #region 0x005 chunk (layers)

    /// <summary>
    /// CPlugCrystal 0x005 chunk (layers)
    /// </summary>
    [Chunk(0x09003005, "layers")]
    public class Chunk09003005 : Chunk<CPlugCrystal>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            var layerCount = r.ReadInt32();

            // in some cases, something is here

            n.Layers ??= r.ReadArray<Layer>(layerCount, r =>
            {
                var type = (ELayerType)r.ReadInt32();
                var version = r.ReadInt32(); // 2 - version
                var u02 = r.ReadBoolean();
                var layerId = r.ReadId(); // Layer0
                var layerName = r.ReadString();
                var isEnabled = true;

                if (version >= 1)
                {
                    isEnabled = r.ReadBoolean();
                }

                var typeVersion = r.ReadInt32(); // not part of SLayer::ArchiveWithoutTyp

                return type switch
                {
                    ELayerType.Geometry => ReadGeometryLayer(r, n, layerId, layerName, isEnabled, typeVersion),
                    ELayerType.Trigger => ReadTriggerLayer(r, n, layerId, layerName, isEnabled, typeVersion),
                    ELayerType.Cubes => ReadCubesLayer(r, typeVersion),
                    _ => ReadMaskLayer(r, type, layerId, layerName)
                };
            });
        }

        private static Layer ReadCubesLayer(GameBoxReader r, int version)
        {
            var u01 = r.ReadByte();
            var voxelSize = r.ReadSingle();
            var u03 = default(Vec3);
            var u04 = default(bool?);
            var u05 = default(bool?);

            if (version >= 4)
            {
                u03 = r.ReadVec3();
            }

            var voxelModelArray = r.ReadInt32(); // SVoxelModel array
            var voxelModelArray2 = r.ReadInt32();

            if (version >= 2)
            {
                u04 = r.ReadBoolean();
                u05 = r.ReadBoolean();
            }

            return new CubesLayer();
        }

        private static Layer ReadMaskLayer(GameBoxReader r, ELayerType type, string layerId, string layerName)
        {
            var mask = r.ReadArray(r2 => new LayerMask()
            {
                GroupIndex = r2.ReadInt32(),
                LayerId = r2.ReadId()
            });

            var mask_u01 = r.ReadInt32(); // version

            switch (type)
            {
                case ELayerType.Scale:
                    {
                        var scale = r.ReadVec3();
                        var independently = r.ReadBoolean();

                        return new ScaleLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Scale = scale,
                            Independently = independently,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.SpawnPosition:
                    {
                        var position = r.ReadVec3();
                        var horizontalAngle = r.ReadSingle();
                        var verticalAngle = r.ReadSingle();
                        var rollAngle = 0f;

                        if (mask_u01 >= 1)
                        {
                            rollAngle = r.ReadSingle();
                        }

                        return new SpawnPositionLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Position = position,
                            HorizontalAngle = horizontalAngle,
                            VerticalAngle = verticalAngle,
                            RollAngle = rollAngle,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.Translation:
                    {
                        var translation = r.ReadVec3();

                        return new TranslationLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Translation = translation,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.Rotation:
                    {
                        var rotation = r.ReadSingle(); // in radians
                        var axis = (EAxis)r.ReadInt32();
                        var independently = r.ReadBoolean();

                        return new RotationLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Rotation = rotation,
                            Axis = axis,
                            Independently = independently,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.Mirror:
                    {
                        var axis = (EAxis)r.ReadInt32();
                        var distance = r.ReadSingle();
                        var independently = r.ReadBoolean();

                        return new MirrorLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Distance = distance,
                            Axis = axis,
                            Independently = independently,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.Deformation:
                    // TODO: how deformation works
                    throw new NotSupportedException("Deformation layer is not currently supported.");
                case ELayerType.Chaos:
                    {
                        var minDistance = r.ReadSingle();
                        var chaos_u01 = r.ReadSingle();
                        var maxDistance = r.ReadSingle();

                        return new ChaosLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            MinDistance = minDistance,
                            U02 = chaos_u01,
                            MaxDistance = maxDistance,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.Subdivide:
                    {
                        var subdivisions = r.ReadInt32(); // max 4

                        return new SubdivideLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Subdivisions = subdivisions,
                            U01 = mask_u01
                        };
                    }

                case ELayerType.Smooth:
                    {
                        var intensity = r.ReadInt32(); // max 4

                        return new SmoothLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Intensity = intensity,
                            U01 = mask_u01
                        };
                    }

                default:
                    throw new NotSupportedException($"Unknown or unsupported layer. ({type})");
            }
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CPlugCrystal 0x006 chunk
    /// </summary>
    [Chunk(0x09003006)]
    public class Chunk09003006 : Chunk<CPlugCrystal>, IVersionable
    {
        public Vec2[]? U01;
        public uint[]? U02;
        public int[]? U03;

        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version == 0)
            {
                rw.Array<Vec2>(ref U01);
            }

            if (version >= 1)
            {
                rw.Array<uint>(ref U02); // two Int16 technically

                if (version >= 2)
                {
                    rw.OptimizedIntArray(ref U03);
                }
            }
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CPlugCrystal 0x007 chunk
    /// </summary>
    [Chunk(0x09003007)]
    public class Chunk09003007 : Chunk<CPlugCrystal>, IVersionable
    {
        private int version;

        public float[]? U01;
        public int[]? U02;
        public float U03;
        public float U04;
        public int[]? U05;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Array<float>(ref U01); // SCrystalSmoothingGroup array
            rw.Array<int>(ref U02);

            // in other versions
            //rw.Single(ref U03);
            //rw.Single(ref U04);
            //rw.Array(ref U05);
        }
    }

    #endregion

    #endregion

    #region Other classes

    public abstract class Layer
    {
        public string? LayerID { get; set; }
        public string? LayerName { get; set; }
        public bool IsEnabled { get; set; }
        public int? U01 { get; set; }

        public override string ToString() => LayerName ?? string.Empty;
    }

    public class GeometryLayer : Layer
    {
        public Crystal Crystal { get; init; }
        public bool Collidable { get; set; }
        public bool IsVisible { get; set; }

        public GeometryLayer(Crystal crystal)
        {
            Crystal = crystal;
        }
    }

    public class TriggerLayer : Layer
    {
        public Crystal? Crystal { get; init; }

        public TriggerLayer(Crystal crystal)
        {
            Crystal = crystal;
        }
    }

    public class TranslationLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public Vec3 Translation { get; set; }
    }

    public class ScaleLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public Vec3 Scale { get; set; }
        public bool Independently { get; set; }
    }

    public class RotationLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public float Rotation { get; set; } // in radians
        public EAxis Axis { get; set; }
        public bool Independently { get; set; }
    }

    public class MirrorLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public float Distance { get; set; }
        public EAxis Axis { get; set; }
        public bool Independently { get; set; }
    }

    public class SpawnPositionLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public Vec3 Position { get; set; }
        public float HorizontalAngle { get; set; }
        public float VerticalAngle { get; set; }
        public float RollAngle { get; set; }
    }

    public class ChaosLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
        public float U02 { get; set; }
    }

    public class SubdivideLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public int Subdivisions { get; set; }
    }

    public class SmoothLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public float Intensity { get; set; }
    }

    public class CubesLayer : Layer
    {
        
    }

    public class Group
    {
        public string Name { get; set; }
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public int[]? U05 { get; set; }

        public override string ToString() => Name;

        public Group()
        {
            Name = null!;
        }
    }

    public class Face
    {
        public int VertCount { get; set; }
        public int[] Indices { get; set; } = Array.Empty<int>();
        public Vec2[] UV { get; set; } = Array.Empty<Vec2>();
        public CPlugMaterialUserInst? Material { get; set; }
        public Group? Group { get; set; }

        public override string ToString()
        {
            return $"({string.Join(" ", Indices)}) ({string.Join(" ", UV)})";
        }
    }

    public class LayerMask
    {
        public string? LayerId { get; set; }
        public int GroupIndex { get; set; }
    }

    public class Crystal
    {
        public Vec3[] Vertices { get; set; } = Array.Empty<Vec3>();
        public Int2[] Edges { get; set; } = Array.Empty<Int2>();
        public Face[] Faces { get; set; } = Array.Empty<Face>();
        public Group[] Groups { get; set; } = Array.Empty<Group>();
    }

    #endregion
}
