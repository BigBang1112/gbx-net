using GBX.NET.Comparers;
using System.Globalization;

namespace GBX.NET.Extensions.Exporters;

internal static class ObjExporter
{
    private static CultureInfo Invariant => CultureInfo.InvariantCulture;

    public static void Export(CPlugCrystal crystal, TextWriter objWriter, TextWriter mtlWriter, int? mergeVerticesDigitThreshold = null)
    {
        if (crystal.Layers is null)
        {
            return;
        }

        objWriter.WriteLine("# GBX.NET 2 - CPlugCrystal - OBJ Exporter (.obj)");
        objWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        objWriter.WriteLine();

        mtlWriter.WriteLine("# GBX.NET 2 - CPlugCrystal - OBJ Exporter (.mtl)");
        mtlWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        mtlWriter.WriteLine();

        foreach (var material in crystal.Materials.Select(x => x.MaterialUserInst).OfType<CPlugMaterialUserInst>())
        {
            mtlWriter.WriteLine("newmtl {0}", material.Link);
            mtlWriter.WriteLine("Ns 250.000000"); // Specular exponent
            mtlWriter.WriteLine("Ka 1.0000 1.0000 1.0000"); // Ambient color

            if (material.Color is null || material.Color.Length == 0)
            {
                mtlWriter.WriteLine("Kd 1.0000 1.0000 1.0000"); // Diffuse color
            }
            else
            {
                mtlWriter.WriteLine("Kd {0} {1} {2}",
                    material.Color.ElementAtOrDefault(0).ToString("0.0000", Invariant),
                    material.Color.ElementAtOrDefault(1).ToString("0.0000", Invariant),
                    material.Color.ElementAtOrDefault(2).ToString("0.0000", Invariant));
            }

            mtlWriter.WriteLine("Ks 0.5000 0.5000 0.5000"); // Specular color
            mtlWriter.WriteLine("Ke 0.0000 0.0000 0.0000");
            mtlWriter.WriteLine("Ni 1.4500"); // Optical density
            mtlWriter.WriteLine("d 1.0000"); // Dissolve
            mtlWriter.WriteLine("illum 2"); // Illumination model

            mtlWriter.WriteLine();
        }

        var positionsDict = mergeVerticesDigitThreshold.HasValue
            ? new Dictionary<Vec3, int>(new Vec3EqualityComparer(mergeVerticesDigitThreshold.Value)) : [];

        foreach (var layer in crystal.Layers)
        {
            if (layer is not CPlugCrystal.GeometryLayer { Crystal: not null } geometryLayer)
            {
                continue;
            }

            var positions = geometryLayer.Crystal.Positions;

            for (int i = 0; i < positions.Length; i++)
            {
                var pos = positions[i];

                if (!positionsDict.ContainsKey(pos))
                {
                    objWriter.WriteLine("v {0} {1} {2}", pos.X.ToString(Invariant), pos.Y.ToString(Invariant), pos.Z.ToString(Invariant));
                    positionsDict.Add(pos, positionsDict.Count);
                }
            }
        }

        var uvs = new Dictionary<Vec2, int>();

        foreach (var layer in crystal.Layers)
        {
            if (layer is not CPlugCrystal.GeometryLayer { Crystal: not null } geometryLayer)
            {
                continue;
            }

            foreach (var face in geometryLayer.Crystal.Faces)
            {
                foreach (var v in face.Vertices)
                {
                    if (uvs.ContainsKey(v.TexCoord))
                    {
                        continue;
                    }

                    objWriter.WriteLine("vt {0} {1}", v.TexCoord.X.ToString(Invariant), v.TexCoord.Y.ToString(Invariant));

                    uvs.Add(v.TexCoord, uvs.Count);
                }
            }
        }

        var counter = 0;

        foreach (var layer in crystal.Layers)
        {
            if (layer is not CPlugCrystal.GeometryLayer { Crystal: not null } geometryLayer)
            {
                continue;
            }

            foreach (var faceGroup in geometryLayer.Crystal.Faces.GroupBy(x => x.Group))
            {
                counter++;

                var group = faceGroup.Key;

                objWriter.WriteLine("g {0}{1}", group.Name, counter);

                foreach (var materialGroup in faceGroup.GroupBy(x => x.Material))
                {
                    objWriter.WriteLine("usemtl {0}", materialGroup.Key?.MaterialUserInst?.Link);

                    foreach (var face in materialGroup)
                    {
                        objWriter.Write('f');

                        for (int i = 0; i < face.Vertices.Length; i++)
                        {
                            var v = face.Vertices[i];
                            var uvIndex = uvs[v.TexCoord];

                            var faceIndex = $" {positionsDict[geometryLayer.Crystal.Positions[v.Index]] + 1}/{uvIndex + 1}";

                            objWriter.Write(faceIndex);
                        }

                        objWriter.WriteLine();
                    }

                    objWriter.WriteLine();
                }
            }
        }
    }

