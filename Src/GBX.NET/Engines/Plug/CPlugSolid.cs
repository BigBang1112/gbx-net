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
}
