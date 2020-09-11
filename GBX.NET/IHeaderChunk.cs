namespace GBX.NET
{
    public interface IHeaderChunk
    {
        bool IsHeavy { get; set; }

        void ReadWrite(GameBoxReaderWriter rw);
    }
}
