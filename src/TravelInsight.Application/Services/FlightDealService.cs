using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Interfaces;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Application.Services;

/// <summary>
/// Application service = use-case layer. Controllers call this instead of directly touching EF Core.
/// This keeps HTTP logic separate from business logic and makes the code testable.
/// </summary>
public sealed class FlightDealService
{
    private readonly IFlightDealRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FlightDealService> _logger;

    public FlightDealService(IFlightDealRepository repository, IMemoryCache cache, ILogger<FlightDealService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PagedResult<FlightDealResponse>> SearchAsync(FlightDealQuery query, CancellationToken ct)
    {
        var safeQuery = query with
        {
            Page = Math.Max(1, query.Page),
            PageSize = Math.Clamp(query.PageSize, 1, 100), // protects the API from huge page-size requests
            Origin = query.Origin?.ToUpperInvariant(),
            Destination = query.Destination?.ToUpperInvariant()
        };

        var cacheKey = $"flight-search:{safeQuery.Origin}:{safeQuery.Destination}:{safeQuery.Airline}:{safeQuery.SortBy}:{safeQuery.Page}:{safeQuery.PageSize}";

        if (_cache.TryGetValue(cacheKey, out PagedResult<FlightDealResponse>? cached) && cached is not null)
        {
            _logger.LogInformation("Flight search returned from cache. Key={CacheKey}", cacheKey);
            return cached;
        }

        var result = await _repository.SearchAsync(safeQuery, ct);
        var response = new PagedResult<FlightDealResponse>(
            result.Items.Select(ToResponse).ToList(),
            result.TotalCount,
            result.Page,
            result.PageSize);

        _cache.Set(cacheKey, response, TimeSpan.FromSeconds(30));
        return response;
    }

    public async Task<FlightDealResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var cacheKey = $"flight-deal:{id}";
        if (_cache.TryGetValue(cacheKey, out FlightDealResponse? cached) && cached is not null)
            return cached;

        var deal = await _repository.GetByIdAsync(id, ct);
        if (deal is null || deal.IsDeleted) return null;

        var response = ToResponse(deal);
        _cache.Set(cacheKey, response, TimeSpan.FromMinutes(1));
        return response;
    }

    public async Task<FlightDealResponse> CreateAsync(CreateFlightDealRequest request, CancellationToken ct)
    {
        ValidateBusinessRules(request.Origin, request.Destination, request.DepartureDate);

        var deal = new FlightDeal
        {
            Origin = request.Origin.ToUpperInvariant(),
            Destination = request.Destination.ToUpperInvariant(),
            DepartureDate = request.DepartureDate,
            Price = request.Price,
            Airline = request.Airline,
            AvailableSeats = request.AvailableSeats,
            Provider = string.IsNullOrWhiteSpace(request.Provider) ? "Internal" : request.Provider!
        };

        await _repository.AddAsync(deal, ct);
        await _repository.SaveChangesAsync(ct);

        _logger.LogInformation("Flight deal created. Id={Id} Route={Origin}-{Destination}", deal.Id, deal.Origin, deal.Destination);
        return ToResponse(deal);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateFlightDealRequest request, CancellationToken ct)
    {
        ValidateBusinessRules(request.Origin, request.Destination, request.DepartureDate);

        var deal = await _repository.GetByIdAsync(id, ct);
        if (deal is null || deal.IsDeleted) return false;

        deal.Origin = request.Origin.ToUpperInvariant();
        deal.Destination = request.Destination.ToUpperInvariant();
        deal.DepartureDate = request.DepartureDate;
        deal.Price = request.Price;
        deal.Airline = request.Airline;
        deal.AvailableSeats = request.AvailableSeats;
        deal.Provider = string.IsNullOrWhiteSpace(request.Provider) ? deal.Provider : request.Provider!;
        deal.UpdatedAtUtc = DateTime.UtcNow;

        await _repository.SaveChangesAsync(ct);
        _cache.Remove($"flight-deal:{id}");
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct)
    {
        var deal = await _repository.GetByIdAsync(id, ct);
        if (deal is null || deal.IsDeleted) return false;

        deal.IsDeleted = true;
        deal.UpdatedAtUtc = DateTime.UtcNow;
        await _repository.SaveChangesAsync(ct);
        _cache.Remove($"flight-deal:{id}");
        return true;
    }

    private static void ValidateBusinessRules(string origin, string destination, DateTime departureDate)
    {
        if (origin.Equals(destination, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Origin and destination cannot be the same.");

        if (departureDate.Date < DateTime.UtcNow.Date)
            throw new InvalidOperationException("Departure date cannot be in the past.");
    }

    private static FlightDealResponse ToResponse(FlightDeal deal) => new(
        deal.Id, deal.Origin, deal.Destination, deal.DepartureDate,
        deal.Price, deal.Airline, deal.AvailableSeats, deal.Provider, deal.IsDeleted);
}
