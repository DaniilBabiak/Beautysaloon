using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.CategoryModels;

namespace BeautySaloon.API.MapperProfiles.CategoryProfiles;

public class CategoryModelToServiceCategoryProfile : Profile
{
    public CategoryModelToServiceCategoryProfile()
    {
        CreateMap<CategoryModel, ServiceCategory>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.ImageBucket, opt => opt.MapFrom(source => source.ImageBucket))
            .ForMember(dest => dest.ImageFileName, opt => opt.MapFrom(source => source.ImageFileName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
            .ForMember(dest => dest.Services, opt => opt.MapFrom(source => new List<Service>()));

    }
}
