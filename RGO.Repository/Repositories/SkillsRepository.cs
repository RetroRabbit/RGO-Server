using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Repositories
{
    public class SkillsRepository : ISkillRepository
    {
        private readonly DatabaseContext _databaseContext;

        public SkillsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<SkillDto>> GetSkillsByUserId(int userid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasSkills(string title)
        {
            throw new NotImplementedException();
        }
    }
}
