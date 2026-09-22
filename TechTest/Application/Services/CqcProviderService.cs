using TechTest.Application.Clients;
using TechTest.Domain.Models;
using TechTest.Persistence.Repositories;

namespace TechTest.Application.Services;

public class CqcProviderService(
    ICqcClient cqcClient,
    IProviderRepository providerRepository) : ICqcProviderService
{
    public async Task<IEnumerable<ProviderSummary>> GetAllProvidersAsync()
    {
        var providers = await cqcClient.GetAllProvidersAsync();

        return providers;
    }

    public async Task<Provider> GetProviderAsync(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        
        // Check if the existing provider is in the database.
        var existingProvider = await providerRepository.GetByIdAsync(id);

        // If there is an existing provider and it hasn't expired, return it.
        if (existingProvider is not null && existingProvider.HasExpired is false)
        {
            return existingProvider;
        }

        // Otherwise, fetch a fresh copy of the provider from the CQC.
        var newProvider = await cqcClient.GetProviderByIdAsync(id);

        newProvider.ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1));

        // If we are replacing an expired record, then delete it.
        if (existingProvider is not null)
        {
            await providerRepository.DeleteAsync(id);
        }

        // Create a fresh provider record in the database.
        await providerRepository.CreateAsync(newProvider);

        return newProvider;
    }
}
