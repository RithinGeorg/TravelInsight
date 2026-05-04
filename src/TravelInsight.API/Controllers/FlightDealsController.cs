using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Services;

namespace TravelInsight.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FlightDealsController : ControllerBase
{
    private readonly FlightDealService _service;
    public FlightDealsController(FlightDealService service) => _service = service;

    [HttpGet]
    [OutputCache(PolicyName = "FlightSearchCache")]
    public Task<PagedResult<FlightDealResponse>> Search([FromQuery] FlightDealQuery query, CancellationToken ct) =>
        _service.SearchAsync(query, ct);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlightDealResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FlightDealResponse>> Create(CreateFlightDealRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateFlightDealRequest request, CancellationToken ct) =>
        await _service.UpdateAsync(id, request, ct) ? NoContent() : NotFound();

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct) =>
        await _service.SoftDeleteAsync(id, ct) ? NoContent() : NotFound();
}
