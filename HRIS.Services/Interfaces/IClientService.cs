using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IClientService
{
    /// <summary>
    ///     Get all clients
    /// </summary>
    /// <returns>List of ClientDto
    Task<List<ClientDto>> GetAllClients();
}