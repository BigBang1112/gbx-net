using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/";
var locatorFile = rootPath + "locator.yml";

foreach (var fileName in args)
{
    ProcessGbx(fileName);
}

void ProcessGbx(string fileName)
{
    if (!File.Exists(fileName))
        return;

    var node = GameBox.ParseNode(fileName);

    if (node is not CGameCtnChallenge map)
        return;

    if (map.Blocks is null || map.Blocks.Count == 0)
        return;

    var locatorUrls = map.Blocks
        .Select(x => x.Skin?.PackDesc?.LocatorUrl!)
        .Where(x => x is not null)
        .Distinct();

    var newDictionary = locatorUrls.ToDictionary(x => x.ToString(), x => default(string));

    var dictionary = UpdateLocatorList(newDictionary);

    foreach (var block in map.Blocks)
    {
        ProcessBlock(block, dictionary);
    }

    Directory.CreateDirectory(rootPath + "output");

    node.Save(rootPath + "output/" + Path.GetFileName(map.GetGbx()!.FileName));
}

Dictionary<string, string?> UpdateLocatorList(Dictionary<string, string?> newDictionary)
{
    var dictionary = new Dictionary<string, string?>();

    if (File.Exists(locatorFile))
    {
        using var reader = new StreamReader(locatorFile);
        dictionary = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, string?>>(reader) ?? new Dictionary<string, string?>();
    }

    foreach (var pair in newDictionary)
    {
        if (!dictionary.ContainsKey(pair.Key))
            dictionary[pair.Key] = pair.Value;
    }

    using var writer = new StreamWriter(locatorFile);
    new YamlDotNet.Serialization.Serializer().Serialize(writer, dictionary);

    return dictionary;
}

void ProcessBlock(CGameCtnBlock? block, Dictionary<string, string?> dictionary)
{
    if (block?.Skin?.PackDesc?.LocatorUrl is null)
        return;

    var oldLocatorUrl = block.Skin.PackDesc.LocatorUrl.ToString();
    if (string.IsNullOrEmpty(oldLocatorUrl))
        return;

    var newLocatorUrl = dictionary[oldLocatorUrl];
    if (string.IsNullOrEmpty(newLocatorUrl))
        return;

    block.Skin.PackDesc = block.Skin.PackDesc with { LocatorUrl = new Uri(newLocatorUrl) };
}
