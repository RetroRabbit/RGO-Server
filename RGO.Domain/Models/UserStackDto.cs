namespace RGO.Domain.Models
{
    public record UserStackDto(
        int Id,
        int UserId,
        int BackendId,
        int FrontendId,
        int DatabaseId,
        string Description,
        int Status,
        DateTime CreateDate);
}
