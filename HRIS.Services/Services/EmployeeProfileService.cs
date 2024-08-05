using HRIS.Models;
using HRIS.Models.EmployeeProfileModels;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;

namespace HRIS.Services.Services;

internal class EmployeeProfileService : IEmployeeProfileService
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
    async Task<EmployeeProfileDetailsDto> IEmployeeProfileService.GetEmployeeProfileDetailsById(int? id)
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

        EmployeeProfileEmployeeDetailsDto employeeDetails = new EmployeeProfileEmployeeDetailsDto
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

        EmployeeProfilePersonalDto personalDetails = new EmployeeProfilePersonalDto
        {
            Id = employee.Id,
            Race = employee.Race,
            Gender = employee.Gender,
            Nationality = employee.Nationality,
            CountryOfBirth = employee.CountryOfBirth,
            Disability = employee.Disability,
            DisabilityNotes = employee.DisabilityNotes
        };

        EmployeeProfileContactDto contactDetails = new EmployeeProfileContactDto
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

        EmployeeProfileDetailsDto employeeProfileDetails = new EmployeeProfileDetailsDto
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

    async Task<EmployeeProfileCareerSummaryDto> IEmployeeProfileService.GetEmployeeCareerSummaryById(int? id)
    {
        var employeeData = await GetEmployeeDataById(id);

        var employeeSalary = await GetEmployeeSalaryById(id);

        var employeeCertifications = await GetEmployeeCertificationsById(id);

        var employeeQualification = await GetEmployeeQualificationById(id);

        var employeeWorkExperience = await GetEmployeeWorkExperienceById(id);

        EmployeeProfileCareerSummaryDto employeeCareerSummary = new EmployeeProfileCareerSummaryDto
        {
            EmployeeProfileSalary = employeeSalary,
            EmployeeCertifications = employeeCertifications,
            EmployeeQualification = employeeQualification,
            WorkExperience = employeeWorkExperience,
            EmployeeData = employeeData,
        };

        return employeeCareerSummary;
    }

    async Task<EmployeeProfileBankingInformationDto> IEmployeeProfileService.GetEmployeeBankingInformationById(int? id)
    {
        var employeeBanking = await _db.EmployeeBanking
                                .Get(banking => banking.EmployeeId == id)
                                .Select(banking => banking.ToDto())
                                .ToListAsync();

        var employee = await _db.Employee
                                .Get(employee => employee.Id == id)
                                .FirstOrDefaultAsync();

        var employeeData = await GetEmployeeDataById(id);

        EmployeeProfileSalaryDto employeeSalary = new EmployeeProfileSalaryDto
        {
            LeaveInterval = employee.LeaveInterval,
            PayRate = employee.PayRate,
            SalaryDays = employee.SalaryDays,
            Salary = employee.Salary,
            TaxNumber = employee.TaxNumber,
        };

        EmployeeProfileBankingInformationDto employeeBankingInformation = new EmployeeProfileBankingInformationDto
        {
            EmployeeBanking = employeeBanking,
            EmployeeProfileSalary = employeeSalary,
            EmployeeData = employeeData,
        };

        return employeeBankingInformation;
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

    async Task<EmployeeProfileSalaryDto> GetEmployeeSalaryById(int? id)
    {
        var employee = await _db.Employee
                        .Get(employee => employee.Id == id)
                        .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        EmployeeProfileSalaryDto employeeProfileSalary = new EmployeeProfileSalaryDto
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