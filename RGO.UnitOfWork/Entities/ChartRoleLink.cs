using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace RGO.UnitOfWork.Entities
{
    public class ChartRoleLink : IModel<ChartRoleLinkDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("roleId")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [Column("chartId")]
        [ForeignKey("Chart")]
        public int ChartId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Chart Chart { get; set; }

        public ChartRoleLink() { }

        public ChartRoleLink(ChartRoleLinkDto chartRoleLinkDto)
        {
            Id = chartRoleLinkDto.Id;
            ChartId = chartRoleLinkDto.Chart.Id;
            RoleId = chartRoleLinkDto.Role.Id;
        }

        public ChartRoleLinkDto ToDto()
        {
            return new ChartRoleLinkDto(
                Id,
                Chart.ToDto(),
                Role.ToDto()
          );
        }

    }
}
