using GBX.NET;
using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;

namespace GbxCLI.Tests;

public class GbxSummaryTests
{
    [Test]
    public async Task Summary_HandlesInheritedReferenceViews()
    {
        using var output = new StringWriter();
        await new GbxSummary(new CliConsole(output)).WriteAsync(new Gbx<CGameItemModel>(new CGameItemModel()));

        await Assert.That(output.ToString()).Contains("Node: CGameItemModel");
    }

    [Test]
    public async Task Summary_DoesNotResolveExternalReferencesOrComputedAliases()
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["external [red].Gbx"] = () =>
        {
            calls++;
            throw new InvalidOperationException("Must not load");
        };
        var node = new CPlugTree();
        node.References.ModelSolid.Descriptor = table.CreateFile("external [red].Gbx");
        node.References.Shader.Descriptor = table.CreateFile("external [red].Gbx");
        using var output = new StringWriter();

        await new GbxSummary(new CliConsole(output)).WriteAsync(new Gbx<CPlugTree>(node));

        await Assert.That(calls).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("ExternalFile, Unresolved: external [red].Gbx");
        await Assert.That(output.ToString()).Contains("HasModel: (computed; inspect with gbx eval)");
    }

    [Test]
    public async Task Summary_BoundsCollectionsAndBinaryDataAndHandlesCyclesAndGetterErrors()
    {
        var sample = new Sample();
        sample.Self = sample;
        using var output = new StringWriter();
        var summary = new ConsoleValueWriter(new CliConsole(output));

        await summary.WriteValueAsync("Sample", sample, 0);

        var text = output.ToString();
        await Assert.That(text).Contains("Already shown (shared or circular reference)");
        await Assert.That(text).Contains("(error: unavailable [red])");
        await Assert.That(text).Contains("More items (showing first 10)");
        await Assert.That(text).Contains("100 bytes (hex)");
        await Assert.That(text).Contains("literal [red]\\nnext");
        await Assert.That(text.Contains("[10]:")).IsFalse();
        await Assert.That(text.Length < 4000).IsTrue();
    }

    private sealed class Sample
    {
        public byte[] Data { get; } = new byte[100];
        public string Name => "literal [red]\nnext";
        public IEnumerable<int> Numbers => Enumerable.Range(0, int.MaxValue);
        public Sample? Self { get; set; }
        public string Throws => throw new InvalidOperationException("unavailable [red]");
    }
}
