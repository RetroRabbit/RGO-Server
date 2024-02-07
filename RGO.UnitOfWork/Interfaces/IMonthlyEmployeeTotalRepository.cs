using RGO.Models;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.UnitOfWork.Interfaces
{
    public interface IMonthlyEmployeeTotalRepository: IRepository<MonthlyEmployeeTotal, MonthlyEmployeeTotalDto>
    {
    }
}
