using System.ComponentModel.DataAnnotations;

namespace TravelInsight.Application.DTOs;

public sealed record FlightDealQuery(
    string? Origin,
    string? Destination,
    string? Airline,
    string? SortBy,
    int Page = 1,
    int PageSize = 10);

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);

public sealed record CreateFlightDealRequest(
    [Required, StringLength(3, MinimumLength = 3)] string Origin,
    [Required, StringLength(3, MinimumLength = 3)] string Destination,
    [Required] DateTime DepartureDate,
    [Range(1, 100000)] decimal Price,
    [Required, MaxLength(100)] string Airline,
    [Range(0, 500)] int AvailableSeats,
    [MaxLength(100)] string? Provider);

public sealed record UpdateFlightDealRequest(
    [Required, StringLength(3, MinimumLength = 3)] string Origin,
    [Required, StringLength(3, MinimumLength = 3)] string Destination,
    [Required] DateTime DepartureDate,
    [Range(1, 100000)] decimal Price,
    [Required, MaxLength(100)] string Airline,
    [Range(0, 500)] int AvailableSeats,
    [MaxLength(100)] string? Provider);

public sealed record FlightDealResponse(
    Guid Id,
    string Origin,
    string Destination,
    DateTime DepartureDate,
    decimal Price,
    string Airline,
    int AvailableSeats,
    string Provider,
    bool IsDeleted);
