using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
namespace RR.UnitOfWork.Interfaces.HRIS;
    public interface IClientProjectRepository : IRepository<ClientProject, ClientProjectsDto>
    {
    }
