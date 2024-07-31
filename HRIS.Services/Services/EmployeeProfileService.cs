using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;

namespace HRIS.Services.Services
{
    internal class EmployeeProfileService : IEmployeeProfileService
    { 
        private readonly AuthorizeIdentity _identity;
        private readonly UnitOfWork _db;
        private readonly EmployeeDataService _employeeDataService;
        private readonly EmployeeQualificationService _employeeQualificationService;
        private readonly WorkExperienceService _workExperienceService;
        private readonly EmployeeCertificationService _employeeCertificationService;
        private readonly EmployeeBankingService _employeeBankingService;
        public EmployeeProfileService(AuthorizeIdentity identity,
            EmployeeService employeeService, 
            EmployeeDataService employeeDataService, 
            EmployeeQualificationService employeeQualificationService,
            WorkExperienceService workExperienceService,
            EmployeeCertificationService employeeCertificationService,
            EmployeeBankingService employeeBankingService, UnitOfWork db)
        {
            _identity = identity;
            _db = db;
            _employeeDataService = employeeDataService;
            _employeeQualificationService = employeeQualificationService;
            _workExperienceService = workExperienceService;
            _employeeCertificationService = employeeCertificationService;
            _employeeBankingService = employeeBankingService;
        }
        async Task<EmployeeProfileDto> IEmployeeProfileService.GetEmployeeProfileById(int? id)
        {
            var employeeId = (int)id; ;

            if (id == null)
            {
                employeeId = _identity.EmployeeId;
            }

            var employee = await _db.Employee
                                    .Get(employee => employee.Id == id)
                                    .Include(employee => employee.EmployeeType)
                                    .Include(employee => employee.PhysicalAddress)
                                    .Include(employee => employee.PostalAddress)
                                    .Include(employee => employee.ChampionEmployee)
                                    .Include(employee => employee.ClientAllocated)
                                    .Include(employee => employee.TeamLeadAssigned)
                                    .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

            EmployeeProfileDetailsDto employeeDetails = new EmployeeProfileDetailsDto
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
                ClientAllocatedName = employee.ClientAssigned?.Name,
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

            EmployeeProfileSalaryDto salaryDetails = new EmployeeProfileSalaryDto
            {

            };

            var employeeData = await _employeeDataService.GetEmployeeData(employeeId);
            var employeeQualification = await _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(employeeId);
            var employeeWorkExperience = await _workExperienceService.GetWorkExperienceByEmployeeId(employeeId);
            var employeeCertifications = await _employeeCertificationService.GetEmployeeCertificationsByEmployeeId(employeeId);
            var employeeBanking = await _employeeBankingService.GetBanking(employeeId);

            EmployeeProfileDto employeeProfile = new EmployeeProfileDto
            {
                EmployeeProfileDetails = employeeDetails,
                EmployeeProfilePersonal = personalDetails,
                EmployeeProfileContact = contactDetails,
                EmployeeProfileSalary = salaryDetails,
                EmployeeData = employeeData,
                EmployeeQualification = employeeQualification,
                WorkExperience = employeeWorkExperience,
                EmployeeCertifications = employeeCertifications,
                EmployeeBanking = employeeBanking,

                AuthUserId = employee.AuthUserId,
                EmployeeNumber = employee.EmployeeNumber,
                Photo = employee.Photo,
                Notes = employee.Notes,
                PassportCountryIssue = employee.PassportCountryIssue,
                PassportExpirationDate = employee.PassportExpirationDate,
                PassportNumber = employee.PassportNumber,
                TerminationDate = employee.TerminationDate,
                Active = employee.Active,
                InactiveReason = employee.InactiveReason
            };

            return employeeProfile;
        }
    }
}