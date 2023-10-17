using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IClientService
    {
        /// <summary>
        /// Get all clients
        /// </summary>
        /// <returns></returns>
        Task<List<ClientDto>> GetAllClients();
    }
}
