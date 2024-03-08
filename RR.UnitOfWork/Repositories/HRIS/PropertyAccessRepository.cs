using HRIS.Models;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class PropertyAccessRepository : BaseRepository<PropertyAccess, PropertyAccessDto>, IPropertyAccessRepository
{
    private readonly DatabaseContext _db;

    public PropertyAccessRepository(DatabaseContext db) : base(db)
    {
        _db = db;
    }

}