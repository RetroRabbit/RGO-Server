using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class FieldCodeOptionsDto
{
    [Required(ErrorMessage = "Field Code Options 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Field Code Options 'FieldCodeId' field is missing.")]
    public int FieldCodeId { get; set; }
    [Required(ErrorMessage = "Field Code Options 'Option' field is missing.")]
    public string Option { get; set; }
}
