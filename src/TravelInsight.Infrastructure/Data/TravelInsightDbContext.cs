using Microsoft.EntityFrameworkCore;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Infrastructure.Data;

public sealed class TravelInsightDbContext : DbContext
{
    public TravelInsightDbContext(DbContextOptions<TravelInsightDbContext> options) : base(options) { }

    public DbSet<FlightDeal> FlightDeals => Set<FlightDeal>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlightDeal>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Origin).HasMaxLength(3).IsRequired();
            entity.Property(x => x.Destination).HasMaxLength(3).IsRequired();
            entity.Property(x => x.Airline).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Price).HasPrecision(18, 2);

            // Indexes are important because travel search is commonly filtered by route/date/price.
            entity.HasIndex(x => new { x.Origin, x.Destination, x.DepartureDate });
            entity.HasIndex(x => x.Price);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
        });
    }
}
