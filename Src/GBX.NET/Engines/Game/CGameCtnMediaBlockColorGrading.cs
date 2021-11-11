using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Color grading
/// </summary>
[Node(0x03186000)]
public sealed class CGameCtnMediaBlockColorGrading : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private FileRef image;
    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public FileRef Image
    {
        get => image;
        set => image = value;
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    private CGameCtnMediaBlockColorGrading()
    {
        image = null!;
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockColorGrading 0x000 chunk
    /// </summary>
    [Chunk(0x03186000)]
    public class Chunk03186000 : Chunk<CGameCtnMediaBlockColorGrading>
    {
        public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
        {
            rw.FileRef(ref n.image!);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockColorGrading 0x001 chunk
    /// </summary>
    [Chunk(0x03186001)]
    public class Chunk03186001 : Chunk<CGameCtnMediaBlockColorGrading>
    {
        public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Intensity = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Intensity);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Intensity { get; set; }
    }

    #endregion
}
