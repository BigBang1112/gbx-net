namespace GBX.NET;

public readonly record struct Color(float R, float G, float B, float A)
{
    public static readonly Color Zero = new();

    public Color(int argb) : this((argb >> 16) & 0xFF, (argb >> 8) & 0xFF, argb & 0xFF, (argb >> 24) & 0xFF)
    {
        
    }

    public int ToArgb() => ((int)A << 24) | ((int)R << 16) | ((int)G << 8) | (int)B;
    public int ToRgba() => ((int)A << 24) | ((int)B << 16) | ((int)G << 8) | (int)R;

    public override string ToString()
    {
        return $"<{R}, {G}, {B}, {A}>";
    }

    public static implicit operator Color((float R, float G, float B, float A) v) => new(v.R, v.G, v.B, v.A);

    public static explicit operator Color(Vec4 v) => new(v.X, v.Y, v.Z, v.W);
}