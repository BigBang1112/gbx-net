namespace GbxCLI.Tests;

public class FileInspectorTests
{
    [Test]
    public async Task GetEntries_IncludesPublicMembersButNotIndexersOrStatics()
    {
        var entries = FileInspector.GetEntries(new Sample()).ToDictionary(entry => entry.Name);

        await Assert.That(entries["Name"].Value).IsEqualTo("sample");
        await Assert.That(entries["Name"].Type).IsEqualTo(typeof(string));
        await Assert.That(entries["Number"].Value).IsEqualTo(42);
        await Assert.That(entries["Number"].Type).IsEqualTo(typeof(int));
        await Assert.That(entries["Throws"].Error).IsEqualTo("unavailable");
        await Assert.That(entries.Count).IsEqualTo(3);
    }

    [Test]
    public async Task GetEntries_CollectionsAreLazyAndIndexed()
    {
        var entries = FileInspector.GetEntries(Enumerable.Range(0, int.MaxValue)).Take(2).ToArray();

        await Assert.That(entries.Length).IsEqualTo(2);
        await Assert.That(entries[0].Name).IsEqualTo("[0]");
        await Assert.That(entries[1].Value).IsEqualTo(1);
        await Assert.That(FileInspector.GetEntries("text").Any()).IsFalse();
        await Assert.That(FileInspector.GetEntries(42).Any()).IsFalse();
    }

    private sealed class Sample
    {
        public string Name => "sample";
        public int Number = 42;
        public string Throws => throw new InvalidOperationException("unavailable");
        public string this[int index] => "indexer";
        public static string Static => "static";
        public string PrivateGetter { private get; set; } = "private";
    }

}
