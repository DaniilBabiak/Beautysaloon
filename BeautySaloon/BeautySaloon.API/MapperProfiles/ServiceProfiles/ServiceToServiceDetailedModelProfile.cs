using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ServiceModels;

namespace BeautySaloon.API.MapperProfiles.ServiceProfiles;

public class ServiceToServiceDetailedModelProfile : Profile
{
    public ServiceToServiceDetailedModelProfile()
    {
        CreateMap<Service, ServiceDetailedModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(source => source.Price))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(source => source.CategoryId))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(source => source.Duration))
            .ForMember(dest => dest.MasterIds, opt => opt.MapFrom(source => source.Masters.Select(m => m.Id)))
            .ForMember(dest => dest.ReservationIds, opt => opt.MapFrom(source => source.Reservations.Select(r => r.Id)));
    }
}
