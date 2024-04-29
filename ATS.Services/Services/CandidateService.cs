using ATS.Models;
using ATS.Services.Interfaces;
using HRIS.Models;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.ATS;
using RR.UnitOfWork.Entities.HRIS;

namespace ATS.Services.Services;

public class CandidateService : ICandidateService
{
    private readonly IUnitOfWork _db;

    public CandidateService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckCandidateExists(string candidateEmail)
    {
        return await _db.Employee
            .Any(candidate => candidate.PersonalEmail == candidateEmail);
    }

    public async Task<CandidateDto> SaveCandidate(CandidateDto candidate)
    {
        if (await CheckCandidateExists(candidate.PersonalEmail))
            throw new Exception("Candidate already exists");

        Employee newCandidate = new Employee(candidate);

        await _db.Employee.Add(newCandidate);
        
        return newCandidate.ToCandidateDto();
    }

    public async Task<List<CandidateDto>> GetAllCandidates()
    {
        List<EmployeeDto> employeeList = await _db.Employee.GetAll(candidate => candidate.IsCandidate);

        List<CandidateDto> candidateList = new List<CandidateDto>();

        if(employeeList != null)
        {
            foreach (var candidate in employeeList)
            {
                Employee employee = new Employee(candidate, new EmployeeTypeDto());

                candidateList.Add(employee.ToCandidateDto());
            }
        }

        return candidateList;
    }

    public async Task<CandidateDto> GetCandidateById(int id) => 
        await _db.Employee.Get(candidate => candidate.Id == id)
        .Select(candidate => candidate.ToCandidateDto())
        .FirstAsync();

    public async Task<CandidateDto> GetCandidateByEmail(string email)
    {
        return await _db.Employee
            .Get(candidate => candidate.PersonalEmail == email)
            .AsNoTracking()
            .Select(candidate => candidate.ToCandidateDto())
            .FirstAsync();
    }

    public async Task<CandidateDto> UpdateCandidate(CandidateDto candidateDto)
    {
        Employee candidate = new Employee(candidateDto);
        await _db.Employee.Update(candidate);
        return candidate.ToCandidateDto();
    }

    public async Task<CandidateDto> DeleteCandidate(int id)
    {
        var employeeDto = await _db.Employee.Delete(id);

        var tempEmployeeTypeDto = new EmployeeTypeDto() { Id = 0, Name = "temp" };

        var candidate = new Employee(employeeDto, tempEmployeeTypeDto);

        return candidate.ToCandidateDto();
    }
}