using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using GBX.NET.Generators.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TUnit.Assertions.Enums;

namespace GBX.NET.Generators.Tests;

public class ExistingTypeAnalysisTests
{
    [Test]
    public async Task MergesPartialsAndKeepsMembersInTheirOwnScope()
    {
        var types = await Analyze(
            Parse("""
                namespace GBX.NET.Engines.Game;
                [Class(0x03043000)]
                public abstract partial class Example : CMwNod
                {
                    private int @event, version;
                    public int Version { get => version; private set => version = value; }
                    public partial class Chunk03043000 : IVersionable
                    {
                        public override void Read(Example n, GbxReader r) { }
                    }
                    public enum Kind { First, Second = 4 }
                }
                """, "Example.cs"),
            Parse("""
                namespace GBX { namespace NET.Engines.Game {
                public partial class Example
                {
                    public Example() { }
                    public Example(int version) { }
                    public static Example Create() => new();
                    public partial class Chunk03043000
                    {
                        public override void Write(Example n, GbxWriter w) { }
                    }
                }
                }}
                """, "Example.More.cs"));

        var type = types["GBX.NET.Engines.Game.Example"];
        await Assert.That(type.Declarations.Length).IsEqualTo(2);
        await Assert.That(type.IsAbstract).IsTrue();
        await Assert.That(type.IsPartial).IsTrue();
        await Assert.That(type.BaseTypes.Single().ToString()).IsEqualTo("CMwNod");
        await Assert.That(type.Attributes.Single().Name.ToString()).IsEqualTo("Class");
        await Assert.That(type.MembersByName.ContainsKey("event")).IsTrue();
        await Assert.That(type.MembersByName.ContainsKey("version")).IsTrue();
        await Assert.That(type.Constructors.Count()).IsEqualTo(2);
        await Assert.That(type.Methods.Single().Identifier.ValueText).IsEqualTo("Create");
        await Assert.That(type.Properties.Single().AccessorList).IsNotNull();

        var chunk = types["GBX.NET.Engines.Game.Example+Chunk03043000"];
        await Assert.That(chunk.ContainingTypeKey).IsEqualTo(type.Key);
        await Assert.That(chunk.Declarations.Length).IsEqualTo(2);
        await Assert.That(chunk.Methods.Select(x => x.Identifier.ValueText))
            .IsEquivalentTo(new[] { "Read", "Write" }, CollectionOrdering.Matching);
        await Assert.That(types["GBX.NET.Engines.Game.Example+Kind"].MembersByName.Count).IsEqualTo(2);
    }

    [Test]
    public async Task PreservesOverloadsExplicitMembersAndGenerationOptions()
    {
        var types = await Analyze(Parse("""
            namespace GBX.NET.Engines.Plug;
            public partial class Example
            {
                public void Read(int unrelated) { }
                public void Read(GbxReader r, int v = 0) { }
                void IReadable.Read(GbxReader r, int v) { }
                int IVersionable.Version { get; set; }
                public partial void ReadWrite(GbxReaderWriter rw);
                public partial void ReadWrite(GbxReaderWriter rw) { }
                [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
                public partial class HeaderChunk09000000;
                [ArchiveGenerationOptions(PrivateSet = true)]
                public partial class Archive : IReadableWritable;
                public partial class Archive<T>;
            }
            """));

        var type = types["GBX.NET.Engines.Plug.Example"];
        await Assert.That(type.MembersByName["Read"].Length).IsEqualTo(2);
        await Assert.That(type.MembersByName["IReadable.Read"]).HasSingleItem();
        await Assert.That(type.MembersByName.ContainsKey("IVersionable.Version")).IsTrue();
        await Assert.That(type.MembersByName.ContainsKey("Version")).IsFalse();
        await Assert.That(type.MembersByName["ReadWrite"].Length).IsEqualTo(2);
        await Assert.That(type.Methods).Contains(x => x.Body is null && x.ExpressionBody is null);
        await Assert.That(types["GBX.NET.Engines.Plug.Example+HeaderChunk09000000"].Attributes).HasSingleItem();
        await Assert.That(types["GBX.NET.Engines.Plug.Example+HeaderChunk09000000"].Members).IsEmpty();
        await Assert.That(types["GBX.NET.Engines.Plug.Example+Archive"].Attributes.Single().ToString()).Contains("PrivateSet = true");
        await Assert.That(types["GBX.NET.Engines.Plug.Example+Archive`1"].Arity).IsEqualTo(1);
    }

