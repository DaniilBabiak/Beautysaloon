using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ReservationModels;

namespace BeautySaloon.API.MapperProfiles.ReservationProfiles;

public class ReservationModelToReservationProfile : Profile
{
    public ReservationModelToReservationProfile()
    {
        CreateMap<ReservationModel, Reservation>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(source => source.ServiceId))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime))
            .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(source => source.CustomerPhoneNumber))
            .ForMember(dest => dest.MasterId, opt => opt.MapFrom(source => source.MasterId));
    }
}
