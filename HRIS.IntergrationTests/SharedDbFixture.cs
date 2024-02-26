using Microsoft.EntityFrameworkCore;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.IntegrationTests
{
    public class SharedDbFixture: IDisposable
    {
        public DatabaseContext DatabaseContext { get; set; }
        public EmployeeAddressTestData testData = new EmployeeAddressTestData();
        public SharedDbFixture() {

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName: "TestDb").Options;
             
            DatabaseContext = new DatabaseContext(options);

            DatabaseContext.employeeAddresses.Add(new EmployeeAddressTestData.)
            DatabaseContext.SaveChanges();
        }

        public void Dispose() {
        
             DatabaseContext.Dispose();
        
        }
    }
  
}
