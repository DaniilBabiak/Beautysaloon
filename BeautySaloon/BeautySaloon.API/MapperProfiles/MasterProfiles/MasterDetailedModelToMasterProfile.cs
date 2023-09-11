using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.MasterModels;

namespace BeautySaloon.API.MapperProfiles.MasterProfiles;

public class MasterDetailedModelToMasterProfile : Profile
{
    public MasterDetailedModelToMasterProfile()
    {
        CreateMap<MasterDetailedModel, Master>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.Schedule, opt => opt.MapFrom(source => new Schedule { Id = source.ScheduleId }))
            .ForMember(dest => dest.Services, opt =>
                                                opt.MapFrom(source => source.ServiceIds.Select(id => new Service { Id = id})))
            .ForMember(dest => dest.Reservations, opt =>
                                                  opt.MapFrom(source => source.ReservationIds.Select(id => new Reservation { Id = id })))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name));
    }
}
