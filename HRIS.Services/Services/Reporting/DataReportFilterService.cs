using HRIS.Models.Enums;
using HRIS.Models.Report.Request;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting
{
    public class DataReportFilterService : IDataReportFilterService
    {
        private readonly AuthorizeIdentity _identity;
        private readonly IUnitOfWork _db;


        public DataReportFilterService(AuthorizeIdentity identity, IUnitOfWork db) 
        { 
            _db = db;
            _identity = identity;
        }
        public async Task<object> AddReportFilter(ReportFilterRequest input)
        {
             return await _db.DataReportFilter.Add(new DataReportFilter { Table = input.TableName, Column = input.ColumnName, Condition = input.Condition, Value = input.Value, Select = "id", ReportId = input.ReportId, Status = 0 }) ?? throw new CustomException($"Failed to add report filter");
            
        }

        public async Task UpdateReportFilter(ReportFilterRequest input)
        {
            await _db.DataReportFilter.ConfirmEditAccess(input.ReportId, input.EmployeeId);
            var filter = await _db.DataReportFilter.GetReportFilter(input.ReportId) ?? throw new CustomException($"Failed to update report");
            filter.Table = input.TableName;
            filter.Column = input.ColumnName;
            filter.Condition = input.Condition;
            filter.Value = input.Value;
            await _db.DataReportFilter.Update(filter);
        }

        public async Task ArchiveReportFilter(int id)
        {

            var column = await _db.DataReportFilter
                .Get(x => x.Id == id && x.Status == ItemStatus.Active).FirstOrDefaultAsync();

            if (column == null)
                throw new CustomException("Could not delete filter.");

            column.Status = ItemStatus.Archive;
            await _db.DataReportFilter.Update(column);
        }
    }
}
