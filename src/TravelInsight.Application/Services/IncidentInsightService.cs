using TravelInsight.Application.DTOs;

namespace TravelInsight.Application.Services;

/// <summary>
/// Demo AI-style diagnostics service. In production this could call Azure OpenAI and summarize App Insights logs.
/// </summary>
public sealed class IncidentInsightService
{
    public IncidentSummaryResponse Summarize(IncidentSummaryRequest request)
    {
        var text = request.LogText.ToLowerInvariant();

        if (text.Contains("timeout") || text.Contains("sql"))
            return new("Database", "The incident looks database-related.", "Check slow queries, indexes, connection pool, Azure SQL CPU/DTU and recent deployments.");

        if (text.Contains("401") || text.Contains("unauthorized") || text.Contains("jwt"))
            return new("Authentication", "The incident looks authentication-related.", "Check token expiry, issuer, audience, signing key and frontend Authorization header.");

        if (text.Contains("cors"))
            return new("CORS", "The browser is blocking the request due to CORS rules.", "Check allowed origins, credentials, headers and HTTP/HTTPS mismatch.");

        return new("General", "No obvious category detected.", "Review correlation ID, recent deployments, logs, metrics and affected endpoints.");
    }
}
