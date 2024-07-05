using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("Chart")]
public class Chart : IModel
{
    public Chart()
    {
    }

    public Chart(ChartDto chartDto)
    {
        Id = chartDto.Id;
        EmployeeId = chartDto.EmployeeId;
        Name = chartDto.Name;
        Type = chartDto.Type;
        Subtype = chartDto.Subtype;
        Labels = chartDto.Labels;
        Roles = chartDto.Roles;
        DataTypes = chartDto.DataTypes;
        Datasets = chartDto.Datasets?.Select(datasetDto => new ChartDataSet(datasetDto)).ToList() ?? new List<ChartDataSet>();
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    [Column("name")] public string? Name { get; set; }

    [Column("type")] public string? Type { get; set; }

    [Column("dataTypes")] public List<string>? DataTypes { get; set; }
    [Column("labels")] public List<string>? Labels { get; set; }
    [Column("roles")] public List<string>? Roles { get; set; }
    [Column("subType")]public string? Subtype { get; set; }


    public virtual List<ChartDataSet> Datasets { get; set; }

    [Key][Column("id")] public int Id { get; set; }

    public ChartDto ToDto()
    {
        return new ChartDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            Name = Name,
            Type = Type,
            Subtype = Subtype,
            DataTypes = DataTypes,
            Labels = Labels,
            Roles = Roles,
            Datasets = Datasets != null ? Datasets.Select(x => x.ToDto()).ToList() : new List<ChartDataSetDto>()
        };
    }
}
