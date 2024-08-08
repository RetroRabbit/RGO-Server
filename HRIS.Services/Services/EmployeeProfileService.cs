﻿using HRIS.Models;
using HRIS.Models.Employee.Commons;
using HRIS.Models.Employee.Profile;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeProfileService : IEmployeeProfileService
{
    private readonly AuthorizeIdentity _identity;
    private readonly IUnitOfWork _db;
    private readonly IEmployeeDataService _employeeDataService;
    private readonly IEmployeeQualificationService _employeeQualificationService;
    private readonly IWorkExperienceService _workExperienceService;
    private readonly IEmployeeCertificationService _employeeCertificationService;
    private readonly IEmployeeBankingService _employeeBankingService;
    public EmployeeProfileService(AuthorizeIdentity identity,
        IEmployeeDataService employeeDataService,
        IEmployeeQualificationService employeeQualificationService,
        IWorkExperienceService workExperienceService,
        IEmployeeCertificationService employeeCertificationService,
        IEmployeeBankingService employeeBankingService, IUnitOfWork db)
    {
        _identity = identity;
        _db = db;
        _employeeDataService = employeeDataService;
        _employeeQualificationService = employeeQualificationService;
        _workExperienceService = workExperienceService;
        _employeeCertificationService = employeeCertificationService;
        _employeeBankingService = employeeBankingService;
    }
    public async Task<ProfileDetailsDto> GetEmployeeProfileDetailsById(int? id)
    {
        var employeeId = (int)id;

        if (id == null)
        {
            employeeId = _identity.EmployeeId;
        }

        var employee = await _db.Employee
                                .Get(employee => employee.Id == id)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Include(employee => employee.PhysicalAddress)
                                .Include(employee => employee.PostalAddress)
                                .Include(employee => employee.ChampionEmployee)
                                .Include(employee => employee.TeamLeadAssigned)
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        var clientAllocated = await _db.Client.Get(c => c.Id == employee.ClientAllocated).FirstOrDefaultAsync();

        EmployeeDetailsDto employeeDetails = new EmployeeDetailsDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Surname = employee.Surname,
            Initials = employee.Initials,
            DateOfBirth = employee.DateOfBirth,
            EmployeeType = employee.EmployeeType!.ToDto(),
            EngagementDate = employee.EngagementDate,
            IdNumber = employee.IdNumber,
            Level = employee.Level,
            ClientAllocatedId = employee.ClientAllocated,
            PeopleChampionId = employee.PeopleChampion,
            TeamLeadId = employee.TeamLead,
            ClientAllocatedName = clientAllocated?.Name,
            PeopleChampionName = employee.ChampionEmployee?.Name + " " + employee.ChampionEmployee?.Surname,
            TeamLeadName = employee.TeamLeadAssigned?.Name + " " + employee.TeamLeadAssigned?.Surname,
        };

        PersonalDetailsDto personalDetails = new PersonalDetailsDto
        {
            Id = employee.Id,
            Race = employee.Race,
            Gender = employee.Gender,
            Nationality = employee.Nationality,
            CountryOfBirth = employee.CountryOfBirth,
            Disability = employee.Disability,
            DisabilityNotes = employee.DisabilityNotes
        };

        ContactDetailsDto contactDetails = new ContactDetailsDto
        {
            Id = employee.Id,
            Email = employee.Email,
            PersonalEmail = employee.PersonalEmail,
            CellphoneNo = employee.CellphoneNo,
            HouseNo = employee.HouseNo,
            EmergencyContactName = employee.EmergencyContactName,
            EmergencyContactNo = employee.EmergencyContactNo
        };

        var employeeData = GetEmployeeDataById(id).Result;

        ProfileDetailsDto employeeProfileDetails = new ProfileDetailsDto
        {
            EmployeeProfileDetails = employeeDetails,
            EmployeeProfilePersonal = personalDetails,
            EmployeeProfileContact = contactDetails,
            EmployeeData = employeeData,
            Photo = employee.Photo,
            Active = employee.Active,
            PhysicalAddress = employee.PhysicalAddress?.ToDto()
        };

        return employeeProfileDetails;
    }

    public async Task<Employee> UpdateEmployeeDetails(EmployeeDetailsDto employeeDetails)
    {
        Employee? model = await GetEmployeeModelById(employeeDetails.Id);

        model.EngagementDate = employeeDetails.EngagementDate;
        model.PeopleChampion = employeeDetails.PeopleChampionId;
        model.Level = employeeDetails.Level;
        model.EmployeeType.Id = employeeDetails.EmployeeType.Id;
        model.EmployeeType.Name = employeeDetails.EmployeeType.Name;
        model.Name = employeeDetails.Name;
        model.Initials = employeeDetails.Initials;
        model.Surname = employeeDetails.Surname;
        model.DateOfBirth = employeeDetails.DateOfBirth;
        model.IdNumber = employeeDetails.IdNumber;
        model.ClientAllocated = employeeDetails.ClientAllocatedId;
        model.TeamLead = employeeDetails.TeamLeadId;

        var updatedEmployee = await _db.Employee.Update(model);

        return updatedEmployee;
    }

    public async Task<Employee> UpdatePersonalDetails(PersonalDetailsDto personalDetails)
    {
        Employee? model = await GetEmployeeModelById(personalDetails.Id);

        model.Gender = personalDetails.Gender;
        model.Race = personalDetails.Race;
        model.Nationality = personalDetails.Nationality;
        model.CountryOfBirth = personalDetails.CountryOfBirth;
        model.Disability = personalDetails.Disability;
        model.DisabilityNotes = personalDetails.DisabilityNotes;

        var updatedEmployee = await _db.Employee.Update(model);

        return updatedEmployee;
    }

    public async Task<Employee> UpdateContactDetails(ContactDetailsDto contactDetails)
    {
        Employee? model = await GetEmployeeModelById(contactDetails.Id);

        model.Email = contactDetails.Email;
        model.PersonalEmail = contactDetails.PersonalEmail;
        model.CellphoneNo = contactDetails.CellphoneNo;
        model.HouseNo = contactDetails.HouseNo;
        model.EmergencyContactNo = contactDetails.EmergencyContactNo;
        model.EmergencyContactName = contactDetails.EmergencyContactName;

        var updatedEmployee = await _db.Employee.Update(model);

        return updatedEmployee;
    }

    public async Task<CareerSummaryDto> GetEmployeeCareerSummaryById(int? id)
    {
        var employeeData = await GetEmployeeDataById(id);

        var employeeSalary = await GetEmployeeSalaryById(id);

        var employeeCertifications = await GetEmployeeCertificationsById(id);

        var employeeQualification = await GetEmployeeQualificationById(id);

        var employeeWorkExperience = await GetEmployeeWorkExperienceById(id);

        CareerSummaryDto employeeCareerSummary = new CareerSummaryDto
        {
            EmployeeProfileSalary = employeeSalary,
            EmployeeCertifications = employeeCertifications,
            EmployeeQualification = employeeQualification,
            WorkExperience = employeeWorkExperience,
            EmployeeData = employeeData,
        };

        return employeeCareerSummary;
    }

    public async Task<BankingInformationDto> GetEmployeeBankingInformationById(int? id)
    {
        var employeeBanking = await _db.EmployeeBanking
                                .Get(banking => banking.EmployeeId == id)
                                .Select(banking => banking.ToDto())
                                .ToListAsync();

        var employee = await _db.Employee
                                .Get(employee => employee.Id == id)
                                .FirstOrDefaultAsync();

        var employeeData = await GetEmployeeDataById(id);

        SalaryDetailsDto employeeSalary = new SalaryDetailsDto
        {
            LeaveInterval = employee.LeaveInterval,
            PayRate = employee.PayRate,
            SalaryDays = employee.SalaryDays,
            Salary = employee.Salary,
            TaxNumber = employee.TaxNumber,
        };

        BankingInformationDto employeeBankingInformation = new BankingInformationDto
        {
            EmployeeBanking = employeeBanking,
            EmployeeData = employeeData,
        };

        return employeeBankingInformation;
    }





    async Task<Employee> GetEmployeeModelById(int? id)
    {
        Employee? model = await _db.Employee
                        .Get(employee => employee.Id == id)
                        .AsNoTracking()
                        .Include(employee => employee.EmployeeType)
                        .Include(employee => employee.PhysicalAddress)
                        .Include(employee => employee.PostalAddress)
                        .Include(employee => employee.ChampionEmployee)
                        .Include(employee => employee.TeamLeadAssigned)
                        .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        return model;
    }
    async Task<EmployeeDataDto> GetEmployeeDataById(int? id)
    {
        var employeeData = await _db.EmployeeData.GetById((int)id!);

        EmployeeDataDto employeeDataDto;

        if (employeeData == null)
        {
            employeeDataDto = new EmployeeDataDto
            {
                Id = 0,
                EmployeeId = 0,
                FieldCodeId = 0,
                Value = "",
            };
        }
        else
        {
            employeeDataDto = new EmployeeDataDto
            {
                Id = employeeData.Id,
                EmployeeId = employeeData.Id,
                FieldCodeId = (int)employeeData.FieldCodeId!,
                Value = employeeData.Value,
            };
        }

        return employeeDataDto;
    }

    async Task<SalaryDetailsDto> GetEmployeeSalaryById(int? id)
    {
        var employee = await _db.Employee
                        .Get(employee => employee.Id == id)
                        .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        SalaryDetailsDto employeeProfileSalary = new SalaryDetailsDto
        {
            Salary = employee.Salary,
            SalaryDays = employee.SalaryDays,
            TaxNumber = employee.TaxNumber,
            LeaveInterval = employee.LeaveInterval,
            PayRate = employee.PayRate,
        };

        return employeeProfileSalary;
    }

    async Task<List<EmployeeCertificationDto>> GetEmployeeCertificationsById(int? id)
    {
        var certifications = await _db.EmployeeCertification
                                      .Get(certification => certification.EmployeeId == id).FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Certifications");

        List<EmployeeCertificationDto> employeeCertifications = new List<EmployeeCertificationDto>
        {
            certifications.ToDto(),
        };

        return employeeCertifications;
    }

    async Task<EmployeeQualificationDto> GetEmployeeQualificationById(int? id)
    {
        var qualification = await _db.EmployeeQualification
                                .Get(qualification => qualification.EmployeeId == id)
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Qualification");
        return qualification.ToDto();
    }

    async Task<List<WorkExperienceDto>> GetEmployeeWorkExperienceById(int? id)
    {
        return await _db.WorkExperience
                .Get(workExperience => workExperience.EmployeeId == id)
                .Select(workExperience => workExperience.ToDto())
                .ToListAsync();
    }
}