using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IOnboardingDocumentService
{
    Task<OnboardingDocumentDto> AddOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto);
    Task<OnboardingDocumentDto> DeleteOnboardingDocument(int id);
    Task<OnboardingDocumentDto> GetOnboardingDocument(int id);
    Task<List<OnboardingDocumentDto>> GetAllOnboardingDocument();
    Task<OnboardingDocumentDto> UpdateOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto);
}
