using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.UnitOfWork.Entities
{
    [Table("Chart")]
    public class Chart : IModel<ChartDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name{ get; set; }

        [Column("Type")]
        public string Type { get; set; }

        [Column("Labels")]
        public List<string> Labels { get; set; }

        [Column("Data")]
        public List<int> Data{ get; set; }

        public Chart() { }

        public Chart(ChartDto chartDto)
        {
            Id = chartDto.Id;
            Name = chartDto.Name;
            Type = chartDto.Type;
            Labels = chartDto.Labels;
            Data = chartDto.Data;
        }

        public ChartDto ToDto()
        {
            return new ChartDto(
                Id,
                Name,
                Type,
                Labels,
                Data
          );
        }
    }
}
