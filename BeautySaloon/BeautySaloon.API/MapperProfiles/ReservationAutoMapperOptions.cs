using AutoMapper;
using BeautySaloon.API.MapperProfiles.ReservationProfiles;

namespace BeautySaloon.API.MapperProfiles;

public static class ReservationAutoMapperOptions
{
    public static IEnumerable<Profile> Profiles
    {
        get
        {
            yield return new ReservationToReservationModelProfile();
            yield return new ReservationModelToReservationProfile();
        }
    }
}
