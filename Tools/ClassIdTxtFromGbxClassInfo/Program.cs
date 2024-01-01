using System.Globalization;

var dict = new Dictionary<uint, string?>();

await foreach (var line in File.ReadLinesAsync("ManiaPlanet32.txt"))
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

    dict[classId] = name;
}

foreach (var engineGroup in dict.OrderBy(x => x.Key).GroupBy(x => (x.Key >> 24) & 0xFF))
{
    Console.WriteLine($"{engineGroup.Key:X2}");

    foreach (var kvp in engineGroup.OrderBy(x => x.Key))
    {
        Console.WriteLine($"  {(kvp.Key >> 12) & 0xFFF:X3} {kvp.Value}");
    }
}