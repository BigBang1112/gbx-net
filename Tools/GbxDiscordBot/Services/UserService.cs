using Discord;
using GbxDiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace GbxDiscordBot.Services;

public interface IUserService
{
    UserModel? CurrentUser { get; }

    UserModel GetRequiredCurrentUser();
    Task<UserModel> GetOrCreateUserAsync(IUser user, CancellationToken cancellationToken = default);
}

internal sealed class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserModel? CurrentUser { get; private set; }

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public UserModel GetRequiredCurrentUser()
    {
        return CurrentUser ?? throw new InvalidOperationException("Current user is not set.");
    }

    public async Task<UserModel> GetOrCreateUserAsync(IUser user, CancellationToken cancellationToken = default)
    {
        var userModel = await _db.Users.FirstOrDefaultAsync(cancellationToken);

        if (userModel is null)
        {
            userModel = new UserModel
            {
                UserId = user.Id
            };

            await _db.AddAsync(userModel, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return CurrentUser = userModel;
    }
}
