using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool;

public interface IComplexConfig
{
    [RequiresDynamicCode("")]
    [RequiresUnreferencedCode("")]
    T Get<T>(string filePathWithoutExtension, bool cache = false) where T : class;

    [RequiresDynamicCode("")]
    [RequiresUnreferencedCode("")]
    Task<T> GetAsync<T>(string filePathWithoutExtension, bool cache = false, CancellationToken cancellationToken = default) where T : class;
}
