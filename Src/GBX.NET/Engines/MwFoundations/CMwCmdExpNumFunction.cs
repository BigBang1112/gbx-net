namespace GBX.NET.Engines.MwFoundations;

public partial class CMwCmdExpNumFunction
{
    public new partial class Chunk01046000
    {
        public override void ReadWrite(CMwCmdExpNumFunction n, GbxReaderWriter rw)
        {
            n.Type = rw.Int32(n.Type);
        }
    }
}
