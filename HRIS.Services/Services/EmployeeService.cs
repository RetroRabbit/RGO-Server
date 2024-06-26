﻿using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ServiceBusClient serviceBusClient = new (Environment.GetEnvironmentVariable("NewEmployeeQueue__ConnectionString"));
    private readonly string queueName = Environment.GetEnvironmentVariable("ServiceBus__QueueName");
    private readonly IUnitOfWork _db;
    private readonly IEmployeeAddressService _employeeAddressService;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IRoleService _roleService;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db,
                           IEmployeeAddressService employeeAddressService, IRoleService roleService, IErrorLoggingService errorLoggingService)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
        _employeeAddressService = employeeAddressService;
        _roleService = roleService;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto)
    {
        var exists = await CheckUserExist(employeeDto.Email);
        if (exists)
        {
            var exception = new Exception("User already exists");
            throw _errorLoggingService.LogException(exception);
        }

        // TODO: After new employee bug is fixed, test if this condition can ever be reached and update accordingly
        if (employeeDto.EmployeeType == null)
        {
            var exception = new Exception("Employee type missing");
            throw _errorLoggingService.LogException(exception);
        }

        Employee employee;

        try
        {
            var ExistingEmployeeType = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType!.Name);

            employee = new Employee(employeeDto, ExistingEmployeeType);

            try
            {
                 PushToProducerAsync(employee);
            }
            catch (Exception ex)
            {
               _errorLoggingService.LogException(ex);
            }
        }
        catch (Exception ex)
        {
            var newEmployeeType = await _employeeTypeService
                .SaveEmployeeType(new EmployeeTypeDto { Id = 0, Name = employeeDto.EmployeeType!.Name });

            _errorLoggingService.LogException(ex);
            employee = new Employee(employeeDto, newEmployeeType);
        }

        var physicalAddressExist = await _employeeAddressService
            .CheckIfExists(employeeDto.PhysicalAddress!);

        EmployeeAddressDto physicalAddress;

        if (!physicalAddressExist)
            physicalAddress = await _employeeAddressService.Save(employeeDto.PhysicalAddress!);
        else
            physicalAddress = await _employeeAddressService.Get(employeeDto.PhysicalAddress!);

        employee.PhysicalAddressId = physicalAddress.Id;

        var postalAddressExist = await _employeeAddressService
            .CheckIfExists(employeeDto.PostalAddress!);

        EmployeeAddressDto postalAddress;

        if (!postalAddressExist)
            postalAddress = await _employeeAddressService.Save(employeeDto.PostalAddress!);
        else
            postalAddress = await _employeeAddressService.Get(employeeDto.PostalAddress!);

        employee.PostalAddressId = postalAddress.Id;

        var roleDto = await _roleService.GetRole("Employee");
        employee.Active = true;
        var newEmployee = await _db.Employee.Add(employee);

        var employeeRoleDto = new EmployeeRoleDto{Id = 0,Employee = newEmployee, Role = roleDto };

        await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

        return newEmployee;
    }

    public async Task<bool> CheckUserExist(string? email)
    {
        return await _db.Employee
                        .Any(employee => employee.Email == email);
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        var existingEmployee = await GetEmployee(email);

        return await _db.Employee.Delete(existingEmployee!.Id);
    }

    public async Task<List<EmployeeDto>> GetAll(string userEmail = "")
    {
        if (userEmail != "" && await IsJourney(userEmail))
        {
            var peopleChampion = await GetEmployee(userEmail);

            return await _db.Employee
                            .Get(employee => employee.PeopleChampion == peopleChampion!.Id)
                            .Include(employee => employee.EmployeeType)
                            .Include(employee => employee.PhysicalAddress)
                            .Include(employee => employee.PostalAddress)
                            .OrderBy(employee => employee.Name)
                            .Select(employee => employee.ToDto())
                            .ToListAsync();
        }

        return await _db.Employee
                        .Get(employee => true)
                        .AsNoTracking()
                        .Include(employee => employee.EmployeeType)
                        .Include(employee => employee.PhysicalAddress)
                        .Include(employee => employee.PostalAddress)
                        .OrderBy(employee => employee.Name)
                        .Select(employee => employee.ToDto())
                        .ToListAsync();
    }

    public async Task<EmployeeDto?> GetEmployee(string email)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Email == email)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Include(employee => employee.PhysicalAddress)
                                .Include(employee => employee.PostalAddress)
                                .Select(employee => employee.ToDto())
                                .Take(1)
                                .FirstOrDefaultAsync();

        if (employee == null)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        return employee;
    }

    public async Task<EmployeeDto> GetEmployeeById(int id)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Id == id)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Include(employee => employee.PhysicalAddress)
                                .Include(employee => employee.PostalAddress)
                                .Select(employee => employee.ToDto())
                                .FirstOrDefaultAsync();

        if (employee == null)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string userEmail)
    {
        EmployeeTypeDto employeeTypeDto = await _employeeTypeService
            .GetEmployeeType(employeeDto.EmployeeType!.Name);
        Employee? employee = null;
        if (employeeDto.Email == userEmail)
        {
            employee = CreateNewEmployeeEntity(employeeDto, employeeTypeDto);
        }
        else
        {
            if (await CheckUserExist(userEmail))
            {
                if (await IsAdmin(userEmail))
                    employee = CreateNewEmployeeEntity(employeeDto, employeeTypeDto);
                else
                {
                    var exception = new Exception("Unauthorized action: You are not an Admin");
                    throw _errorLoggingService.LogException(exception);
                }

            }
            else
            {
                var exception = new Exception("User already exists");
                throw _errorLoggingService.LogException(exception);
            }
        }

        return await _db.Employee.Update(employee);
    }

    public async Task<EmployeeDto?> GetById(int employeeId)
    {
        var employee = await _db.Employee.GetById(employeeId);
        return employee;
    }

    public async Task<EmployeeCountDataCard> GenerateDataCardInformation()
    {
        var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

        var totalNumberOfEmployeesOnBench = GetTotalNumberOfEmployeesOnBench();

        var totalNumberOfEmployeesOnClients = GetTotalNumberOfEmployeesOnClients();

        var totalNumberOfEmployeesDevsScrumsAndDesigners = totalNumberOfEmployeesOnBench.TotalNumberOfEmployeesOnBench
                                                           + totalNumberOfEmployeesOnClients;

        var billableEmployeesPercentage = totalNumberOfEmployeesDevsScrumsAndDesigners > 0
            ? (double)totalNumberOfEmployeesOnClients / totalNumberOfEmployeesDevsScrumsAndDesigners * 100
            : 0;

        var currentMonthTotal = await GetEmployeeCurrentMonthTotal();

        var previousMonthTotal = await GetEmployeePreviousMonthTotal();

        var employeeTotalDifference = currentMonthTotal.EmployeeTotal - previousMonthTotal.EmployeeTotal;
        var isIncrease = employeeTotalDifference > 0;

        return new EmployeeCountDataCard
        {
            DevsCount = employeeCountTotalsByRole.DevsCount,
            DesignersCount = employeeCountTotalsByRole.DesignersCount,
            ScrumMastersCount = employeeCountTotalsByRole.ScrumMastersCount,
            BusinessSupportCount = employeeCountTotalsByRole.BusinessSupportCount,
            DevsOnBenchCount = totalNumberOfEmployeesOnBench.DevsOnBenchCount,
            DesignersOnBenchCount = totalNumberOfEmployeesOnBench.DesignersOnBenchCount,
            ScrumMastersOnBenchCount = totalNumberOfEmployeesOnBench.ScrumMastersOnBenchCount,
            TotalNumberOfEmployeesOnBench = totalNumberOfEmployeesOnBench.TotalNumberOfEmployeesOnBench,
            BillableEmployeesPercentage = Math.Round(billableEmployeesPercentage, 0),
            EmployeeTotalDifference = employeeTotalDifference,
            isIncrease = isIncrease
        };
    }

    public async Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate()
    {
        var employeeCurrentMonthTotal = await GetEmployeeCurrentMonthTotal();

        var employeePreviousMonthTotal = await GetEmployeePreviousMonthTotal();

        return new ChurnRateDataCardDto
        {
            ChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.EmployeeTotal ?? 0, employeePreviousMonthTotal?.EmployeeTotal ?? 0),
            DeveloperChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.DeveloperTotal ?? 0, employeePreviousMonthTotal?.DeveloperTotal ?? 0),
            DesignerChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.DesignerTotal ?? 0, employeePreviousMonthTotal?.DesignerTotal ?? 0),
            ScrumMasterChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.ScrumMasterTotal ?? 0, employeePreviousMonthTotal?.ScrumMasterTotal ?? 0),
            BusinessSupportChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.BusinessSupportTotal ?? 0, employeePreviousMonthTotal?.BusinessSupportTotal ?? 0),
            Month = employeePreviousMonthTotal?.Month ?? DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = employeePreviousMonthTotal?.Year ?? DateTime.Now.Year
        };
    }

    private double CalculateChurnRate(int employeeCurrentMonthTotal, int employeePreviousMonthTotal)
    {
        return Math.Round((employeePreviousMonthTotal > 0)
            ? (double)(employeeCurrentMonthTotal - employeePreviousMonthTotal)
              / employeePreviousMonthTotal * 100
            : 0, 0);
    }

    public async Task<MonthlyEmployeeTotalDto> GetEmployeeCurrentMonthTotal()
    {
        var currentMonth = DateTime.Now.ToString("MMMM");

        var currentYear = DateTime.Now.Year;

        var currentEmployeeTotal = _db.MonthlyEmployeeTotal
                                      .Get()
                                      .Where(e => e.Month == currentMonth && e.Year == currentYear)
                                      .FirstOrDefault();

        if (currentEmployeeTotal == null)
        {
            var employeeTotalCount = await _db.Employee.GetAll();

            var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

            var monthlyEmployeeTotalDto = new MonthlyEmployeeTotalDto
            {
                Id = 0,
                EmployeeTotal = employeeTotalCount.Count,
                DeveloperTotal = employeeCountTotalsByRole.DevsCount,
                DesignerTotal = employeeCountTotalsByRole.DesignersCount,
                ScrumMasterTotal = employeeCountTotalsByRole.ScrumMastersCount,
                BusinessSupportTotal = employeeCountTotalsByRole.BusinessSupportCount,
                Month = currentMonth,
                Year = currentYear
            };
            var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(monthlyEmployeeTotalDto);

            return await _db.MonthlyEmployeeTotal.Add(newMonthlyEmployeeTotal);
        }
        return currentEmployeeTotal.ToDto();
    }

    public async Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail)
    {
        var employeeDto = await GetEmployee(employeeEmail);
        var teamLeadName = "";
        var peopleChampionName = "";
        var teamLeadId = 0;
        var peopleChampionId = 0;
        var clientAllocatedId = 0;
        var clientAllocatedName = "";

        if (employeeDto!.TeamLead != null)
        {
            var teamLeadDto = await GetById((int)employeeDto.TeamLead);
            teamLeadName = teamLeadDto!.Name + " " + teamLeadDto.Surname;
            teamLeadId = teamLeadDto.Id;
        }

        if (employeeDto.PeopleChampion != null)
        {
            var peopleChampionDto = await GetById((int)employeeDto.PeopleChampion);
            peopleChampionName = peopleChampionDto!.Name + " " + peopleChampionDto.Surname;
            peopleChampionId = peopleChampionDto.Id;
        }

        if (employeeDto.ClientAllocated != null)
        {
            var clientDto = await _db.Client
                                     .Get(client => client.Id == employeeDto.ClientAllocated)
                                     .AsNoTracking()
                                     .Select(client => client.ToDto())
                                     .FirstAsync();

            clientAllocatedId = clientDto.Id;
            clientAllocatedName = clientDto.Name;
        }

        var simpleProfile = new SimpleEmployeeProfileDto
        {
            Id = employeeDto.Id,
            EmployeeNumber = employeeDto.EmployeeNumber,
            TaxNumber = employeeDto.TaxNumber,
            EngagementDate = employeeDto.EngagementDate,
            TerminationDate = employeeDto.TerminationDate,
            PeopleChampionName = peopleChampionName,
            PeopleChampionId = peopleChampionId == 0 ? null : peopleChampionId,
            Disability = employeeDto.Disability,
            DisabilityNotes = employeeDto.DisabilityNotes,
            Level = employeeDto.Level,
            EmployeeType = employeeDto.EmployeeType,
            Notes = employeeDto.Notes,
            LeaveInterval = employeeDto.LeaveInterval,
            SalaryDays = employeeDto.SalaryDays,
            PayRate = employeeDto.PayRate,
            Salary = employeeDto.Salary,
            Name = employeeDto.Name,
            Initials = employeeDto.Initials,
            Surname = employeeDto.Surname,
            DateOfBirth = employeeDto.DateOfBirth,
            CountryOfBirth = employeeDto.CountryOfBirth,
            Nationality = employeeDto.Nationality,
            IdNumber = employeeDto.IdNumber,
            PassportNumber = employeeDto.PassportNumber,
            PassportExpirationDate = employeeDto.PassportExpirationDate,
            PassportCountryIssue = employeeDto.PassportCountryIssue,
            Race = employeeDto.Race,
            Gender = employeeDto.Gender,
            Photo = employeeDto.Photo,
            Email = employeeDto.Email,
            PersonalEmail = employeeDto.PersonalEmail,
            CellphoneNo = employeeDto.CellphoneNo,
            ClientAllocatedName = clientAllocatedName,
            ClientAllocatedId = clientAllocatedId,
            TeamLeadName = teamLeadName,
            TeamLeadId = teamLeadId,
            PhysicalAddress = employeeDto.PhysicalAddress,
            PostalAddress = employeeDto.PostalAddress,
            HouseNo = employeeDto.HouseNo,
            EmergencyContactName = employeeDto.EmergencyContactName,
            EmergencyContactNo = employeeDto.EmergencyContactNo
        };
        return simpleProfile;
    }

    public async Task<List<EmployeeDto>> FilterEmployees(int peopleChampId = 0, int employeeType = 0, bool activeStatus = true)
    {
        return await _db.Employee
                        .Get(employee => true)
                        .Where(employee =>
                                   (peopleChampId == 0 || employee.PeopleChampion == peopleChampId)
                                   && (employeeType == 0 || employee.EmployeeType!.Id == employeeType)
                                   && (employee.Active == activeStatus))
                        .Include(employee => employee.EmployeeType)
                        .Include(employee => employee.PhysicalAddress)
                        .Include(employee => employee.PostalAddress)
                        .OrderBy(employee => employee.Name)
                        .Select(employee => employee.ToDto())
                        .ToListAsync();
    }

    public async void PushToProducerAsync(Employee employeeData)
    {
        try
        {
            {
                var messageBody = JsonConvert.SerializeObject(employeeData);
                var body = Encoding.UTF8.GetBytes(messageBody);

                 await using var sender = serviceBusClient.CreateSender(queueName);
                 await sender.SendMessageAsync(new ServiceBusMessage(body));
            }
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
        }
    }

    public EmployeeCountByRoleDataCard GetEmployeeCountTotalByRole()
    {
        var devsTotal = _db.Employee.Get()
                           .Where(e => e.EmployeeTypeId == 2)
                           .ToList().Count;

        var designersTotal = _db.Employee.Get()
                                .Where(e => e.EmployeeTypeId == 3)
                                .ToList().Count;

        var scrumMastersTotal = _db.Employee.Get()
                                   .Where(e => e.EmployeeTypeId == 4)
                                   .ToList().Count;

        var businessSupportTotal = _db.Employee.Get()
                                      .Where(e => e.EmployeeTypeId > 4)
                                      .ToList().Count;

        return new EmployeeCountByRoleDataCard
        {
            DevsCount = devsTotal,
            DesignersCount = designersTotal,
            ScrumMastersCount = scrumMastersTotal,
            BusinessSupportCount = businessSupportTotal
        };
    }

    public int GetTotalNumberOfEmployeesOnClients()
    {
        var totalOfDevsDesignersAndScrumsOnClients = _db.Employee
                                                        .Get()
                                                        .Where(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 ||
                                                                     e.EmployeeTypeId == 4) && e.ClientAllocated != 1)
                                                        .ToList()
                                                        .Count;

        return totalOfDevsDesignersAndScrumsOnClients;
    }

    public async Task<MonthlyEmployeeTotalDto> GetEmployeePreviousMonthTotal()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var previousEmployeeTotal = _db.MonthlyEmployeeTotal
                                       .Get()
                                       .Where(e => e.Month == previousMonth)
                                       .FirstOrDefault();

        if (previousEmployeeTotal == null) return await GetEmployeeCurrentMonthTotal();

        return previousEmployeeTotal.ToDto();
    }

    public EmployeeOnBenchDataCard GetTotalNumberOfEmployeesOnBench()
    {
        var totalNumberOfDevsOnBench = _db.Employee.Get()
                                          .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 2)
                                          .ToList().Count;

        var totalNumberOfDesignersOnBench = _db.Employee.Get()
                                               .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 3)
                                               .ToList().Count;

        var totalNumberOfScrumMastersOnBench = _db.Employee.Get()
                                                  .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 4)
                                                  .ToList().Count;

        var totalnumberOfEmployeesOnBench = totalNumberOfDevsOnBench +
                                            totalNumberOfDesignersOnBench +
                                            totalNumberOfScrumMastersOnBench;

        return new EmployeeOnBenchDataCard
        {
            DevsOnBenchCount = totalNumberOfDevsOnBench,
            DesignersOnBenchCount = totalNumberOfDesignersOnBench,
            ScrumMastersOnBenchCount = totalNumberOfScrumMastersOnBench,
            TotalNumberOfEmployeesOnBench = totalnumberOfEmployeesOnBench
        };
    }

    private async Task<bool> IsAdmin(string userEmail)
    {
        var employeeDto = await GetEmployee(userEmail);

        var empRole = await _db.EmployeeRole
                               .Get(role => role.EmployeeId == employeeDto!.Id)
                               .FirstOrDefaultAsync();

        var role = await _db.Role
                            .Get(role => role.Id == empRole!.RoleId)
                            .FirstOrDefaultAsync();

        return role!.Description is "Admin" or "SuperAdmin";
    }

    private async Task<bool> IsJourney(string userEmail)
    {
        var employeeDto = await GetEmployee(userEmail);
        var empRole = await _db.EmployeeRole
                               .Get(role => role.EmployeeId == employeeDto!.Id)
                               .FirstOrDefaultAsync();

        var role = await _db.Role
                            .Get(role => role.Id == empRole!.RoleId)
                            .FirstOrDefaultAsync();

        return role!.Description is "Journey";
    }

    private Employee CreateNewEmployeeEntity(EmployeeDto employeeDto, EmployeeTypeDto employeeTypeDto)
    {
        var employee = new Employee(employeeDto, employeeTypeDto)
        {
            Email = employeeDto.Email
        };

        return employee;
    }

    public async Task<bool> CheckDuplicateIdNumber(string idNumber, int employeeId)
    {
        var duplicateExists = await _db.Employee
                          .Get(employee => employee.IdNumber == idNumber && (employeeId == 0 || employee.Id != employeeId))
                          .AnyAsync();

        return duplicateExists;
    }
}
