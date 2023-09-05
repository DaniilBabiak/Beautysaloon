using AutoMapper;
using BeautySaloon.API.MapperProfiles.ScheduleProfiles;

namespace BeautySaloon.API.MapperProfiles;

public static class ScheduleAutoMapperOptions
{
    public static IEnumerable<Profile> Profiles
    {
        get
        {
            yield return new ScheduleModelToScheduleProfile();
            yield return new WorkingDayModelToWorkingDayProfile();
        }
    }
}
