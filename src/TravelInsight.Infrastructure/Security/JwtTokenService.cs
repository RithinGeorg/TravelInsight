using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TravelInsight.Application.Interfaces;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Infrastructure.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    public JwtTokenService(IConfiguration configuration) => _configuration = configuration;

    public string CreateToken(AppUser user, DateTime expiresAtUtc)
    {
        var key = _configuration["Jwt:Key"] ?? "LOCAL_DEMO_SECRET_KEY_CHANGE_ME_123456789";
        var issuer = _configuration["Jwt:Issuer"] ?? "TravelInsight";
        var audience = _configuration["Jwt:Audience"] ?? "TravelInsightFrontend";

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expiresAtUtc, signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
