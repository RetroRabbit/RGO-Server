using Microsoft.EntityFrameworkCore;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using RGO.Domain.Interfaces.Repository;


namespace RGO.Repository.Repositories
{
    public class WorkshopRepository:IWorkshopRepository
    {
        private readonly DatabaseContext _databaseContext;

        public WorkshopRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<WorkshopDto[]> GetAllWorkShops(EventsDto events, string presenter)
        {
            Workshop[] workshops = await _databaseContext.workshop.ToArrayAsync();
            WorkshopDto[] workShopDto = new WorkshopDto[workshops.Length];
            int counter = 0;
            foreach (var item in workshops)
            {
                workShopDto[counter++] = WorkshopToDto(events, presenter);
            }
            return workShopDto;
        }

        private WorkshopDto WorkshopToDto(EventsDto events, string presenter)
        {
            return new WorkshopDto(events, presenter);
        }

    }
}
