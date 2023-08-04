using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;
[Table("UserStacks")]
public class UserStacks
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Column("backendId")]
    [ForeignKey("BackendUserStack")]
    public int BackendId { get; set; }
    [Column("frontendId")]
    [ForeignKey("FrontendUserStack")]
    public int FrontendId { get; set; }
    [Column("databaseId")]
    [ForeignKey("DatabaseUserStack")]
    public int DatabaseId { get; set; }
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    [Column("status")]
    public int Status { get; set; }
    [Column("createDate")]
    public DateTime CreateDate { get; set; }
    public virtual User User { get; set; }
    public virtual Stacks BackendUserStack { get; set; }
    public virtual Stacks FrontendUserStack { get; set; }
    public virtual Stacks DatabaseUserStack { get; set; }
    public UserStacks() { }
    public UserStacks(UserStackDto userStackDto)
    {
        Id = userStackDto.Id;
        UserId = userStackDto.UserId;
        BackendId = userStackDto.BackendId;
        FrontendId = userStackDto.FrontendId;
        DatabaseId = userStackDto.DatabaseId;
        Description = userStackDto.Description;
        Status = userStackDto.Status;
        CreateDate = userStackDto.CreateDate;
    }
    public UserStackDto ToDTO()
    {
        return new UserStackDto(
            Id,
            UserId,
            BackendId,
            FrontendId,
            DatabaseId,
            Description,
            Status,
            CreateDate);
    }
}