namespace RGO.Domain.Models
{
    public record UserDto(
        int Id,
        int? GradGroupId,
        string FirstName,
        string LastName,
        string Email,
        int Type,
        DateTime JoinDate,
        int Status,
        string Bio, 
        int Level,
        string Phone
        );

}