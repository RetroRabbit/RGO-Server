using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class TerminationService : ITerminationService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IEmployeeService _employeeService;

    public TerminationService(IUnitOfWork db, IErrorLoggingService errorLoggingService, IEmployeeTypeService employeeTypeService, IEmployeeService employeeService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
        _employeeTypeService = employeeTypeService;
        _employeeService = employeeService;
    }

    public async Task<TerminationDto> SaveTermination(TerminationDto terminationDto)
    {
        var exists = await CheckTerminationExist(terminationDto.Id);
        if (exists)
        {
            var exception = new Exception("termination already exists");
            throw _errorLoggingService.LogException(exception);
        }
        try
        {
            EmployeeDto currentEmployee = await _employeeService.GetEmployeeById(terminationDto.EmployeeId);
            currentEmployee.InactiveReason = terminationDto.TerminationOption.ToString();
            currentEmployee.TerminationDate = terminationDto.LastDayOfEmployment.ToDateTime(new TimeOnly());
            currentEmployee.Active = false;

            EmployeeTypeDto employeeTypeDto = await _employeeTypeService
               .GetEmployeeType(currentEmployee.EmployeeType!.Name);

            await _db.Employee.Update(new Employee(currentEmployee, employeeTypeDto));
            var newTermination = await _db.Termination.Add(new Termination(terminationDto));


            return newTermination;
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }

    public async Task<bool> CheckTerminationExist(int id)
    {
        return await _db.Termination
                        .Any(termination => termination.Id == id);
    }
}