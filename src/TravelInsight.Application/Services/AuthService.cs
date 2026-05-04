using Microsoft.Extensions.Logging;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Interfaces;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Application.Services;

public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwt;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository users, IPasswordHasher passwordHasher, IJwtTokenService jwt, ILogger<AuthService> logger)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
        _logger = logger;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(request.Email, ct);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
            return null;
        }

        var expires = DateTime.UtcNow.AddHours(2);
        return new LoginResponse(_jwt.CreateToken(user, expires), expires, ToDto(user));
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var existing = await _users.GetByEmailAsync(request.Email, ct);
        if (existing is not null) throw new InvalidOperationException("User already exists.");

        var user = new AppUser
        {
            Email = request.Email.ToLowerInvariant(),
            DisplayName = request.DisplayName,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = "User"
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        var expires = DateTime.UtcNow.AddHours(2);
        return new LoginResponse(_jwt.CreateToken(user, expires), expires, ToDto(user));
    }

    public async Task<LoginResponse> OAuthDemoAsync(OAuthDemoRequest request, CancellationToken ct)
    {
        // Demo only: in production, validate the external provider token before trusting the email.
        var user = await _users.GetByEmailAsync(request.ExternalEmail, ct);
        if (user is null)
        {
            user = new AppUser
            {
                Email = request.ExternalEmail.ToLowerInvariant(),
                DisplayName = request.DisplayName ?? request.ExternalEmail,
                PasswordHash = "EXTERNAL_LOGIN_ONLY",
                Role = "User"
            };
            await _users.AddAsync(user, ct);
            await _users.SaveChangesAsync(ct);
        }

        var expires = DateTime.UtcNow.AddHours(2);
        return new LoginResponse(_jwt.CreateToken(user, expires), expires, ToDto(user));
    }

    private static UserDto ToDto(AppUser user) => new(user.Id, user.Email, user.DisplayName, user.Role);
}
