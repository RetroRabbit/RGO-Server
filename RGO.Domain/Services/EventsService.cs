using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Services
{
    public class EventsService : IEventsService
    {
        private IEventsRepository _eventsrepository;

        public EventsService(IEventsRepository eventsRepository) 
        {
            _eventsrepository = eventsRepository;
        }
        public Task<EventsDto[]> GetEvents()
        {
            return _eventsrepository.GetAllEvents();
        }
    }
}
