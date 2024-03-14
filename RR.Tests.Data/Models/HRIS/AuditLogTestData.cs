using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS
{
    public class AuditLogTestData
    {

        public static AuditLogDto auditLog1 = new AuditLogDto
        {
            Id = 1,
            CRUDOperation = CRUDOperations.Update,
            CreatedBy = EmployeeTestData.EmployeeDto,
            Table = "table",
            Date = DateTime.Now,
            Data = "Description"
        };

        public static AuditLogDto auditLog2 = new AuditLogDto
        {
            Id = 2,
            CRUDOperation = CRUDOperations.Update,
            CreatedBy = EmployeeTestData.EmployeeDto2,
            Table = "table",
            Date = DateTime.Now,
            Data = "Description"
        };

        public static List<AuditLogDto> auditLogList = new List<AuditLogDto>()
        {
            auditLog1,
            auditLog2
        };
    }
}
