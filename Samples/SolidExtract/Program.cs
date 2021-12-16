using GBX.NET;
using GBX.NET.Engines.Plug;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

if (args.Length == 0)
    return;

var fileName = args[0];

var node = GameBox.ParseNode<CPlugSolid>(fileName);

if (node.Tree is null)
    return;

var dirName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));

Directory.CreateDirectory(dirName);

Recurse(node.Tree);

void Recurse(CPlugTree? tree, float? distance = null)
{
    if (tree is null)
        return;

    foreach (var plug in tree.Children)
    {
        Recurse(plug, distance);
    }

    if (tree is CPlugTreeVisualMip mip)
    {
        foreach (var level in mip.Levels)
        {
            Recurse(level.Value, distance: level.Key);
        }
    }

    var visual = tree.Visual;

    if (visual is null)
        return;

    var fileName = distance is null ? tree.Name + ".obj" : $"{tree.Name}_{distance}.obj";

    using var w = new StreamWriter(Path.Combine(dirName, fileName));

    if (visual is CPlugVisualIndexed indexed)
    {
        foreach (var vertex in indexed.Vertices)
        {
            w.WriteLine("v {0} {1} {2}", vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
        }

        foreach (var indicies in indexed.Indicies.Chunk(3))
        {
            w.WriteLine("f {0} {1} {2}", indicies[0] + 1, indicies[1] + 1, indicies[2] + 1);
        }
    }
}