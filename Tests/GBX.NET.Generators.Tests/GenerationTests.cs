using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GBX.NET.Generators.Tests;

public class GenerationTests
{
    [Test]
    public async Task CompilesWithCustomPropertiesBackingFieldsOverloadsAndAliasedSerializationMethods()
    {
        const string source = """
            using IO = GBX.NET.Serialization.GbxReaderWriter;
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private int count;
                public int Custom { get; set; }
                public Example() { Custom = 123; }
                public partial class Chunk03043001
                {
                    public override void ReadWrite(Example n, IO rw) { n.Custom++; }
                }
                public partial class Chunk03043002
                {
                    public void ReadWrite(Example n, int differentOverload) { }
                }
            }
            """;
        const string layout = """
            Example 0x03043000
            0x001
              int Custom
            0x002
              int Count
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", layout), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        await Assert.That(root.DescendantNodes().OfType<ConstructorDeclarationSyntax>()).IsEmpty();
        await Assert.That(root.DescendantNodes().OfType<PropertyDeclarationSyntax>()).DoesNotContain(x => x.Identifier.ValueText == "Custom");
        await Assert.That(root.DescendantNodes().OfType<FieldDeclarationSyntax>())
            .DoesNotContain(x => x.Declaration.Variables.Any(v => v.Identifier.ValueText == "count"));
        var chunks = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(x => x.Identifier.ValueText.StartsWith("Chunk")).ToArray();
        await Assert.That(chunks[0].Members.OfType<MethodDeclarationSyntax>().Where(x => x.Identifier.ValueText == "ReadWrite")).IsEmpty();
        await Assert.That(chunks[1].Members.OfType<MethodDeclarationSyntax>().Where(x => x.Identifier.ValueText == "ReadWrite")).HasSingleItem();
        await Assert.That(Engine(result).ToString()).Contains("rw.Int32(ref n.count)");
        await Assert.That(Engine(result).ToString()).Contains("internal override void DeepCloneFields(CMwNod clone, DeepCloneContext context)");
        await Assert.That(Engine(result).ToString()).Contains("((Example)clone).Custom = context.Clone(this.Custom)!");
        await Assert.That(Engine(result).ToString()).Contains("((Example)clone).count = context.Clone(this.count)!");
    }

    [Test]
    public async Task UsesPropertiesWhenThereIsNoBackingFieldAndKeepsUnknownsInTheirChunks()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example { public int Value { get; set; } }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version = 2
              int Value
              int
              v2+
                int Other
            """), compile: true);
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("n.Value = rw.Int32(n.Value)");
        await Assert.That(generated).Contains("public int U01;");
        await Assert.That(generated).DoesNotContain("private int u01");
        await Assert.That(generated).Contains("if (Version >= 2)");
        await Assert.That(generated).Contains("rw.Int32(ref n.other)");
    }

    [Test]
    public async Task CompilesFixedInnerJaggedArchiveArray()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000

            property int ItemCount
              get = Items::Length

            0x001
              Item[] Items

            0x002
              Item[Items::Length][] Rows

            archive Item
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("public Item[][]? Rows");
        await Assert.That(generated).Contains("get => Items?.Length?? 0;");
        await Assert.That(generated).Contains("rw.JaggedArrayReadableWritable<Item>(ref n.rows!, n.Items?.Length?? 0)");
    }

    [Test]
    public async Task CompilesJaggedArrayVariants()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000

            0x001
              int[][] Values
              id[][] Ids
              string[][] Names
              Example[][] Nodes
              Example[][] ExternalNodes (external)
              Item[][] Rows

            archive Item
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.JaggedArray<int>(ref n.values!)");
        await Assert.That(generated).Contains("rw.JaggedArrayId(ref n.ids!)");
        await Assert.That(generated).Contains("rw.JaggedArrayString(ref n.names!)");
        await Assert.That(generated).Contains("rw.JaggedArrayNodeRef<Example>(ref n.nodes!)");
        await Assert.That(generated).Contains("rw.JaggedArrayExternalNodeRef<Example>(ref n.externalNodes!)");
        await Assert.That(generated).Contains("rw.JaggedArrayReadableWritable<Item>(ref n.rows!)");
    }

    [Test]
    public async Task DemonstrationChunksDoNotGenerateFieldsOrProperties()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 (demonstration)
              int Demonstrated
              int
            0x002
              int Serialized
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).DoesNotContain("Demonstrated");
        await Assert.That(generated).DoesNotContain("U01");
        await Assert.That(generated).Contains("public int Serialized");
        await Assert.That(generated).Contains("n.serialized");
    }

    [Test]
    public async Task PartialDemonstrationGeneratesMembersWithoutSerialization()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 (demonstration: partial)
              int Shown
              int
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("public int Shown");
        await Assert.That(generated).Contains("public int U01;");
        await Assert.That(generated).DoesNotContain("rw.Int32");
    }

    [Test]
    public async Task GeneratesAttributesOnHandwrittenPartialProperties()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private int storedValue;
                public virtual partial int Value
                {
                    get => storedValue;
                    set => storedValue = value;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Value
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("[AppliedWithChunk<Chunk03043001>]");
        await Assert.That(generated).Contains("public virtual partial int Value { get; set; }");
        await Assert.That(generated).Contains("n.Value = rw.Int32(n.Value)");
        await Assert.That(generated).DoesNotContain("private int value;");
    }

    [Test]
    public async Task UsesInheritedPropertyWithoutGeneratingDuplicateStorage()
    {
        var (result, compilation) = Run("", true,
        [
            new Text("Engines/Game/Base.chunkl", """
                Base 0x03043000
                0x001
                  int Name
                """),
            new Text("Engines/Game/Derived.chunkl", """
                Derived 0x03044000
                - inherits: Base
                0x001
                  int Name (inherited)
                """)
        ]);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = result.GeneratedSources.Single(x => x.HintName == "Engines/Game/Derived.g.cs").SourceText.ToString();
        await Assert.That(generated).Contains("n.Name = rw.Int32(n.Name)");
        await Assert.That(generated).DoesNotContain("int Name");
        await Assert.That(generated).DoesNotContain("name = context.Clone");
    }

    [Test]
    public async Task AddsFormattingAttributeFromAnyFieldOccurrence()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              string DisplayName
              string PlainName
            0x002
              string DisplayName (formatted)
            archive Metadata
              string Caption (formatted)
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var properties = Engine(result).GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>()
            .Where(x => x.Identifier.ValueText is "DisplayName" or "PlainName" or "Caption")
            .ToDictionary(x => x.Identifier.ValueText);
        await Assert.That(properties["DisplayName"].AttributeLists.SelectMany(x => x.Attributes)
            .Count(x => x.Name.ToString() == "SupportsFormatting")).IsEqualTo(1);
        await Assert.That(properties["Caption"].AttributeLists.SelectMany(x => x.Attributes)
            .Count(x => x.Name.ToString() == "SupportsFormatting")).IsEqualTo(1);
        await Assert.That(properties["PlainName"].AttributeLists.SelectMany(x => x.Attributes)
            .Any(x => x.Name.ToString() == "SupportsFormatting")).IsFalse();
    }

    [Test]
    public async Task CompilesPartialComputedPropertiesWithMergedChunkMetadata()
    {
        const string source = """
            using System.Collections.Generic;
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private int value;
                private List<string>? names;
                public partial int Value { get => value; set => this.value = value; }
                public partial int Combined { get => value; set => this.value = value; }
                public partial int? Derived { get => value; }
                private partial List<string>? Names { get => names; set => names = value; }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            property int Combined
              get = Value
            property int? Derived
              get = Value
            0x001 (demonstration: partial)
              version
              v1-
                int Value
              v2-
                int Combined
              v3+
                int Value
              v1+
                string[] Names (list)
            0x002 (base: 0x001)
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        var properties = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().ToArray();
        var combined = properties.Single(x => x.Identifier.ValueText == "Combined");
        await Assert.That(combined.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>");
        await Assert.That(combined.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>");
        await Assert.That(combined.AttributeLists.SelectMany(x => x.Attributes)).Count().IsEqualTo(2);
        var derived = properties.Single(x => x.Identifier.ValueText == "Derived");
        await Assert.That(derived.ToString()).Contains("public partial int? Derived { get; }");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>(0, 1)");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>(3)");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>(0, 1)");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>(3)");
        await Assert.That(Engine(result).ToString()).Contains("private partial List<string>? Names { get; set; }");
        await Assert.That(Engine(result).ToString()).DoesNotContain("rw.Int32");
    }

    [Test]
    public async Task AnnotatesChunkPropertiesWithTheirSerializationVersionRanges()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version = 8
              int Always
              v2-
                int Legacy
              v3+
                int Modern
                v5-
                  int Middle
              v4=
                int Exact
              v0=
                int Repeated
              v3+
                int Repeated
              if Version >= 7
                int Late
              else
                int Early
              if Version < 2
                int FirstBranch
              else if Version < 5
                int MiddleBranch
              else
                int LastBranch
            0x002 (base: 0x001)
              base
              v4+
                int Extra
            0x003 (base: 0x001)
            0x004 (base: 0x001)
              v4+
                base
            0x04000000 (base: 0x001)
            0x005 (base: 0x001)
              int ChildOnly
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generatedClass = Engine(result).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(x => x.Identifier.ValueText == "Example");
        var properties = generatedClass.Members.OfType<PropertyDeclarationSyntax>()
            .ToDictionary(x => x.Identifier.ValueText, x => x.AttributeLists.SelectMany(y => y.Attributes)
                .Where(y => y.Name.ToString().StartsWith("AppliedWithChunk<", StringComparison.Ordinal))
                .Select(y => y.ToString()).ToArray());

        await Assert.That(properties["Always"]).IsEquivalentTo(new[]
        {
            "AppliedWithChunk<Chunk03043001>", "AppliedWithChunk<Chunk03043002>", "AppliedWithChunk<Chunk03043003>",
            "AppliedWithChunk<Chunk03043004>(4)", "AppliedWithChunk<Chunk04000000>"
        });
        await Assert.That(properties["Legacy"]).IsEquivalentTo(new[]
        {
            "AppliedWithChunk<Chunk03043001>(0, 2)", "AppliedWithChunk<Chunk03043002>(0, 2)",
            "AppliedWithChunk<Chunk03043003>(0, 2)", "AppliedWithChunk<Chunk04000000>(0, 2)"
        });
        await Assert.That(properties["Modern"]).Contains("AppliedWithChunk<Chunk03043001>(3)");
        await Assert.That(properties["Modern"]).Contains("AppliedWithChunk<Chunk03043004>(4)");
        await Assert.That(properties["Middle"]).Contains("AppliedWithChunk<Chunk03043001>(3, 5)");
        await Assert.That(properties["Middle"]).Contains("AppliedWithChunk<Chunk03043004>(4, 5)");
        await Assert.That(properties["Exact"]).Contains("AppliedWithChunk<Chunk03043001>(4, 4)");
        await Assert.That(properties["Repeated"]).Contains("AppliedWithChunk<Chunk03043001>(0, 0)");
        await Assert.That(properties["Repeated"]).Contains("AppliedWithChunk<Chunk03043001>(3)");
        await Assert.That(properties["Late"]).Contains("AppliedWithChunk<Chunk03043001>(7)");
        await Assert.That(properties["Early"]).Contains("AppliedWithChunk<Chunk03043001>(0, 6)");
        await Assert.That(properties["FirstBranch"]).Contains("AppliedWithChunk<Chunk03043001>(0, 1)");
        await Assert.That(properties["MiddleBranch"]).Contains("AppliedWithChunk<Chunk03043001>(2, 4)");
        await Assert.That(properties["LastBranch"]).Contains("AppliedWithChunk<Chunk03043001>(5)");
        await Assert.That(properties["Extra"]).IsEquivalentTo(new[] { "AppliedWithChunk<Chunk03043002>(4)" });
        await Assert.That(properties["ChildOnly"]).IsEquivalentTo(new[] { "AppliedWithChunk<Chunk03043005>" });
    }

    [Test]
    public async Task PreservesWireWidthsDataIdentifiersContextualArchivesAndGameVersions()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                public int Value { get; set; }
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
                public partial class Chunk03043001 { }
                public partial class VisualId { }
            }
            """;
        var (result, _) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM2020.v3, MP4]
              int16 Value
              ident Identity
              VisualId Visual
              data[] Bytes
              Context Context
            0x002
              int16 Value
              filetime Date
            archive VisualId
              id Name
            archive Context (contextual)
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("w.Write((short)n.Value)");
        await Assert.That(generated).Contains("n.Value = (int)rw.Int16((short)n.Value)");
        await Assert.That(generated).Contains("r.ReadIdent()");
        await Assert.That(generated).DoesNotContain("IdAsStringent");
        await Assert.That(generated).DoesNotContain("VisualIdAsString");
        await Assert.That(generated).Contains("w.WriteWritable<Context, Example>(n.context, n)");
        await Assert.That(generated).Contains("w.WriteData(n.bytes)");
        await Assert.That(generated).Contains("private byte[]? bytes");
        await Assert.That(generated).Contains("private DateTime? date");
        await Assert.That(generated).Contains("[ChunkGameVersion(GameVersion.TM2020 | GameVersion.MP4, 3, -1)]");
    }

    [Test]
    public async Task EmitsArchiveInheritanceEnumsAndDeclaredThrowMessages()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            archive
              int Value
              v2+
                throw (type: InvalidOperationException, message: Unsupported format)
            archive Base
              int Value
            archive Derived (inherits: Base)
              base
              int Other
            enum Kind
              First = 1
              Second
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("partial class Derived : Base, IReadableWritable");
        await Assert.That(generated).Contains("public override void ReadWrite(GbxReaderWriter rw, int v = 0)");
        await Assert.That(generated).Contains("base.ReadWrite(rw, v)");
        await Assert.That(generated).Contains("base.DeepCloneArchiveFields(clone, context)");
        await Assert.That(generated).Contains("if (v >= 2)");
        await Assert.That(generated).Contains("throw new InvalidOperationException(\"Unsupported format\")");
        await Assert.That(generated).Contains("First = 1,");
    }

    [Test]
    public async Task PassesCurrentVersionToArchivesAndHonorsExplicitOverrides()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version
              Outer Data
              Outer Legacy (version: 0)
            archive Outer
              version
              v1+ (archive)
                int OwnField
              v2+ (chunk)
                int CallerField
              Inner Child
            archive Inner
              v1+
                int ChildField
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.ReadableWritable<Outer>(ref n.data, version: Version)");
        await Assert.That(generated).Contains("rw.ReadableWritable<Outer>(ref n.legacy, version: 0)");
        await Assert.That(generated).Contains("partial class Outer : IReadableWritable, IReadable, IWritable, IVersionable");
        await Assert.That(generated).Contains("if (Version >= 1)");
        await Assert.That(generated).Contains("if (v >= 2)");
        await Assert.That(generated).Contains("rw.ReadableWritable<Inner>(ref this.child, version: Version)");
    }

    [Test]
    public async Task PassesInheritedArchiveVersionToNestedArchives()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            archive
              Outer Data
            archive Outer
              Inner Child
            archive Inner
              v1+
                int Value
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.ReadableWritable<Outer>(ref this.data, version: v)");
        await Assert.That(generated).Contains("rw.ReadableWritable<Inner>(ref this.child, version: v)");
        await Assert.That(generated).Contains("object IDeepCloneable.DeepClone(DeepCloneContext context)");
        await Assert.That(generated).Contains("if (v >= 1)");
    }

    [Test]
    public async Task EmitsEncapsulatedBlocksForCombinedAndSeparateSerialization()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
                public partial class Chunk03043002 { }
            }
            """;
        var (result, _) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              block (encapsulated)
                int Value
            0x002
              block (encapsulated)
                int Other
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.Encapsulated(rw =>");
        await Assert.That(generated).Contains("r.ReadEncapsulated(r =>");
        await Assert.That(generated).Contains("w.WriteEncapsulated(w =>");
    }

    [Test]
    public async Task RequiresVersionSourceWhenAnArchiveHasTwoVersions()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            archive Named
              version
              v1+
                int Value
            """));

        await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" &&
            x.GetMessage().Contains("must select (archive) or (chunk)"));
    }

    [Test]
    public async Task ReportsInvalidOverlapAndDuplicateLayoutsWithoutCrashingSiblingGenerationOrManagers()
    {
        var (result, _) = Run("namespace GBX.NET.Engines.Game; public class Bad { }",
            new Text("Engines/Game/Bad.chunkl", "Bad 0x03042000"),
            new Text("Engines/Game/Good.chunkl", "Good 0x03043000"),
            new Text("Engines/Plug/Good.chunkl", "Good 0x09001000"),
            new Text("Resources/CollectionId.txt", "1 Stadium"));
        await Assert.That(result.Exception).IsNull();
        await Assert.That(result.Diagnostics.Length).IsEqualTo(2);
        foreach (var diagnostic in result.Diagnostics)
        {
            await Assert.That(diagnostic.Id).IsEqualTo("GBXNETGEN200");
        }
        await Assert.That(result.GeneratedSources.Where(x => x.HintName.StartsWith("Engines/"))).HasSingleItem();
        await Assert.That(result.GeneratedSources).Contains(x => x.HintName == "Managers/ClassManager.g.cs");
    }

    [Test]
    public async Task PreservesChunkOffsetsInRemappingAndReadsExtensionsAfterClassNames()
    {
        var (result, _) = Run("",
            new Text("Engines/Game/Example.chunkl", "Example 0x03043000"),
            new Text("Resources/CollectionId.txt", "1 Stadium"),
            new Text("Resources/Wrap.txt", "03043000 24003000"),
            new Text("Resources/Unwrap.txt", "24003000 03043000"),
            new Text("Resources/Extensions.txt", "03043000 Example Challenge.Gbx Map.Gbx"));
        var manager = result.GeneratedSources.Single(x => x.HintName == "Managers/ClassManager.g.cs").SourceText.ToString();
        await Assert.That(manager).Contains("classId & 0xFFF");
        await Assert.That(manager).Contains("}) | chunkPart");
        await Assert.That(manager).Contains("0x24003000 => 0x03043000");
        await Assert.That(manager).Contains("new string[] { \"Challenge.Gbx\", \"Map.Gbx\" }");
        await Assert.That(manager).DoesNotContain("\"Example Challenge.Gbx Map.Gbx\"");
    }

    [Test]
    public async Task ExposesBuiltInCollectionsAndFallsBackToCustomCollectionNames()
    {
        var (result, output) = Run("", new Text("Resources/CollectionId.txt", "0 Speed\n6 Stadium\n"));
        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = result.GeneratedSources.Single(x => x.HintName == "Managers/CollectionManager.g.cs").SyntaxTree;
        var handwritten = CSharpSyntaxTree.ParseText("""
            namespace GBX.NET.Managers;
            public static partial class CollectionManager
            {
                public static System.Collections.Generic.IDictionary<int, string> CustomCollections { get; } = new System.Collections.Generic.Dictionary<int, string>();
                public static partial string? GetName(int id);
            }
            """, (CSharpParseOptions)generated.Options);
        var compilation = CSharpCompilation.Create("CollectionFixture", new[] { generated, handwritten }, output.References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Managers.CollectionManager")!;
        var collections = (await Assert.That(type.GetProperty("Collections")!.GetValue(null))
            .IsTypeOf<System.Collections.Immutable.ImmutableDictionary<int, string>>())!;
        await Assert.That(collections.Count).IsEqualTo(2);
        await Assert.That(collections[6]).IsEqualTo("Stadium");
        var custom = (IDictionary<int, string>)type.GetProperty("CustomCollections")!.GetValue(null)!;
        custom.Add(6, "Custom Stadium");
        custom.Add(999, "Custom Environment");
        var getName = type.GetMethod("GetName")!;
        await Assert.That(getName.Invoke(null, new object[] { 6 })).IsEqualTo("Stadium");
        await Assert.That(getName.Invoke(null, new object[] { 999 })).IsEqualTo("Custom Environment");
        await Assert.That(getName.Invoke(null, new object[] { 1000 })).IsNull();
        await Assert.That(collections.ContainsKey(999)).IsFalse();
    }

    [Test]
    public async Task CompilesConstructorAndAccessorsAndStoresValueInsteadOfAssigningTheSetterParameter()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            constructor
              Value = 42
            property int Double
              get = Value * 2
              set
                Value = value / 2
            0x001
              int Value = 1
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var assembly = System.Reflection.Assembly.Load(stream.ToArray());
        var type = assembly.GetType("GBX.NET.Engines.Game.Example")!;
        var instance = Activator.CreateInstance(type)!;
        await Assert.That(type.GetProperty("Double")!.GetValue(instance)).IsEqualTo(84);
        type.GetProperty("Double")!.SetValue(instance, 100);
        await Assert.That(type.GetProperty("Value")!.GetValue(instance)).IsEqualTo(50);
    }

    [Test]
    public async Task SupportingGeneratorsHandleUnrelatedCompilations()
    {
        var compilation = CSharpCompilation.Create("Empty");
        var generators = new IIncrementalGenerator[] { new GbxReaderWriterGenerator(), new GbxReaderAndWriterArrayGenerator(), new InputWithTimeGenerator(), new CScriptTraitsMetadataMethodGenerator() };
        var result = CSharpGeneratorDriver.Create(generators.Select(x => x.AsSourceGenerator())).RunGenerators(compilation).GetRunResult();
        await Assert.That(result.Results).All(x => x.Exception == null);
        await Assert.That(result.Diagnostics).IsEmpty();
    }

    private static SyntaxTree Engine(GeneratorRunResult result) => result.GeneratedSources.Single(x => x.HintName.StartsWith("Engines/")).SyntaxTree;
    private static async Task AssertNoErrors(Compilation compilation) => await Assert.That(compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error)).IsEmpty();

    private static (GeneratorRunResult Result, Compilation Compilation) Run(string source, Text file, bool compile = false) => Run(source, compile, new[] { file });
    private static (GeneratorRunResult Result, Compilation Compilation) Run(string source, params Text[] files) => Run(source, false, files);
    private static (GeneratorRunResult Result, Compilation Compilation) Run(string source, bool compile, Text[] files)
    {
        var options = new CSharpParseOptions(LanguageVersion.Preview);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(x => MetadataReference.CreateFromFile(x));
        var compilation = CSharpCompilation.Create("Fixture", new[] { CSharpSyntaxTree.ParseText(source, options), CSharpSyntaxTree.ParseText(compile ? Support : "", options) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { new GbxGenerator().AsSourceGenerator() }, additionalTexts: files, parseOptions: options);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);
        return (driver.GetRunResult().Results.Single(), output);
    }

    private sealed class Text(string path, string source) : AdditionalText
    {
        public override string Path => path;
        public override SourceText GetText(CancellationToken cancellationToken = default) => SourceText.From(source);
    }

    private const string Support = """
        namespace TmEssentials { }
        namespace GBX.NET
        {
            public interface IClass { }
            public interface IVersionable { int Version { get; set; } }
            public class External<T> where T : GBX.NET.Engines.Game.CMwNod { }
        }
        namespace GBX.NET.Attributes
        {
            public class ClassAttribute(uint id) : System.Attribute { }
            public class ChunkAttribute(uint id) : System.Attribute { }
            public class HexadecimalAttribute : System.Attribute { }
            [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
            public class AppliedWithChunkAttribute<T> : System.Attribute
            {
                public AppliedWithChunkAttribute(int sinceVersion = 0) { }
                public AppliedWithChunkAttribute(int sinceVersion, int upToVersion) { }
            }
        }
        namespace GBX.NET.Serialization
        {
            public class GbxReader { }
            public class GbxWriter { }
            public interface IReadable { void Read(GbxReader reader, int version = 0); }
            public interface IWritable { void Write(GbxWriter writer, int version = 0); }
            public interface IReadableWritable { void ReadWrite(GbxReaderWriter readerWriter, int version = 0); }
            public interface IDeepCloneable { object DeepClone(DeepCloneContext context); }
            public class DeepCloneContext
            {
                public void Register(object source, object clone) { }
                public T Clone<T>(T source) => source;
                public T[] CloneArray<T>(T[] source) => source;
                public System.Collections.Generic.List<T> CloneList<T>(System.Collections.Generic.IEnumerable<T> source) => new(source);
                public System.Collections.Generic.Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>> source) where TKey : notnull => new();
                public System.Collections.Generic.HashSet<T> CloneHashSet<T>(System.Collections.Generic.ISet<T> source) => new(source);
            }
            public class GbxReaderWriter : System.IDisposable
            {
                public GbxReaderWriter() { }
                public GbxReaderWriter(GbxReader reader) { }
                public GbxReaderWriter(GbxWriter writer) { }
                public void Int32(ref int value) { }
                public int Int32(int value) => value;
                public void VersionInt32(GBX.NET.IVersionable value) { }
                public void ArrayReadableWritable<T>(ref T[]? value) where T : IReadableWritable, new() { }
                public void JaggedArrayReadableWritable<T>(ref T[][]? value, int? innerLength = null, int? outerLength = null, int version = 0)
                    where T : IReadable, IWritable, new() { }
                public void JaggedArray<T>(ref T[][]? value, int? innerLength = null, int? outerLength = null) where T : struct { }
                public void JaggedArrayId(ref string[][]? value, int? innerLength = null, int? outerLength = null) { }
                public void JaggedArrayString(ref string[][]? value, int? innerLength = null, int? outerLength = null) { }
                public void JaggedArrayNodeRef<T>(ref T?[][]? value, int? innerLength = null, int? outerLength = null) where T : GBX.NET.IClass { }
                public void JaggedArrayExternalNodeRef<T>(ref GBX.NET.External<T>[][]? value, int? innerLength = null, int? outerLength = null) where T : GBX.NET.Engines.Game.CMwNod { }
                public void Dispose() { }
            }
        }
        namespace GBX.NET.Serialization.Chunking
        {
            public interface IChunk { }
            public class Chunk : IChunk
            {
                internal virtual void DeepCloneFields(Chunk clone, GBX.NET.Serialization.DeepCloneContext context) { }
            }
            public class Chunk<T> : Chunk
            {
                public virtual uint Id => 0;
                public virtual void ReadWrite(T node, GBX.NET.Serialization.GbxReaderWriter rw) { }
            }
        }
        namespace GBX.NET.Engines.Game
        {
            public class CMwNod : GBX.NET.IClass
            {
                internal virtual GBX.NET.Serialization.Chunking.IChunk? NewChunk(uint id) => null;
                internal virtual void DeepCloneFields(CMwNod clone, GBX.NET.Serialization.DeepCloneContext context) { }
            }
        }
        """;
}
