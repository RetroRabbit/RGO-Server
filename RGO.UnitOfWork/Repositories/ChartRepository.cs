using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class ChartRepository : BaseRepository<Chart,ChartDto>, IChartRepository
{
    public ChartRepository(DatabaseContext db) : base(db)
    {
    }
}
