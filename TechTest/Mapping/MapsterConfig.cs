using Mapster;
using TechTest.Controllers.Models;
using TechTest.Domain.Models;

namespace TechTest.Mapping;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Provider, GetProviderByIdResponse>
            .NewConfig()
            .Map(dest => dest.LastInspection, src => new LastInspectionDto { Date = src.LastInspectionDate });
    }
}
