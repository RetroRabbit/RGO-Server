using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class OnboardingDocumentUnitTests
{
    [Fact]
    public void OnboardingDocumentTest()
    {
        var onboardingDocument = new OnboardingDocument();
        Assert.IsType<OnboardingDocument>(onboardingDocument);
        Assert.NotNull(onboardingDocument);
    }

    [Fact]
    public void OnboardingDocumentToDtoTest()
    {
        var onboardingDocument = new OnboardingDocument
        {
            Id = 1,
            Title = "Title",
            Description = "Description",
            FileName = "FileName",
            Blob = new byte[] { 0 },
            Status = Models.Enums.ItemStatus.Active
        };

        var onboardingDocumentDto = onboardingDocument.ToDto();

        Assert.Equal(onboardingDocument.Id, onboardingDocumentDto.Id);
        Assert.Equal(onboardingDocument.Title, onboardingDocumentDto.Title);
        Assert.Equal(onboardingDocument.Description, onboardingDocumentDto.Description);
        Assert.Equal(onboardingDocument.FileName, onboardingDocumentDto.FileName);
        Assert.Equal(onboardingDocument.Blob, onboardingDocumentDto.Blob);
        Assert.Equal(onboardingDocument.Status, onboardingDocumentDto.Status);

        onboardingDocument = new OnboardingDocument(onboardingDocumentDto);

        Assert.Equal(onboardingDocument.Id, onboardingDocumentDto.Id);
    }
}
