using RGO.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Repositories
{
    public class TestRepository : ITestRepository
    {
        public string GetValue()
        {
            return "Hello World";
        }


    }
}
