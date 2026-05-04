using TravelInsight.Domain.Entities;

namespace TravelInsight.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}

public interface IJwtTokenService
{
    string CreateToken(AppUser user, DateTime expiresAtUtc);
}
