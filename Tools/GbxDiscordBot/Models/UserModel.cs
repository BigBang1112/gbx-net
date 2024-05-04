namespace GbxDiscordBot.Models;

public sealed class UserModel
{
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public bool IsBanned { get; set; }
}
