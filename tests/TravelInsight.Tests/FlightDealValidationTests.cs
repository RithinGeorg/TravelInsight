using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Interfaces;
using TravelInsight.Application.Services;
using TravelInsight.Domain.Entities;

namespace TravelInsight.Tests;

[TestClass]
public sealed class FlightDealValidationTests
{
    [TestMethod]
    public async Task CreateAsync_WhenOriginAndDestinationSame_Throws()
    {
        var service = new FlightDealService(new FakeRepo(), new MemoryCache(new MemoryCacheOptions()), NullLogger<FlightDealService>.Instance);
        var request = new CreateFlightDealRequest("OOL", "OOL", DateTime.UtcNow.AddDays(3), 100, "Jetstar", 10, "Test");

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => service.CreateAsync(request, CancellationToken.None));
    }

    private sealed class FakeRepo : IFlightDealRepository
    {
        public Task<FlightDeal> AddAsync(FlightDeal deal, CancellationToken ct) => Task.FromResult(deal);
        public Task<FlightDeal?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult<FlightDeal?>(null);
        public Task SaveChangesAsync(CancellationToken ct) => Task.CompletedTask;
        public Task<PagedResult<FlightDeal>> SearchAsync(FlightDealQuery query, CancellationToken ct) => Task.FromResult(new PagedResult<FlightDeal>([], 0, 1, 10));
    }
}
