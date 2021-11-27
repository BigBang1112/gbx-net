using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder : ICGameCtnMediaClipBuilder
{
    public string? Name { get; set; }
    public IList<CGameCtnMediaTrack>? Tracks { get; set; }

    public CGameCtnMediaClipBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CGameCtnMediaClipBuilder WithTracks(IList<CGameCtnMediaTrack> tracks)
    {
        Tracks = tracks;
        return this;
    }

    public CGameCtnMediaClipBuilder WithTracks(params CGameCtnMediaTrack[] tracks)
    {
        Tracks = tracks;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    GameBuilder<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
        IBuilderForTMSX<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>.ForTMSX() => ForTMSX();
    GameBuilder<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
        IBuilderForTMU<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>.ForTMU() => ForTMU();
    GameBuilder<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
        IBuilderForTMUF<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>.ForTMUF() => ForTMUF();
    GameBuilder<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
        IBuilderForTM2<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>.ForTM2() => ForTM2();
    GameBuilder<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
        IBuilderForTM2020<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>.ForTM2020() => ForTM2020();

    internal CGameCtnMediaClip NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaClip>(0x03079000);
        node.Name = Name ?? "Unnamed clip";
        node.Tracks = Tracks ?? new List<CGameCtnMediaTrack>();
        return node;
    }
}