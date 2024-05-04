using RR.UnitOfWork.Interfaces.HRIS;
using RR.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.UnitOfWork.Repositories.HRIS
{
    public interface IClientProjectRepository : IRepository<ClientProject, ClientProjectsDto>
    {
    }
}
