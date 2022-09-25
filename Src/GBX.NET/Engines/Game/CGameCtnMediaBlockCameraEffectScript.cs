namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera effect script.
/// </summary>
/// <remarks>ID: 0x03161000</remarks>
[Node(0x03161000)]
public partial class CGameCtnMediaBlockCameraEffectScript : CGameCtnMediaBlockCameraEffect,
    CGameCtnMediaBlock.IHasKeys,
    CGameCtnMediaBlock.IHasTwoKeys
{
    private string script;
    private IList<Key>? keys;
    private TimeSingle? start;
    private TimeSingle? end;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys?.Cast<CGameCtnMediaBlock.Key>() ?? Enumerable.Empty<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    TimeSingle IHasTwoKeys.Start
    {
        get => start ?? TimeSingle.Zero;
        set => start = value;
    }

    TimeSingle IHasTwoKeys.End
    {
        get => end.GetValueOrDefault(start.GetValueOrDefault() + TimeSingle.FromSeconds(3));
        set => end = value;
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03161000))]
    public string Script { get => script; set => script = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03161000), sinceVersion: 1)]
    public IList<Key>? Keys { get => keys; set => keys = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03161000), sinceVersion: 0, upToVersion: 0)]
    public TimeSingle? Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03161000), sinceVersion: 0, upToVersion: 0)]
    public TimeSingle? End { get => end; set => end = value; }

    internal CGameCtnMediaBlockCameraEffectScript()
    {
        script = "";
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraEffectScript 0x000 chunk
    /// </summary>
    [Chunk(0x03161000)]
    public class Chunk03161000 : Chunk<CGameCtnMediaBlockCameraEffectScript>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockCameraEffectScript n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.String(ref n.script!);

            if (version == 0)
            {
                rw.TimeSingle(ref n.start);
                rw.TimeSingle(ref n.end, n.start.GetValueOrDefault() + TimeSingle.FromSeconds(3));

                return;
            }

            rw.ListKey(ref n.keys);
        }
    }

    #endregion
}