    public static void Export(CPlugSolid solid, TextWriter objWriter, TextWriter mtlWriter, int? mergeVerticesDigitThreshold = null, int lod = 0)
    {
        objWriter.WriteLine("# GBX.NET 2 - CPlugSolid - OBJ Exporter (.obj)");
        objWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        objWriter.WriteLine();

        mtlWriter.WriteLine("# GBX.NET 2 - CPlugSolid - OBJ Exporter (.mtl)");
        mtlWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        mtlWriter.WriteLine();

        if (solid.Tree is not CPlugTree tree)
        {
            return;
        }

        var materials = new HashSet<string>();

        var positionsDict = mergeVerticesDigitThreshold.HasValue
            ? new Dictionary<Vec3, int>(new Vec3EqualityComparer(mergeVerticesDigitThreshold.Value)) : [];

        foreach (var (t, loc) in tree.GetAllChildrenWithLocation(lod))
        {
            if (t.Visual is null)
            {
                continue;
            }

            if (t.ShaderFile is null)
            {
                continue;
            }

            if (t.Visual is not CPlugVisualIndexedTriangles visual)
            {
                continue;
            }

            var materialName = GbxPath.GetFileNameWithoutExtension(t.ShaderFile.FilePath);

            if (!materials.Contains(materialName))
            {
                mtlWriter.WriteLine("newmtl {0}", materialName);
                mtlWriter.WriteLine("Ns 250.000000"); // Specular exponent
                mtlWriter.WriteLine("Ka 1.0000 1.0000 1.0000"); // Ambient color
                mtlWriter.WriteLine("Kd 1.0000 1.0000 1.0000"); // Diffuse color
                mtlWriter.WriteLine("Ks 0.5000 0.5000 0.5000"); // Specular color
                mtlWriter.WriteLine("Ke 0.0000 0.0000 0.0000");
                mtlWriter.WriteLine("Ni 1.4500"); // Optical density
                mtlWriter.WriteLine("d 1.0000"); // Dissolve
                mtlWriter.WriteLine("illum 2"); // Illumination model

                mtlWriter.WriteLine();

                materials.Add(materialName);
            }

            foreach (var pos in visual.VertexStreams
                .SelectMany(x => x.Positions ?? [])
                .Concat(visual.Vertices.Select(x => x.Position)))
            {
                var locatedPos = new Vec3(
                    pos.X * loc.XX + pos.Y * loc.XY + pos.Z * loc.XZ + loc.TX,
                    pos.X * loc.YZ + pos.Y * loc.YY + pos.Z * loc.YZ + loc.TY,
                    pos.X * loc.ZX + pos.Y * loc.ZY + pos.Z * loc.ZZ + loc.TZ
                );

                if (positionsDict.ContainsKey(locatedPos))
                {
                    continue;
                }

                objWriter.WriteLine("v {0} {1} {2}",
                    locatedPos.X.ToString(Invariant),
                    locatedPos.Y.ToString(Invariant),
                    locatedPos.Z.ToString(Invariant));

                positionsDict.Add(locatedPos, positionsDict.Count);
            }
        }

        var uvs = new Dictionary<Vec2, int>();

        foreach (var (t, loc) in tree.GetAllChildrenWithLocation(lod))
        {
            if (t.Visual is null)
            {
                continue;
            }

            if (t.Visual is not CPlugVisualIndexedTriangles visual)
            {
                continue;
            }

            if (visual.TexCoords.Length == 0)
            {
                continue;
            }

            foreach (var uv in visual.TexCoords[0].TexCoords.Select(x => x.UV))
            {
                if (uvs.ContainsKey(uv))
                {
                    continue;
                }

                objWriter.WriteLine("vt {0} {1}",
                    uv.X.ToString(Invariant),
                    uv.Y.ToString(Invariant));

                uvs.Add(uv, uvs.Count);
            }
        }

        foreach (var (t, loc) in tree.GetAllChildrenWithLocation(lod))
        {
            if (t.Visual is null)
            {
                continue;
            }

            if (t.Visual is not CPlugVisualIndexedTriangles visual)
            {
                continue;
            }

            if (t.ShaderFile is null)
            {
                continue;
            }

            if (visual.IndexBuffer is null)
            {
                continue;
            }

            var materialName = GbxPath.GetFileNameWithoutExtension(t.ShaderFile.FilePath);

            objWriter.WriteLine("g {0}", materialName);
            objWriter.WriteLine("usemtl {0}", materialName);

            var triangleCounter = 0;

            foreach (var index in visual.IndexBuffer.Indices)
            {
                if (triangleCounter % 3 == 0)
                {
                    objWriter.Write('f');
                }

                var v = visual.Vertices[index];
                var locatedPos = new Vec3(
                    v.Position.X * loc.XX + v.Position.Y * loc.XY + v.Position.Z * loc.XZ + loc.TX,
                    v.Position.X * loc.YZ + v.Position.Y * loc.YY + v.Position.Z * loc.YZ + loc.TY,
                    v.Position.X * loc.ZX + v.Position.Y * loc.ZY + v.Position.Z * loc.ZZ + loc.TZ
                );

                var faceIndex = visual.TexCoords.Length > 0
                    ? $" {positionsDict[locatedPos] + 1}/{uvs[visual.TexCoords[0].TexCoords[index].UV] + 1}"
                    : $" {positionsDict[locatedPos] + 1}";

                objWriter.Write(faceIndex);

                if (++triangleCounter % 3 == 0)
                {
                    objWriter.WriteLine();
                }
            }

            objWriter.WriteLine();
        }
    }

