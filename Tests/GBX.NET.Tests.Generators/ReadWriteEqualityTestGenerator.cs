using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;
using GBX.NET.Tests.Generators.Extensions;

namespace GBX.NET.Tests.Generators;

[Generator]
public class ReadWriteEqualityTestGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // GBX.NET assembly
        var gbxnet = context.Compilation.SourceModule.ReferencedAssemblySymbols.First(x => x.Name == "GBX.NET");
        var enginesNamespace = gbxnet.GlobalNamespace.NavigateToNamespace("GBX.NET.Engines");
        var engineTypes = enginesNamespace.GetNamespaceMembers().SelectMany(x => x.GetTypeMembers());
        var testsProjectDirectory = Path.GetDirectoryName(context.Compilation.SyntaxTrees.First(x => Path.GetDirectoryName(x.FilePath).EndsWith("GBX.NET.Tests")).FilePath);

        var testsNamespace = context.Compilation
            .GlobalNamespace
            .NavigateToNamespace("GBX.NET.Tests");

        var gameVersionsType = testsNamespace
            .GetTypeMembers()
            .First(x => x.Name == "GameVersions");

        var versions = gameVersionsType.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(x => x.IsConst && x.ConstantValue is string)
            .ToArray();

        var existingTestsNamespace = testsNamespace.NavigateToNamespace($"Integration.Engines");

        foreach (var type in engineTypes)
        {
            var anyVersionExists = false;

            foreach (var version in versions)
            {
                if (Directory.Exists(Path.Combine(testsProjectDirectory, "TestData", "Chunks", $"{type.Name}.{version.ConstantValue}")))
                {
                    anyVersionExists = true;
                    break;
                }
            }

            if (!anyVersionExists)
            {
                continue;
            }

            var existingClassTestsType = existingTestsNamespace
                .NavigateToNamespace(type.ContainingNamespace.Name)
                .GetTypeMembers().FirstOrDefault(x => x.Name == $"{type.Name}Tests");

            var chunkCode = GenerateChunkCode(type, existingClassTestsType, testsProjectDirectory, versions);

            context.AddSource($"{type.Name}Tests.g.cs", $@"using Xunit;
using GBX.NET.Engines.{type.ContainingNamespace.Name};

namespace GBX.NET.Tests.Integration.Engines.{type.ContainingNamespace.Name};

public partial class {type.Name}Tests
{{
    {string.Join("\n\n    ", chunkCode)}
}}
");
        }

        //var collector = engineTypes.First(x => x.Name == "CGameCtnCollector");
        //var nestedTypes = collector.GetTypeMembers();
        //var nestedChunks = nestedTypes.Where(x => x.BaseType?.Name == "Chunk" || x.BaseType?.Name == "HeaderChunk");
    }

    private IEnumerable<string> GenerateChunkCode(INamedTypeSymbol type, INamedTypeSymbol? existingClassTestsType, string testsProjectDirectory, IFieldSymbol[] versions)
    {
        foreach (var typeMember in type.GetTypeMembers())
        {
            if (typeMember.BaseType?.Name != "Chunk" && typeMember.BaseType?.Name != "HeaderChunk")
            {
                continue;
            }

            var chunkType = typeMember;
            
            var existingChunkTestsType = existingClassTestsType?.GetTypeMembers()
                .FirstOrDefault(x => x.Name == $"{chunkType.Name}Tests");

            yield return $@"public partial class {chunkType.Name}Tests
    {{
        {string.Join("\n\n        ", GenerateVersionMethods(type, existingChunkTestsType, chunkType, testsProjectDirectory, versions))}
    }}";
        }
    }

    private IEnumerable<string> GenerateVersionMethods(INamedTypeSymbol type, INamedTypeSymbol? existingChunkTestsType, INamedTypeSymbol chunkType, string testsProjectDirectory, IFieldSymbol[] versions)
    {
        foreach (var version in versions)
        {
            var chunkDataFilePath = Path.Combine(testsProjectDirectory, "TestData", "Chunks", $"{type.Name}.{version.ConstantValue}", $"{type.Name}+{chunkType.Name}.dat");

            if (!File.Exists(chunkDataFilePath))
            {
                continue;
            }

            var methodName = $"ReadAndWrite_{version.Name}_DataShouldEqual";

            var manualTestMethod = existingChunkTestsType?.GetMembers()
                .OfType<IMethodSymbol>()
                .FirstOrDefault(x => x.Name == methodName);

            var ignoreReadWriteEqualityTestAtt = default(AttributeData);

            if (manualTestMethod is not null)
            {
                var manualTestMethodAttributes = manualTestMethod.GetAttributes();

                ignoreReadWriteEqualityTestAtt = manualTestMethodAttributes
                    .FirstOrDefault(x => x.AttributeClass?.Name == "IgnoreReadWriteEqualityTestAttribute");

                if (!manualTestMethod.IsPartialDefinition)
                {
                    continue;
                }
            }

            yield return $@"[Fact{(ignoreReadWriteEqualityTestAtt is null ? "" : "(Skip = \"This chunk was ignored from the automatic test.\")")}]
        public{(manualTestMethod is null ? "" : " partial")} void {methodName}()
        {{
            // Arrange
            using ChunkReadWriteEqualityTester<{type.Name}, {type.Name}.{chunkType.Name}> chunkTester
                = new(GameVersions.{version.Name}, idWasWritten: true); // idWasWritten usage missing

            // Act
            chunkTester.ReadWrite();

            // Assert
            Assert.Equal(expected: chunkTester.InputData, actual: chunkTester.OutputStream.ToArray());
        }}";
        }
    }
}
