using System;
using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game;

[Node(0x030E5000)]
public sealed class CGameCtnMediaBlockGhost : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private TimeSpan? start;
    private TimeSpan? end;
    private IList<Key>? keys;
    private CGameCtnGhost ghostModel;
    private float startOffset;
    private bool noDamage;
    private bool forceLight;
    private bool forceHue;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys?.Cast<CGameCtnMediaBlock.Key>() ?? Enumerable.Empty<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    TimeSpan IHasTwoKeys.Start
    {
        get => start.GetValueOrDefault();
        set => start = value;
    }

    TimeSpan IHasTwoKeys.End
    {
        get => end.GetValueOrDefault(start.GetValueOrDefault() + TimeSpan.FromSeconds(3));
        set => end = value;
    }

    [NodeMember]
    public TimeSpan? Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSpan? End
    {
        get => end;
        set => end = value;
    }

    [NodeMember]
    public IList<Key>? Keys
    {
        get => keys;
        set => keys = value;
    }

    [NodeMember]
    public CGameCtnGhost GhostModel
    {
        get => ghostModel;
        set => ghostModel = value;
    }

    [NodeMember]
    public float StartOffset
    {
        get => startOffset;
        set => startOffset = value;
    }

    [NodeMember]
    public bool NoDamage
    {
        get => noDamage;
        set => noDamage = value;
    }

    [NodeMember]
    public bool ForceLight
    {
        get => forceLight;
        set => forceLight = value;
    }

    [NodeMember]
    public bool ForceHue
    {
        get => forceHue;
        set => forceHue = value;
    }

    #endregion

    #region Constructors

    private CGameCtnMediaBlockGhost()
    {
        ghostModel = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    [Chunk(0x030E5001)]
    public class Chunk030E5001 : Chunk<CGameCtnMediaBlockGhost>
    {
        public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end, n.start.GetValueOrDefault() + TimeSpan.FromSeconds(3));
            rw.NodeRef<CGameCtnGhost>(ref n.ghostModel!);
            rw.Single(ref n.startOffset);
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x030E5002)]
    public class Chunk030E5002 : Chunk<CGameCtnMediaBlockGhost>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (Version >= 3)
            {
                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    Unknown = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.Unknown);
                });
            }
            else
            {
                rw.Single_s(ref n.start);
                rw.Single_s(ref n.end, n.start.GetValueOrDefault() + TimeSpan.FromSeconds(3));
            }

            rw.NodeRef<CGameCtnGhost>(ref n.ghostModel!);
            rw.Single(ref n.startOffset);
            rw.Boolean(ref n.noDamage);
            rw.Boolean(ref n.forceLight);
            rw.Boolean(ref n.forceHue);
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Unknown { get; set; }
    }

    #endregion
}
