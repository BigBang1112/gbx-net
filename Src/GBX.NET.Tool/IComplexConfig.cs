using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool;

public interface IComplexConfig
{
    [RequiresUnreferencedCode("This can cause serialization problems when AOT-compiled.")]
    [RequiresDynamicCode("Some members can get trimmed unexpectedly.")]
    T Get<T>(string filePathWithoutExtension, bool cache = false) where T : class;
    T GetStatically<T>(string filePathWithoutExtension, bool cache = false) where T : class;

    [RequiresUnreferencedCode("This can cause serialization problems when AOT-compiled.")]
    [RequiresDynamicCode("Some members can get trimmed unexpectedly.")]
    Task<T> GetAsync<T>(string filePathWithoutExtension, bool cache = false, CancellationToken cancellationToken = default) where T : class;
    Task<T> GetStaticallyAsync<T>(string filePathWithoutExtension, bool cache = false, CancellationToken cancellationToken = default) where T : class;
}
