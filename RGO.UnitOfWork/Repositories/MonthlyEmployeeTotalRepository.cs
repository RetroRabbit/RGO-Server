using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.UnitOfWork.Repositories
{
    public class MonthlyEmployeeTotalRepository: BaseRepository<MonthlyEmployeeTotal, MonthlyEmployeeTotalDto>, IMonthlyEmployeeTotalRepository
    {
        public MonthlyEmployeeTotalRepository(DatabaseContext db) : base(db)
        {
        }
    }
}
