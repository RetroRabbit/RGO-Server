using RGO.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public record FieldCodeDto(
        int Id,
        string Code,
        string Name,
        string? Description,
        string? Regex,
        FieldCodeType Type,
        ItemStatus Status,
        bool Internal,
        string? InternalTable)
    {
        public List<FieldCodeOptionsDto> Options { get; set; }
    }
}
