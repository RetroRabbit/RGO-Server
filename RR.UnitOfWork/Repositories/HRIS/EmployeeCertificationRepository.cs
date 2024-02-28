using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeCertificationRepository : BaseRepository<EmployeeCertification, EmployeeCertificationDto>,
                                               IEmployeeCertificationRepository
{
    public EmployeeCertificationRepository(DatabaseContext db) : base(db)
    {
    }
}