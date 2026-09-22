using Mapster;
using System.Net;
using TechTest.Application.Clients.Exceptions;
using TechTest.Application.Clients.Models;
using TechTest.Domain.Models;

namespace TechTest.Application.Clients;

public class CqcClient(HttpClient httpClient) : ICqcClient
{
    public async Task<IEnumerable<ProviderSummary>> GetAllProvidersAsync()
    {
        var response = await httpClient.GetAsync("providers");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                
            throw new CqcClientRequestException(error?.Message ?? "No message from CQC.", (int)response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<GetProvidersResponse>();

        return result?.Providers.Adapt<List<ProviderSummary>>() ?? [];
    }

    public async Task<Provider> GetProviderByIdAsync(string providerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerId);

        var response = await httpClient.GetAsync($"providers/{providerId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            throw new CqcClientRequestException(error?.Message ?? "No message from CQC.", (int)response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<GetProviderByIdResponse>();

        return result!.Adapt<Provider>();
    }
}
