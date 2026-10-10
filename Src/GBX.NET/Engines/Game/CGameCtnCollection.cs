using GBX.NET.Components;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnCollection
{
    private Id? collection;
    public Id? Collection { get => collection; set => collection = value; }

    private CGameCtnDecoration? defaultDecoration;
    private GbxRefTableFile? defaultDecorationFile;

    public partial CGameCtnDecoration? DefaultDecoration
    {
        get => defaultDecorationFile?.GetNode(ref defaultDecoration) ?? defaultDecoration;
        set => defaultDecoration = value;
    }

    public GbxRefTableFile? DefaultDecorationFile { get => defaultDecorationFile; set => defaultDecorationFile = value; }

    public CGameCtnDecoration? GetDefaultDecoration(GbxReadSettings settings = default, bool exceptions = false)
        => defaultDecorationFile?.GetNode(ref defaultDecoration, settings, exceptions) ?? defaultDecoration;

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
    public External<CGameCtnZone>[]? CompleteListZoneList { get => CompleteZoneList; set => CompleteZoneList = value; }

    [Obsolete("Use DecalFade_cBlock_FullDensity instead.")]
    public int DecalFadeCBlockFullDensity { get => DecalFade_cBlock_FullDensity; set => DecalFade_cBlock_FullDensity = value; }

    [Obsolete("Use VehicleEnvLayer_FidBitmap instead.")]
    public CPlugBitmap? VehicleEnvLayerFidBitmap { get => VehicleEnvLayer_FidBitmap; set => VehicleEnvLayer_FidBitmap = value; }

    [Obsolete("Use VehicleEnvLayer_FidBitmapFile instead.")]
    public GbxRefTableFile? VehicleEnvLayerFidBitmapFile { get => VehicleEnvLayer_FidBitmapFile; set => VehicleEnvLayer_FidBitmapFile = value; }

    [Obsolete("Use GetVehicleEnvLayer_FidBitmap instead.")]
    public CPlugBitmap? GetVehicleEnvLayerFidBitmap(GbxReadSettings settings = default, bool exceptions = false)
        => GetVehicleEnvLayer_FidBitmap(settings, exceptions);

    [Obsolete("Use OffZone_FogMatter instead.")]
    public CPlugFogMatter? OffZoneFogMatter { get => OffZone_FogMatter; set => OffZone_FogMatter = value; }

    [Obsolete("Use OffZone_FogMatterFile instead.")]
    public GbxRefTableFile? OffZoneFogMatterFile { get => OffZone_FogMatterFile; set => OffZone_FogMatterFile = value; }

    [Obsolete("Use GetOffZone_FogMatter instead.")]
    public CPlugFogMatter? GetOffZoneFogMatter(GbxReadSettings settings = default, bool exceptions = false)
        => GetOffZone_FogMatter(settings, exceptions);

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

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10AFile instead.")]
    public GbxRefTableFile? BitmapDisplayControlDefaultTVProgram64x10AFile { get => BitmapDisplayControlDefaultTVProgram_64x10AFile; set => BitmapDisplayControlDefaultTVProgram_64x10AFile = value; }

    [Obsolete("Use GetBitmapDisplayControlDefaultTVProgram_64x10A instead.")]
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram64x10A(GbxReadSettings settings = default, bool exceptions = false)
        => GetBitmapDisplayControlDefaultTVProgram_64x10A(settings, exceptions);

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10B instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10B { get => BitmapDisplayControlDefaultTVProgram_64x10B; set => BitmapDisplayControlDefaultTVProgram_64x10B = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10BFile instead.")]
    public GbxRefTableFile? BitmapDisplayControlDefaultTVProgram64x10BFile { get => BitmapDisplayControlDefaultTVProgram_64x10BFile; set => BitmapDisplayControlDefaultTVProgram_64x10BFile = value; }

    [Obsolete("Use GetBitmapDisplayControlDefaultTVProgram_64x10B instead.")]
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram64x10B(GbxReadSettings settings = default, bool exceptions = false)
        => GetBitmapDisplayControlDefaultTVProgram_64x10B(settings, exceptions);

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10C instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10C { get => BitmapDisplayControlDefaultTVProgram_64x10C; set => BitmapDisplayControlDefaultTVProgram_64x10C = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_64x10CFile instead.")]
    public GbxRefTableFile? BitmapDisplayControlDefaultTVProgram64x10CFile { get => BitmapDisplayControlDefaultTVProgram_64x10CFile; set => BitmapDisplayControlDefaultTVProgram_64x10CFile = value; }

    [Obsolete("Use GetBitmapDisplayControlDefaultTVProgram_64x10C instead.")]
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram64x10C(GbxReadSettings settings = default, bool exceptions = false)
        => GetBitmapDisplayControlDefaultTVProgram_64x10C(settings, exceptions);

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_2x3 instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram2x3 { get => BitmapDisplayControlDefaultTVProgram_2x3; set => BitmapDisplayControlDefaultTVProgram_2x3 = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_2x3File instead.")]
    public GbxRefTableFile? BitmapDisplayControlDefaultTVProgram2x3File { get => BitmapDisplayControlDefaultTVProgram_2x3File; set => BitmapDisplayControlDefaultTVProgram_2x3File = value; }

    [Obsolete("Use GetBitmapDisplayControlDefaultTVProgram_2x3 instead.")]
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram2x3(GbxReadSettings settings = default, bool exceptions = false)
        => GetBitmapDisplayControlDefaultTVProgram_2x3(settings, exceptions);

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_155 instead.")]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram16x9 { get => BitmapDisplayControlDefaultTVProgram_155; set => BitmapDisplayControlDefaultTVProgram_155 = value; }

    [Obsolete("Use BitmapDisplayControlDefaultTVProgram_155File instead.")]
    public GbxRefTableFile? BitmapDisplayControlDefaultTVProgram16x9File { get => BitmapDisplayControlDefaultTVProgram_155File; set => BitmapDisplayControlDefaultTVProgram_155File = value; }

    [Obsolete("Use GetBitmapDisplayControlDefaultTVProgram_155 instead.")]
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram16x9(GbxReadSettings settings = default, bool exceptions = false)
        => GetBitmapDisplayControlDefaultTVProgram_155(settings, exceptions);

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
