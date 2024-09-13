
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool.CLI;

internal sealed class ComplexConfig : IComplexConfig
{
    private readonly string configName;
    private readonly SettingsManager settings;

    public ComplexConfig(string configName, SettingsManager settings)
    {
        this.configName = configName;
        this.settings = settings;
    }

    [RequiresDynamicCode(DynamicCodeMessages.JsonSerializeMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.JsonSerializeMessage)]
    public T Get<T>(string filePathWithoutExtension)
    {
        return settings.GetFileFromConfig<T>(configName, filePathWithoutExtension);
    }

    [RequiresDynamicCode(DynamicCodeMessages.JsonSerializeMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.JsonSerializeMessage)]
    public async Task<T> GetAsync<T>(string filePathWithoutExtension, CancellationToken cancellationToken = default)
    {
        return await settings.GetFileFromConfigAsync<T>(configName, filePathWithoutExtension, cancellationToken);
    }
}
