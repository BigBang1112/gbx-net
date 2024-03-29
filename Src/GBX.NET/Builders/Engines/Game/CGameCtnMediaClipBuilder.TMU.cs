﻿namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    public class TMU : GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>
    {
        public TMU(CGameCtnMediaClipBuilder baseBuilder, CGameCtnMediaClip node) : base(baseBuilder, node) { }

        public override CGameCtnMediaClip Build()
        {
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079003>();
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079004>();
            return Node;
        }
    }
}