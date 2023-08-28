using RGO.UnitOfWork.Entities;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class MetaPropertyRepository : BaseRepository<MetaProperty, MetaPropertyDto>, IMetaPropertyRepository
    {
        public MetaPropertyRepository(DatabaseContext db): base(db)
        {
        }
    }
}