    [Test]
    public async Task SeparatesNamespacesContainersAndGenericArities()
    {
        var types = await Analyze(Parse("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example { public partial class Key; }
                public partial class Example<T> { public partial class Key; }
                public partial class Other { public partial class Key; }
                public partial record Positional(int Version);
            }
            namespace GBX.NET.Engines.Plug
            {
                public partial class Example { public partial class Key; }
            }
            """));

        await Assert.That(types.Count).IsEqualTo(9);
        await Assert.That(types["GBX.NET.Engines.Game.Example`1+Key"].ContainingTypeKey).IsEqualTo("GBX.NET.Engines.Game.Example`1");
        await Assert.That(types["GBX.NET.Engines.Game.Positional"].PrimaryConstructors).HasSingleItem();
    }

    [Test]
    public async Task FiltersOtherNamespacesAndFileLocalTypes()
    {
        var types = await Analyze(
            Parse("namespace GBX.NET.EnginesOther.Game; public partial class Example;"),
            Parse("namespace Other; public partial class Example;"),
            Parse("namespace GBX.NET.Engines.Game; file class Local { public partial class Nested; }"),
            Parse("namespace GBX.NET.Engines.Game; public class Custom { }"));

        var type = types.Single();
        await Assert.That(type.Key).IsEqualTo("GBX.NET.Engines.Game.Custom");
        await Assert.That(type.Value.IsPartial).IsFalse();
    }

    [Test]
    public async Task AnalyzesRealEngineSourcesWithoutMetadataReferences()
    {
        var directory = TestPaths.GetEngineDirectory();
        var trees = Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories)
            .OrderBy(x => x, StringComparer.Ordinal)
            .Select(path => Parse(File.ReadAllText(path), path)).ToArray();
        var types = await Analyze(trees);

        await Assert.That(types.Count > 300).IsTrue();
        await Assert.That(types["GBX.NET.Engines.MwFoundations.CMwNod"].Attributes).Contains(x => x.Name.ToString() == "Class");
        await Assert.That(types.ContainsKey("GBX.NET.Engines.Script.CScriptTraitsMetadata+ScriptTrait")).IsTrue();
        await Assert.That(types.ContainsKey("GBX.NET.Engines.Script.CScriptTraitsMetadata+ScriptTrait`1")).IsTrue();
        await Assert.That(types["GBX.NET.Engines.Plug.CPlugMaterialCustom+Chunk0903A00A"].Methods).IsEmpty();
        await Assert.That(types["GBX.NET.Engines.Plug.CPlugVertexStream+Chunk09056000"].Methods).Contains(x => x.Identifier.ValueText == "ReadWrite");
    }

    private static SyntaxTree Parse(string source, string path = "Source.cs") =>
        CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview), path);

    private static async Task<ImmutableDictionary<string, ExistingType>> Analyze(params SyntaxTree[] trees)
    {
        // Intentionally supply no references. Engine bases, attributes and parameter
        // types are unresolved; analysis must still run and never emit C# source.
        var compilation = CSharpCompilation.Create("AnalysisOnly", trees);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            new[] { new GbxGenerator().AsSourceGenerator() },
            driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));
        driver = driver.RunGenerators(compilation);
        var result = await Assert.That(driver.GetRunResult().Results).HasSingleItem();
        await Assert.That(result.Exception).IsNull();
        await Assert.That(result.Diagnostics).IsEmpty();
        await Assert.That(result.GeneratedSources).IsEmpty();
        var step = await Assert.That(result.TrackedSteps["ExistingEngineTypes"]).HasSingleItem();
        return (await Assert.That(step.Outputs.Single().Value).IsTypeOf<ImmutableDictionary<string, ExistingType>>())!;
    }
}
