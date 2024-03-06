using ATS.Models;
using ATS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.ATS;

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
        return await _db.Candidate
            .Any(candidate => candidate.PersonalEmail == candidateEmail);
    }

    public async Task<CandidateDto> SaveCandidate(CandidateDto candidate)
    {
        if (await CheckCandidateExists(candidate.PersonalEmail))
            throw new Exception("Candidate already exists");

        Candidate newCandidate = new Candidate(candidate);
        return await _db.Candidate.Add(newCandidate);
    }

    public async Task<List<CandidateDto>> GetAllCandidates() => 
        await _db.Candidate.GetAll();

    public async Task<CandidateDto> GetCandidateById(int id) => 
        await _db.Candidate.Get(candidate => candidate.Id == id)
        .Select(candidate => candidate.ToDto())
        .FirstAsync();

    public async Task<CandidateDto> GetCandidateByEmail(string email)
    {
        return await _db.Candidate
            .Get(candidate => candidate.PersonalEmail == email)
            .AsNoTracking()
            .Select(candidate => candidate.ToDto())
            .FirstAsync();
    }

    public async Task<CandidateDto> UpdateCandidate(CandidateDto candidateDto) => 
        await _db.Candidate.Update(new Candidate(candidateDto));

    public async Task<CandidateDto> DeleteCandidate(int id) => await 
        _db.Candidate.Delete(id);
}