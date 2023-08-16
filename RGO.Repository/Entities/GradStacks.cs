using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;
[Table("GradStacks")]
public class GradStacks
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Column("backendId")]
    [ForeignKey("BackendGradStack")]
    public int BackendId { get; set; }
    [Column("frontendId")]
    [ForeignKey("FrontendGradStack")]
    public int FrontendId { get; set; }
    [Column("databaseId")]
    [ForeignKey("DatabaseGradStack")]
    public int DatabaseId { get; set; }
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    [Column("status")]
    public int Status { get; set; }
    [Column("createDate")]
    public DateTime CreateDate { get; set; }
    public virtual User User { get; set; }
    public virtual Stacks BackendGradStack { get; set; }
    public virtual Stacks FrontendGradStack { get; set; }
    public virtual Stacks DatabaseGradStack { get; set; }
    public GradStacks() { }
    public GradStacks(GradStackDto gradStackDto)
    {
        Id = gradStackDto.Id;
        UserId = gradStackDto.UserId;
        BackendId = gradStackDto.Backend.Id;
        FrontendId = gradStackDto.Frontend.Id;
        DatabaseId = gradStackDto.Database.Id;
        Description = gradStackDto.Description;
        Status = (int)gradStackDto.Status;
        CreateDate = gradStackDto.CreateDate;
    }
    public GradStackDto ToDTO()
    {
        return new GradStackDto(
            Id,
            UserId,
            BackendGradStack.ToDTO(),
            FrontendGradStack.ToDTO(),
            DatabaseGradStack.ToDTO(),
            Description,
            (GradStackStatus)Status,
            CreateDate);
    }
}