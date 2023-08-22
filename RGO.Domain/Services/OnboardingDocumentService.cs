using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class OnboardingDocumentService : IOnboardingDocumentService
{
    private readonly IUnitOfWork _db;

    public OnboardingDocumentService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<OnboardingDocumentDto> AddOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto)
    {
        OnboardingDocumentDto newOnboardingDocument = await _db.OnboardingDocuments.Add(new OnboardingDocument(onboardingDocumentDto));

        return newOnboardingDocument;
    }

    public async Task<OnboardingDocumentDto> DeleteOnboardingDocument(int id)
    {
        OnboardingDocumentDto existingOnboardingDocument = await GetOnboardingDocument(id);

        OnboardingDocumentDto deletedOnboardingDocument = await _db.OnboardingDocuments
            .Delete(existingOnboardingDocument.Id);

        return deletedOnboardingDocument;
    }

    public Task<List<OnboardingDocumentDto>> GetAllOnboardingDocument()
    {
        return _db.OnboardingDocuments
            .GetAll();
    }

    public async Task<OnboardingDocumentDto> GetOnboardingDocument(int id)
    {
        OnboardingDocumentDto existingOnboardingDocument = await _db.OnboardingDocuments
            .Get(onboardingDocument => onboardingDocument.Id == id)
            .Select(onboardingDocument => onboardingDocument.ToDto())
            .FirstAsync();

        return existingOnboardingDocument;
    }

    public async Task<OnboardingDocumentDto> UpdateOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto)
    {
        OnboardingDocumentDto updatedOnboardingDocument = await _db.OnboardingDocuments
            .Update(new OnboardingDocument(onboardingDocumentDto));

        return updatedOnboardingDocument;
    }
}
