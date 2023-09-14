using RGO.Models;
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


        [Fact]
        public async Task ChartToDtoTest()
        {
            var chart = new Chart(new ChartDto(1, "Genders", "Pie", new List<string> { "Male", "Female" }
            , new List<int> { 1, 1 }));
            Assert.IsType<ChartDto>(chart.ToDto());
            Assert.NotNull(chart.ToDto());
        }

    }
}
