using GBX.NET.Components;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnCollection
{
    private Id? collection;
    public Id? Collection { get => collection; set => collection = value; }

    public partial CGameCtnDecoration? DefaultDecoration
    {
        get => defaultDecorationRef?.Resolve();
        set => NodeRef.SetEmbedded(ref defaultDecorationRef, value);
    }

    public bool HasBackgroundShadow
    {
        get => BackgroundShadow != EBackgroundShadow.None;
        set => BackgroundShadow = value ? EBackgroundShadow.Receive : EBackgroundShadow.None;
    }

    public bool HasVertexLighting
    {
        get => VertexLighting != EVertexLighting.None;
        set => VertexLighting = value ? EVertexLighting.Sunrise : EVertexLighting.None;
    }

    [Obsolete("Use CompleteZoneList instead.")]
    public NodeRefList<CGameCtnZone>? CompleteListZoneList { get => CompleteZoneList; set => CompleteZoneList = value; }

    [Obsolete("Use DecalFade_cBlock_FullDensity instead.")]
    public int DecalFadeCBlockFullDensity { get => DecalFade_cBlock_FullDensity; set => DecalFade_cBlock_FullDensity = value; }

    [Obsolete("Use VehicleEnvLayer_FidBitmap instead.")]
    public CPlugBitmap? VehicleEnvLayerFidBitmap { get => VehicleEnvLayer_FidBitmap; set => VehicleEnvLayer_FidBitmap = value; }

    [Obsolete("Use OffZone_FogMatter instead.")]
    public CPlugFogMatter? OffZoneFogMatter { get => OffZone_FogMatter; set => OffZone_FogMatter = value; }

    [Obsolete("Use WaterG_BitmapNormal instead.")]
    public CPlugBitmap? WaterGBitmapNormal { get => WaterG_BitmapNormal; set => WaterG_BitmapNormal = value; }

    [Obsolete("Use WaterG_BumpSpeedUV instead.")]
    public float WaterGBumpSpeedUV { get => WaterG_BumpSpeedUV; set => WaterG_BumpSpeedUV = value; }

    [Obsolete("Use WaterG_BumpScaleUV instead.")]
    public float WaterGBumpScaleUV { get => WaterG_BumpScaleUV; set => WaterG_BumpScaleUV = value; }

    [Obsolete("Use WaterG_BumpScale instead.")]
    public float WaterGBumpScale { get => WaterG_BumpScale; set => WaterG_BumpScale = value; }

    [Obsolete("Use WaterG_RefracPertub instead.")]
    public float WaterGRefracPertub { get => WaterG_RefracPertub; set => WaterG_RefracPertub = value; }

    [Obsolete("Use TurboColor_Roulette1 instead.")]
    public uint? TurboColorRoulette1 { get => TurboColor_Roulette1; set => TurboColor_Roulette1 = value ?? 0xFF00FFFF; }

    [Obsolete("Use TurboColor_Roulette2 instead.")]
    public uint? TurboColorRoulette2 { get => TurboColor_Roulette2; set => TurboColor_Roulette2 = value ?? 0xFF0000FF; }

    [Obsolete("Use TurboColor_Roulette3 instead.")]
    public uint? TurboColorRoulette3 { get => TurboColor_Roulette3; set => TurboColor_Roulette3 = value ?? 0xFFFF00FF; }

    [Obsolete("Use TurboColor_Turbo instead.")]
    public uint? TurboColorTurbo { get => TurboColor_Turbo; set => TurboColor_Turbo = value ?? 0xFFFF00FF; }

    [Obsolete("Use TurboColor_Turbo2 instead.")]
    public uint? TurboColorTurbo2 { get => TurboColor_Turbo2; set => TurboColor_Turbo2 = value ?? 0xFF0000FF; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10A instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10A { get => BitmapDisplayControlDefaultTVProgram_64x10A; set => BitmapDisplayControlDefaultTVProgram_64x10A = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10B instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10B { get => BitmapDisplayControlDefaultTVProgram_64x10B; set => BitmapDisplayControlDefaultTVProgram_64x10B = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10C instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10C { get => BitmapDisplayControlDefaultTVProgram_64x10C; set => BitmapDisplayControlDefaultTVProgram_64x10C = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_2x3 instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram2x3 { get => BitmapDisplayControlDefaultTVProgram_2x3; set => BitmapDisplayControlDefaultTVProgram_2x3 = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_155 instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram16x9 { get => BitmapDisplayControlDefaultTVProgram_155; set => BitmapDisplayControlDefaultTVProgram_155 = value; }

    public partial class Water
    {
        [Obsolete("Use Id instead.")]
        public string? U01 { get => Id; set => Id = value; }

        [Obsolete("Use OffsetTop instead.")]
        public float U02 { get => OffsetTop; set => OffsetTop = value; }

        [Obsolete("Use OffsetBottom instead.")]
        public float U03 { get => OffsetBottom; set => OffsetBottom = value; }
    }
}
