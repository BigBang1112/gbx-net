using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;

namespace GbxCLI.Tests;

public class ScriptCommandTests
{
    [Test]
    public async Task Script_ParsesAndSavesGbxWithArgumentsAndAsyncCode()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var input = Path.Combine(directory.FullName, "input.Gbx");
            var destination = Path.Combine(directory.FullName, "output.Gbx");
            var script = Path.Combine(directory.FullName, "inspect.csx");
            new Gbx<CMwNod>(new CMwNod()) { BodyCompression = GbxCompression.Uncompressed }.Save(input);
            await File.WriteAllTextAsync(script, """
                var gbx = await Gbx.ParseAsync(Args[0]);
                Print(gbx.Node.GetType().Name);
                Print(await Task.FromResult(new[] { 1, 2, 3 }.Sum()));
                gbx.Save(Args[1]);
                Print(File.Exists(Args[1]));
                Print(typeof(CGameCtnChallenge).Name);
                Print(typeof(GBX.NET.Crypto.Cry).Name);
                Print(typeof(GBX.NET.Hashing.CRC32).Name);
                Print(typeof(GBX.NET.LZO.Lzo).Name);
                Print(typeof(GBX.NET.ZLib.ZLib).Name);
                """);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["script", script, "--", input, destination], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).IsEqualTo(string.Join(Environment.NewLine,
                "CMwNod", "6", "True", "CGameCtnChallenge", "Cry", "CRC32", "Lzo", "ZLib", ""));
            await Assert.That(Gbx.Parse(destination).Node is not null).IsTrue();
            using var reopened = new FileStream(script, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Script_CanOpenPakWithoutAnImplicitInputObject()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var archive = Path.Combine(directory.FullName, "input.pak");
            var script = Path.Combine(directory.FullName, "archive.csx");
            await File.WriteAllBytesAsync(archive, ParseCommandTests.CreatePakData());
            await File.WriteAllTextAsync(script, """
                {
                    await using var pak = await Pak.ParseAsync(Args[0]);
                    Print(pak.Files.Values.Single().Name);
                }
                """);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["script", script, archive], output, error);

            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo($"sample [blue].txt{Environment.NewLine}");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Script_PreservesArgumentsAndWritesLiteralOutputWithoutResultPreviews()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var script = Path.Combine(directory.FullName, "args [red].csx");
            await File.WriteAllTextAsync(script, """
                foreach (var arg in Args) Print(arg);
                await Output.WriteLineAsync("done [blue]");
                42
                """);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["script", script, "--", "a b", "", "--help", "--version", "--"], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).IsEqualTo(string.Join(Environment.NewLine,
                "a b", "", "--help", "--version", "--", "done [blue]", ""));
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Script_LoadsNestedSourcesRelativeToTheirFiles()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var helpers = directory.CreateSubdirectory("helpers");
            var script = Path.Combine(directory.FullName, "main.csx");
            await File.WriteAllTextAsync(Path.Combine(helpers.FullName, "value.csx"), "var value = 42;");
            await File.WriteAllTextAsync(Path.Combine(helpers.FullName, "helper.csx"), "#load \"value.csx\"");
            await File.WriteAllTextAsync(script, """
                #load "helpers/helper.csx"
                Print(await Task.FromResult(value));
                """);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["script", script], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).IsEqualTo($"42{Environment.NewLine}");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Script_ResolvesAssemblyReferencesAndSupportsConsoleOutput()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var references = directory.CreateSubdirectory("references");
            var compilation = CSharpCompilation.Create("ScriptHelper",
                [CSharpSyntaxTree.ParseText("public static class ScriptHelper { public static int Value => 42; }")],
                [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var library = File.Create(Path.Combine(references.FullName, "ScriptHelper.dll")))
            {
                await Assert.That(compilation.Emit(library).Success).IsTrue();
            }

            var script = Path.Combine(directory.FullName, "reference.csx");
            await File.WriteAllTextAsync(script, """
                #r "references/ScriptHelper.dll"
                Console.WriteLine(ScriptHelper.Value);
                """);
            // A separate process releases Roslyn-loaded assemblies before deleting the fixture.
            var startInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            startInfo.ArgumentList.Add(typeof(Cli).Assembly.Location);
            startInfo.ArgumentList.Add("script");
            startInfo.ArgumentList.Add(script);
            using var process = Process.Start(startInfo)!;
            var output = process.StandardOutput.ReadToEndAsync();
            var error = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            await Assert.That(await error).IsEqualTo(string.Empty);
            await Assert.That(process.ExitCode).IsEqualTo(0);
            await Assert.That(await output).IsEqualTo($"42{Environment.NewLine}");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Script_EmptyFileNeedsNoGbxInputAndProducesNoOutput()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var script = Path.Combine(directory.FullName, "empty.csx");
            await File.WriteAllTextAsync(script, string.Empty);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["script", script], output, error);

            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("MissingMember", "CS0103")]
    [Arguments("throw new InvalidOperationException(\"literal [red] failure\");", "literal [red] failure")]
    public async Task Script_ReportsCompilationAndRuntimeFailures(string code, string message)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-script-");

        try
        {
            var script = Path.Combine(directory.FullName, "failure.csx");
            await File.WriteAllTextAsync(script, code);
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["script", script], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).Contains(message);
            await Assert.That(error.ToString()).DoesNotContain("\u001b");
            using var reopened = new FileStream(script, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("--help")]
    [Arguments("--version")]
    public async Task Script_InformationAfterInputDoesNotOpenMissingScript(string option)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["script", "missing.csx", option], output, error);

        await Assert.That(exitCode).IsEqualTo(0);
        await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        await Assert.That(output.ToString()).Contains(option == "--help" ? "Usage: gbx script" : "gbx 0.1.0");
    }

    [Test]
    [Arguments("")]
    [Arguments("--unknown")]
    [Arguments("missing.csx")]
    public async Task Script_RejectsInvalidInput(string argument)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        var exitCode = await Cli.RunAsync(["script", argument], output, error);

        await Assert.That(exitCode).IsEqualTo(1);
        await Assert.That(output.ToString()).IsEqualTo(string.Empty);
        await Assert.That(error.ToString()).Contains("gbx:");
    }
}
