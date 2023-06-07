using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Utils;

public interface IModelExporter
{
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    void Export(CPlugCrystal crystal);
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(Lzo.TrimWarningIfDynamic)]
#endif
    void Export(CPlugTree tree);
}