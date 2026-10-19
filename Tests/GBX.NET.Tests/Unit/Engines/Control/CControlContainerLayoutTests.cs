using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Control;

[Category("Unit")]
public class CControlContainerLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_MenuFrameInstance_PreservesScriptsAndFollowingChildLocation(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(1); // Frame child version.
            writer.Write(2); // Legacy model pointer.
            writer.WriteNodeRef(new CGameMenuFrame());
            writer.Write(2); // Mobil own data version.
            writer.Write(uint.MaxValue); // Own data records.
            writer.WriteIdAsString("Instance");
            writer.Write(0); // Links.
            writer.Write(3); // Container own data version.
            writer.Write(0); // Anchors.
            writer.WriteIdAsString(""); // Terminal records.
            writer.Write(version);
            writer.WriteNodeRef<CMwCmdBlockMain>(null);
            writer.WriteNodeRef<CMwCmdBlockMain>(null);
            writer.WriteNodeRef<CMwCmdBlockMain>(null);
            if (version >= 1) writer.WriteIdAsString("ButtonBack");
            writer.Write(Iso4.Identity);
            writer.Write(0x11223344);
        }

        payload.Position = 0;
        var context = new CControlFrame();
        var child = new CControlContainer.Child();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            child.ReadWrite(rw, context);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }

        await Assert.That(child.Instance!.Data!.MenuFrameData!.Version).IsEqualTo(version);
        await Assert.That(child.Instance.Data.MenuFrameData.AutoBackButtonId).IsEqualTo(version >= 1 ? "ButtonBack" : "CardBack");
        await Assert.That(child.RelativeLocation).IsEqualTo(Iso4.Identity);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) child.ReadWrite(rw, context);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }
}
