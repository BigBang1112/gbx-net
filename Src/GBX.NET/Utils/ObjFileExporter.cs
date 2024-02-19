using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace GBX.NET.Utils;

public class ObjFileExporter : IModelExporter, IDisposable
{
    private static readonly CultureInfo invariant = CultureInfo.InvariantCulture;
    private static readonly Encoding utf8NoBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    private readonly StreamWriter objWriter;
    private readonly StreamWriter? mtlWriter;
    private readonly StringWriter objFaceWriter;
    private readonly StringWriter objUvWriter;
    
    private readonly int? mergeVerticesDigitThreshold;
    private readonly string? gameDataFolderPath;
    private readonly bool corruptedMaterials;
    
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
    /// <param name="corruptedMaterials">If to use a different way to handle corrupted material files (via header reference table, to avoid body parse). Exists due to TMTurbo problems. Can give much less accurate results.</param>
    public ObjFileExporter(Stream objStream,
                           Stream? mtlStream,
                           int? mergeVerticesDigitThreshold = null,
                           string? gameDataFolderPath = null,
                           Encoding? encoding = null,
                           bool leaveOpen = false,
                           bool corruptedMaterials = false)
    {
        objWriter = new StreamWriter(objStream, encoding ?? utf8NoBOM, bufferSize: 1024, leaveOpen);

        if (mtlStream is not null)
        {
            mtlWriter = new StreamWriter(mtlStream, encoding ?? utf8NoBOM, bufferSize: 1024, leaveOpen);
        }
        
        objFaceWriter = new StringWriter();
        objUvWriter = new StringWriter();
        
        this.mergeVerticesDigitThreshold = mergeVerticesDigitThreshold;
        this.gameDataFolderPath = gameDataFolderPath;
        this.corruptedMaterials = corruptedMaterials;
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    public virtual void Export(CPlugCrystal crystal)
    {
        var invariant = CultureInfo.InvariantCulture;

        mtlWriter?.WriteLine("newmtl _Nothing");
        mtlWriter?.WriteLine("Ka 1.000 1.000 1.000");
        mtlWriter?.WriteLine("Kd 1.000 1.000 1.000");

        var validMaterials = new List<string>();

        foreach (var mat in crystal.Materials)
        {
            if (mat.MaterialUserInst is null || mat.MaterialUserInst.Link is null)
            {
                continue;
            }
                
            mtlWriter?.WriteLine("newmtl " + mat.MaterialUserInst.Link);
            validMaterials.Add(mat.MaterialUserInst.Link);

            if (gameDataFolderPath is null)
            {
                continue;
            }

            var materialPath = $"{gameDataFolderPath}/{mat.MaterialUserInst.Link}.Material.Gbx";

            if (!File.Exists(materialPath))
            {
                continue;
            }

            var matGbx = GameBox.ParseHeader($"{gameDataFolderPath}/{mat.MaterialUserInst.Link}.Material.Gbx");

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

            mtlWriter?.WriteLine($"map_Ka \"{texDdsFilePath}\"");
            mtlWriter?.WriteLine($"map_Kd \"{texDdsFilePath}\"");

            mtlWriter?.WriteLine("Ka 1.000 1.000 1.000");
            mtlWriter?.WriteLine("Kd 1.000 1.000 1.000");
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
                    var thisMaterial = face.Material?.MaterialUserInst?.Link is not null && validMaterials.Contains(face.Material.MaterialUserInst.Link)
                        ? face.Material.MaterialUserInst.Link : "_Nothing";

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
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    internal virtual void Export(CPlugTree tree, CPlugSolid? mainNode)
    {
        var gbx = default(GameBox);

        if (corruptedMaterials)
        {
            gbx = mainNode?.GetGbx();
        }

        ExportRecurse(tree, gbx);
        Merge();
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    public virtual void Export(CPlugTree tree)
    {
        Export(tree, mainNode: null);
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    public virtual void Export(CPlugSolid2Model solid2)
    {
        for (var i = 0; i < solid2.Visuals.Length; i++)
        {
            var visual = solid2.Visuals[i];

            if (visual is null)
            {
                continue;
            }
            
            var material = solid2.Materials.Length > i ? solid2.Materials[i] : null;
            var materialName = solid2.MaterialIds.Length > i ? solid2.MaterialIds[i] : null;

            WriteVisualWithMaterial(i.ToString(), visual, material?.Node, material?.File, materialName, solid2.GetGbx());
        }

        Merge();
    }
    
    public virtual void Export(CPlugSurface surface)
    {
        if (surface.Surf is not CPlugSurface.Mesh mesh)
        {
            return;
        }

        foreach (var v in mesh.Vertices)
        {
            objWriter.WriteLine("v {0} {1} {2}", v.X.ToString(invariant), v.Y.ToString(invariant), v.Z.ToString(invariant));
        }

        objWriter.WriteLine();

        foreach (var (_, t, _, _, _) in mesh.CookedTriangles)
        {
            objWriter.WriteLine("f {0} {1} {2}", t.X + 1, t.Y + 1, t.Z + 1);
        }
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

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private void ExportRecurse(CPlugTree tree, GameBox? gbx)
    {
        foreach (var t in tree.Children)
        {
            if (t is null)
            {
                continue;
            }

            ExportRecurse(t, gbx);
        }

        if (tree is CPlugTreeVisualMip mip)
        {
            foreach (var t in mip.Levels)
            {
                ExportRecurse(t.Value, gbx);
            }
        }

        var visual = tree.Visual;

        if (visual is null)
        {
            return;
        }
        
        WriteVisualWithMaterial(tree.Name, visual, corruptedMaterials ? null : tree.Shader as CPlugMaterial, tree.ShaderFile, materialName: null, gbx);
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private void WriteVisualWithMaterial(string? visualName,
        CPlugVisual visual, CPlugMaterial? material, GameBoxRefTable.File? materialFile, string? materialName, GameBox? gbx)
    {
        objFaceWriter.WriteLine($"\no {visualName}");

        if (corruptedMaterials)
        {
            if (materialName is null)
            {
                WriteCorruptedMaterialToMtl(materialFile, gbx);
            }
            else
            {
                mtlWriter?.WriteLine("newmtl " + materialName);
                mtlWriter?.WriteLine("Ka 1.000 1.000 1.000");
                mtlWriter?.WriteLine("Kd 1.000 1.000 1.000");
            }
        }
        else if (material is not null)
        {
            WriteMaterialToMtl(material);
        }
        else if (materialName is not null)
        {
            mtlWriter?.WriteLine("newmtl " + materialName);
            mtlWriter?.WriteLine("Ka 1.000 1.000 1.000");
            mtlWriter?.WriteLine("Kd 1.000 1.000 1.000");
        }

        if (visual is not CPlugVisualIndexedTriangles indexed)
        {
            return;
        }

        if (indexed.VertexStreams.Length > 0)
        {
            foreach (var pos in indexed.VertexStreams[0].Positions)
            {
                objWriter.WriteLine("v {0} {1} {2}",
                    pos.X.ToString(invariant),
                    pos.Y.ToString(invariant),
                    pos.Z.ToString(invariant));
                //objNormalWriter.WriteLine("vn {0} {1} {2}", vertex.U01.X, vertex.U01.Y, vertex.U01.Z);
            }
        }
        else
        {
            foreach (var v in indexed.Vertices)
            {
                objWriter.WriteLine("v {0} {1} {2}",
                    v.Position.X.ToString(invariant),
                    v.Position.Y.ToString(invariant),
                    v.Position.Z.ToString(invariant));
                //objNormalWriter.WriteLine("vn {0} {1} {2}", vertex.U01.X, vertex.U01.Y, vertex.U01.Z);
            }
        }

        var texCoords = indexed.TexCoords.FirstOrDefault();

        if (texCoords is null)
        {
            if (indexed.VertexStreams.Length > 0)
            {
                var vertStream = indexed.VertexStreams[0];

                if (vertStream.UVs.Count > 0)
                {
                    foreach (var uv in vertStream.UVs.First().Value ?? [])
                    {
                        objUvWriter.WriteLine("vt {0} {1}",
                            uv.X.ToString(invariant),
                            uv.Y.ToString(invariant));
                    }
                }
            }
        }
        else
        {
            foreach (var tex in texCoords)
            {
                objUvWriter.WriteLine("vt {0} {1}",
                    tex.UV.X.ToString(invariant),
                    tex.UV.Y.ToString(invariant));
            }
        }

        if (indexed.Indices is not null)
        {
            var enumerator = indexed.Indices.AsEnumerable().GetEnumerator();

            while (enumerator.MoveNext())
            {
                var a = Math.Abs(enumerator.Current);
                if (!enumerator.MoveNext()) break;
                var b = Math.Abs(enumerator.Current);
                if (!enumerator.MoveNext()) break;
                var c = Math.Abs(enumerator.Current);

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
            offsetUv += indexed.TexCoords[0].Count;
        }
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    private void WriteCorruptedMaterialToMtl(GameBoxRefTable.File? materialFile, GameBox? gbx)
    {
        if (materialFile is null || gbx is null)
        {
            return;
        }

        objFaceWriter.WriteLine("usemtl " + materialFile.FileName);
        mtlWriter?.WriteLine("newmtl " + materialFile.FileName);

        var materialFullFileName = Path.Combine(Path.GetDirectoryName(gbx.FileName)!,
            gbx.RefTable?.GetRelativeFolderPathToFile(materialFile),
            materialFile.FileName);

        if (!File.Exists(materialFullFileName))
        {
            return;
        }
        var matRefTable = GameBox.ParseHeader(materialFullFileName).RefTable ?? throw new Exception();
        var diffuseFile = matRefTable.Files
            .FirstOrDefault(x => x.FileName?.EndsWith("D.Texture.gbx", StringComparison.OrdinalIgnoreCase) == true);

        if (diffuseFile is null)
        {
            return;
        }
        
        var textureFullFileName = Path.Combine(Path.GetDirectoryName(materialFullFileName)!,
            matRefTable.GetRelativeFolderPathToFile(diffuseFile), diffuseFile.FileName);

        if (GameBox.ParseNode(textureFullFileName) is CPlugBitmap texture)
        {
            WriteTextureForMaterial(texture);
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
        mtlWriter?.WriteLine("newmtl " + materialName);

        if (material.CustomMaterial is null)
        {
            return;
        }

        var textures = material.CustomMaterial.Textures;

        if (textures is null)
        {
            return;
        }

        var diffuse = textures.FirstOrDefault(x => x.Name == "Diffuse" || x.Name == "Blend3" || x.Name.StartsWith("Soil"))?.Texture;

        diffuse ??= textures.FirstOrDefault()?.Texture;

        if (diffuse is null)
        {
            return;
        }

        WriteTextureForMaterial(diffuse);
    }

    private void WriteTextureForMaterial(CPlugBitmap diffuse)
    {
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

        mtlWriter?.WriteLine("Ka 1.000 1.000 1.000");
        mtlWriter?.WriteLine("Kd 1.000 1.000 1.000");

        try
        {
            var textureDirectory = Path.Combine(Path.GetDirectoryName(diffuse.GetGbx()!.PakFileName) ?? "",
                refTable.GetRelativeFolderPathToFile(textureFile));

            var fullTextureFileName = gameDataFolderPath is null
                ? Path.Combine(textureDirectory, textureFile.FileName)
                : Path.Combine(gameDataFolderPath, textureDirectory, textureFile.FileName);

            if (corruptedMaterials)
            {
                fullTextureFileName = Path.Combine(Path.GetDirectoryName(gbx.FileName)!, textureDirectory, textureFile.FileName);
            }

            fullTextureFileName = fullTextureFileName.Replace('\\', '/');

            mtlWriter?.WriteLine($"map_Ka \"{fullTextureFileName}\"");
            mtlWriter?.WriteLine($"map_Kd \"{fullTextureFileName}\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public virtual void Dispose()
    {
        objWriter.Dispose();
        mtlWriter?.Dispose();
        objFaceWriter.Dispose();
        objUvWriter.Dispose();
    }
}
