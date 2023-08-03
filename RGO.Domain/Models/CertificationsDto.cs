namespace RGO.Domain.Models
{
    public record CertificationsDto
    (
        int Id,
        int UserId,
        string Title,
        string Description
    );
}