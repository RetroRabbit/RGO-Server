using HRIS.Services.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Org.BouncyCastle.Ocsp;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Repositories.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.IntegrationTests.Controllers;

public class EmployeeControllerTest : IClassFixture<SharedDbFixture>
{

    private readonly SharedDbFixture _sharedDbFixture;
   
    public EmployeeControllerTest(SharedDbFixture sharedDbFixture)
    {
        _sharedDbFixture = sharedDbFixture;
    }

    [Fact(Skip ="test incomplete")]
    public void ValidId_ReturnsEnitity()
    {
          //find the ID in the shared database 

         var ValidId =  _sharedDbFixture.DatabaseContext.employeeAddresses.Select( x => x.Id).FirstOrDefault();
         Assert.True(ValidId > 0);


        //fetchb employee by id
        var employeeAddressRetrieved = _sharedDbFixture.DatabaseContext.employeeAddresses.Find(ValidId);
        Assert.NotNull(employeeAddressRetrieved);

        Assert.Equal(1, employeeAddressRetrieved.Id);
     
    }
   

}
