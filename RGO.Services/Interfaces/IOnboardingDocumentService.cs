using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IOnboardingDocumentService
{
    /// <summary>
    /// Save Onboarding Document
    /// </summary>
    /// <param name="onboardingDocumentDto"></param>
    /// <returns></returns>
    Task<OnboardingDocumentDto> SaveOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto);

    /// <summary>
    /// Delete Onboarding Document
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OnboardingDocumentDto> DeleteOnboardingDocument(int id);

    /// <summary>
    /// Get Onboarding Document
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OnboardingDocumentDto> GetOnboardingDocument(int id);

    /// <summary>
    /// Get All Onboarding Document
    /// </summary>
    /// <returns></returns>
    Task<List<OnboardingDocumentDto>> GetAllOnboardingDocument();

    /// <summary>
    /// Update Onboarding Document
    /// </summary>
    /// <param name="onboardingDocumentDto"></param>
    /// <returns></returns>
    Task<OnboardingDocumentDto> UpdateOnboardingDocument(OnboardingDocumentDto onboardingDocumentDto);
}
