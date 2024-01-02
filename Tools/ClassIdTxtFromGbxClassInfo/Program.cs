using System.Globalization;

if (args.Length == 0)
{
    Console.WriteLine("Usage: ClassIdTxtFromGbxClassInfo <GbxClassInfo.txt> [<GbxClassInfo.txt>..]");
    Console.ReadKey();
    return;
}

var dict = new Dictionary<uint, string?>();

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

        dict[classId] = name;
    }
}

foreach (var engineGroup in dict.OrderBy(x => x.Key).GroupBy(x => (x.Key >> 24) & 0xFF))
{
    Console.WriteLine($"{engineGroup.Key:X2} {engineGroup.Key switch
    {
        0x01 => "MwFoundations",
        0x03 => "Game",
        0x04 => "Graphic",
        0x05 => "Function",
        0x06 => "Hms",
        0x07 => "Control",
        0x09 => "Plug",
        0x0A => "Scene",
        0x0B => "System",
        0x0C => "Vision",
        0x10 => "Audio",
        0x11 => "Script",
        0x12 => "Net",
        0x13 => "Input",
        0x14 => "Xml",
        0x21 => "VirtualSkipper",
        0x24 => "TrackMania",
        0x2D => "ShootMania",
        0x2E => "GameData",
        0x2F => "Meta",
        0x30 => "MetaNotPersistent",
        _ => "",
    }}");

    foreach (var kvp in engineGroup.OrderBy(x => x.Key))
    {
        Console.WriteLine($"  {(kvp.Key >> 12) & 0xFFF:X3} {kvp.Value}");
    }
}