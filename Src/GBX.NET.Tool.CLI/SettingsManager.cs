using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace GBX.NET.Tool.CLI;

internal sealed class SettingsManager
{
    private readonly string runningDir;
    private readonly JsonSerializerContext? jsonContext;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly YamlDotNet.Serialization.IDeserializer? ymlDeserializer;
    private readonly YamlDotNet.Serialization.ISerializer? ymlSerializer;

    public SettingsManager(
        string runningDir,
        JsonSerializerContext? jsonContext,
        JsonSerializerOptions jsonOptions,
        YamlDotNet.Serialization.IDeserializer? yamlDeserializer,
        YamlDotNet.Serialization.ISerializer? yamlSerializer)
    {
        this.runningDir = runningDir;
        this.jsonContext = jsonContext;
        this.jsonOptions = jsonOptions;
        this.ymlDeserializer = yamlDeserializer;
        this.ymlSerializer = yamlSerializer;
    }

    public async Task<T> GetOrCreateFileAsync<T>(
        string fileName,
        JsonTypeInfo<T> typeInfo,
        bool resetOnException = false,
        ILogger? logger = null,
        CancellationToken cancellationToken = default) where T : new()
    {
        T result;

        var filePath = Path.Combine(runningDir, fileName + ".json");

        if (File.Exists(filePath))
        {
            logger?.LogDebug("File {FileName} exists. Deserializing...", fileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
                result = await JsonSerializer.DeserializeAsync(fs, typeInfo, cancellationToken) ?? new();
            }
            catch (Exception ex)
            {
                if (!resetOnException)
                {
                    throw;
                }

                AnsiConsole.WriteException(ex);

                result = new();
            }
        }
        else
        {
            result = new();

            logger?.LogDebug("File {FileName} does not exist.", fileName);

            var directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        logger?.LogDebug("Creating and serializing {FileName}...", fileName);

        await using var fsCreate = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fsCreate, result, typeInfo, cancellationToken);

        return result;
    }

    [RequiresDynamicCode(DynamicCodeMessages.JsonSerializeMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.JsonSerializeMessage)]
    public async Task PopulateConfigAsync(string configName, Config config, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configName);

        var configType = config.GetType();
        var existingConfig = default(Config);

        var configDir = Path.Combine(runningDir, "Config", configName);
        var mainConfigFilePath = Path.Combine(configDir, ymlDeserializer is null ? "Config.json" : "Config.yml");

        if (File.Exists(mainConfigFilePath))
        {
            await using var fs = new FileStream(mainConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            using var reader = new StreamReader(fs);

            if (ymlDeserializer is null)
            {
                existingConfig = jsonContext is null
                    ? (Config?)await JsonSerializer.DeserializeAsync(fs, configType, jsonOptions, cancellationToken)
                    : (Config?)await JsonSerializer.DeserializeAsync(fs, configType, jsonContext, cancellationToken: cancellationToken);
            }
            else
            {
                existingConfig = (Config?)ymlDeserializer.Deserialize(reader, configType);
            }
        }
        else
        {
            Directory.CreateDirectory(configDir);
        }

        foreach (var prop in configType.GetProperties().Where(x => x.CanWrite))
        {
            if (existingConfig is not null)
            {
                prop.SetValue(config, prop.GetValue(existingConfig));
            }

            if (Attribute.IsDefined(prop, typeof(ExternalFileAttribute)))
            {
                var att = prop.GetCustomAttribute<ExternalFileAttribute>()!;

                var filePathWithoutExtension = Path.Combine(configDir, att.FileName);

                if (File.Exists(filePathWithoutExtension + ".json"))
                {
                    await using var fs = new FileStream(filePathWithoutExtension + ".json", FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);

                    var value = jsonContext is null
                        ? await JsonSerializer.DeserializeAsync(fs, prop.PropertyType, jsonOptions, cancellationToken)
                        : await JsonSerializer.DeserializeAsync(fs, prop.PropertyType, jsonContext, cancellationToken: cancellationToken);

                    prop.SetValue(config, value);
                }
                else if (ymlDeserializer is not null && File.Exists(filePathWithoutExtension + ".yml"))
                {
                    await using var fs = new FileStream(filePathWithoutExtension + ".yml", FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
                    using var reader = new StreamReader(fs);

                    var value = ymlDeserializer.Deserialize(reader, prop.PropertyType);

                    prop.SetValue(config, value);
                }
            }
        }

        await using var fsCreate = new FileStream(mainConfigFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);

        if (ymlSerializer is null)
        {
            if (jsonContext is null)
            {
                await JsonSerializer.SerializeAsync(fsCreate, config, configType, jsonOptions, cancellationToken);
            }
            else
            {
                await JsonSerializer.SerializeAsync(fsCreate, config, configType, jsonContext, cancellationToken);
            }
        }
        else
        {
            using var writer = new StreamWriter(fsCreate);
            ymlSerializer.Serialize(writer, config);
        }
    }
}
