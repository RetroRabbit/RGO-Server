using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("Chart")]
public class Chart : IModel<ChartDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } 

    [Column("type")]
    public string Type { get; set; }

    [Column("dataTypes")]
    public List<string> DataTypes { get; set; }

    [Column("labels")]
    public List<string> Labels { get; set; }

    [Column("data")]
    public List<int> Data{ get; set; }

    public Chart() { }

    public Chart(ChartDto chartDto)
    {
        Id = chartDto.Id;
        Name = chartDto.Name;
        Type = chartDto.Type;
        DataTypes = chartDto.DataTypes;
        Labels = chartDto.Labels;
        Data = chartDto.Data;

    }

    public ChartDto ToDto()
    {
        return new ChartDto(
            Id,
            Name,
            Type,
            DataTypes,
            Labels,
            Data
        );
    }
}
