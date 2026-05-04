using TravelInsight.Application.DTOs;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Application.Interfaces;

public interface IFlightDealRepository
{
    Task<PagedResult<FlightDeal>> SearchAsync(FlightDealQuery query, CancellationToken ct);
    Task<FlightDeal?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<FlightDeal> AddAsync(FlightDeal deal, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
