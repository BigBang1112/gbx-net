namespace GBX.NET.Engines.Plug;

/// <summary>
/// Level of detail for individual <see cref="CPlugTree"/>s.
/// </summary>
/// <remarks>ID: 0x09015000</remarks>
[Node(0x09015000)]
public class CPlugTreeVisualMip : CPlugTree
{
    private IDictionary<float, CPlugTree> levels;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk09015002))]
    public IDictionary<float, CPlugTree> Levels { get => levels; set => levels = value; }

    protected CPlugTreeVisualMip()
    {
#if NET6_0_OR_GREATER
        levels = global::System.Collections.Immutable.ImmutableDictionary.Create<float, CPlugTree>();
#else
        levels = null!;
#endif
    }

    /// <summary>
    /// CPlugTreeVisualMip 0x002 chunk
    /// </summary>
    [Chunk(0x09015002)]
    public class Chunk09015002 : Chunk<CPlugTreeVisualMip>
    {
        public override void ReadWrite(CPlugTreeVisualMip n, GameBoxReaderWriter rw)
        {
            rw.DictionaryNode(ref n.levels!, overrideKey: true);
        }

        public override async Task ReadWriteAsync(CPlugTreeVisualMip n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            n.levels = (await rw.DictionaryNodeAsync(n.levels!, overrideKey: true, cancellationToken: cancellationToken))!;
        }
    }
}
