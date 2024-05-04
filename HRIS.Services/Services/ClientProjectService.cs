using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;

namespace HRIS.Services.Services
{
    internal class ClientProjectService : IClientProjectService
    {
        private readonly IUnitOfWork _db;

        public ClientProjectService(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<List<ClientProjectsDto>?> GetAllClientProject()
        {
            var clients = await _db.Client.GetAll();
            var allClients = clients
                             .Select(client => client)
                             .ToList();
            return allClients;
        }
    }
}
