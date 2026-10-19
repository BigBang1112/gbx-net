namespace GBX.NET.PAK.Tests;

public class EncryptionInitializerTests
{
    [Test]
    [Arguments(17, 0x090EF000U, 0x01001000U)]
    [Arguments(17, 0x090F6000U, 0x01001000U)]
    [Arguments(17, 0x09112000U, 0x01001000U)]
    [Arguments(18, 0x090EF000U, 0x0910C000U)]
    [Arguments(18, 0x090F6000U, 0x0910C000U)]
    [Arguments(18, 0x09112000U, 0x0910C000U)]
    public async Task InitializeClass_CameraModels_UsesParentFromArchiveGeneration(int pakVersion, uint classId, uint expectedParentId)
    {
        using var actualInput = new MemoryStream(new byte[264]);
        using var expectedInput = new MemoryStream(new byte[264]);
        using var actual = new BlowfishStream(actualInput, new byte[16], 0, pakVersion);
        using var expected = new BlowfishStream(expectedInput, new byte[16], 0, pakVersion);

        // Read the first cipher block, as the Gbx header does before node initialization.
        var actualData = new byte[264];
        var expectedData = new byte[264];
        actual.ReadExactly(actualData.AsSpan(0, 8));
        expected.ReadExactly(expectedData.AsSpan(0, 8));
        new EncryptionInitializer(actual, pakVersion).InitializeClass(classId, 0x0910C000);
        expected.Initialize(BitConverter.GetBytes(expectedParentId), 0, 4);
        actual.ReadExactly(actualData.AsSpan(8));
        expected.ReadExactly(expectedData.AsSpan(8));

        await Assert.That(actualData.SequenceEqual(expectedData)).IsTrue();
    }
}
