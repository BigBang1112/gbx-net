using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public interface IChunk
    {
        CMwNod Node { get; set; }
        GameBoxPart Part { get; set; }
        int Progress { get; set; }
        void OnLoad();
        void ReadWrite(CMwNod n, GameBoxReaderWriter rw);
    }
}
