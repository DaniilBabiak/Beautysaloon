using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class BestWorkService : IBestWorkService
{
    private readonly BeautySaloonContext _context;

    public BestWorkService(BeautySaloonContext context)
    {
        _context = context;
    }

    public async Task<List<BestWork>> GetAllBestWorksAsync()
    {
        var result = await _context.BestWorks.ToListAsync();

        return result;
    }

    public async Task<BestWork> CreateBestWorkAsync(BestWork bestWork)
    {
        await _context.BestWorks.AddAsync(bestWork);
        await _context.SaveChangesAsync();

        return bestWork;
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
