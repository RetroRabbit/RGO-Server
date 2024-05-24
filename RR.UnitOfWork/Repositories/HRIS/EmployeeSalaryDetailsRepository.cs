using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeSalaryDetailsRepository : BaseRepository<EmployeeSalaryDetails, EmployeeSalaryDetailsDto>, IEmployeeSalaryDetails
{
    public EmployeeSalaryDetailsRepository(DatabaseContext db) : base(db)
    {
    }
}