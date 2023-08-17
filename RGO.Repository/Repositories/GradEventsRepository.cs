using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Repository.Interfaces;
using RGO.UnitOfWork.Entities;

namespace RGO.Repository.Repositories
{
    public class GradEventsRepository : IGradEventsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public GradEventsRepository(DatabaseContext databaseContext) 
        {
            _databaseContext = databaseContext;
        }

        public async Task<GradEventsDto[]> GetAllEvents()
        {
            GradEvents[] events = await _databaseContext.events.ToArrayAsync();
            GradEventsDto[] eventsDtos = new GradEventsDto[events.Length];
            int counter = 0;
            foreach (var item  in events)
            {
                eventsDtos[counter++] = item.ToDto();   
            }
            return eventsDtos;
        }
    }
}
