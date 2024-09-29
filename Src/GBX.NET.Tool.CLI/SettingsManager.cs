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
    internal const string DynamicCodeMessage = "If JsonContext is not set, or YAML is used and YmlContext is not set, this can cause serialization problems when AOT-compiled.";
    internal const string UnreferencedCodeMessage = "If JsonContext is not set, or YAML is used and YmlContext is not set, some members can get trimmed unexpectedly.";
    
    private readonly string runningDir;
    private readonly JsonSerializerContext? jsonContext;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly YamlDotNet.Serialization.IDeserializer? ymlDeserializer;
    private readonly YamlDotNet.Serialization.ISerializer? ymlSerializer;
    private readonly bool isYmlStatic;

    public SettingsManager(
        string runningDir,
        JsonSerializerContext? jsonContext,
        JsonSerializerOptions jsonOptions,
        YamlDotNet.Serialization.IDeserializer? ymlDeserializer,
        YamlDotNet.Serialization.ISerializer? ymlSerializer,
        YamlDotNet.Serialization.StaticContext? ymlContext)
    {
        this.runningDir = runningDir;
        this.jsonContext = jsonContext;
        this.jsonOptions = jsonOptions;
        this.ymlDeserializer = ymlDeserializer;
        this.ymlSerializer = ymlSerializer;
        isYmlStatic = ymlContext is not null;
    }

    public async Task<T> GetOrCreateJsonFileAsync<T>(
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

    public T GetOrCreateYmlFile<T>(string fileName, bool resetOnException = false, ILogger? logger = null) where T : new()
    {
        T result;

        if (ymlDeserializer is null || ymlSerializer is null)
        {
            throw new InvalidOperationException("YAML deserializer or serializer is not available.");
        }

        var filePath = Path.Combine(runningDir, fileName + ".yml");

        if (File.Exists(filePath))
        {
            logger?.LogDebug("File {FileName} exists. Deserializing...", fileName);

            try
            {
                using var fs = File.OpenRead(filePath);
                using var reader = new StreamReader(fs);

                result = ymlDeserializer.Deserialize<T>(reader);
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

        using var writer = File.CreateText(filePath);

        ymlSerializer.Serialize(writer, result);

        return result;
    }

    [RequiresDynamicCode(DynamicCodeMessage)]
    [RequiresUnreferencedCode(UnreferencedCodeMessage)]
    public async Task PopulateConfigAsync(string configName, Config config, CancellationToken cancellationToken)
    {
        await PopulateConfigAsync(configName, config, async (stream, configType,  token) => jsonContext is null
            ? (Config?)await JsonSerializer.DeserializeAsync(stream, configType, jsonOptions, cancellationToken)
            : (Config?)await JsonSerializer.DeserializeAsync(stream, configType, jsonContext, cancellationToken),
            async (stream, config, configType, token) =>
            {
                if (jsonContext is null)
                {
                    await JsonSerializer.SerializeAsync(stream, config, configType, jsonOptions, cancellationToken);
                }
                else
                {
                    await JsonSerializer.SerializeAsync(stream, config, configType, jsonContext, cancellationToken);
                }
            },
            cancellationToken);
    }

    public async Task PopulateConfigStaticallyAsync(string configName, Config config, CancellationToken cancellationToken)
    {
        if (jsonContext is null)
        {
            throw new InvalidOperationException("JsonSerializerContext is not available.");
        }

        if (isYmlStatic)
        {
            throw new InvalidOperationException("YAML StaticContext is not available.");
        }

        await PopulateConfigAsync(configName, config,
            async (stream, configType, token) => (Config?)await JsonSerializer.DeserializeAsync(stream, configType, jsonContext, cancellationToken),
            async (stream, config, configType, token) => await JsonSerializer.SerializeAsync(stream, config, configType, jsonContext, cancellationToken),
            cancellationToken);
    }

    private async Task PopulateConfigAsync(string configName, Config config,
        Func<Stream, Type, CancellationToken, Task<object?>> deserializeJsonFunc,
        Func<Stream, Config, Type, CancellationToken, Task> serializeJsonFunc,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configName);

        var configType = config.GetType();
        var existingConfig = default(Config);

        var configDir = Path.Combine(runningDir, "Config", configName);
        var mainConfigFilePath = Path.Combine(configDir, ymlDeserializer is null ? "Config.json" : "Config.yml");

        if (File.Exists(mainConfigFilePath))
        {
            await using var fs = new FileStream(mainConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);

            if (ymlDeserializer is null)
            {
                existingConfig = (Config?)await deserializeJsonFunc(fs, configType, cancellationToken);
            }
            else
            {
                using var reader = new StreamReader(fs);
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
                if (!prop.CanWrite)
                {
                    continue;
                }

                var att = prop.GetCustomAttribute<ExternalFileAttribute>()!;

                var value = await GetFileFromConfigAsync(configName, att.FileNameWithoutExtension, prop.PropertyType, deserializeJsonFunc, cancellationToken);

                if (value is not null)
                {
                    prop.SetValue(config, value);
                }
            }
        }

        await using var fsCreate = new FileStream(mainConfigFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);

        if (ymlSerializer is null)
        {
            await serializeJsonFunc(fsCreate, config, configType, cancellationToken);
        }
        else
        {
            using var writer = new StreamWriter(fsCreate);
            ymlSerializer.Serialize(writer, config);
        }
    }

    [RequiresDynamicCode(DynamicCodeMessage)]
    [RequiresUnreferencedCode(UnreferencedCodeMessage)]
    public async Task<object> GetFileFromConfigAsync(string configName, string relativeFilePathWithoutExtension, Type type, CancellationToken cancellationToken)
    {
        return await GetFileFromConfigAsync(configName, relativeFilePathWithoutExtension, type,
            async (stream, type, token) => jsonContext is null
                ? await JsonSerializer.DeserializeAsync(stream, type, jsonOptions, cancellationToken)
                : await JsonSerializer.DeserializeAsync(stream, type, jsonContext, cancellationToken),
            cancellationToken);
    }

    public async Task<object> GetFileFromConfigStaticallyAsync(string configName, string relativeFilePathWithoutExtension, Type type, CancellationToken cancellationToken)
    {
        if (jsonContext is null)
        {
            throw new InvalidOperationException("JsonSerializerContext is not available.");
        }

        if (isYmlStatic)
        {
            throw new InvalidOperationException("YAML StaticContext is not available.");
        }

        return await GetFileFromConfigAsync(configName, relativeFilePathWithoutExtension, type,
            async (stream, type, token) => await JsonSerializer.DeserializeAsync(stream, type, jsonContext, cancellationToken: cancellationToken),
            cancellationToken);
    }

    private async Task<object> GetFileFromConfigAsync(
        string configName,
        string relativeFilePathWithoutExtension,
        Type type,
        Func<Stream, Type, CancellationToken, Task<object?>> deserializeJsonFunc,
        CancellationToken cancellationToken)
    {
        var configDir = Path.Combine(runningDir, "Config", configName);
        var filePathWithoutExtension = Path.Combine(configDir, relativeFilePathWithoutExtension);

        if (ymlDeserializer is not null && File.Exists(filePathWithoutExtension + ".yml"))
        {
            await using var fs = new FileStream(filePathWithoutExtension + ".yml", FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            using var reader = new StreamReader(fs);

            return ymlDeserializer.Deserialize(reader, type) ?? throw new InvalidOperationException("Deserialization failed.");
        }

        if (File.Exists(filePathWithoutExtension + ".json"))
        {
            await using var fs = new FileStream(filePathWithoutExtension + ".json", FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);

            var obj = await deserializeJsonFunc(fs, type, cancellationToken);

            return obj ?? throw new InvalidOperationException("Deserialization failed.");
        }

        throw new FileNotFoundException("File not found.", filePathWithoutExtension);
    }

    [RequiresDynamicCode(DynamicCodeMessage)]
    [RequiresUnreferencedCode(UnreferencedCodeMessage)]
    public async Task<T> GetFileFromConfigAsync<T>(
        string configName,
        string relativeFilePathWithoutExtension,
        CancellationToken cancellationToken)
    {
        return await GetFileFromConfigAsync<T>(configName, relativeFilePathWithoutExtension,
            async (stream, type, token) => jsonContext is null
                ? await JsonSerializer.DeserializeAsync(stream, type, jsonOptions, cancellationToken)
                : await JsonSerializer.DeserializeAsync(stream, type, jsonContext, cancellationToken),
            cancellationToken);
    }

    public async Task<T> GetFileFromConfigStaticallyAsync<T>(
        string configName,
        string relativeFilePathWithoutExtension,
        CancellationToken cancellationToken)
    {
        if (jsonContext is null)
        {
            throw new InvalidOperationException("JsonSerializerContext is not available.");
        }

        if (isYmlStatic)
        {
            throw new InvalidOperationException("YAML StaticContext is not available.");
        }

        return await GetFileFromConfigAsync<T>(configName, relativeFilePathWithoutExtension,
            async (stream, type, token) => await JsonSerializer.DeserializeAsync(stream, type, jsonContext, cancellationToken),
            cancellationToken);
    }

    private async Task<T> GetFileFromConfigAsync<T>(string configName, string relativeFilePathWithoutExtension,
        Func<Stream, Type, CancellationToken, Task<object?>> deserializeJsonFunc,
        CancellationToken cancellationToken)
    {
        return (T)await GetFileFromConfigAsync(configName, relativeFilePathWithoutExtension, typeof(T), deserializeJsonFunc, cancellationToken);
    }

    [RequiresDynamicCode(DynamicCodeMessage)]
    [RequiresUnreferencedCode(UnreferencedCodeMessage)]
    public object GetFileFromConfig(string configName, string relativeFilePathWithoutExtension, Type type)
    {
        return GetFileFromConfig(configName, relativeFilePathWithoutExtension, type, (stream, type) => jsonContext is null
            ? JsonSerializer.Deserialize(stream, type, jsonOptions)
            : JsonSerializer.Deserialize(stream, type, jsonContext));
    }

    public object GetFileFromConfigStatically(string configName, string relativeFilePathWithoutExtension, Type type)
    {
        if (jsonContext is null)
        {
            throw new InvalidOperationException("JsonSerializerContext is not available.");
        }

        if (isYmlStatic)
        {
            throw new InvalidOperationException("YAML StaticContext is not available.");
        }

        return GetFileFromConfig(configName, relativeFilePathWithoutExtension, type,
            (stream, type) => JsonSerializer.Deserialize(stream, type, jsonContext));
    }

    private object GetFileFromConfig(
        string configName,
        string relativeFilePathWithoutExtension,
        Type type,
        Func<Stream, Type, object?> deserializeJsonFunc)
    {
        var configDir = Path.Combine(runningDir, "Config", configName);
        var filePathWithoutExtension = Path.Combine(configDir, relativeFilePathWithoutExtension);

        if (ymlDeserializer is not null && File.Exists(filePathWithoutExtension + ".yml"))
        {
            using var fs = File.OpenRead(filePathWithoutExtension + ".yml");
            using var reader = new StreamReader(fs);

            return ymlDeserializer.Deserialize(reader, type) ?? throw new InvalidOperationException("Deserialization failed.");
        }

        if (File.Exists(filePathWithoutExtension + ".json"))
        {
            using var fs = File.OpenRead(filePathWithoutExtension + ".json");

            return deserializeJsonFunc(fs, type) ?? throw new InvalidOperationException("Deserialization failed.");
        }

        throw new FileNotFoundException("File not found.", filePathWithoutExtension);
    }

    [RequiresDynamicCode(DynamicCodeMessage)]
    [RequiresUnreferencedCode(UnreferencedCodeMessage)]
    public T GetFileFromConfig<T>(string configName, string relativeFilePathWithoutExtension)
    {
        return GetFileFromConfig<T>(configName, relativeFilePathWithoutExtension,
            (stream, type) => jsonContext is null
                ? JsonSerializer.Deserialize(stream, type, jsonOptions)
                : JsonSerializer.Deserialize(stream, type, jsonContext));
    }

    public T GetFileFromConfigStatically<T>(string configName, string relativeFilePathWithoutExtension)
    {
        if (jsonContext is null)
        {
            throw new InvalidOperationException("JsonSerializerContext is not available.");
        }

        if (isYmlStatic)
        {
            throw new InvalidOperationException("YAML StaticContext is not available.");
        }

        return GetFileFromConfig<T>(configName, relativeFilePathWithoutExtension,
            (stream, type) => JsonSerializer.Deserialize(stream, type, jsonContext));
    }

    private T GetFileFromConfig<T>(
        string configName,
        string relativeFilePathWithoutExtension,
        Func<Stream, Type, object?> deserializeJsonFunc)
    {
        return (T)GetFileFromConfig(configName, relativeFilePathWithoutExtension, typeof(T), deserializeJsonFunc);
    }
}
