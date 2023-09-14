using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Models.BestWorkModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class BestWorkService : IBestWorkService
{
    private readonly BeautySaloonContext _context;
    private readonly IMapper _mapper;
    public BestWorkService(BeautySaloonContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BestWorkModel>> GetAllBestWorksAsync()
    {
        var bestWorks = await _context.BestWorks.AsNoTracking().ToListAsync();

        var result = _mapper.Map<List<BestWorkModel>>(bestWorks);

        return result;
    }

    public async Task<BestWorkModel> CreateBestWorkAsync(BestWorkModel bestWorkModel)
    {
        var newBestWork = _mapper.Map<BestWork>(bestWorkModel);
        await _context.BestWorks.AddAsync(newBestWork);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<BestWorkModel>(newBestWork);

        return result;
    }

    public async Task DeleteBestWorkAsync(int id)
    {
        var bestWork = await _context.BestWorks.FirstOrDefaultAsync(s => s.Id == id);

        if (bestWork is null)
        {
            throw new ArgumentException($"Best work with id {id} not found!");
        }

        _context.BestWorks.Remove(bestWork);
        await _context.SaveChangesAsync();
    }
}
