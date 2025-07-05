using GBX.NET.Extensions.Exporters;

namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid
{
    public void ExportToObj(TextWriter objWriter, TextWriter mtlWriter, int? mergeVerticesDigitThreshold = null, int lod = 0)
    {
        ObjExporter.Export(this, objWriter, mtlWriter, mergeVerticesDigitThreshold, lod);
    }

    public void ExportToObj(string objFilePath, string mtlFilePath, int? mergeVerticesDigitThreshold = null, int lod = 0)
    {
        using var objWriter = new StreamWriter(objFilePath);
        using var mtlWriter = new StreamWriter(mtlFilePath);

        ExportToObj(objWriter, mtlWriter, mergeVerticesDigitThreshold, lod);
    }

    public IEnumerable<CPlugTree> GetAllChildren(bool includeVisualMipLevels = false)
    {
        if (Tree is not CPlugTree tree)
        {
            yield break;
        }

        yield return tree;

        foreach (var child in tree.GetAllChildren(includeVisualMipLevels))
        {
            yield return child;
        }
    }

    public IEnumerable<(CPlugTree Tree, Iso4 Location)> GetAllChildrenWithLocation(int lod = 0)
    {
        if (Tree is not CPlugTree tree)
        {
            yield break;
        }

        yield return (tree, tree.Location ?? Iso4.Identity);

        foreach (var pair in tree.GetAllChildrenWithLocation(lod))
        {
            yield return pair;
        }
    }
}
