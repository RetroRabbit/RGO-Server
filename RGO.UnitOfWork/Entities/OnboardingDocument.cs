using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("OnboardingDocument")]
public class OnboardingDocument : IModel<OnboardingDocumentDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("fileName")]
    public string FileName { get; set; }

    [Column("blob")]
    public byte[] Blob { get; set; }

    [Column("status")]
    public ItemStatus Status { get; set; }

    public OnboardingDocument() { }

    public OnboardingDocument(OnboardingDocumentDto onboardingDocumentsDto)
    {
        Id = onboardingDocumentsDto.Id;
        Title = onboardingDocumentsDto.Title;
        Description = onboardingDocumentsDto.Description;
        FileName = onboardingDocumentsDto.FileName;
        Blob = onboardingDocumentsDto.Blob;
        Status = onboardingDocumentsDto.Status;
    }

    public OnboardingDocumentDto ToDto()
    {
        return new OnboardingDocumentDto(
            Id,
            Title,
            Description,
            FileName,
            Blob,
            Status);
    }
}
