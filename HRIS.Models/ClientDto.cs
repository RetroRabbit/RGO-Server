using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class ClientDto
{
    [Required (ErrorMessage = "Client 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Client 'Name' field is missing.")]
    public string? Name { get; set; }
}