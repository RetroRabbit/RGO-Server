using Microsoft.EntityFrameworkCore;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using RGO.Domain.Interfaces.Repository;


namespace RGO.Repository.Repositories
{
    public class WorkshopRepository:IWorkshopRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IEventsRepository _eventsRepository;

        public WorkshopRepository(DatabaseContext databaseContext, IEventsRepository eventsRepository)
        {
            _databaseContext = databaseContext;
            _eventsRepository = eventsRepository;
        }

        public async Task<WorkshopDto[]> GetAllWorkShops()
        {
            EventsDto[] events = await _eventsRepository.GetAllEventDtos();
            Workshop[] workshops = await _databaseContext.workshop.ToArrayAsync();
            WorkshopDto[] workShopDto = new WorkshopDto[workshops.Length];
            int counter = 0;
            foreach (var item in workshops)
            {
                var workshopEvent = events.First(e => e.Id == item.EventId);
                if (workshopEvent == null)
                {
                    continue;
                }
                workShopDto[counter++] = item.ToDto(workshopEvent);
            }
            return workShopDto;
        }

    }
}
