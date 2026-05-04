using TravelInsight.Application.Interfaces;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(TravelInsightDbContext db, IPasswordHasher hasher)
    {
        if (!db.Users.Any())
        {
            db.Users.AddRange(
                new AppUser { Email = "admin@travelinsight.demo", DisplayName = "Admin User", Role = "Admin", PasswordHash = hasher.Hash("Admin1234") },
                new AppUser { Email = "user@travelinsight.demo", DisplayName = "Demo User", Role = "User", PasswordHash = hasher.Hash("User1234") });
        }

        if (!db.FlightDeals.Any())
        {
            db.FlightDeals.AddRange(
                new FlightDeal { Origin = "OOL", Destination = "SYD", DepartureDate = DateTime.UtcNow.Date.AddDays(20), Price = 129, Airline = "Jetstar", AvailableSeats = 42, Provider = "Skyscanner" },
                new FlightDeal { Origin = "OOL", Destination = "MEL", DepartureDate = DateTime.UtcNow.Date.AddDays(25), Price = 159, Airline = "Virgin", AvailableSeats = 18, Provider = "Kayak" },
                new FlightDeal { Origin = "BNE", Destination = "LAX", DepartureDate = DateTime.UtcNow.Date.AddDays(60), Price = 899, Airline = "Qantas", AvailableSeats = 9, Provider = "Google Flights" },
                new FlightDeal { Origin = "SYD", Destination = "AKL", DepartureDate = DateTime.UtcNow.Date.AddDays(14), Price = 279, Airline = "Air New Zealand", AvailableSeats = 0, Provider = "Internal" });
        }

        db.SaveChanges();
    }
}
