using GBX.NET.Attributes;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVisualSprite
{
    [AppliedWithChunk<Chunk09010000>]
    [AppliedWithChunk<Chunk09010001>]
    [AppliedWithChunk<Chunk09010002>]
    [AppliedWithChunk<Chunk09010005>]
    public ERenderMode RenderMode
    {
        get => (ERenderMode)(SpriteFlags & 7);
        set => SpriteFlags = (SpriteFlags & 0xFFFFFFF8) | ((uint)value & 7);
    }

    [AppliedWithChunk<Chunk09010001>]
    [AppliedWithChunk<Chunk09010002>]
    [AppliedWithChunk<Chunk09010005>]
    public bool SortBackToFront
    {
        get => (SpriteFlags & 0x10) != 0;
        set => SpriteFlags = value ? SpriteFlags | 0x10 : SpriteFlags & 0xFFFFFFEF;
    }

    public CPlugVisualSprite()
    {
        Flags |= 0x100;
        SpriteParam = new CPlugSpriteParam();
        AtlasTexCoords = [new Rect(0, 0, 1, 1)];
    }
}
