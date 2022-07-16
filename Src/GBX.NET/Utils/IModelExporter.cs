namespace GBX.NET.Utils;

public interface IModelExporter
{
    void Export(CPlugCrystal crystal);
    void Export(CPlugTree tree);
}