using System.Text;

namespace GBX.NET.Utils;

public class ObjFileExporter : IModelExporter, IDisposable
{
    private readonly StreamWriter objWriter;
    private readonly StreamWriter mtlWriter;
    private readonly StringWriter objFaceWriter;
    private readonly StringWriter objUvWriter;
    
    private readonly int? mergeVerticesDigitThreshold;
    private readonly string? gameDataFolderPath;

    private int offsetVert;
    private int offsetUv;

    /// <summary>
    /// Creates an exporter to .obj file.
    /// </summary>
    /// <param name="objStream">Stream to write OBJ content into.</param>
    /// <param name="mtlStream">Stream to write MTL content into.</param>
    /// <param name="mergeVerticesDigitThreshold">If set, overlapping vertices (usually between the mesh groups) will be merged. 3 or 4 give the best accuracy.</param>
    /// <param name="gameDataFolderPath">Folder for the Material.Gbx, Texture.Gbx, and .dds lookup.</param>
    /// <param name="encoding">Encoding to use.</param>
    /// <param name="leaveOpen">If to keep the streams open.</param>
    public ObjFileExporter(Stream objStream,
                           Stream mtlStream,
                           int? mergeVerticesDigitThreshold = null,
                           string? gameDataFolderPath = null,
                           Encoding? encoding = null,
                           bool leaveOpen = false)
    {
        objWriter = new StreamWriter(objStream, encoding, bufferSize: 1024, leaveOpen);
        mtlWriter = new StreamWriter(mtlStream, encoding, bufferSize: 1024, leaveOpen);
        objFaceWriter = new StringWriter();
        objUvWriter = new StringWriter();
        
        this.mergeVerticesDigitThreshold = mergeVerticesDigitThreshold;
        this.gameDataFolderPath = gameDataFolderPath;
    }

    public virtual void Export(CPlugCrystal crystal)
    {
        var invariant = System.Globalization.CultureInfo.InvariantCulture;

        mtlWriter.WriteLine("newmtl _Nothing");
        mtlWriter.WriteLine("Ka 1.000 1.000 1.000");
        mtlWriter.WriteLine("Kd 1.000 1.000 1.000");

        var validMaterials = new List<string>();

        foreach (var mat in crystal.Materials)
        {
            if (mat is null || mat.Link is null)
            {
                continue;
            }
                
            mtlWriter.WriteLine("newmtl " + mat.Link);
            validMaterials.Add(mat.Link);

            if (gameDataFolderPath is null)
            {
                continue;
            }

            var materialPath = $"{gameDataFolderPath}/{mat.Link}.Material.Gbx";

            if (!File.Exists(materialPath))
            {
                continue;
            }

            var matGbx = GameBox.ParseHeader($"{gameDataFolderPath}/{mat.Link}.Material.Gbx");

            var refTable = matGbx.RefTable;

            if (refTable is null)
            {
                continue;
            }

            var texGbxFile = refTable.Files.FirstOrDefault(x => x.FileName?.Contains("D.Texture") == true); // Needs to get diffuse, not ideal atm
                
            if (texGbxFile is null)
            {
                continue;
            }
                
            var texGbxFolder = Path.GetDirectoryName(matGbx.FileName) ?? "";
            var textGbxPath = Path.Combine(texGbxFolder, refTable.GetRelativeFolderPathToFile(texGbxFile), texGbxFile.FileName!);

            var texGbx = GameBox.ParseHeader(textGbxPath);

            var texRefTable = texGbx.RefTable;

            if (texRefTable is null)
            {
                continue;
            }

            var texDdsFile = texRefTable.Files.FirstOrDefault(x => x.FileName?.ToLower().EndsWith(".dds") == true);

            if (texDdsFile is null)
            {
                continue;
            }

            var texDdsFolder = Path.GetDirectoryName(texGbx.FileName) ?? "";
            var texDdsFilePath = Path.Combine(texDdsFolder, texRefTable.GetRelativeFolderPathToFile(texDdsFile), texDdsFile.FileName!);

            mtlWriter.WriteLine($"map_Ka \"{texDdsFilePath}\"");
            mtlWriter.WriteLine($"map_Kd \"{texDdsFilePath}\"");

            mtlWriter.WriteLine("Ka 1.000 1.000 1.000");
            mtlWriter.WriteLine("Kd 1.000 1.000 1.000");
        }

        var positionsDict = mergeVerticesDigitThreshold.HasValue
            ? new Dictionary<Vec3, int>(new Comparers.Vec3EqualityComparer(mergeVerticesDigitThreshold.Value))
            : new Dictionary<Vec3, int>();

        foreach (var layer in crystal.Layers)
        {
            objFaceWriter.WriteLine("\no {0}", layer.LayerName);

            if (layer is not CPlugCrystal.GeometryLayer geometryLayer)
            {
                continue;
            }

            var positions = geometryLayer.Crystal.Positions;

            for (int i = 0; i < positions.Length; i++)
            {
                var pos = positions[i];

                if (!positionsDict.ContainsKey(pos))
                {
                    objWriter.WriteLine("v {0} {1} {2}", pos.X.ToString(invariant), pos.Y.ToString(invariant), pos.Z.ToString(invariant));
                    positionsDict.Add(pos, positionsDict.Count);
                }
            }

            var uvs = new Dictionary<Vec2, int>();

            var currentMat = default(string);

            var counter = 0;
            
            foreach (var faceGroup in geometryLayer.Crystal.Faces.GroupBy(x => x.Group))
            {
                counter++;
                
                var group = faceGroup.Key;

                objFaceWriter.WriteLine("\ng {0}{1}", counter, group.Name);

                foreach (var face in faceGroup)
                {
                    var thisMaterial = face.Material?.Link is not null && validMaterials.Contains(face.Material.Link) ? face.Material.Link : "_Nothing";

                    if (currentMat != thisMaterial)
                    {
                        currentMat = thisMaterial;
                        objFaceWriter.WriteLine("usemtl {0}", currentMat);
                    }

                    objFaceWriter.Write("f");

                    for (int i = 0; i < face.Vertices.Length; i++)
                    {
                        var v = face.Vertices[i];

                        if (!uvs.TryGetValue(v.UV, out int uvIndex))
                        {
                            objUvWriter.WriteLine("vt {0} {1}", v.UV.X.ToString(invariant), v.UV.Y.ToString(invariant));

                            uvIndex = uvs.Count;
                            uvs.Add(v.UV, uvIndex);
                        }

                        var faceIndex = $" {positionsDict[v.Position] + 1}/{uvIndex + 1}";

                        if (i == face.Vertices.Length - 1)
                        {
                            objFaceWriter.WriteLine(faceIndex);
                        }
                        else
                        {
                            objFaceWriter.Write(faceIndex);
                        }
                    }
                }
            }
        }
        
        Merge();
    }

    public virtual void Export(CPlugTree tree)
    {
        ExportRecurse(tree);
        Merge();
    }

    private void Merge()
    {
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

        var refTable = gbx.RefTable;

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

            var fullTextureFileName = gameDataFolderPath is null
                ? Path.Combine(textureDirectory, textureFile.FileName)
                : Path.Combine(gameDataFolderPath, textureDirectory, textureFile.FileName);

            fullTextureFileName = fullTextureFileName.Replace('\\', '/');

            mtlWriter.WriteLine($"map_Ka \"{fullTextureFileName}\"");
            mtlWriter.WriteLine($"map_Kd \"{fullTextureFileName}\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public virtual void Dispose()
    {
        objWriter.Dispose();
        mtlWriter.Dispose();
        objFaceWriter.Dispose();
        objUvWriter.Dispose();
    }
}
