namespace GBX.NET.Utils;

internal class ObjFileExporter : IDisposable
{
    private readonly StreamWriter objWriter;
    private readonly StreamWriter mtlWriter;
    private readonly StringWriter objFaceWriter;
    private readonly StringWriter objUvWriter;

    private int offsetVert;
    private int offsetUv;

    public ObjFileExporter(Stream objStream, Stream mtlStream)
    {
        objWriter = new StreamWriter(objStream);
        mtlWriter = new StreamWriter(mtlStream);
        objFaceWriter = new StringWriter();
        objUvWriter = new StringWriter();
    }

    public void Export(CPlugTree tree)
    {
        ExportRecurse(tree);

        objWriter.WriteLine();
        //objWriter.WriteLine(objNormalWriter);
#if NET6_0_OR_GREATER
        objWriter.WriteLine(objUvWriter.GetStringBuilder());
        objWriter.WriteLine(objFaceWriter.GetStringBuilder());
#else
        objWriter.WriteLine(objUvWriter.ToString());
        objWriter.WriteLine(objFaceWriter.ToString());
#endif
    }

    private void ExportRecurse(CPlugTree tree)
    {
        foreach (var t in tree.Children)
        {
            if (t is null)
            {
                continue;
            }

            ExportRecurse(t);
        }

        if (tree is CPlugTreeVisualMip mip)
        {
            foreach (var t in mip.Levels)
            {
                ExportRecurse(t.Value);
            }
        }

        var visual = tree.Visual;

        if (visual is null)
        {
            return;
        }

        objFaceWriter.WriteLine($"\no {tree.Name}");

        if (tree.Shader is CPlugMaterial material)
        {
            WriteMaterialToMtl(material);
        }

        if (visual is not CPlugVisualIndexedTriangles indexed)
        {
            return;
        }

        foreach (var v in indexed.Vertices)
        {
            objWriter.WriteLine("v {0} {1} {2}", v.Position.X, v.Position.Y, v.Position.Z);
            //objNormalWriter.WriteLine("vn {0} {1} {2}", vertex.U01.X, vertex.U01.Y, vertex.U01.Z);
        }

        if (indexed.TexCoords is not null)
        {
            var texCoords = indexed.TexCoords.FirstOrDefault();

            if (texCoords is not null)
            {
                foreach (var uv in texCoords)
                {
                    objUvWriter.WriteLine("vt {0} {1}", uv.X, uv.Y);
                }
            }
        }

        if (indexed.Indices is not null)
        {
            var enumerator = indexed.Indices.AsEnumerable().GetEnumerator();

            while (enumerator.MoveNext())
            {
                var a = enumerator.Current;
                if (!enumerator.MoveNext()) break;
                var b = enumerator.Current;
                if (!enumerator.MoveNext()) break;
                var c = enumerator.Current;

                var aVert = a + 1 + offsetVert;
                var bVert = b + 1 + offsetVert;
                var cVert = c + 1 + offsetVert;

                var aUV = a + 1 + offsetUv;
                var bUV = b + 1 + offsetUv;
                var cUV = c + 1 + offsetUv;

                if (indexed.TexCoords is null || indexed.TexCoords.Length == 0)
                {
                    objFaceWriter.WriteLine("f {0} {1} {2}", aVert, bVert, cVert);
                    continue;
                }

                objFaceWriter.WriteLine("f {0}/{1} {2}/{3} {4}/{5}", aVert, aUV, bVert, bUV, cVert, cUV);
            }
        }

        offsetVert += indexed.Vertices.Length;

        if (indexed.TexCoords is not null && indexed.TexCoords.Length > 0)
        {
            offsetUv += indexed.TexCoords[0].Length;
        }
    }

    private void WriteMaterialToMtl(CPlugMaterial material)
    {
        if (material.CustomMaterial is null)
        {
            return;
        }

        var materialName = Path.GetFileNameWithoutExtension(
            Path.GetFileNameWithoutExtension(material.GetGbx()!.PakFileName));

        objFaceWriter.WriteLine("usemtl " + materialName);
        mtlWriter.WriteLine("newmtl " + materialName);

        if (material.CustomMaterial is null)
        {
            return;
        }

        var textures = material.CustomMaterial.Textures;

        if (textures is null)
        {
            return;
        }

        var diffuse = textures.FirstOrDefault(x => x.Name == "Diffuse" || x.Name == "Blend3" || x.Name.StartsWith("Soil"))?.Bitmap;

        if (diffuse is null)
        {
            diffuse = textures.FirstOrDefault()?.Bitmap;
        }

        if (diffuse is null)
        {
            return;
        }

        var gbx = diffuse.GetGbx();

        if (gbx is null)
        {
            return;
        }

        var refTable = gbx.GetRefTable();

        if (refTable is null)
        {
            return;
        }

        var textureFile = refTable.Files
            .FirstOrDefault(x => x.FileName?.ToLower().EndsWith(".dds") == true);

        if (textureFile?.FileName is null)
        {
            return;
        }

        mtlWriter.WriteLine("Ka 1.000 1.000 1.000");
        mtlWriter.WriteLine("Kd 1.000 1.000 1.000");

        try
        {
            var textureDirectory = Path.Combine(Path.GetDirectoryName(diffuse.GetGbx()!.PakFileName) ?? "",
                refTable.GetRelativeFolderPathToFile(textureFile));

            var fullTextureFileName = Path.Combine(@"E:\Games\TmUnitedForever\GameData", textureDirectory, textureFile.FileName)
                .Replace('\\', '/');

            mtlWriter.WriteLine($"map_Ka \"{fullTextureFileName}\"");
            mtlWriter.WriteLine($"map_Kd \"{fullTextureFileName}\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public void Dispose()
    {
        objWriter.Dispose();
        mtlWriter.Dispose();
        objFaceWriter.Dispose();
        objUvWriter.Dispose();
    }
}
