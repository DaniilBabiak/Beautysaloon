using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.MasterModels;

namespace BeautySaloon.API.MapperProfiles.MasterProfiles;

public class MasterToMasterModelProfile : Profile
{
    public MasterToMasterModelProfile()
    {
        CreateMap<Master, MasterModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
            .ForMember(dest => dest.ScheduleId,
                       opt => opt.MapFrom(source =>
                                            source.Schedule == null ? 0 : source.Schedule.Id));
    }
}
