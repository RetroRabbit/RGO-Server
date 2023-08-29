using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities
{
    public class AuditLogUnitTests
    {
         [Fact]
        public async Task auditLogTest()
        {
            var auditLog = new AuditLog();
            Assert.IsType<AuditLog>(auditLog);
            Assert.NotNull(auditLog);
        }
    }
}
