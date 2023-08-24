using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class OnboardingDocumentsRepository : BaseRepository<OnboardingDocument, OnboardingDocumentDto>, IOnboardingDocumentsRepository
{
    public OnboardingDocumentsRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}