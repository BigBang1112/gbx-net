using Discord.Interactions;
using Discord;
using System.Text.Json;
using System.Text;

namespace GbxDiscordBot.Modules;

public sealed class GbxModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly HttpClient _http;

    public GbxModule(HttpClient http)
    {
        _http = http;
    }

    [SlashCommand("gbx", "Inspect a Gbx file.")]
    public async Task Gbx(IAttachment file)
    {
        using var response = await _http.GetAsync(file.Url);

        response.EnsureSuccessStatusCode();

        using var netStream = await response.Content.ReadAsStreamAsync();

        var gbx = GBX.NET.Gbx.Parse(netStream);
        gbx.FilePath = file.Filename;

        var json = JsonSerializer.Serialize(gbx, new JsonSerializerOptions()
        {
            WriteIndented = true
        });

        var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

        await RespondWithFileAsync(ms, gbx.FilePath + ".json");
    }
}