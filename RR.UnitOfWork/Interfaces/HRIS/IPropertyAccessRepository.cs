using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IPropertyAccessRepository : IRepository<PropertyAccess, PropertyAccessDto>
{
    //Task<List<PropertyAccessDto>> GetForEmployee(string email);
}