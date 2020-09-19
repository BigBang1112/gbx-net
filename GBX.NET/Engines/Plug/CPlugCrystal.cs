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
        public CPlugMaterialUserInst[] Materials { get; set; }
        public Layer[] Layers { get; set; }

        [Chunk(0x09003003)]
        public class Chunk09003003 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }

            public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Materials = rw.Array(n.Materials, i =>
                {
                    var name = rw.Reader.ReadString();
                    if(name == "") // If the material file exists (name != ""), it references the file instead
                        return rw.Reader.ReadNodeRef<CPlugMaterialUserInst>();
                    return null;
                },
                x =>
                {
                    rw.Writer.Write(0); //
                    rw.Writer.Write(x);
                });
            }
        }

        [Chunk(0x09003004)]
        public class Chunk09003004 : SkippableChunk<CPlugCrystal>
        {
            
        }

        [Chunk(0x09003005)]
        public class Chunk09003005 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }

            public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Layers = rw.Array(n.Layers, i =>
                {
                    var uA = rw.Reader.ReadInt32();
                    var uB = rw.Reader.ReadInt32();
                    var uC = rw.Reader.ReadInt32();
                    var layerId = rw.Reader.ReadLookbackString();
                    var layerName = rw.Reader.ReadString();
                    var uD = rw.Reader.ReadInt32();
                    var uE = rw.Reader.ReadInt32();
                    var uF = rw.Reader.ReadInt32();
                    var uG = rw.Reader.ReadInt32();
                    var uH = rw.Reader.ReadInt32();
                    var uI = rw.Reader.ReadInt32();
                    var uJ = rw.Reader.ReadSingle();
                    var uK = rw.Reader.ReadInt32();
                    var uL = rw.Reader.ReadSingle();
                    var uM = rw.Reader.ReadInt32();
                    var uN = rw.Reader.ReadSingle();
                    var uO = rw.Reader.ReadInt32();

                    var num = rw.Reader.ReadInt32();
                    var unknownArray = new object[num];

                    for (var j = 0; j < num; j++)
                    {
                        var unknownValues = new object[6];

                        unknownValues[0] = rw.Reader.ReadInt32();
                        unknownValues[1] = rw.Reader.ReadInt32();
                        unknownValues[2] = rw.Reader.ReadInt32();
                        unknownValues[3] = rw.Reader.ReadString();
                        unknownValues[4] = rw.Reader.ReadInt32();

                        var count = rw.Reader.ReadInt32();
                        unknownValues[5] = new int[count];

                        for (var k = 0; k < count; k++)
                            ((int[])unknownValues[5])[k] = rw.Reader.ReadInt32();

                        unknownArray[j] = unknownValues;
                    }

                    var uP = rw.Reader.ReadInt32();
                    var verticies = rw.Reader.ReadArray(j => rw.Reader.ReadVec3());
                    var indicies = rw.Reader.ReadArray(j => rw.Reader.ReadInt2());

                    var uvmaps = rw.Reader.ReadArray(j =>
                    {
                        var uvVerticies = rw.Reader.ReadInt32();
                        var inds = rw.Reader.ReadArray<int>(uvVerticies);
                        var xy = new Vec2[uvVerticies];
                        for (var k = 0; k < uvVerticies; k++)
                            xy[k] = rw.Reader.ReadVec2();
                        var one = rw.Reader.ReadInt32();
                        var two = rw.Reader.ReadInt32();

                        return new UVMap()
                        {
                            VertCount = uvVerticies,
                            Inds = inds,
                            XY = xy,
                            Unknown1 = one,
                            Unknown2 = two
                        };
                    });

                    var uQ = rw.Reader.ReadInt32();
                    var numUVs = rw.Reader.ReadInt32();
                    var numIndicies = rw.Reader.ReadInt32();
                    var numVerts = rw.Reader.ReadInt32();
                    var empty = rw.Reader.ReadArray<int>(numUVs + numIndicies + numVerts);

                    if (numUVs + numIndicies + numVerts == 0)
                    {
                        numUVs = rw.Reader.ReadInt32();
                        numIndicies = rw.Reader.ReadInt32();
                        numVerts = rw.Reader.ReadInt32();

                        empty = rw.Reader.ReadArray<int>(numUVs + numIndicies + numVerts);
                    }

                    var uR = rw.Reader.ReadInt32();
                    var unknownNum = rw.Reader.ReadInt32();
                    var counter = rw.Reader.ReadArray<int>(unknownNum);

                    var uS = rw.Reader.ReadInt32();
                    var uT = rw.Reader.ReadInt32();

                    return new Layer()
                    {
                        LayerID = layerId,
                        LayerName = layerName,
                        Verticies = verticies,
                        Indicies = indicies,
                        UVs = uvmaps,
                        Unknown = new object[]
                        {
                            uA, uB, uC, uD, uE, uF, uG, uH, uI, uJ, uK, uL, uM, uN, uO,
                            unknownArray, uP, uQ, numUVs, numIndicies, numVerts, empty, uR, counter, uS, uT
                        }
                    };
                }, x => { });
            }
        }

        [Chunk(0x09003006)]
        public class Chunk09003006 : Chunk<CPlugCrystal>
        {
            public int Version { get; set; }
            public Vec2[] Vectors { get; set; }

            public override void ReadWrite(CPlugCrystal n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Vectors = rw.Array(Vectors, i => rw.Reader.ReadVec2(), x => rw.Writer.Write(x));
            }
        }

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
                Numbers = rw.Array(Numbers, i => rw.Reader.ReadInt32(), x => rw.Writer.Write(x));
            }
        }

        public class Layer
        {
            public string LayerID { get; set; }
            public string LayerName { get; set; }
            public Vec3[] Verticies { get; set; }
            public Int2[] Indicies { get; set; }
            public UVMap[] UVs { get; set; }
            public object[] Unknown { get; set; }
        }

        public class UVMap
        {
            public int VertCount { get; set; }
            public int[] Inds { get; set; }
            public Vec2[] XY { get; set; }
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }

            public override string ToString()
            {
                return $"({string.Join(" ", Inds)}) ({string.Join(" ", XY)})";
            }
        }
    }
}
