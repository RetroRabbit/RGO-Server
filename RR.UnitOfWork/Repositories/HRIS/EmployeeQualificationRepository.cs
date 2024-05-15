using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeQualificationRepository : BaseRepository<EmployeeQualification, EmployeeQualificationDto>, IEmployeeQualificationRepository
{
    public EmployeeQualificationRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}