    public static void Export(CPlugSolid2Model solid, TextWriter objWriter, TextWriter mtlWriter, int? mergeVerticesDigitThreshold = null, int lod = 0)
    {
        objWriter.WriteLine("# GBX.NET 2 - CPlugSolid2Model - OBJ Exporter (.obj)");
        objWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        objWriter.WriteLine();

        mtlWriter.WriteLine("# GBX.NET 2 - CPlugSolid2Model - OBJ Exporter (.mtl)");
        mtlWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        mtlWriter.WriteLine();

        var materials = new HashSet<string>();

        var positionsDict = mergeVerticesDigitThreshold.HasValue
            ? new Dictionary<Vec3, int>(new Vec3EqualityComparer(mergeVerticesDigitThreshold.Value)) : [];

        if (solid.Visuals is null || solid.Visuals.Length == 0)
        {
            throw new Exception("CPlugSolid2Model has no Visuals.");
        }

        foreach (var geom in solid.ShadedGeoms ?? [])
        {
            if (geom.Lod != -1 && geom.Lod != lod)
            {
                continue;
            }

            if (solid.Visuals?[geom.VisualIndex] is not CPlugVisualIndexedTriangles visual)
            {
                continue;
            }

            var materialName = GetMaterialName(solid, geom.MaterialIndex);

            if (!materials.Contains(materialName))
            {
                mtlWriter.WriteLine("newmtl {0}", materialName);
                mtlWriter.WriteLine("Ns 250.000000"); // Specular exponent
                mtlWriter.WriteLine("Ka 1.0000 1.0000 1.0000"); // Ambient color
                mtlWriter.WriteLine("Kd 1.0000 1.0000 1.0000"); // Diffuse color
                mtlWriter.WriteLine("Ks 0.5000 0.5000 0.5000"); // Specular color
                mtlWriter.WriteLine("Ke 0.0000 0.0000 0.0000");
                mtlWriter.WriteLine("Ni 1.4500"); // Optical density
                mtlWriter.WriteLine("d 1.0000"); // Dissolve
                mtlWriter.WriteLine("illum 2"); // Illumination model

                mtlWriter.WriteLine();

                materials.Add(materialName);
            }

            foreach (var pos in visual.VertexStreams
                .SelectMany(x => x.Positions ?? [])
                .Concat(visual.Vertices.Select(x => x.Position)))
            {
                if (positionsDict.ContainsKey(pos))
                {
                    continue;
                }

                objWriter.WriteLine("v {0} {1} {2}",
                    pos.X.ToString(Invariant),
                    pos.Y.ToString(Invariant),
                    pos.Z.ToString(Invariant));

                positionsDict.Add(pos, positionsDict.Count);
            }
        }

        var uvs = new Dictionary<Vec2, int>();

        foreach (var geom in solid.ShadedGeoms ?? [])
        {
            if (geom.Lod != -1 && geom.Lod != lod)
            {
                continue;
            }

            if (solid.Visuals?[geom.VisualIndex] is not CPlugVisualIndexedTriangles visual)
            {
                continue;
            }

            if (visual.TexCoords.Length > 0)
            {
                foreach (var uv in visual.TexCoords[0].TexCoords.Select(x => x.UV))
                {
                    if (uvs.ContainsKey(uv))
                    {
                        continue;
                    }

                    objWriter.WriteLine("vt {0} {1}",
                        uv.X.ToString(Invariant),
                        uv.Y.ToString(Invariant));

                    uvs.Add(uv, uvs.Count);
                }
            }

            foreach (var uv in visual.VertexStreams
                .SelectMany(x => x.UVs.Values.FirstOrDefault() ?? []))
            {
                if (uvs.ContainsKey(uv))
                {
                    continue;
                }

                objWriter.WriteLine("vt {0} {1}",
                    uv.X.ToString(Invariant),
                    uv.Y.ToString(Invariant));

                uvs.Add(uv, uvs.Count);
            }
        }

        foreach (var geom in solid.ShadedGeoms ?? [])
        {
            if (geom.Lod != -1 && geom.Lod != lod)
            {
                continue;
            }

            if (solid.Visuals?[geom.VisualIndex] is not CPlugVisualIndexedTriangles visual)
            {
                continue;
            }
            
            if (visual.IndexBuffer is null)
            {
                continue;
            }

            var materialName = GetMaterialName(solid, geom.MaterialIndex);
            
            objWriter.WriteLine("g {0}", materialName);
            objWriter.WriteLine("usemtl {0}", materialName);

            var triangleCounter = 0;

            foreach (var index in visual.IndexBuffer.Indices)
            {
                if (triangleCounter % 3 == 0)
                {
                    objWriter.Write('f');
                }

                var v = visual.VertexStreams.FirstOrDefault()?.Positions?[index] ?? visual.Vertices[index].Position;

                var uv = visual.TexCoords.Length == 0
                    ? (visual.VertexStreams.Count > 0 ? visual.VertexStreams[0].UVs.Values.First()[index] : (0, 0))
                    : visual.TexCoords[0].TexCoords[index].UV;

                var uvIndex = uvs[uv];

                var faceIndex = $" {positionsDict[v] + 1}/{uvIndex + 1}";

                objWriter.Write(faceIndex);

                if (++triangleCounter % 3 == 0)
                {
                    objWriter.WriteLine();
                }
            }

            objWriter.WriteLine();
        }
    }

    private static string GetMaterialName(CPlugSolid2Model solid, int materialIndex)
    {
        if (solid.CustomMaterials is { Length: > 0 } customMaterials)
        {
            return customMaterials[materialIndex].MaterialUserInst?.Link ?? "Unknown";
        }
        
        if (solid.Materials is { Length: > 0 } materialsArray)
        {
            return GbxPath.GetFileNameWithoutExtension(materialsArray[materialIndex].File?.FilePath) ?? "Unknown";
        }

        if (solid.MaterialInsts is { Length: > 0 } materialInsts)
        {
            return materialInsts[materialIndex].Link ?? "Unknown";
        }

        return "Unknown";
    }
}
