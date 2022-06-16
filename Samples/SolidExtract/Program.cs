using GBX.NET;
using GBX.NET.Engines.Plug;
using Microsoft.Extensions.Logging;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

if (args.Length == 0)
    return;

var rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location)!;
var extractPath = Path.Combine(rootPath, "Extract");

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
    builder.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

Directory.CreateDirectory(extractPath);

foreach (var fileName in args)
{
    logger.LogInformation("{fileName}", fileName);

    try
    {
        ProcessFile(fileName);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Exception for {fileName}", fileName);
    }
}

Console.ReadKey();

void ProcessFile(string fileName)
{
    var node = GameBox.ParseNode<CPlugSolid>(fileName, logger: logger);
    var refTable = node.GetGbx()!.GetRefTable();

    if (node.Tree is not CPlugTree tree)
    {
        return;
    }

    var fullFileNameWithoutExt = Path.Combine(extractPath, Path.GetFileName(fileName));

    using var fs = File.Create(fullFileNameWithoutExt + ".obj");
    using var w = new StreamWriter(fs);

    using var normalStream = new MemoryStream();
    using var texStream = new MemoryStream();
    using var faceStream = new MemoryStream();

    using var normalWriter = new StreamWriter(normalStream);
    using var texWriter = new StreamWriter(texStream);
    using var faceWriter = new StreamWriter(faceStream);

    using var mtlWriter = new StreamWriter(fullFileNameWithoutExt + ".mtl");

    w.WriteLine($"mtlib {Path.GetFileName(fileName)}.mtl");

    var offsetVert = 0;
    var offsetUv = 0;

    Recurse(tree);

    normalWriter.Flush();
    texWriter.Flush();
    faceWriter.Flush();

    normalStream.Position = 0;
    texStream.Position = 0;
    faceStream.Position = 0;

    w.WriteLine();
    w.Flush();

    normalStream.CopyTo(fs);

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

        logger.LogInformation("{name}", tree.Name);

        faceWriter.WriteLine();
        faceWriter.WriteLine("o " + tree.Name);

        WriteMaterial(tree.Shader as CPlugMaterial);

        if (visual is not CPlugVisualIndexedTriangles indexed)
            return;

        foreach (var vertex in indexed.Vertices)
        {
            w.WriteLine("v {0} {1} {2}", vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
            normalWriter.WriteLine("vn {0} {1} {2}", vertex.U01.X, vertex.U01.Y, vertex.U01.Z);
        }

        if (indexed.TexCoords is not null)
        {
            var texCoords = indexed.TexCoords.FirstOrDefault();

            if (texCoords is not null)
            {
                foreach (var uv in texCoords)
                {
                    texWriter.WriteLine("vt {0} {1}", uv.X, uv.Y);
                }
            }
        }

        foreach (var indicies in indexed.Indices.Chunk(3))
        {
            var aVert = indicies[0] + 1 + offsetVert;
            var bVert = indicies[1] + 1 + offsetVert;
            var cVert = indicies[2] + 1 + offsetVert;

            var aUV = indicies[0] + 1 + offsetUv;
            var bUV = indicies[1] + 1 + offsetUv;
            var cUV = indicies[2] + 1 + offsetUv;

            if (indexed.TexCoords is null || indexed.TexCoords.Length == 0)
            {
                faceWriter.WriteLine("f {0} {1} {2}", aVert, bVert, cVert);
                continue;
            }

            faceWriter.WriteLine("f {0}/{1}/{0} {2}/{3}/{2} {4}/{5}/{4}", aVert, aUV, bVert, bUV, cVert, cUV);
        }

        offsetVert += indexed.Vertices.Length;

        if (indexed.TexCoords is not null && indexed.TexCoords.Length > 0)
            offsetUv += indexed.TexCoords[0].Length;
    }

    void WriteMaterial(CPlugMaterial? material)
    {
        if (material is null)
            return;

        var materialName = Path.GetFileNameWithoutExtension(
            Path.GetFileNameWithoutExtension(material.GetGbx()!.FileName));

        faceWriter.WriteLine("usemtl " + materialName);
        mtlWriter.WriteLine("newmtl " + materialName);

        if (material.CustomMaterial is null)
            return;

        var textures = material.CustomMaterial.Textures;

        if (textures is null)
            return;

        var diffuse = textures.FirstOrDefault(x => x.Name == "Diffuse")?.Bitmap;

        if (diffuse is null)
            diffuse = textures.FirstOrDefault()?.Bitmap;

        if (diffuse is null)
            return;

        var gbx = diffuse.GetGbx();

        if (gbx is null)
            return;

        var refTable = gbx.GetRefTable();

        if (refTable is null)
            return;

        var textureFile = refTable.GetAllFiles()
            .FirstOrDefault(x => x.FileName?.ToLower().EndsWith(".dds") == true);

        if (textureFile is null)
            return;

        if (textureFile.FileName is null)
            return;

        mtlWriter.WriteLine("Ka 1.000 1.000 1.000");
        mtlWriter.WriteLine("Kd 1.000 1.000 1.000");

        try
        {
            var textureDirectory = Path.Combine(Path.GetDirectoryName(diffuse.GetGbx()!.FileName) ?? "",
                refTable.GetRelativeFolderPathToFile(textureFile));

            var fullTextureFileName = Path.Combine(textureDirectory, textureFile.FileName)
                .Replace('\\', '/');

            mtlWriter.WriteLine($"map_Ka \"{fullTextureFileName}\"");
            mtlWriter.WriteLine($"map_Kd \"{fullTextureFileName}\"");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception during path extract");
        }
    }
}