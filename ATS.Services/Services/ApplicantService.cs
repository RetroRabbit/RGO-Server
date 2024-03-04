using ATS.Models;
using ATS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.ATS;

namespace ATS.Services.Services;

public class ApplicantService : IApplicantService
{
    private readonly IUnitOfWork _db;

    public ApplicantService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckApplicantExists(string applicantEmail)
    {
        return await _db.Applicant
            .Any(applicant => applicant.PersonalEmail == applicantEmail);
    }

    public async Task<ApplicantDto> SaveApplicant(ApplicantDto applicant)
    {
        if (await CheckApplicantExists(applicant.PersonalEmail))
             throw new Exception("Applicant already exists");

        Applicant newApplicant = new Applicant(applicant);
        return await _db.Applicant.Add(newApplicant);
    }

    public async Task<List<ApplicantDto>> GetAllApplicants() => 
        await _db.Applicant.GetAll();

    public async Task<ApplicantDto> GetApplicantById(int id) => 
        await _db.Applicant.Get(applicant => applicant.Id == id)
        .Select(applicant => applicant.ToDto())
        .FirstAsync();

    public async Task<ApplicantDto> GetApplicantByEmail(string email)
    {
        return await _db.Applicant
            .Get(applicant => applicant.PersonalEmail == email)
            .AsNoTracking()
            .Select(applicant => applicant.ToDto())
            .FirstAsync();
    }

    public async Task<ApplicantDto> UpdateApplicant(ApplicantDto applicantDto) => 
        await _db.Applicant.Update(new Applicant(applicantDto));

    public async Task<ApplicantDto> DeleteApplicant(int id) => await 
        _db.Applicant.Delete(id);
}