using System.Diagnostics;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900F000</remarks>
[Node(0x0900F000), WritingNotSupported]
public class CPlugSurfaceGeom : CPlugSurface
{
    internal CPlugSurfaceGeom()
    {

    }

    #region 0x002 chunk

    /// <summary>
    /// CPlugSurfaceGeom 0x002 chunk
    /// </summary>
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
            U02 = rw.Array<Vec3>();

            U03 = rw.Array(null, r =>
            {
                return (r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadSingle(),
                    r.ReadInt3(),
                    r.ReadUInt16(),
                    r.ReadByte(),
                    r.ReadByte());
            }, (x, w) => { });

            U04 = rw.Int32();

            U05 = rw.Array(null, r =>
            {
                return (r.ReadInt32(),
                    r.ReadVec3(),
                    r.ReadVec3(),
                    r.ReadInt32());
            }, (x, w) => { });
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CPlugSurfaceGeom 0x004 chunk
    /// </summary>
    [Chunk(0x0900F004)]
    public class Chunk0900F004 : Chunk<CPlugSurfaceGeom>
    {
        public override void Read(CPlugSurfaceGeom n, GameBoxReader r)
        {
            var u01 = r.ReadId();
            var u02 = r.ReadBox();
            var u03 = r.ReadInt32();

            if (r.BaseStream is IXorTrickStream cryptedStream)
            {
                cryptedStream.InitializeXorTrick(BitConverter.GetBytes(u02.X - u02.X2), 0, 4);
            }

            switch (u03)
            {
                case 7:
                    // SurfMesh
                    var u04 = r.ReadInt32();

                    switch (u04)
                    {
                        case 1:
                        case 2:
                        case 3:
                            // Array of Vec3
                            r.ReadArray<Vec3>();
                            // Array of STriangle
                            r.ReadArray(r => r.ReadBytes(32));

                            // SMeshOctreeCell (GmOctree)
                            var type = r.ReadInt32();

                            switch (type)
                            {
                                case 1:
                                    uint version = r.ReadUInt32();
                                    uint size = r.ReadUInt32();
                                    break;
                                case 3:
                                    r.ReadArray(r => r.ReadBytes(32));
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }

            r.ReadUInt16();
        }
    }

    #endregion
}