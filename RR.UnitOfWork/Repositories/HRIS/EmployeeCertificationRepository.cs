using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeCertificationRepository : IRepository<EmployeeCertification>
{
}

public class EmployeeCertificationRepository : BaseRepository<EmployeeCertification>, IEmployeeCertificationRepository
{
    public EmployeeCertificationRepository(DatabaseContext db) : base(db)
    {
    }
}