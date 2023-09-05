using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ScheduleModels;

namespace BeautySaloon.API.MapperProfiles.ScheduleProfiles;

public class WorkingDayModelToWorkingDayProfile : Profile
{
    public WorkingDayModelToWorkingDayProfile()
    {
        CreateMap<WorkingDayModel, WorkingDay>()
            .ForMember(dest => dest.Day, opt => opt.MapFrom(source => source.Day))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(source => source.EndTime))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(source => source.StartTime));

    }
}
