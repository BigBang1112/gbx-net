namespace GBX.NET.Engines.System;

public partial class CSystemConfigDisplay
{
    [Obsolete("Use AdapterDesc instead.")]
    public string? Adapter { get => AdapterDesc; set => AdapterDesc = value; }

    [Obsolete("Use AsyncRenderEnabled instead.")]
    public bool AsyncRender { get => AsyncRenderEnabled; set => AsyncRenderEnabled = value; }

    [Obsolete("Use TextureDecals_3D instead.")]
    public bool Decals_3D__TextureDecals { get => TextureDecals_3D; set => TextureDecals_3D = value; }

    [Obsolete("Use TextureDecals_2D instead.")]
    public bool Decals_2D__TextureDecals { get => TextureDecals_2D; set => TextureDecals_2D = value; }

    [Obsolete("Use LightMapQuality instead.")]
    public ELightMapQuality LM_Quality { get => LightMapQuality; set => LightMapQuality = value; }

    [Obsolete("Use LightMapQualityUltra instead.")]
    public bool LM_QUltra { get => LightMapQualityUltra; set => LightMapQualityUltra = value; }

    [Obsolete("Use LightMapLightIndex instead.")]
    public bool LM_iLight { get => LightMapLightIndex; set => LightMapLightIndex = value; }

    [Obsolete("Use MultiThreadEnable instead.")]
    public bool MultiThread { get => MultiThreadEnable; set => MultiThreadEnable = value; }

    [Obsolete("Use MultiThreadCountMax instead.")]
    public int ThreadCountMax { get => MultiThreadCountMax; set => MultiThreadCountMax = value; }

    [Obsolete("Use StereoscopyByDefault instead.")]
    public bool StereoByDefault { get => StereoscopyByDefault; set => StereoscopyByDefault = value; }

    [Obsolete("Use StereoscopyAdvanced instead.")]
    public bool StereoAdvanced { get => StereoscopyAdvanced; set => StereoscopyAdvanced = value; }

    [Obsolete("Use PostFxEnable instead.")]
    public bool PostFx { get => PostFxEnable; set => PostFxEnable = value; }

    [Obsolete("Use OptimizePartialDynaGeom instead.")]
    public bool OptimPartDynaGeom { get => OptimizePartialDynaGeom; set => OptimizePartialDynaGeom = value; }
}
