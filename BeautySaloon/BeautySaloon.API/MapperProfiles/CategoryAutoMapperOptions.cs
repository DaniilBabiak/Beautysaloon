using AutoMapper;
using BeautySaloon.API.MapperProfiles.CategoryProfiles;

namespace BeautySaloon.API.MapperProfiles;

public static class CategoryAutoMapperOptions
{
    public static IEnumerable<Profile> Profiles
    {
        get
        {
            yield return new ServiceCategoryToCategoryModelProfile();
            yield return new CategoryModelToServiceCategoryProfile();
        }
    }
}
