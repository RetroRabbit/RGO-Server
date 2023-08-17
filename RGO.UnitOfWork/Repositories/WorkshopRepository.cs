using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class WorkshopRepository: BaseRepository<Workshop, WorkshopDto>, IWorkshopRepository
    {
        private readonly DatabaseContext _databaseContext;

        public WorkshopRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<WorkshopDto>> GetAllWorkShops()
        {
            var now = DateTime.UtcNow;
            var today = now.Date.AddDays(1).AddTicks(-1).ToUniversalTime();

            var query = from ev in _databaseContext.events where ev.StartDate <= today select ev;

            var events = await query.ToListAsync();
            var workshops = await _databaseContext.workshop.ToListAsync();
            var workShopDto = new List<WorkshopDto>();
            foreach (var item in workshops)
            {
                var workshopEvent = events.FirstOrDefault(e => e.Id == item.EventId);
                if (workshopEvent == null)
                {
                    continue;
                }
                workShopDto.Add(item.ToDto());
            }
            return workShopDto;
        }

    }
}
