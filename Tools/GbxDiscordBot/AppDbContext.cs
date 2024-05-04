using GbxDiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GbxDiscordBot;

public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<GbxModel> Gbxs { get; set; }
    public DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GbxModel>()
            .Property(x => x.ReadSettings)
            .HasConversion(
                x => JsonSerializer.Serialize(x, AppJsonContext.Default.GbxReadSettingsModel),
                x => JsonSerializer.Deserialize(x, AppJsonContext.Default.GbxReadSettingsModel)!);
    }
}