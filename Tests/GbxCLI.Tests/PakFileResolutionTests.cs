using GBX.NET.Crypto;

namespace GbxCLI.Tests;

public class PakFileResolutionTests
{
    private const string Hash = "91A747924FFCC4A05C106FB086ECF95C34";
    private const string Name = "2dFlareAdd_Hdr.Shader.Gbx";
    private const string UnresolvedHash = "010F9E0EA7C9EBDC2B9C5FBCBF76595693";

    [Test]
    [Arguments("parse", "Data/" + Name)]
    [Arguments("eval", "data\\2dflareadd_hdr.shader.gbx")]
    [Arguments("parse", Name)]
    [Arguments("eval", Hash)]
    [Arguments("parse", "Data/" + Hash)]
    [Arguments("eval", "Data/*Flare*.gbx")]
    [Arguments("parse", "Advertising/Ad2x1A.Texture.Gbx")]
    [Arguments("eval", "Advertising/*.Texture.Gbx")]
    [Arguments("parse", "Ad2x1A.Texture.Gbx")]
    public async Task Open_ResolvedStoredBasenameOrWildcard_FindsHashedEntry(string command, string entry)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                ("Data/" + Hash, PakEntryTests.CreateGbx(false)),
                ("Media/E12BFDCD462D1D5C4FCFB167C7982078DD", PakEntryTests.CreateGbx(false))));
            using var output = new StringWriter();
            using var error = new StringWriter();
            string[] args = command == "eval"
                ? [command, archive + "/" + entry, "Node.Keys[0].TimeValue"]
                : [command, archive + "/" + entry];

            var exitCode = await Cli.RunAsync(args, output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).Contains("42.5");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("", false, "")]
    [Arguments("--resolved", false, "")]
    [Arguments("--resolved", true, "")]
    [Arguments("--unresolved", false, "")]
    [Arguments("--unresolved", true, "")]
    [Arguments("--resolved", false, "/Data/*")]
    [Arguments("--unresolved", true, "/Data/*")]
    public async Task Files_ResolutionFilter_IncludesNamedEntriesAndClassifiesHashes(string option, bool optionFirst, string selection)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            var unknownHash = new string('F', 34);
            var namedHex = unknownHash + ".Gbx";
            var nonHex = new string('Z', 34);
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                ("Data\\" + Hash.ToLowerInvariant(), []),
                ("Data\\" + UnresolvedHash, []),
                ("Unknown\\" + unknownHash, []),
                ("Named\\" + namedHex, []),
                (UnresolvedHash + "\\readme [blue].txt", []),
                (nonHex, [])));
            using var output = new StringWriter();
            using var error = new StringWriter();
            var input = archive + selection;
            string[] args = option.Length == 0 ? ["pak", "files", input]
                : optionFirst ? ["pak", "files", option, input] : ["pak", "files", input, option];
            string[] resolved = [Path.Combine("Data", Name), Path.Combine("Named", namedHex), Path.Combine(UnresolvedHash, "readme [blue].txt"), nonHex];
            string[] unresolved = [Path.Combine("Data", UnresolvedHash), Path.Combine("Unknown", unknownHash)];
            var expected = option switch
            {
                "--resolved" => resolved.AsEnumerable(),
                "--unresolved" => unresolved.AsEnumerable(),
                _ => resolved.Concat(unresolved)
            };

            if (selection.Length > 0)
            {
                expected = expected.Where(path => path.StartsWith("Data" + Path.DirectorySeparatorChar, StringComparison.Ordinal));
            }

            var exitCode = await Cli.RunAsync(args, output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).IsEqualTo(string.Join(Environment.NewLine, expected.Order(StringComparer.Ordinal)) + Environment.NewLine);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("--resolved", UnresolvedHash)]
    [Arguments("--unresolved", Hash)]
    public async Task Files_ResolutionFilterWithNoMatches_PrintsNothingAndSucceeds(string option, string storedName)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false, (storedName, [])));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "files", archive, option], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).IsEqualTo(string.Empty);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("")]
    [Arguments("/Data/*")]
    [Arguments("/Data/*Flare*.gbx")]
    [Arguments("/Data/" + Hash)]
    [Arguments("/" + Name)]
    public async Task Files_EmbeddedCatalogue_ListsResolvedPathsAndKeepsUnknownHashes(string selection)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.Pack.Gbx");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                ("Data/" + Hash.ToLowerInvariant(), []),
                (UnresolvedHash, []),
                (new string('F', 34), [])));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "files", archive + selection], output, error);
            var resolved = Path.Combine("Data", Name);
            var expected = selection.Length == 0
                ? string.Join(Environment.NewLine, new[] { resolved, UnresolvedHash, new string('F', 34) }.Order(StringComparer.Ordinal))
                : resolved;

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).IsEqualTo(expected + Environment.NewLine);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("/Data/*Flare*.gbx")]
    [Arguments("/Data/" + Hash)]
    [Arguments("/" + Name)]
    public async Task Extract_ResolvedOrStoredSelection_WritesResolvedNamesAndExactBytes(string selection)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            var contents = "Stored bytes, without parsing the resolved Gbx name"u8.ToArray();
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(true,
                ("Data/" + Hash, contents), ("unselected.txt", "unselected"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", archive + selection], output, error);
            var extracted = Path.Combine(archive + ".extracted", "Data", Name);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(Convert.ToHexString(await File.ReadAllBytesAsync(extracted))).IsEqualTo(Convert.ToHexString(contents));
            await Assert.That(Directory.GetFiles(archive + ".extracted", "*", SearchOption.AllDirectories).Length).IsEqualTo(1);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("Custom/NotInCatalogue [blue].Gbx", "", false)]
    [Arguments("Custom/NotInCatalogue [blue].Gbx", "Data/", false)]
    [Arguments("Custom/NotInCatalogue [blue].Gbx", "Data/", true)]
    [Arguments("NotInCatalogue [blue].Gbx", "Data/", true)]
    public async Task Open_UncataloguedName_ComputesHashForRelativePaths(string relativePath, string folder, bool omitFolder)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            var hash = MD5.Compute136(relativePath.Replace('/', '\\'));
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                (folder + hash, PakEntryTests.CreateGbx(false))));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var selection = (omitFolder ? "" : folder) + relativePath.ToUpperInvariant();
            var exitCode = await Cli.RunAsync(["parse", archive + "/" + selection], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).Contains("TimeValue: 42.5");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("parse")]
    [Arguments("eval")]
    public async Task Open_AmbiguousBasenameOrWildcard_ReportsCandidates(string command)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                ("One/" + Hash, PakEntryTests.CreateGbx(false)), ("Two/" + Hash, PakEntryTests.CreateGbx(false))));
            using var output = new StringWriter();
            using var error = new StringWriter();
            var entry = command == "parse" ? Name : "*Flare*.gbx";
            string[] args = command == "eval" ? [command, archive + "/" + entry, "Node"] : [command, archive + "/" + entry];

            var exitCode = await Cli.RunAsync(args, output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).Contains("ambiguous");
            await Assert.That(error.ToString()).Contains("One");
            await Assert.That(error.ToString()).Contains("Two");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Extract_AllEntries_PreservesResolvedSubdirectoriesAndUnresolvedHashes()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                ("Media/E12BFDCD462D1D5C4FCFB167C7982078DD", "resolved"u8.ToArray()),
                (UnresolvedHash, "unresolved"u8.ToArray()), ("readme.txt", "plain"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", archive], output, error);
            var root = archive + ".extracted";

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(root, "Media", "Advertising", "Ad2x1A.Texture.Gbx"))).IsEqualTo("resolved");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(root, UnresolvedHash))).IsEqualTo("unresolved");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(root, "readme.txt"))).IsEqualTo("plain");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Open_StoredPath_TakesPrecedenceOverResolvedAlias()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                (Hash, "Wrong entry"u8.ToArray()), (Name, PakEntryTests.CreateGbx(false))));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["parse", archive + "/" + Name], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).Contains("TimeValue: 42.5");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Extract_CollidingResolvedNames_FailsBeforeWritingFiles()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                (Hash, "hashed"u8.ToArray()), (Name, "named"u8.ToArray())));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "extract", archive], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).Contains("Multiple PAK entries");
            await Assert.That(Directory.Exists(archive + ".extracted")).IsFalse();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Verify_ResolvedNameAndSelection_ReportsCorruptHashedGbxByName()
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-pak-hashes-");

        try
        {
            var archive = Path.Combine(directory.FullName, "archive.pak");
            await File.WriteAllBytesAsync(archive, PakExtractionTests.CreatePak(false,
                (Hash, "Invalid signature"u8.ToArray()), ("valid.gbx", PakEntryTests.CreateGbx(false))));
            using var output = new StringWriter();
            using var error = new StringWriter();

            var exitCode = await Cli.RunAsync(["pak", "verify", archive + "/" + Name], output, error);

            await Assert.That(exitCode).IsEqualTo(1);
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
            await Assert.That(output.ToString()).Contains(Name + ":");
            await Assert.That(output.ToString()).Contains("Parsed 0 Gbx files; 1 failed; 0 non-Gbx files skipped.");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
