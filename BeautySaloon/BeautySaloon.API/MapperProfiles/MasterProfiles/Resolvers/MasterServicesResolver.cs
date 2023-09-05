using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.MasterModels;

namespace BeautySaloon.API.MapperProfiles.MasterProfiles.Resolvers;

public class MasterServicesResolver : IValueResolver<Master, MasterDetailedModel, List<int>>
{
    public List<int> Resolve(Master source, MasterDetailedModel destination, List<int> destMember, ResolutionContext context)
    {
        if (source.Services == null)
        {
            return new List<int>();
        }

        return source.Services.Select(s => s.Id).ToList();
    }
}
