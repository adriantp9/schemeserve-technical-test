using Dapper;
using System.Data;
using TechTest.Domain.Models;

namespace TechTest.Persistence.Repositories;

public class ProviderRepository(IDbConnection db) : IProviderRepository
{
    public async Task<Provider?> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var provider = await db.QuerySingleOrDefaultAsync<Provider>(
            @"SELECT *
              FROM [TechTestDb].[dbo].[Provider]
              WHERE [ProviderId] = @Id",
            new { Id = id });

        if (provider is null)
        {
            return null;
        }

        var locationIds = await db.QueryAsync<string>(
            @"SELECT [LocationId]
              FROM [TechTestDb].[dbo].[ProviderLocation]
              WHERE [ProviderId] = @Id",
            new { Id = id });

        provider.LocationIds = [.. locationIds];

        return provider;
    }

    public async Task CreateAsync(Provider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        await db.ExecuteAsync(
            @"INSERT INTO [dbo].[Provider]
               ([ProviderId]
               ,[OrganisationType]
               ,[OwnershipType]
               ,[Type]
               ,[Name]
               ,[BrandId]
               ,[BrandName]
               ,[RegistrationStatus]
               ,[RegistrationDate]
               ,[CompaniesHouseNumber]
               ,[CharityNumber]
               ,[Website]
               ,[PostalAddressLine1]
               ,[PostalAddressLine2]
               ,[PostalAddressTownCity]
               ,[PostalAddressCounty]
               ,[Region]
               ,[PostalCode]
               ,[Uprn]
               ,[OnspdLatitude]
               ,[OnspdLongitude]
               ,[MainPhoneNumber]
               ,[InspectionDirectorate]
               ,[Constituency]
               ,[LocalAuthority]
               ,[LastInspectionDate]
               ,[ExpiryDate])
            VALUES
               (@ProviderId
               ,@OrganisationType
               ,@OwnershipType
               ,@Type
               ,@Name
               ,@BrandId
               ,@BrandName
               ,@RegistrationStatus
               ,@RegistrationDate
               ,@CompaniesHouseNumber
               ,@CharityNumber
               ,@Website
               ,@PostalAddressLine1
               ,@PostalAddressLine2
               ,@PostalAddressTownCity
               ,@PostalAddressCounty
               ,@Region
               ,@PostalCode
               ,@Uprn
               ,@OnspdLatitude
               ,@OnspdLongitude
               ,@MainPhoneNumber
               ,@InspectionDirectorate
               ,@Constituency
               ,@LocalAuthority
               ,@LastInspectionDate
               ,@ExpiryDate)",
            provider);

        foreach (var locationId in provider.LocationIds)
        {
            await db.ExecuteAsync(
                @"INSERT INTO [dbo].[ProviderLocation]
                    ([ProviderId]
                    ,[LocationId])
                  VALUES
                    (@ProviderId
                    ,@LocationId)",
                new { provider.ProviderId, LocationId = locationId });
        }
    }

    public async Task DeleteAsync(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        await db.ExecuteAsync("DELETE FROM [Provider] WHERE ProviderId = @Id", new { Id = id });
        await db.ExecuteAsync("DELETE FROM [ProviderLocation] WHERE ProviderId = @Id", new { Id = id });
    }
}
