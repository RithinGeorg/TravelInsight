namespace TravelInsight.Domain.Entities;

/// <summary>
/// Audit logs help support teams answer: who changed what and when?
/// </summary>
public sealed class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
}
