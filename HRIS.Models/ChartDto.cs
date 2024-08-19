using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class ChartDto
{
    [Required(ErrorMessage = "Chart 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Chart 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Subtype { get; set; }
    public List<string>? DataTypes { get; set; }
    public List<string>? Labels { get; set; }
    public List<string>? Roles { get; set; }
    public List<ChartDataSetDto>? Datasets { get; set; }
}