using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ServiceModels;

namespace BeautySaloon.API.MapperProfiles.ServiceProfiles;

public class ServiceToServiceModelProfile : Profile
{
    public ServiceToServiceModelProfile()
    {
        CreateMap<Service, ServiceModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(source => source.Price))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(source => source.CategoryId));
    }
}
