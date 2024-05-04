﻿using Discord;
using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Managers;
using GbxDiscordBot.Models;
using System.Reflection;
using System.Text;

namespace GbxDiscordBot.Services;

public interface IResponseService
{
    Task<InteractionResponse> GbxPropertiesAsync(GbxModel gbxModel, CancellationToken cancellationToken = default);
    Task<InteractionResponse> UnavailableAsync(GbxModel? gbxModel = null, CancellationToken cancellationToken = default);
    Task<InteractionResponse> MainNodeAsync(Gbx gbx, CMwNod node, GbxModel gbxModel, IMessage? inspectedMessage = null, CancellationToken cancellationToken = default);
}

internal sealed class ResponseService : IResponseService
{
    public async Task<InteractionResponse> GbxPropertiesAsync(GbxModel gbxModel, CancellationToken cancellationToken = default)
    {
        return new();
    }

    public async Task<InteractionResponse> UnavailableAsync(GbxModel? gbxModel = null, CancellationToken cancellationToken = default)
    {
        return new();
    }

    public async Task<InteractionResponse> MainNodeAsync(Gbx gbx, CMwNod node, GbxModel gbxModel, IMessage? inspectedMessage = null, CancellationToken cancellationToken = default)
    {
        var validProperties = GetValidProperties(node)
            .OrderBy(x => x.Info.Name)
            .Take(25)
            .ToList();

        var message = default(string);
        var embeds = new List<Embed>();

        if (inspectedMessage is not null)
        {
            message = $"Inspected Gbx: {inspectedMessage.GetJumpUrl()} (first attachment)";
        }

        var type = node.GetType();

        var embed = new EmbedBuilder()
            .WithTitle(gbx.FilePath)
            .WithDescription($"**{type.Name}** 0x{ClassManager.GetId(type)}\n\n" + string.Join('\n', validProperties.Select((x, i) => $"` {GetPropertyString(x.Info, x.Value)} `")))
            .WithColor(Discord.Color.Blue)
            .WithFooter(Guid.NewGuid().ToString());
        embeds.Add(embed.Build());

        if (gbx.Body.Exception is not null)
        {
            var exception = $"{gbx.Body.Exception.GetType().Name}: {gbx.Body.Exception.Message}";

            var errorEmbed = new EmbedBuilder()
                .WithTitle("Error")
                .WithDescription($"An error occurred while reading the Gbx file. Exporting has been disabled.\n```\n{exception}```")
                .WithColor(Discord.Color.Red);
            embeds.Add(errorEmbed.Build());
        }

        var componentBuilder = new ComponentBuilder()
            .WithSelectMenu("member", validProperties.Select(x =>
            {
                var valueStr = x.Value.ToString() ?? "null";

                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    valueStr = "empty";
                }
                else if (valueStr.Length > 100)
                {
                    valueStr = string.Concat(valueStr.AsSpan(0, 97), "...");
                }

                return new SelectMenuOptionBuilder()
                    .WithLabel(x.Info.Name)
                    .WithValue(x.Info.Name)
                    .WithDescription(valueStr);
            }).ToList())
            .WithButton("Export", "export")
            .WithButton("⏫", "up", ButtonStyle.Secondary, disabled: true)
            .WithButton("⏬", "down", ButtonStyle.Secondary)
            .WithButton("◀️", "left", ButtonStyle.Secondary, disabled: true)
            .WithButton("▶️", "right", ButtonStyle.Secondary)
            .WithButton("Gbx properties", "gbx-properties", ButtonStyle.Secondary)
            .WithButton("Mobile mode", "mobile-mode", ButtonStyle.Secondary)
            .WithButton("Discard", "discard", ButtonStyle.Danger);

        return new InteractionResponse
        {
            Message = message,
            Embeds = embeds.ToArray(),
            Components = componentBuilder.Build()
        };
    }

    private static IEnumerable<(PropertyInfo Info, object Value)> GetValidProperties(CMwNod node)
    {
        var properties = node.GetType().GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(node);

            if (value is null)
            {
                continue;
            }

            yield return (property, value);
        }
    }

    private static string GetPropertyString(PropertyInfo prop, object value)
    {
        var propLength = 32;
        var valueLength = 24;

        var sb = new StringBuilder();

        if (prop.Name.Length > propLength)
        {
            sb.Append(string.Concat(prop.Name.AsSpan(0, propLength - 2), ".."));
        }
        else
        {
            sb.Append(prop.Name);

            if (prop.Name.Length < propLength)
            {
                sb.Append(' ', propLength - prop.Name.Length);
            }
        }

        sb.Append(" | ");

        var valueStr = value.ToString() ?? "null";

        if (valueStr.Length > valueLength)
        {
            sb.Append(string.Concat(valueStr.AsSpan(0, valueLength - 2), ".."));
        }
        else
        {
            sb.Append(valueStr);

            if (valueStr.Length < valueLength)
            {
                sb.Append(' ', valueLength - valueStr.Length);
            }
        }

        return sb.ToString();
    }
}
