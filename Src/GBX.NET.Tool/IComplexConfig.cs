using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool;

public interface IComplexConfig
{
    [RequiresDynamicCode("")]
    [RequiresUnreferencedCode("")]
    T Get<T>(string filePathWithoutExtension);

    [RequiresDynamicCode("")]
    [RequiresUnreferencedCode("")]
    Task<T> GetAsync<T>(string filePathWithoutExtension, CancellationToken cancellationToken = default);
}
