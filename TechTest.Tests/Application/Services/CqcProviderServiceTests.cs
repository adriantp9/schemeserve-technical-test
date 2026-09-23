using Moq;
using TechTest.Application.Clients;
using TechTest.Application.Services;
using TechTest.Domain.Models;
using TechTest.Persistence.Repositories;

namespace TechTest.Tests.Application.Services;

public class CqcProviderServiceTests
{
    private readonly Mock<ICqcClient> _mockCqcClient;
    private readonly Mock<IProviderRepository> _mockProviderRepository;
    private readonly CqcProviderService _sut;

    public CqcProviderServiceTests()
    {
        _mockCqcClient = new Mock<ICqcClient>();
        _mockProviderRepository = new Mock<IProviderRepository>();
        _sut = new CqcProviderService(_mockCqcClient.Object, _mockProviderRepository.Object);
    }

    [Fact]
    public async Task GetAllProviders_WhenCalled_CallsCqcClientOnce()
    {
        // Arrange.
        _mockCqcClient
            .Setup(x => x.GetAllProvidersAsync())
            .ReturnsAsync([]);

        // Act.
        await _sut.GetAllProvidersAsync();

        // Assert.
        _mockCqcClient.Verify(x => x.GetAllProvidersAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllProviders_WhenCalled_ReturnsProviders()
    {
        // Arrange.
        var providers = new List<ProviderSummary>()
        {
            new() { ProviderId = "1", ProviderName = "Provider One" }
        };

        _mockCqcClient
            .Setup(x => x.GetAllProvidersAsync())
            .ReturnsAsync(providers);

        // Act.
        var result = await _sut.GetAllProvidersAsync();

        // Assert.
        Assert.Equal(providers, result);
    }

    [Fact]
    public async Task GetProviderAsync_WhenPassedNullId_ThrowException()
    {
        // Arrange.
        string? providerId = null;

        // Act.
        var act = async () => await _sut.GetProviderAsync(providerId!);

        // Assert.
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task GetProviderAsync_WhenProviderIsNotCached_CreatesNewProvider()
    {
        // Arrange.
        var newProvider = new Provider()
        {
            ProviderId = "1",
            OrganisationType = "Hospital",
            Type = "Health",
            Name = "Provider One",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
        };

        _mockProviderRepository
            .Setup(x => x.GetByIdAsync("1"))
            .ReturnsAsync(default(Provider));

        _mockCqcClient
            .Setup(x => x.GetProviderByIdAsync("1"))
            .ReturnsAsync(newProvider);

        // Act.
        var result = await _sut.GetProviderAsync("1");

        // Assert.
        Assert.Equal(newProvider, result);

        _mockProviderRepository.Verify(x => x.DeleteAsync("1"), Times.Never);
        _mockProviderRepository.Verify(x => x.CreateAsync(newProvider), Times.Once);
    }

    [Fact]
    public async Task GetProviderAsync_WhenCachedProviderExpiredToday_RecreatesProvider()
    {
        // Arrange.
        var expiredProvider = new Provider()
        {
            ProviderId = "1",
            OrganisationType = "Hospital",
            Type = "Health",
            Name = "Provider One",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var newProvider = new Provider()
        {
            ProviderId = "1",
            OrganisationType = "Hospital",
            Type = "Health",
            Name = "Provider One",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
        };

        _mockProviderRepository
            .Setup(x => x.GetByIdAsync("1"))
            .ReturnsAsync(expiredProvider);

        _mockCqcClient
            .Setup(x => x.GetProviderByIdAsync("1"))
            .ReturnsAsync(newProvider);

        // Act.
        var result = await _sut.GetProviderAsync("1");

        // Assert.
        Assert.Equal(newProvider, result);

        _mockProviderRepository.Verify(x => x.DeleteAsync("1"), Times.Once);
        _mockProviderRepository.Verify(x => x.CreateAsync(newProvider), Times.Once);
    }

    [Fact]
    public async Task GetProviderAsync_WhenCachedProviderExpiredYesterday_RecreatesProvider()
    {
        // Arrange.
        var expiredProvider = new Provider()
        {
            ProviderId = "1",
            OrganisationType = "Hospital",
            Type = "Health",
            Name = "Provider One",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
        };

        var newProvider = new Provider()
        {
            ProviderId = "1",
            OrganisationType = "Hospital",
            Type = "Health",
            Name = "Provider One",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
        };

        _mockProviderRepository
            .Setup(x => x.GetByIdAsync("1"))
            .ReturnsAsync(expiredProvider);

        _mockCqcClient
            .Setup(x => x.GetProviderByIdAsync("1"))
            .ReturnsAsync(newProvider);

        // Act.
        var result = await _sut.GetProviderAsync("1");

        // Assert.
        Assert.Equal(newProvider, result);

        _mockProviderRepository.Verify(x => x.DeleteAsync("1"), Times.Once);
        _mockProviderRepository.Verify(x => x.CreateAsync(newProvider), Times.Once);
    }

    [Fact]
    public async Task GetProviderAsync_WhenProviderIsCached_ReturnsCachedProvider()
    {
        // Arrange.
        var cachedProvider = new Provider()
        {
            ProviderId = "1",
            OrganisationType = "Hospital",
            Type = "Health",
            Name = "Provider One",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
        };

        _mockProviderRepository
            .Setup(x => x.GetByIdAsync("1"))
            .ReturnsAsync(cachedProvider);

        _mockCqcClient
            .Setup(x => x.GetProviderByIdAsync("1"))
            .ReturnsAsync(cachedProvider);

        // Act.
        var result = await _sut.GetProviderAsync("1");

        // Assert.
        Assert.Equal(cachedProvider, result);

        _mockCqcClient.Verify(x => x.GetProviderByIdAsync(It.IsAny<string>()), Times.Never);
        _mockProviderRepository.Verify(x => x.CreateAsync(cachedProvider), Times.Never);
    }
}
