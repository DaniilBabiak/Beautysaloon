using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ReservationModels;

namespace BeautySaloon.API.MapperProfiles.ReservationProfiles;

public class ReservationToReservationModelProfile : Profile
{
    public ReservationToReservationModelProfile()
    {
        CreateMap<Reservation, ReservationModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(source => source.ServiceId))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(source => source.Service != null ? source.Service.Name : ""))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime))
            .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(source => source.CustomerPhoneNumber))
            .ForMember(dest => dest.MasterId, opt => opt.MapFrom(source => source.MasterId))
            .ForMember(dest => dest.MasterName, opt => opt.MapFrom(source => source.Master != null ? source.Master.Name : ""));
    }
}
