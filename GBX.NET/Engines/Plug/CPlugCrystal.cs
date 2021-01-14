using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Xml.Schema;

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
            Cubes,
            Trigger,
            SpawnPosition
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
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            using (var w = new StreamWriter(stream))
            {
                w.WriteLine("# Mesh data extracted from GBX with GBX.NET");
                w.WriteLine();

                foreach (GeometryLayer layer in Layers)
                {
                    w.WriteLine($"o {layer.LayerName}");

                    w.WriteLine();

                    foreach (var vertex in layer.Vertices)
                    {
                        w.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
                    }

                    w.WriteLine();

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

                n.materials = r.ReadArray(i =>
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

                n.Layers = r.ReadArray(i =>
                {
                    var type = (ELayerType)r.ReadInt32();
                    var u01 = r.ReadInt32(); // 2
                    var u02 = r.ReadInt32();
                    var layerId = r.ReadId(); // Layer0
                    var layerName = r.ReadString();
                    var u03 = r.ReadInt32(); // 1
                    var u04 = r.ReadInt32(); // 1
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

                    var groups = r.ReadArray(j => new Group()
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
                        vertices = r.ReadArray(j => r.ReadVec3());
                        edges = r.ReadArray(j => r.ReadInt2());

                        faces = r.ReadArray(j =>
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

                    var u18 = r.ReadInt32(); // 1
                    var u19 = r.ReadInt32(); // 1

                    return new GeometryLayer()
                    {
                        LayerType = type,
                        LayerID = layerId,
                        LayerName = layerName,
                        Vertices = vertices,
                        Edges = edges,
                        Faces = faces,
                        Groups = groups,
                        Unknown = new object[]
                        {
                            u01, u02, u03, u04, u05, u06, u07, u08, u09, u10, u11, u12, u13, u14,
                            u15, u16, numUVs, numEdges, numVerts, empty, u17, counter, u18, u19
                        }
                    };
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
            public ELayerType LayerType { get; set; }
        }

        public class GeometryLayer : Layer
        {
            public string LayerID { get; set; }
            public string LayerName { get; set; }
            public Vec3[] Vertices { get; set; }
            public Int2[] Edges { get; set; }
            public Face[] Faces { get; set; }
            public Group[] Groups { get; set; }
            public object[] Unknown { get; set; }
        }

        public class Group
        {
            public string Name { get; set; }
            public int U01 { get; set; }
            public int U02 { get; set; }
            public int U03 { get; set; }
            public int U04 { get; set; }
            public int[] U05 { get; set; }
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

        #endregion
    }
}
