using TechTest.Domain.Models;

namespace TechTest.Persistence.Repositories;

public interface IProviderRepository
{
    Task CreateAsync(Provider provider);
    
    Task DeleteAsync(string id);
    
    Task<Provider?> GetByIdAsync(string id);
}