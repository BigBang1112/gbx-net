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

        objWriter.WriteLine("# GBX.NET 2 - OBJ Exporter (.obj)");
        objWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        objWriter.WriteLine();

        mtlWriter.WriteLine("# GBX.NET 2 - OBJ Exporter (.mtl)");
        mtlWriter.WriteLine("# Exported on {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Invariant));
        mtlWriter.WriteLine();

        foreach (var material in crystal.Materials.Select(x => x.MaterialUserInst).OfType<CPlugMaterialUserInst>())
        {
            mtlWriter.WriteLine("newmtl {0}", material.Link);
            mtlWriter.WriteLine("Ns 250.000000"); // Specular exponent
            mtlWriter.WriteLine("Ka 1.0000 1.0000 1.0000"); // Ambient color

            if (material.Color is null)
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
}
