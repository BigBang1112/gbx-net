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

    [RequiresUnreferencedCode(SettingsManager.UnreferencedCodeMessage)]
    [RequiresDynamicCode(SettingsManager.DynamicCodeMessage)]
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

    public T GetStatically<T>(string filePathWithoutExtension, bool cache = false) where T : class
    {
        if (cache && this.cache.TryGetValue(filePathWithoutExtension, out var value))
        {
            return (T)value;
        }

        var result = settings.GetFileFromConfigStatically<T>(configName, filePathWithoutExtension);

        if (cache)
        {
            this.cache.TryAdd(filePathWithoutExtension, result);
        }

        return result;
    }

    [RequiresUnreferencedCode(SettingsManager.UnreferencedCodeMessage)]
    [RequiresDynamicCode(SettingsManager.DynamicCodeMessage)]
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

    public async Task<T> GetStaticallyAsync<T>(string filePathWithoutExtension, bool cache = false, CancellationToken cancellationToken = default) where T : class
    {
        if (cache && this.cache.TryGetValue(filePathWithoutExtension, out var value))
        {
            return (T)value;
        }

        var result = await settings.GetFileFromConfigStaticallyAsync<T>(configName, filePathWithoutExtension, cancellationToken).ConfigureAwait(false);

        if (cache)
        {
            this.cache.TryAdd(filePathWithoutExtension, result);
        }

        return result;
    }
}
