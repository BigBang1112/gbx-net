namespace GBX.NET.Engines.MwFoundations;

public partial class CMwCmdExpNumIdent
{
    public new partial class Chunk01046000
    {
        public override void ReadWrite(CMwCmdExpNumIdent n, GbxReaderWriter rw)
        {
            n.Type = rw.Int32(n.Type);
        }
    }
}
