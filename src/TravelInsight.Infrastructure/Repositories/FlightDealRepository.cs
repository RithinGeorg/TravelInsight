using Microsoft.EntityFrameworkCore;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Interfaces;
using TravelInsight.Domain.Entities;
using TravelInsight.Infrastructure.Data;

namespace TravelInsight.Infrastructure.Repositories;

public sealed class FlightDealRepository : IFlightDealRepository
{
    private readonly TravelInsightDbContext _db;
    public FlightDealRepository(TravelInsightDbContext db) => _db = db;

    public async Task<PagedResult<FlightDeal>> SearchAsync(FlightDealQuery query, CancellationToken ct)
    {
        IQueryable<FlightDeal> dbQuery = _db.FlightDeals
            .AsNoTracking() // optimization: faster for read-only queries because EF does not track changes
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.Origin)) dbQuery = dbQuery.Where(x => x.Origin == query.Origin);
        if (!string.IsNullOrWhiteSpace(query.Destination)) dbQuery = dbQuery.Where(x => x.Destination == query.Destination);
        if (!string.IsNullOrWhiteSpace(query.Airline)) dbQuery = dbQuery.Where(x => x.Airline.Contains(query.Airline));

        dbQuery = query.SortBy?.ToLowerInvariant() switch
        {
            "price_desc" => dbQuery.OrderByDescending(x => x.Price),
            "date" => dbQuery.OrderBy(x => x.DepartureDate),
            "date_desc" => dbQuery.OrderByDescending(x => x.DepartureDate),
            _ => dbQuery.OrderBy(x => x.Price)
        };

        var total = await dbQuery.CountAsync(ct);
        var items = await dbQuery.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync(ct);
        return new PagedResult<FlightDeal>(items, total, query.Page, query.PageSize);
    }

    public Task<FlightDeal?> GetByIdAsync(Guid id, CancellationToken ct) => _db.FlightDeals.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<FlightDeal> AddAsync(FlightDeal deal, CancellationToken ct)
    {
        await _db.FlightDeals.AddAsync(deal, ct);
        return deal;
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
