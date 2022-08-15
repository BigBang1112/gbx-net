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
    public IDictionary<float, CPlugTree> Levels
    {
        get => levels;
        set => levels = value;
    }

    protected CPlugTreeVisualMip()
    {
        levels = null!;
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

        public override async Task ReadWriteAsync(CPlugTreeVisualMip n, GameBoxReaderWriter rw, ILogger? logger, CancellationToken cancellationToken = default)
        {
            n.levels = (await rw.DictionaryNodeAsync(n.levels!, overrideKey: true, cancellationToken: cancellationToken))!;
        }
    }
}
