using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.BestWorkModels;

namespace BeautySaloon.API.MapperProfiles.BestWorkProfiles;

public class BestWorkToBestWorkModelProfile : Profile
{
    public BestWorkToBestWorkModelProfile()
    {
        CreateMap<BestWork, BestWorkModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(dest => dest.ImageBucket, opt => opt.MapFrom(source => source.ImageBucket))
            .ForMember(dest => dest.ImageFileName, opt => opt.MapFrom(source => source.ImageFileName));
    }
}
