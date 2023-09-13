using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities
{
    public class ChartUnitTests
    {
        [Fact]
        public async Task ChartTest()
        {
            var chart = new Chart();
            Assert.IsType<Chart>(chart);
            Assert.NotNull(chart);
        }
      
    }
}
