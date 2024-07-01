using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;

namespace HRIS.Services.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _db;

    public ClientService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<List<ClientDto>?> GetAllClients()
    {
        var clients = await _db.Client.GetAll();
        var allClients = clients
                         .Select(client => client.ToDto())
                         .ToList();
        return allClients;
    }
}