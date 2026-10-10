namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class Vec3Tests
{
    [Test]
    public async Task MagnitudeAndNormalizationFollowThreeFourFiveTriangle()
    {
        var vector = new Vec3(3, 4, 0);

        await Assert.That(vector.GetSqrMagnitude()).IsEqualTo(25f);
        await Assert.That(vector.GetMagnitude()).IsEqualTo(5f);
        await Assert.That(vector.GetNormalized()).IsEqualTo(new Vec3(0.6f, 0.8f, 0));
    }

    [Test]
    public async Task NormalizingZeroReturnsZero()
    {
        await Assert.That(Vec3.Zero.GetNormalized()).IsEqualTo(Vec3.Zero);
    }

    [Test]
    public async Task DotAndCrossProductsUseAllComponents()
    {
        var x = new Vec3(1, 0, 0);
        var y = new Vec3(0, 1, 0);

        await Assert.That(Vec3.GetDotProduct(x, y)).IsEqualTo(0f);
        await Assert.That(Vec3.GetDotProduct(new Vec3(1, 2, 3), new Vec3(4, 5, 6))).IsEqualTo(32f);
        await Assert.That(Vec3.GetCrossProduct(x, y)).IsEqualTo(new Vec3(0, 0, 1));
        await Assert.That(Vec3.GetCrossProduct(y, x)).IsEqualTo(new Vec3(0, 0, -1));
    }

    [Test]
    public async Task ArithmeticKeepsComponentOrder()
    {
        var vector = new Vec3(2, 4, 6);

        await Assert.That(vector + new Vec3(1, 3, 5)).IsEqualTo(new Vec3(3, 7, 11));
        await Assert.That(vector - new Int3(1, 2, 3)).IsEqualTo(new Vec3(1, 2, 3));
        await Assert.That(vector * 2).IsEqualTo(new Vec3(4, 8, 12));
        await Assert.That(vector / 2).IsEqualTo(new Vec3(1, 2, 3));
    }
}
