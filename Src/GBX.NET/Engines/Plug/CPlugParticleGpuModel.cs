namespace GBX.NET.Engines.Plug;

public partial class CPlugParticleGpuModel
{
    private float legacyLifeAverage;
    private float legacyLifeVariation;
    private float legacySizeAverage;
    private float legacySizeVariation;
    private float legacyIntensityDecayPower;
    private float legacyPersistence;

    [Obsolete("Used by lifetime and size chunk versions before 4. Use LifeMax and LifeVar01.")]
    public float LegacyLifeAverage { get => legacyLifeAverage; set => legacyLifeAverage = value; }

    [Obsolete("Used by lifetime and size chunk versions before 4. Use LifeMax and LifeVar01.")]
    public float LegacyLifeVariation { get => legacyLifeVariation; set => legacyLifeVariation = value; }

    [Obsolete("Used by lifetime and size chunk versions before 4. Use SizeMax and SizeVar01.")]
    public float LegacySizeAverage { get => legacySizeAverage; set => legacySizeAverage = value; }

    [Obsolete("Used by lifetime and size chunk versions before 4. Use SizeMax and SizeVar01.")]
    public float LegacySizeVariation { get => legacySizeVariation; set => legacySizeVariation = value; }

    [Obsolete("Used by rendering chunk versions before 3. Use IntensityFadeIn, IntensityFadeOut and IntensityPower.")]
    public float LegacyIntensityDecayPower { get => legacyIntensityDecayPower; set => legacyIntensityDecayPower = value; }

    [Obsolete("Used by rendering chunk version 0. Use Persistence.")]
    public float LegacyPersistence { get => legacyPersistence; set => legacyPersistence = value; }

    private float ConvertedLegacyLifeMax => legacyLifeAverage + Math.Max(0f, legacyLifeVariation);
    private float ConvertedLegacyLifeVar01 => ConvertLegacyVariation(legacyLifeAverage, legacyLifeVariation);
    private float ConvertedLegacySizeMax => legacySizeAverage + Math.Max(0f, legacySizeVariation);
    private float ConvertedLegacySizeVar01 => ConvertLegacyVariation(legacySizeAverage, legacySizeVariation);
    private float ConvertedLegacyIntensityFadeOut => Math.Max(0.01f, 1f - 1f / legacyIntensityDecayPower);
    private float ConvertedLegacyIntensityPower => Math.Min(6f, 1f / legacyIntensityDecayPower);

    private static float ConvertLegacyVariation(float average, float variation)
    {
        variation = Math.Max(0f, variation);
        var maximum = average + variation;
        var minimum = Math.Max(0.01f, average - variation);
        return Math.Min(0.95f, Math.Max(0f, (maximum - minimum) / Math.Max(0.001f, maximum)));
    }
}
