using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Services
{


    public class TestService : ITestService
    {
        public ITestRepository _testRepository;
        public TestService(ITestRepository testRepository) {
            _testRepository = testRepository;
        }
        public string GetValue()
        {
            return _testRepository.GetValue();
        }
    }
}
