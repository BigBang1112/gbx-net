namespace GBX.NET.PAK.Tests;

public class LZ4StreamTests
{
    [Test]
    public async Task Read_AtBlockBoundary_DoesNotReadNextBlock()
    {
        using var compressed = new MemoryStream();
        using (var writer = new BinaryWriter(compressed, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            for (var block = 0; block < 2; block++)
            {
                writer.Write((ushort)4114);
                writer.Write((byte)0xF0);
                writer.Write(Enumerable.Repeat((byte)255, 16).ToArray());
                writer.Write((byte)1);
                writer.Write(Enumerable.Repeat((byte)(block + 1), 4096).ToArray());
            }
        }
        compressed.Position = 0;
        using var stream = new LZ4Stream(compressed, 8192);
        var buffer = new byte[4096];

        await Assert.That(stream.Read(buffer, 0, buffer.Length)).IsEqualTo(4096);
        await Assert.That(compressed.Position).IsEqualTo(4116L);
        await Assert.That(buffer.All(value => value == 1)).IsTrue();
        await Assert.That(stream.Read(buffer, 0, 0)).IsEqualTo(0);
        await Assert.That(compressed.Position).IsEqualTo(4116L);

        await Assert.That(stream.Read(buffer, 0, buffer.Length)).IsEqualTo(4096);
        await Assert.That(buffer.All(value => value == 2)).IsTrue();
        await Assert.That(stream.Read(buffer, 0, buffer.Length)).IsEqualTo(0);
    }
}
