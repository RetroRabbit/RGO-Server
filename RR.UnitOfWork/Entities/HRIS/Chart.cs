using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("Chart")]
public class Chart : IModel<ChartDto>
{
    public Chart()
    {
    }

    public Chart(ChartDto chartDto)
    {
        Id = chartDto.Id;
        Name = chartDto.Name;
        Type = chartDto.Type;
        DataTypes = chartDto.DataTypes;
    }

    [Column("name")] public string? Name { get; set; }

    [Column("type")] public string? Type { get; set; }

    [Column("dataTypes")] public List<string>? DataTypes { get; set; }

    public virtual List<ChartDataSet> DataSets { get; set; }



    [Key][Column("id")] public int Id { get; set; }

    public ChartDto ToDto()
    {
        return new ChartDto
        {
            Id = Id,
            Name = Name,
            Type = Type,
            DataTypes = DataTypes
        };
    }
}
