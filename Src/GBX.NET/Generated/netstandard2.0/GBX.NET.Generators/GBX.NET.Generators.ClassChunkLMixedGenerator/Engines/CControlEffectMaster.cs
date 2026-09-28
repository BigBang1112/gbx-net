namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x0701C000</remarks>
[Class(0x0701C000)]
public partial class CControlEffectMaster : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0701C000;




    private CControlEffect? focusEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? FocusEffect { get => focusEffectFile?.GetNode(ref focusEffect) ?? focusEffect; set => focusEffect = value; }
    private Components.GbxRefTableFile? focusEffectFile;
    public Components.GbxRefTableFile? FocusEffectFile { get => focusEffectFile; set => focusEffectFile = value; }
    public CControlEffect? GetFocusEffect(GbxReadSettings settings = default, bool exceptions = false) => focusEffectFile?.GetNode(ref focusEffect, settings, exceptions) ?? focusEffect;

    private CControlEffect? focusGainedEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? FocusGainedEffect { get => focusGainedEffectFile?.GetNode(ref focusGainedEffect) ?? focusGainedEffect; set => focusGainedEffect = value; }
    private Components.GbxRefTableFile? focusGainedEffectFile;
    public Components.GbxRefTableFile? FocusGainedEffectFile { get => focusGainedEffectFile; set => focusGainedEffectFile = value; }
    public CControlEffect? GetFocusGainedEffect(GbxReadSettings settings = default, bool exceptions = false) => focusGainedEffectFile?.GetNode(ref focusGainedEffect, settings, exceptions) ?? focusGainedEffect;

    private CControlEffect? focusLostEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? FocusLostEffect { get => focusLostEffectFile?.GetNode(ref focusLostEffect) ?? focusLostEffect; set => focusLostEffect = value; }
    private Components.GbxRefTableFile? focusLostEffectFile;
    public Components.GbxRefTableFile? FocusLostEffectFile { get => focusLostEffectFile; set => focusLostEffectFile = value; }
    public CControlEffect? GetFocusLostEffect(GbxReadSettings settings = default, bool exceptions = false) => focusLostEffectFile?.GetNode(ref focusLostEffect, settings, exceptions) ?? focusLostEffect;

    private CControlEffect? focusGainedByAnotherEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? FocusGainedByAnotherEffect { get => focusGainedByAnotherEffectFile?.GetNode(ref focusGainedByAnotherEffect) ?? focusGainedByAnotherEffect; set => focusGainedByAnotherEffect = value; }
    private Components.GbxRefTableFile? focusGainedByAnotherEffectFile;
    public Components.GbxRefTableFile? FocusGainedByAnotherEffectFile { get => focusGainedByAnotherEffectFile; set => focusGainedByAnotherEffectFile = value; }
    public CControlEffect? GetFocusGainedByAnotherEffect(GbxReadSettings settings = default, bool exceptions = false) => focusGainedByAnotherEffectFile?.GetNode(ref focusGainedByAnotherEffect, settings, exceptions) ?? focusGainedByAnotherEffect;

    private CControlEffect? focusLostByAnotherEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? FocusLostByAnotherEffect { get => focusLostByAnotherEffectFile?.GetNode(ref focusLostByAnotherEffect) ?? focusLostByAnotherEffect; set => focusLostByAnotherEffect = value; }
    private Components.GbxRefTableFile? focusLostByAnotherEffectFile;
    public Components.GbxRefTableFile? FocusLostByAnotherEffectFile { get => focusLostByAnotherEffectFile; set => focusLostByAnotherEffectFile = value; }
    public CControlEffect? GetFocusLostByAnotherEffect(GbxReadSettings settings = default, bool exceptions = false) => focusLostByAnotherEffectFile?.GetNode(ref focusLostByAnotherEffect, settings, exceptions) ?? focusLostByAnotherEffect;

    private CControlEffect? sleepingEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? SleepingEffect { get => sleepingEffectFile?.GetNode(ref sleepingEffect) ?? sleepingEffect; set => sleepingEffect = value; }
    private Components.GbxRefTableFile? sleepingEffectFile;
    public Components.GbxRefTableFile? SleepingEffectFile { get => sleepingEffectFile; set => sleepingEffectFile = value; }
    public CControlEffect? GetSleepingEffect(GbxReadSettings settings = default, bool exceptions = false) => sleepingEffectFile?.GetNode(ref sleepingEffect, settings, exceptions) ?? sleepingEffect;

    private CControlEffect? showingEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? ShowingEffect { get => showingEffectFile?.GetNode(ref showingEffect) ?? showingEffect; set => showingEffect = value; }
    private Components.GbxRefTableFile? showingEffectFile;
    public Components.GbxRefTableFile? ShowingEffectFile { get => showingEffectFile; set => showingEffectFile = value; }
    public CControlEffect? GetShowingEffect(GbxReadSettings settings = default, bool exceptions = false) => showingEffectFile?.GetNode(ref showingEffect, settings, exceptions) ?? showingEffect;

    private CControlEffect? hidingEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? HidingEffect { get => hidingEffectFile?.GetNode(ref hidingEffect) ?? hidingEffect; set => hidingEffect = value; }
    private Components.GbxRefTableFile? hidingEffectFile;
    public Components.GbxRefTableFile? HidingEffectFile { get => hidingEffectFile; set => hidingEffectFile = value; }
    public CControlEffect? GetHidingEffect(GbxReadSettings settings = default, bool exceptions = false) => hidingEffectFile?.GetNode(ref hidingEffect, settings, exceptions) ?? hidingEffect;

    private CControlEffect? actionEffect;
    [AppliedWithChunk<Chunk0701C000>]
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? ActionEffect { get => actionEffectFile?.GetNode(ref actionEffect) ?? actionEffect; set => actionEffect = value; }
    private Components.GbxRefTableFile? actionEffectFile;
    public Components.GbxRefTableFile? ActionEffectFile { get => actionEffectFile; set => actionEffectFile = value; }
    public CControlEffect? GetActionEffect(GbxReadSettings settings = default, bool exceptions = false) => actionEffectFile?.GetNode(ref actionEffect, settings, exceptions) ?? actionEffect;

    private CControlEffect? managedEffect;
    [AppliedWithChunk<Chunk0701C002>]
    public CControlEffect? ManagedEffect { get => managedEffectFile?.GetNode(ref managedEffect) ?? managedEffect; set => managedEffect = value; }
    private Components.GbxRefTableFile? managedEffectFile;
    public Components.GbxRefTableFile? ManagedEffectFile { get => managedEffectFile; set => managedEffectFile = value; }
    public CControlEffect? GetManagedEffect(GbxReadSettings settings = default, bool exceptions = false) => managedEffectFile?.GetNode(ref managedEffect, settings, exceptions) ?? managedEffect;

    private CMwRefBuffer? specialEffect;
    [AppliedWithChunk<Chunk0701C003>]
    public CMwRefBuffer? SpecialEffect { get => specialEffectFile?.GetNode(ref specialEffect) ?? specialEffect; set => specialEffect = value; }
    private Components.GbxRefTableFile? specialEffectFile;
    public Components.GbxRefTableFile? SpecialEffectFile { get => specialEffectFile; set => specialEffectFile = value; }
    public CMwRefBuffer? GetSpecialEffect(GbxReadSettings settings = default, bool exceptions = false) => specialEffectFile?.GetNode(ref specialEffect, settings, exceptions) ?? specialEffect;

    /// <summary>
    /// Creates a new instance of <see cref="CControlEffectMaster"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlEffectMaster() { }


    /// <summary>
    /// CControlEffectMaster 0x000 chunk
    /// </summary>
    [Chunk(0x0701C000)]
    public partial class Chunk0701C000 : Chunk<CControlEffectMaster>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701C000;


        public override void ReadWrite(CControlEffectMaster n, GbxReaderWriter rw)
        {
            rw.NodeRef<CControlEffect>(ref n.focusEffect, ref n.focusEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusGainedEffect, ref n.focusGainedEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusLostEffect, ref n.focusLostEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusGainedByAnotherEffect, ref n.focusGainedByAnotherEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusLostByAnotherEffect, ref n.focusLostByAnotherEffectFile);
            rw.NodeRef<CControlEffect>(ref n.sleepingEffect, ref n.sleepingEffectFile);
            rw.NodeRef<CControlEffect>(ref n.showingEffect, ref n.showingEffectFile);
            rw.NodeRef<CControlEffect>(ref n.hidingEffect, ref n.hidingEffectFile);
            rw.NodeRef<CControlEffect>(ref n.actionEffect, ref n.actionEffectFile);
        }
    }

    /// <summary>
    /// CControlEffectMaster 0x001 chunk
    /// </summary>
    [Chunk(0x0701C001)]
    public partial class Chunk0701C001 : Chunk<CControlEffectMaster>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701C001;

        public bool U01;
        public int U02;

        public override void ReadWrite(CControlEffectMaster n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CControlEffectMaster 0x002 chunk
    /// </summary>
    [Chunk(0x0701C002)]
    public partial class Chunk0701C002 : Chunk<CControlEffectMaster>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701C002;


        public override void ReadWrite(CControlEffectMaster n, GbxReaderWriter rw)
        {
            rw.NodeRef<CControlEffect>(ref n.focusEffect, ref n.focusEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusGainedEffect, ref n.focusGainedEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusLostEffect, ref n.focusLostEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusGainedByAnotherEffect, ref n.focusGainedByAnotherEffectFile);
            rw.NodeRef<CControlEffect>(ref n.focusLostByAnotherEffect, ref n.focusLostByAnotherEffectFile);
            rw.NodeRef<CControlEffect>(ref n.sleepingEffect, ref n.sleepingEffectFile);
            rw.NodeRef<CControlEffect>(ref n.showingEffect, ref n.showingEffectFile);
            rw.NodeRef<CControlEffect>(ref n.hidingEffect, ref n.hidingEffectFile);
            rw.NodeRef<CControlEffect>(ref n.actionEffect, ref n.actionEffectFile);
            rw.NodeRef<CControlEffect>(ref n.managedEffect, ref n.managedEffectFile);
        }
    }

    /// <summary>
    /// CControlEffectMaster 0x003 chunk
    /// </summary>
    [Chunk(0x0701C003)]
    public partial class Chunk0701C003 : Chunk<CControlEffectMaster>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701C003;


        public override void ReadWrite(CControlEffectMaster n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwRefBuffer>(ref n.specialEffect, ref n.specialEffectFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0701C000 => new Chunk0701C000(),
        0x0701C001 => new Chunk0701C001(),
        0x0701C002 => new Chunk0701C002(),
        0x0701C003 => new Chunk0701C003(),
        _ => base.NewChunk(chunkId),
    };
}
