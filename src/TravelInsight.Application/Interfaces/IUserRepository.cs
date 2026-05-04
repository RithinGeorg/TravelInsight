using TravelInsight.Domain.Entities;

namespace TravelInsight.Application.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct);
    Task<AppUser> AddAsync(AppUser user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
