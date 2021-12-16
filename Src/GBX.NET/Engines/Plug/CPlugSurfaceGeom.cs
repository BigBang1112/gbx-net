using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Plug;

[Node(0x0900F000), WritingNotSupported]
public class CPlugSurfaceGeom : CPlugSurface
{
    protected CPlugSurfaceGeom()
    {

    }

    [Chunk(0x0900F002)]
    public class Chunk0900F002 : Chunk<CPlugSurfaceGeom>
    {
        public int U01;
        public Vec3[]? U02;
        public object? U03;
        public int U04;
        public object? U05;

        public override void ReadWrite(CPlugSurfaceGeom n, GameBoxReaderWriter rw)
        {
            U01 = rw.Int32();
            U02 = rw.ArrayVec3();

            U03 = rw.Array(null, r =>
            {
                return (r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadInt32(),
                    r.ReadInt32(),
                    r.ReadInt32(),
                    r.ReadInt32());
            }, (x, w) => { });

            U04 = rw.Int32();

            U05 = rw.Array(null, r =>
            {
                return (r.ReadInt32(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadInt32());
            }, (x, w) => { });
        }
    }
}
