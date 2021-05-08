using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace GBX.NET.Engines.Plug
{
    [Node(0x09003000)]
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

        private CPlugMaterialUserInst[] materials;

        #endregion

        #region Properties

        [NodeMember]
        public CPlugMaterialUserInst[] Materials
        {
            get => materials;
            set => materials = value;
        }

        [NodeMember]
        public Layer[] Layers { get; set; }

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

                    foreach (var vertex in layer.Vertices)
                    {
                        w.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
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

                    foreach (var group in layer.Faces.GroupBy(x => x.Group))
                    {
                        w.WriteLine();

                        //w.WriteLine($"o {layer.LayerName}");

                        var uvCounter = 0;
                        foreach (var face in group)
                        {
                            foreach(var uv in face.UV)
                            {
                                uvCounter++;
                                w.WriteLine($"vt {(uv.X+3)/6} {(uv.Y+3)/6}"); // doesnt properly work
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
        public class Chunk09003003 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }

            public override void Read(CPlugCrystal n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.materials = r.ReadArray(() =>
                {
                    var name = r.ReadString();
                    if (name.Length == 0)  // If the material file exists (name != ""), it references the file instead
                    {
                        var material = r.ReadNodeRef<CPlugMaterialUserInst>();
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
        public class Chunk09003004 : SkippableChunk<CPlugCrystal>
        {
            
        }

        #endregion

        #region 0x005 chunk (layers)

        /// <summary>
        /// CPlugCrystal 0x005 chunk (layers)
        /// </summary>
        [Chunk(0x09003005, "layers")]
        public class Chunk09003005 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }

            public override void Read(CPlugCrystal n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.Layers = r.ReadArray<Layer>(() =>
                {
                    var type = (ELayerType)r.ReadInt32();
                    var u01 = r.ReadInt32(); // 2
                    var u02 = r.ReadInt32();
                    var layerId = r.ReadId(); // Layer0
                    var layerName = r.ReadString();
                    var isEnabled = r.ReadBoolean();

                    var u04 = r.ReadInt32(); // 1

                    if (type == ELayerType.Geometry || type == ELayerType.Trigger)
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
                        var u14 = r.ReadInt32(); // 0

                        var groups = r.ReadArray(() => new Group()
                        {
                            U01 = r.ReadInt32(),
                            U02 = r.ReadInt32(),
                            U03 = r.ReadInt32(),
                            Name = r.ReadString(),
                            U04 = r.ReadInt32(),
                            U05 = r.ReadArray<int>()
                        });

                        Vec3[] vertices = null;
                        Int2[] edges = null;
                        Face[] faces = null;

                        var u15 = r.ReadInt32();
                        if (u15 == 1)
                        {
                            vertices = r.ReadArray(() => r.ReadVec3());
                            edges = r.ReadArray(() => r.ReadInt2());

                            faces = r.ReadArray(() =>
                            {
                                var uvVertices = r.ReadInt32();
                                var inds = r.ReadArray<int>(uvVertices);
                                var uv = new Vec2[uvVertices];
                                for (var k = 0; k < uvVertices; k++)
                                    uv[k] = r.ReadVec2();
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
                        var numGroups2 = r.ReadInt32();
                        var counter = r.ReadArray<int>(numGroups2);

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
                                u01, u02, u04, u05, u06, u07, u08, u09, u10, u11, u12, u13, u14,
                                u15, u16, numUVs, numEdges, numVerts, empty, u17, counter
                            }
                        };
                    }
                    else if (type == ELayerType.Cubes)
                    {
                        throw new NotSupportedException("Cubes layer is not currently supported.");
                    }
                    else
                    {
                        var mask = r.ReadArray(() =>
                        {
                            return new LayerMask()
                            {
                                GroupIndex = r.ReadInt32(),
                                LayerId = r.ReadId()
                            };
                        });

                        var mask_u01 = r.ReadInt32();

                        if (type == ELayerType.Scale)
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
                        else if (type == ELayerType.SpawnPosition)
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
                        else if (type == ELayerType.Translation)
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
                        else if (type == ELayerType.Rotation)
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
                        else if (type == ELayerType.Mirror)
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
                        else if(type == ELayerType.Deformation)
                        {
                            // TODO: how deformation works
                            throw new NotSupportedException("Deformation layer is not currently supported.");
                        }
                        else if (type == ELayerType.Chaos)
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
                        else if (type == ELayerType.Subdivide)
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
                        else if (type == ELayerType.Smooth)
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
                        else
                            throw new NotSupportedException($"Unknown or unsupported layer. ({type})");
                    }
                });
            }
        }

        #endregion

        #region 0x006 chunk

        /// <summary>
        /// CPlugCrystal 0x006 chunk
        /// </summary>
        [Chunk(0x09003006)]
        public class Chunk09003006 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }
            public Vec2[] Vectors { get; set; }

            public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Vectors = rw.Array(Vectors, (i, r) => r.ReadVec2(), (x, w) => w.Write(x));
            }
        }

        #endregion

        #region 0x007 chunk

        /// <summary>
        /// CPlugCrystal 0x007 chunk
        /// </summary>
        [Chunk(0x09003007)]
        public class Chunk09003007 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }
            public int[] Numbers { get; set; }

            public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                Numbers = rw.Array(Numbers, (i, r) => r.ReadInt32(), (x, w) => w.Write(x));
            }
        }

        #endregion

        #endregion

        #region Other classes

        public abstract class Layer
        {
            public string LayerID { get; set; }
            public string LayerName { get; set; }
            public bool IsEnabled { get; set; }
            public object[] Unknown { get; set; }

            public override string ToString() => LayerName;
        }

        public class GeometryLayer : Layer
        {
            public Vec3[] Vertices { get; set; }
            public Int2[] Edges { get; set; }
            public Face[] Faces { get; set; }
            public Group[] Groups { get; set; }
            public bool Collidable { get; set; }
            public bool IsVisible { get; set; }
        }

        public class TranslationLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public Vec3 Translation { get; set; }
        }

        public class ScaleLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public Vec3 Scale { get; set; }
            public bool Independently { get; set; }
        }

        public class RotationLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public float Rotation { get; set; } // in radians
            public EAxis Axis { get; set; }
            public bool Independently { get; set; }
        }

        public class MirrorLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public float Distance { get; set; }
            public EAxis Axis { get; set; }
            public bool Independently { get; set; }
        }

        public class SpawnPositionLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public Vec3 Position { get; set; }
            public float HorizontalAngle { get; set; }
            public float VerticalAngle { get; set; }
        }

        public class ChaosLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public float MinDistance { get; set; }
            public float MaxDistance { get; set; }
            public float U01 { get; set; }
        }

        public class SubdivideLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public int Subdivisions { get; set; }
        }

        public class SmoothLayer : Layer
        {
            public LayerMask[] Mask { get; set; }
            public float Intensity { get; set; }
        }

        public class Group
        {
            public string Name { get; set; }
            public int U01 { get; set; }
            public int U02 { get; set; }
            public int U03 { get; set; }
            public int U04 { get; set; }
            public int[] U05 { get; set; }

            public override string ToString() => Name;
        }

        public class Face
        {
            public int VertCount { get; set; }
            public int[] Indices { get; set; }
            public Vec2[] UV { get; set; }
            public CPlugMaterialUserInst Material { get; set; }
            public Group Group { get; set; }

            public override string ToString()
            {
                return $"({string.Join(" ", Indices)}) ({string.Join(" ", UV)})";
            }
        }

        public class LayerMask
        {
            public string LayerId { get; set; }
            public int GroupIndex { get; set; }
        }

        #endregion
    }
}
