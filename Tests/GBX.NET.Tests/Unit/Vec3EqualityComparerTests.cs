using GBX.NET.Comparers;

namespace GBX.NET.Tests.Unit;

public class Vec3EqualityComparerTests
{
    [Test]
    public async Task HashSetCoalescesVectorsThatRoundToTheSameCoordinates()
    {
        var comparer = new Vec3EqualityComparer(2);
        var first = new Vec3(1.231f, -2.341f, 3.451f);
        var near = new Vec3(1.234f, -2.344f, 3.454f);
        var distinct = new Vec3(1.236f, -2.344f, 3.454f);
        var vectors = new HashSet<Vec3>(comparer) { first, near, distinct };

        await Assert.That(comparer.Equals(first, near)).IsTrue();
        await Assert.That(comparer.GetHashCode(first)).IsEqualTo(comparer.GetHashCode(near));
        await Assert.That(comparer.Equals(first, distinct)).IsFalse();
        await Assert.That(vectors.Count).IsEqualTo(2);
    }

    [Test]
    public async Task EachCoordinateParticipatesInEquality()
    {
        var comparer = new Vec3EqualityComparer(2);
        var origin = new Vec3(1, 2, 3);

        await Assert.That(comparer.Equals(origin, new Vec3(1.02f, 2, 3))).IsFalse();
        await Assert.That(comparer.Equals(origin, new Vec3(1, 2.02f, 3))).IsFalse();
        await Assert.That(comparer.Equals(origin, new Vec3(1, 2, 3.02f))).IsFalse();
    }
}
