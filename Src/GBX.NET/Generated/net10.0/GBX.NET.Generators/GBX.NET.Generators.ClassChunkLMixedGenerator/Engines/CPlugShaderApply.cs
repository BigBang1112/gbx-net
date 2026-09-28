namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09026000</remarks>
[Class(0x09026000)]
public partial class CPlugShaderApply : CPlugShaderGeneric, IClass
{
    [Hexadecimal] public static new uint Id => 0x09026000;




    private CPlugBitmapAddress[]? bitmapAddresses;
    [AppliedWithChunk<Chunk09026001>]
    [AppliedWithChunk<Chunk09026002>]
    [AppliedWithChunk<Chunk0902600C>]
    public CPlugBitmapAddress[]? BitmapAddresses { get => bitmapAddresses; set => bitmapAddresses = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugShaderApply"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugShaderApply() { }


    /// <summary>
    /// CPlugShaderApply 0x001 chunk
    /// </summary>
    [Chunk(0x09026001)]
    public partial class Chunk09026001 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026001;

        public int U01;
        public int U02;
        public uint U03;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.ArrayNodeRef<CPlugBitmapAddress>(ref n.bitmapAddresses!);
            rw.UInt32(ref U03);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x002 chunk
    /// </summary>
    [Chunk(0x09026002)]
    public partial class Chunk09026002 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026002;


        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CPlugBitmapAddress>(ref n.bitmapAddresses!);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x004 chunk
    /// </summary>
    [Chunk(0x09026004)]
    public partial class Chunk09026004 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026004;

        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x006 chunk
    /// </summary>
    [Chunk(0x09026006)]
    public partial class Chunk09026006 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026006;

        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x007 chunk
    /// </summary>
    [Chunk(0x09026007)]
    public partial class Chunk09026007 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026007;

        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x008 chunk
    /// </summary>
    [Chunk(0x09026008)]
    public partial class Chunk09026008 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026008;

        public int U01;
        public int U02;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x00A chunk
    /// </summary>
    [Chunk(0x0902600A)]
    public partial class Chunk0902600A : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x0902600A;

        public uint U01;
        public uint U02;
        public uint U03;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
            rw.UInt32(ref U02);
            rw.UInt32(ref U03);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x00C chunk
    /// </summary>
    [Chunk(0x0902600C)]
    public partial class Chunk0902600C : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x0902600C;


        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugBitmapAddress>(ref n.bitmapAddresses!);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x00D chunk
    /// </summary>
    [Chunk(0x0902600D)]
    public partial class Chunk0902600D : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x0902600D;

        /// <summary>
        /// DoData
        /// </summary>
        public byte U01;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01); // DoData
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x010 chunk
    /// </summary>
    [Chunk(0x09026010)]
    public partial class Chunk09026010 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09026010;

        /// <summary>
        /// DoData
        /// </summary>
        public byte U01;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01); // DoData
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x011 chunk
    /// </summary>
    [Chunk(0x09026011)]
    public partial class Chunk09026011 : Chunk<CPlugShaderApply>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09026011;

        public int Version { get; set; }

        public CMwNod? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CMwNod>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x012 chunk
    /// </summary>
    [Chunk(0x09026012)]
    public partial class Chunk09026012 : Chunk<CPlugShaderApply>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09026012;

        public int Version { get; set; }

        public uint U01;
        public uint U02;
        public uint U03;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt32(ref U01);
            rw.UInt32(ref U02);
            rw.UInt32(ref U03);
        }
    }

    /// <summary>
    /// CPlugShaderApply 0x002 chunk
    /// </summary>
    [Chunk(0x09063002)]
    public partial class Chunk09063002 : Chunk<CPlugShaderApply>
    {
        /// <inheritdoc />
        public override uint Id => 0x09063002;

        public int U01;
        public int U02;
        public CPlugBitmapAddress? U03;
        public CPlugBitmapAddress? U04;
        public CPlugBitmapAddress? U05;
        public CPlugBitmapAddress? U06;
        public CPlugBitmapAddress? U07;

        public override void ReadWrite(CPlugShaderApply n, GbxReaderWriter rw)
        {
            rw.DataInt32(ref U01);
            rw.Int32(ref U02);
            rw.NodeRef<CPlugBitmapAddress>(ref U03);
            rw.NodeRef<CPlugBitmapAddress>(ref U04);
            rw.NodeRef<CPlugBitmapAddress>(ref U05);
            rw.NodeRef<CPlugBitmapAddress>(ref U06);
            rw.NodeRef<CPlugBitmapAddress>(ref U07);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09026001 => new Chunk09026001(),
        0x09026002 => new Chunk09026002(),
        0x09026004 => new Chunk09026004(),
        0x09026006 => new Chunk09026006(),
        0x09026007 => new Chunk09026007(),
        0x09026008 => new Chunk09026008(),
        0x0902600A => new Chunk0902600A(),
        0x0902600C => new Chunk0902600C(),
        0x0902600D => new Chunk0902600D(),
        0x09026010 => new Chunk09026010(),
        0x09026011 => new Chunk09026011(),
        0x09026012 => new Chunk09026012(),
        0x09063002 => new Chunk09063002(),
        _ => base.NewChunk(chunkId),
    };
}
