namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F4000</remarks>
[Node(0x090F4000)]
public class CPlugGameSkin : CMwNod
{
    protected CPlugGameSkin()
    {

    }

    /// <summary>
    /// CPlugGameSkin 0x000 header chunk
    /// </summary>
    [Chunk(0x090F4000)]
    public class Chunk090F4000 : HeaderChunk<CPlugGameSkin>, IVersionable
    {
        public int Version { get; set; }
        public string? SkinDirectory { get; set; }

        public override void ReadWrite(CPlugGameSkin n, GameBoxReaderWriter rw)
        {
            Version = rw.Byte(Version);
            SkinDirectory = rw.String(SkinDirectory);

            // ...
        }
    }
}
