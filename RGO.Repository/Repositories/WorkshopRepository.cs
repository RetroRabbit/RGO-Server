using Microsoft.EntityFrameworkCore;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using RGO.Domain.Interfaces.Repository;

namespace RGO.Repository.Repositories
{
    public class WorkshopRepository:IWorkshopRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IGradEventsRepository _eventsRepository;


        public WorkshopRepository(DatabaseContext databaseContext, IGradEventsRepository eventsRepository)
        {
            _databaseContext = databaseContext;
            _eventsRepository = eventsRepository;
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
