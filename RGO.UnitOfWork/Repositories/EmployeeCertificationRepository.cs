using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeCertificationRepository : BaseRepository<EmployeeCertification, EmployeeCertificationDto>, IEmployeeCertificationRepository
{
    public EmployeeCertificationRepository(DatabaseContext db) : base(db)
    {
    }
}