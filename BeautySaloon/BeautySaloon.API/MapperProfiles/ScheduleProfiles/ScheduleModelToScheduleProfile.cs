using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ScheduleModels;

namespace BeautySaloon.API.MapperProfiles.ScheduleProfiles;

public class ScheduleModelToScheduleProfile : Profile
{
    public ScheduleModelToScheduleProfile()
    {
        CreateMap<ScheduleModel, Schedule>()
            .ForMember(dest => dest.MasterId, opt => opt.MapFrom(src => src.MasterId))
            .ForMember(dest => dest.WorkingDays, opt => opt.MapFrom(src => src.Workingdays));
    }
}
