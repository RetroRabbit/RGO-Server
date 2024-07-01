using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeQualificationRepository : IRepository<EmployeeQualification>
{
}

public class EmployeeQualificationRepository : BaseRepository<EmployeeQualification>, IEmployeeQualificationRepository
{
    public EmployeeQualificationRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}