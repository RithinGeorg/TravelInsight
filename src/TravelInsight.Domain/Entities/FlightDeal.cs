namespace TravelInsight.Domain.Entities;

/// <summary>
/// Represents a travel offer returned by a partner/provider.
/// In a real product this could come from Google Flights, Skyscanner, Kayak, or internal inventory.
/// </summary>
public sealed class FlightDeal : BaseEntity
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureDate { get; set; }
    public decimal Price { get; set; }
    public string Airline { get; set; } = string.Empty;
    public int AvailableSeats { get; set; }
    public string Provider { get; set; } = "Internal";
}
