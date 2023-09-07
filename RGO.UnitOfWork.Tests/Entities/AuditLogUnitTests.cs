using RGO.UnitOfWork.Entities;
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
