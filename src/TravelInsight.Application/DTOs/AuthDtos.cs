using System.ComponentModel.DataAnnotations;

namespace TravelInsight.Application.DTOs;

public sealed record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);
public sealed record RegisterRequest([Required, EmailAddress] string Email, [Required] string Password, [Required] string DisplayName);
public sealed record UserDto(Guid Id, string Email, string DisplayName, string Role);
public sealed record LoginResponse(string AccessToken, DateTime ExpiresAtUtc, UserDto User);
public sealed record OAuthDemoRequest([Required] string Provider, [Required] string ExternalEmail, string? DisplayName);
