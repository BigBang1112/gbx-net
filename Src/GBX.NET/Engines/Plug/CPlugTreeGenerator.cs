namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09051000</remarks>
[Node(0x09051000)]
public abstract class CPlugTreeGenerator : CPlug
{
    internal CPlugTreeGenerator()
    {

    }

    /// <summary>
    /// CPlugTreeGenerator 0x000 chunk
    /// </summary>
    [Chunk(0x09051000)]
    public class Chunk09051000 : Chunk<CPlugTreeGenerator>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugTreeGenerator n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
        }
    }
}
