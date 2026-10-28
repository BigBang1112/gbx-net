using System.Numerics;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class QuatTests
{
    [Test]
    public async Task Multiply_IdentityQuaternion_LeavesOtherQuaternionUnchanged()
    {
        var rotation = new Quat(1, 2, 3, 4);

        await Assert.That(Quat.Identity * rotation).IsEqualTo(rotation);
        await Assert.That(rotation * Quat.Identity).IsEqualTo(rotation);
    }

    [Test]
    public async Task Multiply_AxisRotations_PreservesOrder()
    {
        var xHalfTurn = new Quat(1, 0, 0, 0);
        var yHalfTurn = new Quat(0, 1, 0, 0);

        await Assert.That(xHalfTurn * yHalfTurn).IsEqualTo(new Quat(0, 0, 1, 0));
        await Assert.That(yHalfTurn * xHalfTurn).IsEqualTo(new Quat(0, 0, -1, 0));
    }

    [Test]
    public async Task Conversion_SystemQuaternion_PreservesComponents()
    {
        var rotation = new Quat(1, 2, 3, 4);
        Quaternion systemRotation = rotation;
        Quat roundTripped = systemRotation;

        await Assert.That(systemRotation).IsEqualTo(new Quaternion(1, 2, 3, 4));
        await Assert.That(roundTripped).IsEqualTo(rotation);
    }
}
