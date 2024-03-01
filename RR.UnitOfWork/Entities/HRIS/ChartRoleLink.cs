using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

public class ChartRoleLink : IModel<ChartRoleLinkDto>
{
    public ChartRoleLink()
    {
    }

    public ChartRoleLink(ChartRoleLinkDto chartRoleLinkDto)
    {
        Id = chartRoleLinkDto.Id;
        ChartId = chartRoleLinkDto.Chart!.Id;
        RoleId = chartRoleLinkDto.Role!.Id;
    }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Column("chartId")]
    [ForeignKey("Chart")]
    public int ChartId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Chart? Chart { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public ChartRoleLinkDto ToDto()
    {
        return new ChartRoleLinkDto {
                                    Id = Id,
                                    Chart = Chart?.ToDto(),
                                    Role = Role?.ToDto()
                                   };
    }
}
