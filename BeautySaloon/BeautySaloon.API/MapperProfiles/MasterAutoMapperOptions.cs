using AutoMapper;
using BeautySaloon.API.MapperProfiles.MasterProfiles;

namespace BeautySaloon.API.MapperProfiles;

public static class MasterAutoMapperOptions
{
    public static IEnumerable<Profile> Profiles
    {
        get
        {
            yield return new MasterToMasterModelProfile();
            yield return new MasterToMasterDetailedModelProfile();
        }
    }
}
