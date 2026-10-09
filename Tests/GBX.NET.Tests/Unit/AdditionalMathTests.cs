namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class AdditionalMathTests
{
    [Test]
    public async Task ConvertsCardinalDirectionAndDegreesToRadians()
    {
        await Assert.That(MathF.Abs(AdditionalMath.ToRadians(Direction.East) - MathF.PI / 2)).IsLessThan(0.00001f);
        await Assert.That(MathF.Abs(AdditionalMath.ToRadians(180f) - MathF.PI)).IsLessThan(0.00001f);
        await Assert.That(MathF.Abs(AdditionalMath.ToDegrees(MathF.PI / 2) - 90f)).IsLessThan(0.00001f);
    }

    [Test]
    public async Task RotateAroundCenterMovesOnlyHorizontalCoordinates()
    {
        var point = new Vec3(3, 7, 4);
        var center = new Vec3(1, 2, 4);

        var rotated = AdditionalMath.RotateAroundCenter(point, center, MathF.PI / 2);

        await Assert.That(MathF.Abs(rotated.X - 1)).IsLessThan(0.00001f);
        await Assert.That(rotated.Y).IsEqualTo(7);
        await Assert.That(MathF.Abs(rotated.Z - 6)).IsLessThan(0.00001f);
    }

    [Test]
    public async Task LerpSupportsInterpolationAndExtrapolation()
    {
        await Assert.That(AdditionalMath.Lerp(10f, 20f, 0.25f)).IsEqualTo(12.5f);
        await Assert.That(AdditionalMath.Lerp(new Vec3(0, 10, 20), new Vec3(8, 18, 28), 1.5f))
            .IsEqualTo(new Vec3(12, 22, 32));
    }

    [Test]
    public async Task ClampIncludesBothBounds()
    {
        await Assert.That(AdditionalMath.Clamp(-2, 0, 10)).IsEqualTo(0);
        await Assert.That(AdditionalMath.Clamp(5, 0, 10)).IsEqualTo(5);
        await Assert.That(AdditionalMath.Clamp(12, 0, 10)).IsEqualTo(10);
    }
}
