using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.UnitOfWork.Repositories
{
    internal class MetaPropertyOptionsRepository : BaseRepository<MetaPropertyOptions, MetaPropertyOptionsDto>, IMetaPropertyOptionsRepository
    {
        public MetaPropertyOptionsRepository(DatabaseContext db): base(db) 
        {
        }
    }
}
