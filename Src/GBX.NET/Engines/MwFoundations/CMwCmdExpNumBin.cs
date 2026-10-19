namespace GBX.NET.Engines.MwFoundations;

public partial class CMwCmdExpNumBin
{
    public new partial class Chunk01046000
    {
        public override void ReadWrite(CMwCmdExpNumBin n, GbxReaderWriter rw)
        {
            n.Type = rw.Int32(n.Type);
        }
    }
}
