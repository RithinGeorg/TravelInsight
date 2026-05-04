namespace TravelInsight.Domain.Entities;

/// <summary>
/// Common fields used by every table. This is useful for auditing and support investigations.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}
