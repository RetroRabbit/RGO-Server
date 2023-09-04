using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;


namespace RGO.UnitOfWork.Repositories
{
    public class PropertyAccessRepository : BaseRepository< PropertyAccess,PropertyAccessDto>, IPropertyAccessRepository
    {
        public PropertyAccessRepository(DatabaseContext db): base(db)
        {
        }
    }
}
