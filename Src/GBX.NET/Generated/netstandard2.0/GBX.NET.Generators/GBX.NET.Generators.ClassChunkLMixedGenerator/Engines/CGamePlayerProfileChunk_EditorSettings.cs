namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03163000</remarks>
[Class(0x03163000)]
public partial class CGamePlayerProfileChunk_EditorSettings : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03163000;




    private PluginInfo[]? pluginInfos;
    [AppliedWithChunk<Chunk03163000>]
    public PluginInfo[]? PluginInfos { get => pluginInfos; set => pluginInfos = value; }

    private int mapEditorCoppersLimit;
    [AppliedWithChunk<Chunk03163001>]
    public int MapEditorCoppersLimit { get => mapEditorCoppersLimit; set => mapEditorCoppersLimit = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_EditorSettings"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_EditorSettings() { }


    /// <summary>
    /// CGamePlayerProfileChunk_EditorSettings 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03163000)]
    public partial class Chunk03163000 : SkippableChunk<CGamePlayerProfileChunk_EditorSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03163000;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_EditorSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<PluginInfo>(ref n.pluginInfos!);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_EditorSettings 0x001 skippable chunk
    /// </summary>
    [Chunk(0x03163001)]
    public partial class Chunk03163001 : SkippableChunk<CGamePlayerProfileChunk_EditorSettings>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03163001;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_EditorSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.mapEditorCoppersLimit);
        }
    }

    /// <summary>
    /// CGamePlayerProfileChunk_EditorSettings 0x002 skippable chunk
    /// </summary>
    [Chunk(0x03163002)]
    public partial class Chunk03163002 : SkippableChunk<CGamePlayerProfileChunk_EditorSettings>
    {
        /// <inheritdoc />
        public override uint Id => 0x03163002;

        public bool U01;

        public override void ReadWrite(CGamePlayerProfileChunk_EditorSettings n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }


    public sealed partial class PluginInfo : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.String(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Boolean(ref u05);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03163000 => new Chunk03163000(),
        0x03163001 => new Chunk03163001(),
        0x03163002 => new Chunk03163002(),
        _ => base.NewChunk(chunkId),
    };
}
