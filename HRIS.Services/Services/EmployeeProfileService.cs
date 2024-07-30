using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Services.Services
{
    internal class EmployeeProfileService : IEmployeeProfileService
    { 
        private readonly AuthorizeIdentity _identity;
        private readonly EmployeeService _employeeService;
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
            EmployeeBankingService employeeBankingService)
        {
            _identity = identity;
            _employeeService = employeeService;
            _employeeDataService = employeeDataService;
            _employeeQualificationService = employeeQualificationService;
            _workExperienceService = workExperienceService;
            _employeeCertificationService = employeeCertificationService;
            _employeeBankingService = employeeBankingService;
        }
        async Task<EmployeeProfileDto> IEmployeeProfileService.GetEmployeeProfileByEmail(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            var employeeData = await _employeeDataService.GetEmployeeData(id);
            var employeeQualification = await _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(id);
            var employeeWorkExperience = await _workExperienceService.GetWorkExperienceByEmployeeId(id);
            var employeeCertifications = await _employeeCertificationService.GetEmployeeCertificationsByEmployeeId(id);
            var employeeBanking = await _employeeBankingService.GetBanking(id);

            EmployeeProfileDto employeeProfile = new EmployeeProfileDto
            {
                Employee = employee,
                EmployeeData = employeeData,
                EmployeeQualification = employeeQualification,
                WorkExperience = employeeWorkExperience,
                EmployeeCertifications = employeeCertifications,
                EmployeeBanking = employeeBanking,
            };

            return employeeProfile;
        }
    }
}
