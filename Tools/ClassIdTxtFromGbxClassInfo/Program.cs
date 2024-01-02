using System.Globalization;

if (args.Length == 0)
{
    Console.WriteLine("Usage: ClassIdTxtFromGbxClassInfo <GbxClassInfo.txt> [<GbxClassInfo.txt>..]");
    Console.ReadKey();
    return;
}

var engineDict = new Dictionary<uint, string>();

await foreach (var line in File.ReadLinesAsync("Engine.txt"))
{
    var split = line.Split(' ');
    var engineIdStr = split[0];

    if (!uint.TryParse(engineIdStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var engineId))
    {
        continue;
    }

    engineDict[engineId] = split[1];
}

var classIdDict = new Dictionary<uint, string?>();

foreach (var fileName in args)
{
    await foreach (var line in File.ReadLinesAsync(fileName))
    {
        var split = line.Split('\t');
        var classIdStr = split[0];

        if (!uint.TryParse(classIdStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var classId))
        {
            continue;
        }

        var name = default(string);

        if (split.Length > 1)
        {
            name = split[1];
        }

        if (classIdDict.TryGetValue(classId, out var existingName) && string.IsNullOrWhiteSpace(name))
        {
            continue;
        }

        classIdDict[classId] = name;
    }
}

foreach (var engineGroup in classIdDict.OrderBy(x => x.Key).GroupBy(x => (x.Key >> 24) & 0xFF))
{
    _ = engineDict.TryGetValue(engineGroup.Key, out var engineName);

    Console.WriteLine($"{engineGroup.Key:X2} {engineName}");

    foreach (var kvp in engineGroup.OrderBy(x => x.Key))
    {
        Console.WriteLine($"  {(kvp.Key >> 12) & 0xFFF:X3} {kvp.Value}");
    }
}