using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    [Node(0x03029000)]
    public class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock
    {
        #region Fields

        private List<Key> keys = new List<Key>();
        private Vec4[] vertices;
        private Int3[] triangles;

        #endregion

        #region Properties

        [NodeMember]
        public List<Key> Keys
        {
            get => keys;
            set => keys = value;
        }

        [NodeMember]
        public Vec4[] Vertices
        {
            get => vertices;
            set
            {
                if (vertices == null || value.Length != vertices.Length)
                {
                    if (vertices == null)
                        vertices = value;

                    foreach (var key in keys)
                    {
                        var positions = key.Positions;
                        Array.Resize(ref positions, value.Length);
                        key.Positions = positions;
                    }

                    RemoveTrianglesOutOfRange();
                }

                vertices = value;
            }
        }

        [NodeMember]
        public Int3[] Triangles
        {
            get => triangles;
            set
            {
                if (vertices == null)
                    return;

                foreach (var int3 in value)
                {
                    if (int3.X >= vertices.Length
                    || int3.Y >= vertices.Length
                    || int3.Z >= vertices.Length)
                        throw new Exception($"Index in {int3} is not available in vertices.");
                }

                triangles = value;
            }
        }

        #endregion

        #region Methods

        private void RemoveTrianglesOutOfRange()
        {
            if (triangles == null) return;

            var trianglesToRemove = new List<Int3>();

            foreach (var triangle in triangles)
            {
                if (triangle.X >= vertices.Length
                || triangle.Y >= vertices.Length
                || triangle.Z >= vertices.Length)
                {
                    trianglesToRemove.Add(triangle);
                }
            }

            triangles = triangles.Where(x => !trianglesToRemove.Contains(x)).ToArray();
        }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x03029001)]
        public class Chunk03029001 : Chunk<CGameCtnMediaBlockTriangles>
        {
            public int U01 { get; set; }
            public int U02 { get; set; }
            public int U03 { get; set; }
            public float U04 { get; set; }
            public int U05 { get; set; }
            public long U06 { get; set; }

            public override void Read(CGameCtnMediaBlockTriangles n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.keys = r.ReadArray(r1 => new Key(n)
                {
                    Time = r1.ReadSingle()
                }).ToList();
                
                var numKeys = r.ReadInt32();
                var numVerts = r.ReadInt32();

                for (var i = 0; i < numKeys; i++)
                {
                    n.keys[i].Positions = new Vec3[numVerts];

                    for (var j = 0; j < numVerts; j++)
                    {
                        n.keys[i].Positions[j] = r.ReadVec3();
                    }
                }

                n.vertices = r.ReadArray(r1 => r1.ReadVec4());
                n.triangles = r.ReadArray(r1 => r1.ReadInt3());

                U01 = r.ReadInt32();
                U02 = r.ReadInt32();
                U03 = r.ReadInt32();
                U04 = r.ReadSingle();
                U05 = r.ReadInt32();
                U06 = r.ReadInt64();
            }

            public override void Write(CGameCtnMediaBlockTriangles n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.keys, (x, w1) => w1.Write(x.Time));
                w.Write(n.keys.Count);
                w.Write(n.vertices.Length);

                foreach (var key in n.keys)
                {
                    foreach (var pos in key.Positions)
                    {
                        w.Write(pos);
                    }
                }

                w.Write(n.vertices, (x, w1) => w1.Write(x));
                w.Write(n.triangles, (x, w1) => w1.Write(x));

                w.Write(U01);
                w.Write(U02);
                w.Write(U03);
                w.Write(U04);
                w.Write(U05);
                w.Write(U06);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            private readonly CGameCtnMediaBlockTriangles node;

            private Vec3[] positions;

            public Vec3[] Positions
            {
                get => positions;
                set
                {
                    if (value.Length != positions.Length)
                    {
                        Array.Resize(ref node.vertices, value.Length);

                        foreach(var k in node.keys)
                            if(k != this)
                                Array.Resize(ref k.positions, value.Length);

                        node.RemoveTrianglesOutOfRange();
                    }

                    positions = value;
                }
            }

            public Key(CGameCtnMediaBlockTriangles node)
            {
                this.node = node;
                positions = new Vec3[node.vertices?.Length ?? 0];
            }
        }

        #endregion
    }
}
