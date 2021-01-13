using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
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

                    var u15 = r.ReadInt32();
                    var verticies = r.ReadArray(j => r.ReadVec3());
                    var indicies = r.ReadArray(j => r.ReadInt2());

                    var uvmaps = r.ReadArray(j =>
                    {
                        var uvVerticies = r.ReadInt32();
                        var inds = r.ReadArray<int>(uvVerticies);
                        var xy = new Vec2[uvVerticies];
                        for (var k = 0; k < uvVerticies; k++)
                            xy[k] = r.ReadVec2();
                        var materialIndex = r.ReadInt32();
                        var groupIndex = r.ReadInt32();

                        return new UVMap()
                        {
                            VertCount = uvVerticies,
                            Inds = inds,
                            XY = xy,
                            Material = n.Materials[materialIndex],
                            Group = groups[groupIndex]
                        };
                    });

                    var u16 = r.ReadInt32();
                    var numUVs = r.ReadInt32();
                    var numIndicies = r.ReadInt32();
                    var numVerts = r.ReadInt32();
                    var empty = r.ReadArray<int>(numUVs + numIndicies + numVerts);

                    if (numUVs + numIndicies + numVerts == 0)
                    {
                        numUVs = r.ReadInt32();
                        numIndicies = r.ReadInt32();
                        numVerts = r.ReadInt32();

                        empty = r.ReadArray<int>(numUVs + numIndicies + numVerts);
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
                        Verticies = verticies,
                        Indicies = indicies,
                        UVs = uvmaps,
                        Groups = groups,
                        Unknown = new object[]
                        {
                            u01, u02, u03, u04, u05, u06, u07, u08, u09, u10, u11, u12, u13, u14,
                            u15, u16, numUVs, numIndicies, numVerts, empty, u17, counter, u18, u19
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
            public Vec3[] Verticies { get; set; }
            public Int2[] Indicies { get; set; }
            public UVMap[] UVs { get; set; }
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

        public class UVMap
        {
            public int VertCount { get; set; }
            public int[] Inds { get; set; }
            public Vec2[] XY { get; set; }
            public CPlugMaterialUserInst Material { get; set; }
            public object Group { get; set; }

            public override string ToString()
            {
                return $"({string.Join(" ", Inds)}) ({string.Join(" ", XY)})";
            }
        }

        #endregion
    }
}
