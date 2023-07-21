using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public EventsRepository(DatabaseContext databaseContext) 
        {
            _databaseContext = databaseContext;
        }

        public async Task<EventsDto[]> GetAllEvents()
        {
            Events[] events = await _databaseContext.events.ToArrayAsync();
            EventsDto[] eventsDtos = new EventsDto[events.Length];
            int counter = 0;
            foreach (var item  in events)
            {
                eventsDtos[counter++] = item.ToDto();   
            }
            return eventsDtos;
        }
    }
}
