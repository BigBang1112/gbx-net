using System.Globalization;

namespace GBX.NET.Engines.Plug;

/// <summary>
/// CPlugCrystal (0x09003000)
/// </summary>
/// <remarks>A custom mesh or model.</remarks>
[Node(0x09003000)]
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

    private CPlugMaterialUserInst?[] materials;

    #endregion

    #region Properties

    [NodeMember]
    public CPlugMaterialUserInst?[] Materials
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
        materials = null!;
        Layers = null!;
    }

    #endregion

    #region Methods

    public void ToOBJ(Stream stream)
    {
        var previousCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        using (var w = new StreamWriter(stream))
        {
            w.WriteLine("# Mesh data extracted from GBX with GBX.NET");
            w.WriteLine();

            foreach (GeometryLayer layer in Layers.Where(x => x is GeometryLayer && x.IsEnabled))
            {
                w.WriteLine($"o {layer.LayerName}");

                w.WriteLine();

                if (layer.Vertices is not null)
                {
                    foreach (var vertex in layer.Vertices)
                    {
                        w.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
                    }
                }

                w.WriteLine();

                /*var uvCounter = 0;
                foreach (var face in layer.Faces)
                {
                    foreach (var uv in face.UV)
                    {
                        uvCounter++;
                        w.WriteLine($"vt {(uv.X + 3) / 6} {(uv.Y + 3) / 6}"); // doesnt properly work
                    }

                    w.WriteLine($"f {string.Join(" ", face.Indices.Select((x, i) => $"{x + 1}/{uvCounter - (face.UV.Length - i) + 1}"))}");
                }*/

                if (layer.Faces is null)
                    continue;

                foreach (var group in layer.Faces.GroupBy(x => x.Group))
                {
                    w.WriteLine();

                    //w.WriteLine($"o {layer.LayerName}");

                    var uvCounter = 0;

                    foreach (var face in group)
                    {
                        if (face.UV is null) continue;

                        foreach (var uv in face.UV)
                        {
                            uvCounter++;
                            w.WriteLine($"vt {(uv.X + 3) / 6} {(uv.Y + 3) / 6}"); // doesnt properly work
                        }

                        w.WriteLine($"f {string.Join(" ", face.Indices.Select((x, i) => $"{x + 1}/{uvCounter - (face.UV.Length - i) + 1}"))}");
                    }
                }
            }
        }

        Thread.CurrentThread.CurrentCulture = previousCulture;
    }

    #endregion

    #region Chunks

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

                    if (version >= 2) // Just an assume
                    {
                        // u04 = r.ReadBoolean();
                    }
                }

                var typeVersion = r.ReadInt32(); // not part of SLayer::ArchiveWithoutTyp

                return type switch
                {
                    ELayerType.Geometry or ELayerType.Trigger => ReadGeometryOrTriggerLayer(r, n, type, u02, layerId, layerName, isEnabled, typeVersion),
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

            var mask_u01 = r.ReadInt32();

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
                            Unknown = new object[] { mask_u01 }
                        };
                    }

                case ELayerType.SpawnPosition:
                    {
                        var position = r.ReadVec3();
                        var horizontalAngle = r.ReadSingle();
                        var verticalAngle = r.ReadSingle();

                        return new SpawnPositionLayer()
                        {
                            LayerID = layerId,
                            LayerName = layerName,
                            Mask = mask,
                            Position = position,
                            HorizontalAngle = horizontalAngle,
                            VerticalAngle = verticalAngle,
                            Unknown = new object[] { mask_u01 }
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
                            Unknown = new object[] { mask_u01 }
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
                            Independently = independently
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
                            Independently = independently
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
                            U01 = chaos_u01,
                            MaxDistance = maxDistance,
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
                            Subdivisions = subdivisions
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
                            Intensity = intensity
                        };
                    }

                default:
                    throw new NotSupportedException($"Unknown or unsupported layer. ({type})");
            }
        }

        private static Layer ReadGeometryOrTriggerLayer(GameBoxReader r, CPlugCrystal n, ELayerType type, bool u02, string layerId, string layerName, bool isEnabled, int typeVersion)
        {
            var u05 = r.ReadInt32(); // 32
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
                U01 = r.ReadInt32(),
                U02 = r.ReadInt32(), // maybe bool
                U03 = r.ReadInt32(),
                Name = r.ReadString(),
                U04 = r.ReadInt32(),
                U05 = r.ReadArray<int>()
            });

            Vec3[]? vertices = null;
            Int2[]? edges = null;
            Face[]? faces = null;

            var u15 = r.ReadBoolean();

            if (u15)
            {
                vertices = r.ReadArray<Vec3>();
                edges = r.ReadArray<Int2>();

                faces = r.ReadArray(r =>
                {
                    var uvVertices = r.ReadInt32();
                    var inds = r.ReadArray<int>(uvVertices);
                    var uv = r.ReadArray<Vec2>(uvVertices);
                    var materialIndex = r.ReadInt32();
                    var groupIndex = r.ReadInt32();

                    return new Face()
                    {
                        VertCount = uvVertices,
                        Indices = inds,
                        UV = uv,
                        Material = n.Materials[materialIndex],
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

                var wtf = r.ReadArray<int>(15007);

                var verts = r.ReadArray<Vec3>();

                throw new NotSupportedException("Unsupported crystal.");
            }

            var u16 = r.ReadInt32();
            var numUVs = r.ReadInt32();
            var numEdges = r.ReadInt32();
            var numVerts = r.ReadInt32();
            var empty = r.ReadArray<int>(numUVs + numEdges + numVerts);

            if (numUVs + numEdges + numVerts == 0)
            {
                numUVs = r.ReadInt32();
                numEdges = r.ReadInt32();
                numVerts = r.ReadInt32();

                empty = r.ReadArray<int>(numUVs + numEdges + numVerts);
            }

            var u17 = r.ReadInt32();
            var counter = r.ReadArray<int>(); // each element is related to each group

            var isVisible = false;
            var collidable = true;

            if (type == ELayerType.Geometry)
            {
                isVisible = r.ReadBoolean();
                collidable = r.ReadBoolean();
            }
            else
            {

            }

            return new GeometryLayer()
            {
                LayerID = layerId,
                LayerName = layerName,
                Vertices = vertices,
                Edges = edges,
                Faces = faces,
                Groups = groups,
                IsEnabled = isEnabled,
                IsVisible = isVisible,
                Collidable = collidable,
                Unknown = new object[]
                {
                    u02, u05, u06, u07, u08, u09, u10, u11, u12, u13, u14,
                    u15, u16, numUVs, numEdges, numVerts, empty, u17, counter
                }
            };
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

        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Array(ref U01,
                (i, r) => r.ReadVec2(),
                (x, w) => w.Write(x));
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

        public int U01;
        public int U02;
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
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Array(ref U05);
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
        public object[]? Unknown { get; set; }

        public override string ToString() => LayerName ?? string.Empty;
    }

    public class GeometryLayer : Layer
    {
        public Vec3[]? Vertices { get; set; }
        public Int2[]? Edges { get; set; }
        public Face[]? Faces { get; set; }
        public Group[]? Groups { get; set; }
        public bool Collidable { get; set; }
        public bool IsVisible { get; set; }
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
    }

    public class ChaosLayer : Layer
    {
        public LayerMask[]? Mask { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
        public float U01 { get; set; }
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

    #endregion
}
