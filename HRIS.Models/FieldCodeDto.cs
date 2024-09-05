using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class FieldCodeDto
{
    [Required(ErrorMessage = "Field Code 'Options' field is missing.")]
    public List<FieldCodeOptionsDto> Options { get; set; }
    [Required(ErrorMessage = "Field Code 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Field Code 'Code' field is missing.")]
    public string? Code { get; set; }
    [Required(ErrorMessage = "Field Code 'Name' field is missing.")]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Regex { get; set; }
    [Required(ErrorMessage = "Field Code 'Type' field is missing.")]
    public FieldCodeType Type { get; set; }
    [Required(ErrorMessage = "Field Code 'Status' field is missing.")]
    public ItemStatus Status { get; set; }
    [Required(ErrorMessage = "Field Code 'Internal' field is missing.")]
    public bool Internal { get; set; }
    public string? InternalTable { get; set; }
    [Required(ErrorMessage = "Field Code 'Category' field is missing.")]
    public FieldCodeCategory Category { get; set; }
    [Required(ErrorMessage = "Field Code 'Required' field is missing.")]
    public bool Required { get; set; }
}
