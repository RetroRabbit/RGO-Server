using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        var exists = await CheckTerminationExist(terminationDto.EmployeeId);
        if (exists)
        {
            TerminationDto updatedTermination = await UpdateTermination(terminationDto);
            return updatedTermination;
        }

        EmployeeDto currentEmployee = await _employeeService.GetEmployeeById(terminationDto.EmployeeId);
        currentEmployee.InactiveReason = terminationDto.TerminationOption.ToString();
        currentEmployee.TerminationDate = terminationDto.LastDayOfEmployment;
        currentEmployee.Active = false;

        EmployeeTypeDto employeeTypeDto = await _employeeTypeService
           .GetEmployeeType(currentEmployee.EmployeeType!.Name);

        await _db.Employee.Update(new Employee(currentEmployee, employeeTypeDto));
        TerminationDto newTermination = await _db.Termination.Add(new Termination(terminationDto));

        return newTermination;
    }

    public async Task<TerminationDto> UpdateTermination(TerminationDto terminationDto)
    {
        return await _db.Termination.Update(new Termination(terminationDto));
    }

    public async Task<TerminationDto> GetTerminationByEmployeeId(int employeeId)
    {
        var exists = await CheckTerminationExist(employeeId);
        if (!exists)
        {
            var exception = new Exception("termination not found");
            throw _errorLoggingService.LogException(exception);
        }

        TerminationDto newTermination = await _db.Termination.FirstOrDefault(termination => termination.EmployeeId == employeeId);

        return newTermination;
    }

    public async Task<bool> CheckTerminationExist(int employeeId)
    {
        return await _db.Termination
                        .Any(termination => termination.EmployeeId == employeeId);
    }
}