using System.Globalization;

if (args.Length == 0)
{
    Console.WriteLine("Usage: ClassIdFormatter <Wrap.txt>");
    Console.ReadKey();
    return;
}

var fileName = args[0];

var lines = File.ReadLines(fileName);

var classIdDict = new Dictionary<uint, uint>();

foreach (var line in lines)
{
    var split = line.Split(' ');
    var classIdStr = split[0];

    if (!uint.TryParse(classIdStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var classId1))
    {
        continue;
    }

    var classId2Str = split[1];

    if (!uint.TryParse(classId2Str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var classId2))
    {
        continue;
    }

    classIdDict[classId1] = classId2;
}

using var w = File.CreateText(fileName);

foreach (var (classId1, classId2) in classIdDict)
{
    var classId1Str = classId1.ToString("X8", CultureInfo.InvariantCulture);
    var classId2Str = classId2.ToString("X8", CultureInfo.InvariantCulture);

    w.WriteLine($"{classId1Str} {classId2Str}");
}