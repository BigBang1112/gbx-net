
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool.CLI;

internal sealed class ComplexConfig : IComplexConfig
{
    private readonly string configName;
    private readonly SettingsManager settings;

    private readonly ConcurrentDictionary<string, object> cache = new();

    public ComplexConfig(string configName, SettingsManager settings)
    {
        this.configName = configName;
        this.settings = settings;
    }

    [RequiresDynamicCode(DynamicCodeMessages.JsonSerializeMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.JsonSerializeMessage)]
    public T Get<T>(string filePathWithoutExtension, bool cache = false) where T : class
    {
        if (cache && this.cache.TryGetValue(filePathWithoutExtension, out var value))
        {
            return (T)value;
        }

        var result = settings.GetFileFromConfig<T>(configName, filePathWithoutExtension);

        if (cache)
        {
            this.cache.TryAdd(filePathWithoutExtension, result);
        }

        return result;
    }

    [RequiresDynamicCode(DynamicCodeMessages.JsonSerializeMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.JsonSerializeMessage)]
    public async Task<T> GetAsync<T>(string filePathWithoutExtension, bool cache = false, CancellationToken cancellationToken = default) where T : class
    {
        if (cache && this.cache.TryGetValue(filePathWithoutExtension, out var value))
        {
            return (T)value;
        }

        var result = await settings.GetFileFromConfigAsync<T>(configName, filePathWithoutExtension, cancellationToken).ConfigureAwait(false);

        if (cache)
        {
            this.cache.TryAdd(filePathWithoutExtension, result);
        }

        return result;
    }
}
