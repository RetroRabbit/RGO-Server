using Google.Apis.Services;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class ClientService : Interfaces.IClientService
    {
        private readonly IUnitOfWork _db;

        public ClientService(IUnitOfWork db)
        {
           _db = db;
        }

        public async Task<List<ClientDto>> GetAllClients()
        {
            var clients = await _db.Client.GetAll();
            var allClients = clients
                .Select(client => client)
                .ToList();
            return allClients;
        }
    }
}
 