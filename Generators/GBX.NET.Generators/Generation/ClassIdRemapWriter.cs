using System.Globalization;

namespace GBX.NET.Generators.Generation;

internal static class ClassIdRemapWriter
{
    private static readonly string[] locations = ["Root", "HeaderChunk", "BodyChunk", "NodeRef", "MetaRef"];

    private static readonly DiagnosticDescriptor invalidMapping = new("GBXNETGEN303",
        "Invalid class ID remap profile", "{0}", "GBX.NET.Generators", DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static void Generate(SourceProductionContext context, string source, string aliases)
    {
        var profiles = new Dictionary<string, Dictionary<(string Location, uint ClassId), uint>>(StringComparer.Ordinal)
        {
            ["Latest"] = new()
        };

        var wrap = ReadLines(aliases).ToDictionary(static row => ParseId(row[0]), static row => ParseId(row[1]));
        Dictionary<(string Location, uint ClassId), uint>? profile = null;
        var declared = new HashSet<(string? Location, uint ClassId)>();

        foreach (var row in ReadLines(source))
        {
            if (row[0][0] == '@')
            {
                var name = row[0].Substring(1);

                if (!SyntaxFacts.IsValidIdentifier(name) || profiles.ContainsKey(name) || row.Skip(1).Any(parent => !profiles.ContainsKey(parent)))
                {
                    Report(context, row);
                    return;
                }

                profile = new();
                profiles.Add(name, profile);
                declared.Clear();

                foreach (var parent in row.Skip(1))
                {
                    foreach (var entry in profiles[parent])
                    {
                        profile[entry.Key] = entry.Value;
                    }
                }

                continue;
            }

            var location = row.Length == 3 ? row[2] : null;

            if (profile is null || row.Length is < 2 or > 3 || (location is not null && !locations.Contains(location))
                || !TryParseId(row[0], out var classId) || !TryParseId(row[1], out var rawClassId)
                || (wrap.TryGetValue(rawClassId, out var normalized) ? normalized : rawClassId) != classId
                || !declared.Add((location, classId)))
            {
                Report(context, row);
                return;
            }

            foreach (var site in location is null ? locations : [location])
            {
                if (classId == rawClassId)
                {
                    profile.Remove((site, classId));
                }
                else
                {
                    profile[(site, classId)] = rawClassId;
                }
            }
        }

        if (profiles.Count > 64)
        {
            context.ReportDiagnostic(Diagnostic.Create(invalidMapping, Location.None, "At most 64 class ID remap modes are supported"));
            return;
        }

        Write(context, profiles);
    }

    private static void Write(SourceProductionContext context, Dictionary<string, Dictionary<(string Location, uint ClassId), uint>> profiles)
    {
        var modes = profiles.Keys.ToArray();
        var code = CodeWriter.Generated("GBX.NET.Serialization");
        code.Open("internal static partial class ClassIdRemapping");

        code.Open("private static partial ulong GetModeMask(ClassIdRemapMode mode)");
        code.Open("return mode switch");
        for (var i = 0; i < modes.Length; i++)
        {
            code.Line($"ClassIdRemapMode.{modes[i]} => 0x{1UL << i:X}UL,");
        }
        code.Line("_ => 0");
        code.Close(";");
        code.Close();

        code.Open("private static partial ClassIdRemapMode GetFirstMode(ulong candidates)");
        for (var i = 1; i < modes.Length; i++)
        {
            code.Line($"if ((candidates & 0x{1UL << i:X}UL) != 0) return ClassIdRemapMode.{modes[i]};");
        }
        code.Line("return ClassIdRemapMode.Latest;");
        code.Close();

        code.Open("private static partial uint GetWireClassId(ClassIdRemapMode mode, ClassIdLocation location, uint classId)");
        code.Open("return (mode, classId) switch");
        foreach (var mode in modes)
        {
            foreach (var group in profiles[mode].GroupBy(static entry => (entry.Key.ClassId, Raw: entry.Value)))
            {
                var guard = group.Count() == locations.Length ? "" : $" when location is {LocationPattern(group.Select(static entry => entry.Key.Location))}";
                code.Line($"(ClassIdRemapMode.{mode}, 0x{group.Key.ClassId:X8}){guard} => 0x{group.Key.Raw:X8}u,");
            }
        }
        code.Line("_ => classId");
        code.Close(";");
        code.Close();

        code.Open("internal static partial ulong MatchModes(ClassIdLocation location, uint classId, uint rawClassId)");
        code.Open("return classId switch");
        foreach (var classId in profiles.Values.SelectMany(static profile => profile.Keys).Select(static entry => entry.ClassId).Distinct().OrderBy(static id => id))
        {
            var sites = locations.Select(site => (Site: site, Cases: BuildMatchCases(profiles, modes, site, classId)));

            foreach (var group in sites.GroupBy(static item => item.Cases))
            {
                var guard = group.Count() == locations.Length ? "" : $" when location is {LocationPattern(group.Select(static item => item.Site))}";
                code.Line($"0x{classId:X8}{guard} => rawClassId switch {{ {group.Key}, _ => 0 }},");
            }
        }
        code.Line("_ => classId == rawClassId ? ulong.MaxValue : 0");
        code.Close(";");
        code.Close();
        code.Close();

        context.AddSource("Serialization/ClassIdRemapping.g.cs", code.ToString());
    }

    private static string BuildMatchCases(Dictionary<string, Dictionary<(string Location, uint ClassId), uint>> profiles, string[] modes, string location, uint classId)
    {
        var matches = new Dictionary<uint, ulong>();
        for (var i = 0; i < modes.Length; i++)
        {
            var raw = profiles[modes[i]].TryGetValue((location, classId), out var id) ? id : classId;
            matches.TryGetValue(raw, out var mask);
            matches[raw] = mask | (1UL << i);
        }

        return string.Join(", ", matches.OrderBy(static entry => entry.Key).Select(static entry => $"0x{entry.Key:X8} => 0x{entry.Value:X}UL"));
    }

    private static string LocationPattern(IEnumerable<string> names) => string.Join(" or ", names.Select(static name => "ClassIdLocation." + name));

    private static IEnumerable<string[]> ReadLines(string source) => source.Split('\n')
        .Select(static line => line.Split(["//"], StringSplitOptions.None)[0].Trim().Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries))
        .Where(static row => row.Length > 0);

    private static uint ParseId(string value) => uint.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

    private static bool TryParseId(string value, out uint id)
        => uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out id) && (id & 0xFFF) == 0;

    private static void Report(SourceProductionContext context, string[] row)
        => context.ReportDiagnostic(Diagnostic.Create(invalidMapping, Location.None, $"Invalid row in ClassIdRemap.txt: {string.Join(" ", row)}"));
}
