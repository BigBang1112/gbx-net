namespace GBX.NET.Engines.MwFoundations;

public partial class CMwCmdExpNumParam
{
    public new partial class Chunk01046000
    {
        public override void ReadWrite(CMwCmdExpNumParam n, GbxReaderWriter rw)
        {
            n.Type = rw.Int32(n.Type);
        }
    }
}
