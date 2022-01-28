using System.Diagnostics;

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

    #region 0x004 chunk

    /// <summary>
    /// CPlugSurfaceGeom 0x004 chunk
    /// </summary>
    [Chunk(0x0900F004)]
    public class Chunk0900F004 : Chunk<CPlugSurfaceGeom>
    {
        public override void ReadWrite(CPlugSurfaceGeom n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Id();
            var u02 = rw.Box();
            var u03 = rw.Int32();

            switch (u03)
            {
                case 7:
                    // SurfMesh
                    var u04 = rw.Int32();

                    switch (u04)
                    {
                        case 1:
                        case 2:

                            break;
                        case 3:
                            rw.UntilFacade(Unknown);
                            // Array of Vec3
                            //var ddshfsah = rw.Reader.ReadSpan<Vec3>();
                            // Array of STriangle
                            /*var ddshh = rw.Reader.ReadArray(r => (
                                r.ReadVec4(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32()
                            ));*/

                            // SMeshOctreeCell (GmOctree)
                            //var u05 = rw.Int32();

                            /*switch (u05)
                            {
                                case 1:
                                case 3:
                                    var ddshh2 = rw.Reader.ReadArray(r =>
                                        (r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32())
                                    );
                                    break;
                            }*/
                            
                            break;
                    }

                    break;
            }

            /*var count = rw.Int32() / 3 / 4;

            var niceVecArray = rw.Reader.ReadArray(count, r => r.ReadVec3());*/
        }
    }

    #endregion
}
