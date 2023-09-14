using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ServiceModels;

namespace BeautySaloon.API.MapperProfiles.ServiceProfiles;

public class ServiceDetailedModelToServiceProfile : Profile
{
    public ServiceDetailedModelToServiceProfile()
    {
        CreateMap<ServiceDetailedModel, Service>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
            .ForMember(dest => dest.Masters, opt => opt.MapFrom(source => new List<Master>()))
            .ForMember(dest => dest.Reservations, opt => opt.MapFrom(source => new List<Reservation>()))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(source => source.Price))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(source => source.Id == 0 ? new int?() : source.Id))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(source => source.Duration));

    }
}
