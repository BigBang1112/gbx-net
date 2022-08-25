using GBX.NET.Utils;
using System.Globalization;

namespace GBX.NET.Engines.Plug;

/// <summary>
/// A custom mesh or model with materials.
/// </summary>
/// <remarks>ID: 0x09003000</remarks>
[Node(0x09003000), WritingNotSupported]
[NodeExtension("Crystal")]
public partial class CPlugCrystal : CPlugTreeGenerator
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

    private CPlugMaterialUserInst?[] materials;

    #endregion

    #region Properties

    [NodeMember]
    public CPlugMaterialUserInst?[] Materials { get => materials; set => materials = value; }

    [NodeMember]
    public Layer[] Layers { get; set; }

    #endregion

    #region Constructors

    protected CPlugCrystal()
    {
        materials = Array.Empty<CPlugMaterialUserInst>();
        Layers = Array.Empty<Layer>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Exports the crystal to .obj file.
    /// </summary>
    /// <param name="objStream">Stream to write OBJ content into.</param>
    /// <param name="mtlStream">Stream to write MTL content into.</param>
    /// <param name="mergeVerticesDigitThreshold">If set, overlapping vertices (usually between the mesh groups) will be merged. 3 or 4 give the best accuracy.</param>
    /// <param name="gameDataFolderPath">Folder for the Material.Gbx, Texture.Gbx, and .dds lookup.</param>
    public void ExportToObj(Stream objStream, Stream mtlStream, int? mergeVerticesDigitThreshold = null, string? gameDataFolderPath = null)
    {
        using var exporter = new ObjFileExporter(objStream, mtlStream, mergeVerticesDigitThreshold, gameDataFolderPath);

        exporter.Export(this);
    }

    private static GeometryLayer ReadGeometryLayer(GameBoxReader r,
                                                   CPlugMaterialUserInst?[]? materials,
                                                   string layerId,
                                                   string layerName,
                                                   bool isEnabled,
                                                   int typeVersion)
    {
        var crystal = Crystal.Read(r, materials);

        var countup = r.ReadArray<int>(); // ID for each group?

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
                                                 CPlugMaterialUserInst?[]? materials,
                                                 string layerId,
                                                 string layerName,
                                                 bool isEnabled,
                                                 int typeVersion)
    {
        var crystal = Crystal.Read(r, materials);

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

    #endregion

    #region Chunks

    #region 0x000 chunk (one layer only)

    /// <summary>
    /// CPlugCrystal 0x000 chunk (one layer only)
    /// </summary>
    [Chunk(0x09003000, "one layer only")]
    public class Chunk09003000 : Chunk<CPlugCrystal>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugCrystal n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.Layers = new[]
            {
                new GeometryLayer(Crystal.Read(r, n.materials))
                {
                    LayerID = "Layer0",
                    LayerName = "Geometry",
                    IsEnabled = true
                }
            };
        }
    }

    #endregion

    #region 0x003 chunk (materials)

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

            n.Layers = r.ReadArray<Layer>(layerCount, r =>
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
                    ELayerType.Geometry => ReadGeometryLayer(r, n.materials, layerId, layerName, isEnabled, typeVersion),
                    ELayerType.Trigger => ReadTriggerLayer(r, n.materials, layerId, layerName, isEnabled, typeVersion),
                    ELayerType.Cubes => ReadCubesLayer(r, typeVersion),
                    _ => ReadMaskLayer(r, type, layerId, layerName)
                };
            });
        }

        private static Layer ReadCubesLayer(GameBoxReader r, int version)
        {
            throw new NotSupportedException("Cubes/Voxel layer is not supported");

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

        public int Version { get => version; set => version = value; }

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
        public Vertex[] Vertices { get; set; }
        public Group Group { get; set; }
        public CPlugMaterialUserInst? Material { get; set; }

        public Face(Vertex[] vertices, Group group, CPlugMaterialUserInst? material = null)
        {
            Vertices = vertices;
            Group = group;
            Material = material;
        }

        public override string ToString()
        {
            return string.Join(" ", Vertices);
        }
    }

    public readonly record struct Vertex(Vec3 Position, Vec2 UV);

    public class LayerMask
    {
        public string? LayerId { get; set; }
        public int GroupIndex { get; set; }
    }

    #endregion
}
