using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.CategoryModels;

namespace BeautySaloon.API.MapperProfiles.CategoryProfiles;

public class ServiceCategoryToCategoryModelProfile : Profile
{
    public ServiceCategoryToCategoryModelProfile()
    {
        CreateMap<ServiceCategory, CategoryModel>()
            .ForMember(dest => dest.ServiceIds, opt => opt.MapFrom(source => source.Services.Select(s => s.Id)));
    }
}
