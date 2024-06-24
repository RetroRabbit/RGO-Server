using HRIS.Models.Enums;
using HRIS.Models.Report;
using HRIS.Models.Report.Response;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportRepository : BaseRepository<DataReport, DataReportDto>, IDataReportRepository
{
    public DataReportRepository(DatabaseContext db) : base(db)
    {
    }

    public async Task<DataReport?> GetReport(string code)
    {
        var report = await _db.dataReport
            .Where(x => x.Status == ItemStatus.Active && x.Code == code)
            .Include(x => x.DataReportFilter)
            .Include(x => x.DataReportValues)
            .Include(x => x.DataReportAccess)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (report == null) return report;

        report.DataReportColumns = await GetColumns(report.Id);

        return report;
    }

    public async Task<DataReport?> GetReport(int id)
    {
        var report = await Get(x => x.Status == ItemStatus.Active && x.Id == id)
            .Include(x => x.DataReportFilter)
            .Include(x => x.DataReportValues)
            .Include(x => x.DataReportAccess)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (report == null) return report;

        report.DataReportColumns = await GetColumns(report.Id);

        return report;
    }
    
    public async Task<List<DataReportListResponse>?> GetReportsForEmployee(string employeeEmail)
    {
        var employee = await (
            from e in _db.employees
            where e.Email == employeeEmail
            select e).FirstOrDefaultAsync();

        if (employee == null)
            return new List<DataReportListResponse>();

        var roles = await (
            from r in _db.employeeRoles
            where r.EmployeeId == employee.Id
            select r.RoleId).ToListAsync();

        var access = await (from a in _db.dataReportAccess
            where roles.Contains(a.RoleId ?? 0) || a.EmployeeId == employee.Id
            select a.ReportId).ToListAsync();

        var reports = await Get(x => x.Status == ItemStatus.Active && access.Contains(x.Id))
            .Include(x => x.DataReportFilter)
            .Include(x => x.DataReportValues)
            .Include(x => x.DataReportAccess)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var list = new List<DataReportListResponse>();
        foreach (var report in reports)
        {
            report.DataReportColumns = await GetColumns(report.Id);
            var viewOnly = report.DataReportAccess?.FirstOrDefault(a => roles.Contains(a.RoleId ?? 0) || a.EmployeeId == employee.Id)?.ViewOnly ?? false;
            list.Add(new DataReportListResponse
            {
                Code = report.Code,
                Name = report.Name,
                Id = report.Id,
                Columns = report.DataReportColumns?.Select(x => x.ToDto()).ToList(),
                Filters = report.DataReportFilter?.Select(x => x.ToDto()).ToList(),
                Status = report.Status,
                ViewOnly = viewOnly
            });
        }

        return list;
    }

    private async Task<List<DataReportColumns>> GetColumns(int reportId)
    {
        return await _db.dataReportColumns
            .Where(x => x.Status == ItemStatus.Active && x.ReportId == reportId)
            .Include(x => x.Menu)
            .ThenInclude(y => y.FieldCode)
            .OrderBy(x => x.Sequence)
            .ToListAsync();
    }
}