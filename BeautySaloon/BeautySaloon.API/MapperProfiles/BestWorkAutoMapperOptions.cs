using AutoMapper;
using BeautySaloon.API.MapperProfiles.BestWorkProfiles;

namespace BeautySaloon.API.MapperProfiles;

public static class BestWorkAutoMapperOptions
{
    public static IEnumerable<Profile> Profiles
    {
        get
        {
            yield return new BestWorkModelToBestWorkProfile();
        }
    }
}
