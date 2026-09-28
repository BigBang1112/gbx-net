using GBX.NET.Generators.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using Xunit;

namespace GBX.NET.Generators.Tests;

public class ExistingTypeAnalysisTests
{
    [Fact]
    public void MergesPartialsAndKeepsMembersInTheirOwnScope()
    {
        var types = Analyze(
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
        Assert.Equal(2, type.Declarations.Length);
        Assert.True(type.IsAbstract);
        Assert.True(type.IsPartial);
        Assert.Equal("CMwNod", Assert.Single(type.BaseTypes).ToString());
        Assert.Equal("Class", Assert.Single(type.Attributes).Name.ToString());
        Assert.True(type.MembersByName.ContainsKey("event"));
        Assert.True(type.MembersByName.ContainsKey("version"));
        Assert.Equal(2, type.Constructors.Count());
        Assert.Equal("Create", Assert.Single(type.Methods).Identifier.ValueText);
        Assert.NotNull(Assert.Single(type.Properties).AccessorList);

        var chunk = types["GBX.NET.Engines.Game.Example+Chunk03043000"];
        Assert.Equal(type.Key, chunk.ContainingTypeKey);
        Assert.Equal(2, chunk.Declarations.Length);
        Assert.Equal(new[] { "Read", "Write" }, chunk.Methods.Select(x => x.Identifier.ValueText));
        Assert.Equal(2, types["GBX.NET.Engines.Game.Example+Kind"].MembersByName.Count);
    }

    [Fact]
    public void PreservesOverloadsExplicitMembersAndGenerationOptions()
    {
        var types = Analyze(Parse("""
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
        Assert.Equal(2, type.MembersByName["Read"].Length);
        Assert.Single(type.MembersByName["IReadable.Read"]);
        Assert.True(type.MembersByName.ContainsKey("IVersionable.Version"));
        Assert.False(type.MembersByName.ContainsKey("Version"));
        Assert.Equal(2, type.MembersByName["ReadWrite"].Length);
        Assert.Contains(type.Methods, x => x.Body is null && x.ExpressionBody is null);
        Assert.Single(types["GBX.NET.Engines.Plug.Example+HeaderChunk09000000"].Attributes);
        Assert.Empty(types["GBX.NET.Engines.Plug.Example+HeaderChunk09000000"].Members);
        Assert.Contains("PrivateSet = true",
            Assert.Single(types["GBX.NET.Engines.Plug.Example+Archive"].Attributes).ToString());
        Assert.Equal(1, types["GBX.NET.Engines.Plug.Example+Archive`1"].Arity);
    }

    [Fact]
    public void SeparatesNamespacesContainersAndGenericArities()
    {
        var types = Analyze(Parse("""
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

        Assert.Equal(9, types.Count);
        Assert.Equal("GBX.NET.Engines.Game.Example`1",
            types["GBX.NET.Engines.Game.Example`1+Key"].ContainingTypeKey);
        Assert.Single(types["GBX.NET.Engines.Game.Positional"].PrimaryConstructors);
    }

    [Fact]
    public void FiltersOtherNamespacesAndFileLocalTypes()
    {
        var types = Analyze(
            Parse("namespace GBX.NET.EnginesOther.Game; public partial class Example;"),
            Parse("namespace Other; public partial class Example;"),
            Parse("namespace GBX.NET.Engines.Game; file class Local { public partial class Nested; }"),
            Parse("namespace GBX.NET.Engines.Game; public class Custom { }"));

        Assert.Equal("GBX.NET.Engines.Game.Custom", Assert.Single(types).Key);
        Assert.False(Assert.Single(types).Value.IsPartial);
    }

    [Fact]
    public void AnalyzesRealEngineSourcesWithoutMetadataReferences()
    {
        var directory = TestPaths.GetEngineDirectory();
        var trees = Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories)
            .OrderBy(x => x, StringComparer.Ordinal)
            .Select(path => Parse(File.ReadAllText(path), path)).ToArray();
        var types = Analyze(trees);

        Assert.True(types.Count > 300);
        Assert.Contains(types["GBX.NET.Engines.MwFoundations.CMwNod"].Attributes,
            x => x.Name.ToString() == "Class");
        Assert.True(types.ContainsKey("GBX.NET.Engines.Script.CScriptTraitsMetadata+ScriptTrait"));
        Assert.True(types.ContainsKey("GBX.NET.Engines.Script.CScriptTraitsMetadata+ScriptTrait`1"));
        Assert.Empty(types["GBX.NET.Engines.Plug.CPlugMaterialCustom+Chunk0903A00A"].Methods);
        Assert.Contains(types["GBX.NET.Engines.Plug.CPlugVertexStream+Chunk09056000"].Methods,
            x => x.Identifier.ValueText == "ReadWrite");
    }

    private static SyntaxTree Parse(string source, string path = "Source.cs") =>
        CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview), path);

    private static ImmutableDictionary<string, ExistingType> Analyze(params SyntaxTree[] trees)
    {
        // Intentionally supply no references. Engine bases, attributes and parameter
        // types are unresolved; analysis must still run and never emit C# source.
        var compilation = CSharpCompilation.Create("AnalysisOnly", trees);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            new[] { new GbxGenerator().AsSourceGenerator() },
            driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));
        driver = driver.RunGenerators(compilation);
        var result = Assert.Single(driver.GetRunResult().Results);
        Assert.Null(result.Exception);
        Assert.Empty(result.Diagnostics);
        Assert.Empty(result.GeneratedSources);
        var step = Assert.Single(result.TrackedSteps["ExistingEngineTypes"]);
        return Assert.IsType<ImmutableDictionary<string, ExistingType>>(Assert.Single(step.Outputs).Value);
    }
}
