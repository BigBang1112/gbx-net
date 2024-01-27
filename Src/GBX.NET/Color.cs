namespace GBX.NET;

public readonly record struct Color(float R, float G, float B, float A)
{
    public static readonly Color Zero = new();

    public override string ToString()
    {
        return $"<{R}, {G}, {B}, {A}>";
    }

    public static implicit operator Color((float R, float G, float B, float A) v) => new(v.R, v.G, v.B, v.A);

    public static implicit operator (float R, float G, float B, float A)(Color v) => (v.R, v.G, v.B, v.A);
}