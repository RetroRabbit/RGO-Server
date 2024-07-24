using HRIS.Models.Enums;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IDataReportFilterRepository : IRepository<DataReportFilter>
{
    Task<DataReportFilter?> GetReportFilter(int id);
    Task ConfirmEditAccess(int reportId, int employeeId);
    Task ConfirmAnyAccess(int reportId, int employeeId);
}

public class DataReportFilterRepository : BaseRepository<DataReportFilter>,
    IDataReportFilterRepository
{
    public DataReportFilterRepository(DatabaseContext db) : base(db)
    {
    }

    public async Task<DataReportFilter?> GetReportFilter(int id)
    {
        var report = await Get(x => x.Status == ItemStatus.Active && x.Id == id).AsNoTracking()
            .FirstOrDefaultAsync();

        if (report == null) return report;

        return report;
    }
   

    public async Task ConfirmEditAccess(int reportId, int employeeId)
    {
        var access = await GetAccessByReportIdAndEmployeeId(reportId, employeeId);

        if (access.Any(x => x.ViewOnly))
            throw new UnauthorizedAccessException("Unauthorized Access");
    }

    public async Task ConfirmAnyAccess(int reportId, int employeeId)
    {
        var access = await GetAccessByReportIdAndEmployeeId(reportId, employeeId);

        if (!access.Any())
            throw new UnauthorizedAccessException("Unauthorized Access");
    }
    private async Task<List<DataReportAccess>> GetAccessByReportIdAndEmployeeId(int reportId, int employeeId)
    {
        var employee = (await (
            from e in _db.employees
            where e.Id == employeeId
            select e).FirstOrDefaultAsync()) ?? throw new UnauthorizedAccessException("Unauthorized Access");

        var roles = await (
            from r in _db.employeeRoles
            where r.EmployeeId == employee.Id
            select r.RoleId).ToListAsync();

        var access = await (from a in _db.dataReportAccess
                            where a.ReportId == reportId &&
                                  a.Status == ItemStatus.Active &&
                                  (roles.Contains(a.RoleId ?? 0) || a.EmployeeId == employee.Id)
                            select a).ToListAsync();

        return access;
    }
}