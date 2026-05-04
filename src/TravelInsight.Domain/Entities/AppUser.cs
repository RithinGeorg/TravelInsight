namespace TravelInsight.Domain.Entities;

/// <summary>
/// Very small demo user model. Production systems usually use ASP.NET Identity or external identity providers.
/// </summary>
public sealed class AppUser : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
