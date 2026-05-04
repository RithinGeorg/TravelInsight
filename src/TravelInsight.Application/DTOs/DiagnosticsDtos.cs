using System.ComponentModel.DataAnnotations;

namespace TravelInsight.Application.DTOs;

public sealed record IncidentSummaryRequest([Required] string LogText);
public sealed record IncidentSummaryResponse(string Category, string Summary, string SuggestedAction);
