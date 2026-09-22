using TechTest.Domain.Models;

namespace TechTest.Application.Clients;

public interface ICqcClient
{
    Task<IEnumerable<ProviderSummary>> GetAllProvidersAsync();

    Task<Provider> GetProviderByIdAsync(string providerId);
}