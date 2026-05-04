using Microsoft.EntityFrameworkCore;
using TravelInsight.Application.Interfaces;
using TravelInsight.Domain.Entities;
using TravelInsight.Infrastructure.Data;

namespace TravelInsight.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly TravelInsightDbContext _db;
    public UserRepository(TravelInsightDbContext db) => _db = db;

    public Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct) =>
        _db.Users.FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant(), ct);

    public async Task<AppUser> AddAsync(AppUser user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct);
        return user;
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
