namespace GBX.NET.Engines.Function;

[Node(0x0500B000)]
public abstract class CFuncPlug : CFunc
{
    private float period;
    private float phase;
    private bool autoCreateMotion;
    private bool randomizePhase;
    private string? inputValId;

    public float Period
    {
        get => period;
        set => period = value;
    }

    public float Phase
    {
        get => phase;
        set => phase = value;
    }

    public bool AutoCreateMotion
    {
        get => autoCreateMotion;
        set => autoCreateMotion = value;
    }

    public bool RandomizePhase
    {
        get => randomizePhase;
        set => randomizePhase = value;
    }

    public string? InputValId
    {
        get => inputValId;
        set => inputValId = value;
    }

    protected CFuncPlug()
    {

    }

    [Chunk(0x0500B003)]
    public class Chunk0500B003 : Chunk<CFuncPlug>
    {
        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
        }
    }

    [Chunk(0x0500B004)]
    public class Chunk0500B004 : Chunk<CFuncPlug>
    {
        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
            rw.Boolean(ref n.randomizePhase);
        }
    }

    [Chunk(0x0500B005)]
    public class Chunk0500B005 : Chunk<CFuncPlug>
    {
        public override void ReadWrite(CFuncPlug n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.period);
            rw.Single(ref n.phase);
            rw.Boolean(ref n.autoCreateMotion);
            rw.Boolean(ref n.randomizePhase);
            rw.Id(ref n.inputValId);
        }
    }
}
