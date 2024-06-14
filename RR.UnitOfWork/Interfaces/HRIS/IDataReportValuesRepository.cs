﻿using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IDataReportValuesRepository : IRepository<DataReportValues, DataReportValuesDto>
{
}