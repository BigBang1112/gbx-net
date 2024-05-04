namespace GbxDiscordBot.Models;

public sealed class GbxModel
{
    public int Id { get; set; }
    public required UserModel User { get; set; }
    public required Guid Guid { get; set; }
    public required string FileName { get; set; }
    public required byte[] Data { get; set; }
    public required GbxReadSettingsModel ReadSettings { get; set; }
}
