using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class FieldCodeData
{
    [Required(ErrorMessage = "Field Code Data 'NewFieldCode' field is missing.")]
    public List<FieldCodeDto>? NewFieldCode { get; set; }
    [Required(ErrorMessage = "Field Code Data 'FieldCodeOptions' field is missing.")]
    public List<FieldCodeOptionsDto>? FieldCodeOptions { get; set; }
}