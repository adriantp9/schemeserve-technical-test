using TechTest.Domain.Models;

namespace TechTest.Application.Services;

public interface ICqcProviderService
{
    Task<IEnumerable<ProviderSummary>> GetAllProvidersAsync();

    Task<Provider> GetProviderAsync(string id);
}