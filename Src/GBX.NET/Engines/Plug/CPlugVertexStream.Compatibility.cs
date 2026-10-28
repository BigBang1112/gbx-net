namespace GBX.NET.Engines.Plug;

public partial class CPlugVertexStream
{
    public partial class DataDecl
    {
        [Obsolete("Use Data instead.")]
        public byte[]? U01 { get => Data; set => Data = value; }

        [Obsolete("Use SharedDeclIndex instead.")]
        public ushort U02 { get => SharedDeclIndex; set => SharedDeclIndex = value; }
    }
}
