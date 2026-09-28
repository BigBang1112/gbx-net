namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A01F000</remarks>
[Class(0x0A01F000)]
public partial class CSceneMessageHandler : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A01F000;




    private CMwNod? onClickScript;
    [AppliedWithChunk<Chunk0A01F000>]
    public CMwNod? OnClickScript { get => onClickScriptFile?.GetNode(ref onClickScript) ?? onClickScript; set => onClickScript = value; }
    private Components.GbxRefTableFile? onClickScriptFile;
    public Components.GbxRefTableFile? OnClickScriptFile { get => onClickScriptFile; set => onClickScriptFile = value; }
    public CMwNod? GetOnClickScript(GbxReadSettings settings = default, bool exceptions = false) => onClickScriptFile?.GetNode(ref onClickScript, settings, exceptions) ?? onClickScript;

    private CMwNod? onContactScript;
    [AppliedWithChunk<Chunk0A01F001>]
    public CMwNod? OnContactScript { get => onContactScriptFile?.GetNode(ref onContactScript) ?? onContactScript; set => onContactScript = value; }
    private Components.GbxRefTableFile? onContactScriptFile;
    public Components.GbxRefTableFile? OnContactScriptFile { get => onContactScriptFile; set => onContactScriptFile = value; }
    public CMwNod? GetOnContactScript(GbxReadSettings settings = default, bool exceptions = false) => onContactScriptFile?.GetNode(ref onContactScript, settings, exceptions) ?? onContactScript;

    /// <summary>
    /// Creates a new instance of <see cref="CSceneMessageHandler"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneMessageHandler() { }


    /// <summary>
    /// CSceneMessageHandler 0x000 chunk
    /// </summary>
    [Chunk(0x0A01F000)]
    public partial class Chunk0A01F000 : Chunk<CSceneMessageHandler>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A01F000;


        public override void ReadWrite(CSceneMessageHandler n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref n.onClickScript, ref n.onClickScriptFile);
        }
    }

    /// <summary>
    /// CSceneMessageHandler 0x001 chunk
    /// </summary>
    [Chunk(0x0A01F001)]
    public partial class Chunk0A01F001 : Chunk<CSceneMessageHandler>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A01F001;


        public override void ReadWrite(CSceneMessageHandler n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref n.onContactScript, ref n.onContactScriptFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A01F000 => new Chunk0A01F000(),
        0x0A01F001 => new Chunk0A01F001(),
        _ => base.NewChunk(chunkId),
    };
}
