using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.MapperProfiles.MasterProfiles.Resolvers;
using BeautySaloon.API.Models.MasterModels;

namespace BeautySaloon.API.MapperProfiles.MasterProfiles;

public class MasterToMasterDetailedModelProfile : Profile
{
    public MasterToMasterDetailedModelProfile()
    {
        CreateMap<Master, MasterDetailedModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
            .ForMember(dest => dest.ScheduleId,
                       opt => opt.MapFrom(source =>
                                            source.Schedule == null ? 0 : source.Schedule.Id))
            .ForMember(dest => dest.ReservationIds, opt => opt.MapFrom<MasterReservationsResolver>())
            .ForMember(dest => dest.ServiceIds, opt => opt.MapFrom<MasterServicesResolver>());
    }
}
