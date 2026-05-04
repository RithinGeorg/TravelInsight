using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Services;

namespace TravelInsight.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DiagnosticsController : ControllerBase
{
    private readonly IncidentInsightService _insights;
    public DiagnosticsController(IncidentInsightService insights) => _insights = insights;

    [HttpPost("ai-incident-summary")]
    public ActionResult<IncidentSummaryResponse> Summarize(IncidentSummaryRequest request) => Ok(_insights.Summarize(request));
}
