using HRIS.Models;
using HRIS.Models.Enums;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class AuditLogUnitTests
{

    [Fact]
    public void auditLogTest()
    {
        var auditLog = new AuditLog();

        Assert.IsType<AuditLog>(auditLog);
        Assert.NotNull(auditLog);
    }

    [Fact]
    public void AuditLogToDtoTest()
    {
        var auditLog = new AuditLog(AuditLogTestData.auditLog1);
        var auditLogDto = auditLog.ToDto();

        Assert.IsType<AuditLogDto>(auditLogDto);
        Assert.NotNull(auditLogDto);
    }
}
