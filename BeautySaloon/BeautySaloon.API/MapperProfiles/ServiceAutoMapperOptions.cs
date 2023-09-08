using AutoMapper;
using BeautySaloon.API.MapperProfiles.ServiceProfiles;

namespace BeautySaloon.API.MapperProfiles;

public static class ServiceAutoMapperOptions
{
    public static IEnumerable<Profile> Profiles
    {
        get
        {
            yield return new ServiceToServiceModelProfile();
            yield return new ServiceToServiceDetailedModelProfile();
            yield return new ServiceDetailedModelToServiceProfile();
        }
    }
}
