using GBX.NET;
using GBX.NET.Engines.Plug;
using System.Globalization;
using System.Text;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

if (args.Length == 0)
    return;

var rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location)!;
var extractPath = Path.Combine(rootPath, "Extract");

foreach (var fileName in args)
{
    Console.WriteLine(fileName);

    try
    {
        ProcessFile(fileName);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

Console.ReadKey();

void ProcessFile(string fileName)
{
    var node = GameBox.ParseNode<CPlugSolid>(fileName);

    var tree = node.Tree;

    if (tree is null)
        return;

    using var fs = File.Create(Path.Combine(extractPath, Path.GetFileName(fileName) + ".obj"));
    using var w = new StreamWriter(fs);

    using var texStream = new MemoryStream();
    using var faceStream = new MemoryStream();

    using var texWriter = new StreamWriter(texStream);
    using var faceWriter = new StreamWriter(faceStream);

    var offsetVert = 0;
    var offsetUv = 0;

    Recurse(tree);

    texWriter.Flush();
    faceWriter.Flush();

    texStream.Position = 0;
    faceStream.Position = 0;

    w.WriteLine();
    w.Flush();

    texStream.CopyTo(fs);
    faceStream.CopyTo(fs);

    void Recurse(CPlugTree? tree, float? lodValue = null)
    {
        if (tree is null)
            return;

        foreach (var plug in tree.Children)
        {
            Recurse(plug);
        }

        if (tree is CPlugTreeVisualMip mip)
        {
            foreach (var (lod, plug) in mip.Levels)
            {
                Recurse(plug, lod);
            }
        }

        var visual = tree.Visual;

        if (visual is null)
            return;

        Console.WriteLine(tree.Name);

        faceWriter.WriteLine();
        faceWriter.WriteLine("o " + tree.Name);

        if (visual is not CPlugVisualIndexed indexed)
            return;

        foreach (var vertex in indexed.Vertices)
        {
            w.WriteLine("v {0} {1} {2}", vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
        }

        if (indexed.TexCoords is not null)
        {
            foreach (var uv in indexed.TexCoords)
            {
                texWriter.WriteLine("vt {0} {1}", uv.X, uv.Y);
            }
        }

        foreach (var indicies in indexed.Indicies.Chunk(3))
        {
            var aVert = indicies[0] + 1 + offsetVert;
            var bVert = indicies[1] + 1 + offsetVert;
            var cVert = indicies[2] + 1 + offsetVert;

            var aUV = indicies[0] + 1 + offsetUv;
            var bUV = indicies[1] + 1 + offsetUv;
            var cUV = indicies[2] + 1 + offsetUv;

            if (indexed.TexCoords is null)
            {
                faceWriter.WriteLine("f {0} {1} {2}", aVert, bVert, cVert);
                continue;
            }

            faceWriter.WriteLine("f {0}/{1} {2}/{3} {4}/{5}", aVert, aUV, bVert, bUV, cVert, cUV);
        }

        offsetVert += indexed.Vertices.Length;

        if (indexed.TexCoords is not null)
            offsetUv += indexed.TexCoords.Length;
    }
}