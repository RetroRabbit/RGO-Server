using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class OnboardingDocumentService : IOnboardingDocumentService
{
    private readonly IOnboardingDocumentsRepository _onboardingDocumentRepository;

    public OnboardingDocumentService(IOnboardingDocumentsRepository onboardingDocumentRepository)
    {
        _onboardingDocumentRepository = onboardingDocumentRepository;
    }

    public async Task<OnboardingDocumentDto> AddOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto)
    {
        OnboardingDocumentDto newOnboardingDocument = await _onboardingDocumentRepository.Add(new OnboardingDocument(onboardingDocumentDto));

        return newOnboardingDocument;
    }

    public async Task<OnboardingDocumentDto> DeleteOnboardingDocument(int id)
    {
        OnboardingDocumentDto existingOnboardingDocument = await GetOnboardingDocument(id);

        OnboardingDocumentDto deletedOnboardingDocument = await _onboardingDocumentRepository
            .Delete(existingOnboardingDocument.Id);

        return deletedOnboardingDocument;
    }

    public Task<List<OnboardingDocumentDto>> GetAllOnboardingDocument()
    {
        return _onboardingDocumentRepository
            .GetAll();
    }

    public async Task<OnboardingDocumentDto> GetOnboardingDocument(int id)
    {
        OnboardingDocumentDto existingOnboardingDocument = await _onboardingDocumentRepository
            .Get(onboardingDocument => onboardingDocument.Id == id)
            .Select(onboardingDocument => onboardingDocument.ToDto())
            .FirstAsync();

        return existingOnboardingDocument;
    }

    public async Task<OnboardingDocumentDto> UpdateOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto)
    {
        OnboardingDocumentDto updatedOnboardingDocument = await _onboardingDocumentRepository
            .Update(new OnboardingDocument(onboardingDocumentDto));

        return updatedOnboardingDocument;
    }
}
