using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisualTests
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task IsIndexationStatic_WhenChanged_PreservesOtherFlags(bool enabled)
    {
        // Arrange
        var otherFlags = (1 << 20) | (1 << 3) | (1 << 7);
        var visual = new CPlugVisual { Flags = otherFlags | (1 << 5) };

        // Act
        visual.IsIndexationStatic = enabled;

        // Assert
        await Assert.That(visual.Flags).IsEqualTo(otherFlags | (enabled ? 1 << 5 : 0));
        await Assert.That(visual.IsIndexationStatic).IsEqualTo(enabled);
        await Assert.That(visual.IsGeometryStatic).IsTrue();
        await Assert.That(visual.HasVertexNormals).IsTrue();
        await Assert.That(visual.IsFlagBitSet(20)).IsTrue();
    }

    [Test]
    [Arguments(2, false, 1 << 9)]
    [Arguments(11, true, (1 << 2) | (1 << 9) | (1 << 11))]
    public async Task SetFlagBit_WhenChanged_PreservesOtherBits(int bit, bool enabled, int expectedFlags)
    {
        // Arrange
        var visual = new CPlugVisual { Flags = (1 << 2) | (1 << 9) };

        // Act
        visual.SetFlagBit(bit, enabled);

        // Assert
        await Assert.That(visual.Flags).IsEqualTo(expectedFlags);
    }
}
