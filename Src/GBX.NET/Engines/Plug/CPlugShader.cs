namespace GBX.NET.Engines.Plug;

/// <summary>
/// Shader.
/// </summary>
/// <remarks>ID: 0x09002000</remarks>
[Node(0x09002000)]
public abstract class CPlugShader : CPlug
{
    internal CPlugShader()
    {

    }

    #region 0x007 chunk

    /// <summary>
    /// CPlugShader 0x007 chunk
    /// </summary>
    [Chunk(0x09002007)]
    public class Chunk09002007 : Chunk<CPlugShader>
    {
        public Node? U01;
        public GameBoxRefTable.File? U01File;
        public CMwNod?[]? U02;
        public Node? U03;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01, ref U01File);
            rw.ArrayNode<CMwNod>(ref U02); // CPlugShaderPass array
            rw.NodeRef(ref U03);
        }
    }

    #endregion

    /// <summary>
    /// CPlugShader 0x00E chunk
    /// </summary>
    [Chunk(0x0900200E)]
    public class Chunk0900200E : Chunk09002007
    {
        public CMwNod?[]? U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.ArrayNode<CMwNod>(ref U04);
        }
    }

    /// <summary>
    /// CPlugShader 0x010 chunk
    /// </summary>
    [Chunk(0x09002010)]
    public class Chunk09002010 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }

    /// <summary>
    /// CPlugShader 0x014 chunk
    /// </summary>
    [Chunk(0x09002014)]
    public class Chunk09002014 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }

    /// <summary>
    /// CPlugShader 0x015 chunk
    /// </summary>
    [Chunk(0x09002015)]
    public class Chunk09002015 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }

    /// <summary>
    /// CPlugShader 0x016 chunk
    /// </summary>
    [Chunk(0x09002016)]
    public class Chunk09002016 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }

    #region 0x018 chunk

    /// <summary>
    /// CPlugShader 0x018 chunk
    /// </summary>
    [Chunk(0x09002018)]
    public class Chunk09002018 : Chunk<CPlugShader>
    {
        public uint U01;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x01B chunk

    /// <summary>
    /// CPlugShader 0x01B chunk
    /// </summary>
    [Chunk(0x0900201B)]
    public class Chunk0900201B : Chunk<CPlugShader>, IVersionable
    {
        private int version;
        
        public float U01;
        public float U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x01C chunk

    /// <summary>
    /// CPlugShader 0x01C chunk
    /// </summary>
    [Chunk(0x0900201C)]
    public class Chunk0900201C : Chunk<CPlugShader>, IVersionable
    {
        private int version;
        
        public Node? U01;
        public Node? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01);
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x01D chunk

    /// <summary>
    /// CPlugShader 0x01D chunk
    /// </summary>
    [Chunk(0x0900201D)]
    public class Chunk0900201D : Chunk<CPlugShader>
    {
        public uint U01;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x01E chunk

    /// <summary>
    /// CPlugShader 0x01E chunk
    /// </summary>
    [Chunk(0x0900201E)]
    public class Chunk0900201E : Chunk<CPlugShader>, IVersionable
    {
        private int version;

        public uint U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.UInt32(ref U01);
        }
    }

    #endregion

    #region 0x01F chunk

    /// <summary>
    /// CPlugShader 0x01F chunk
    /// </summary>
    [Chunk(0x0900201F)]
    public class Chunk0900201F : Chunk<CPlugShader>, IVersionable
    {
        private int version;

        public (Node?, bool?, Node?)[]? U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Array<(Node?, bool?, Node?)>(ref U01, r =>
            {
                var u01 = r.ReadNodeRef();
                var u02 = default(bool?);
                var u03 = default(Node);

                if (u01 is null)
                {
                    u02 = r.ReadBoolean();

                    if (u02.GetValueOrDefault())
                    {
                        u03 = r.ReadNodeRef();
                    }
                }

                return (u01, u02, u03);
            }, (x, w) =>
            {
                w.Write(x.Item1);

                if (x.Item2 is null)
                {
                    w.Write(x.Item2.GetValueOrDefault());

                    if (x.Item2.GetValueOrDefault())
                    {
                        w.Write(x.Item3);
                    }
                }
            });
        }
    }

    #endregion

    #region 0x020 chunk

    /// <summary>
    /// CPlugShader 0x020 chunk
    /// </summary>
    [Chunk(0x09002020)]
    public class Chunk09002020 : Chunk<CPlugShader>
    {
        private int version;

        public uint U01;
        public float U02;
        public Node? U03;
        public short U04;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.UInt32(ref U01); // DoData
            rw.Single(ref U02);
            rw.NodeRef(ref U03); // CPlugMaterialFx
            rw.Int16(ref U04);
        }
    }

    #endregion
}