namespace GBX.NET.Engines.Plug;

[Node(0x09062000)]
public class CPlugTreeLight : CPlugTree
{
    private CPlugLight? plugLight;

    public CPlugLight? PlugLight
    {
        get => plugLight;
        set => plugLight = value;
    }

    protected CPlugTreeLight()
    {

    }

    [Chunk(0x09062004)]
    public class Chunk09062004 : Chunk<CPlugTreeLight>
    {
        public override void ReadWrite(CPlugTreeLight n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.plugLight);
        }
    }
}
