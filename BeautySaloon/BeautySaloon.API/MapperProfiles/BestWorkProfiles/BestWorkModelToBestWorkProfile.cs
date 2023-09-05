using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.BestWorkModels;

namespace BeautySaloon.API.MapperProfiles.BestWorkProfiles;

public class BestWorkModelToBestWorkProfile : Profile
{
    public BestWorkModelToBestWorkProfile()
    {
        CreateMap<BestWorkModel, BestWork>()
            .ForMember(dest => dest.ImageBucket, opt => opt.MapFrom(source => source.ImageBucket))
            .ForMember(dest => dest.ImageFileName, opt => opt.MapFrom(source => source.ImageFileName));
    }
}
