﻿using Microsoft.EntityFrameworkCore;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using RGO.Domain.Interfaces.Repository;


namespace RGO.Repository.Repositories
{
    public class WorkshopRepository:IWorkshopRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly EventsRepository _eventsRepository;

        public WorkshopRepository(DatabaseContext databaseContext, EventsRepository eventsRepository)
        {
            _databaseContext = databaseContext;
            _eventsRepository = eventsRepository;
        }

        public async Task<WorkshopDto[]> GetAllWorkShops()
        {

            Events[] events = await _eventsRepository.GetAllEvents();
            Workshop[] workshops = await _databaseContext.workshop.ToArrayAsync();
            WorkshopDto[] workShopDto = new WorkshopDto[workshops.Length];
            int counter = 0;
            foreach (var item in workshops)
            {
                var workshopEvent = events.First(e => e.id == item.eventId).ToDto();
                workShopDto[counter++] = item.ToDto(workshopEvent);
            }
            return workShopDto;
        }

    }
}
