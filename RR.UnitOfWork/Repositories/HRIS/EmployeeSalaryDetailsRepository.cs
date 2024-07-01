using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeSalaryDetails : IRepository<EmployeeSalaryDetails>
{
}

public class EmployeeSalaryDetailsRepository : BaseRepository<EmployeeSalaryDetails>, IEmployeeSalaryDetails
{
    public EmployeeSalaryDetailsRepository(DatabaseContext db) : base(db)
    {
    }
}