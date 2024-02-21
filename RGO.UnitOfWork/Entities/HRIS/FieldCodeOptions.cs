using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("FieldCodeOptions")]
public class FieldCodeOptions : IModel<FieldCodeOptionsDto>
{
    public FieldCodeOptions()
    {
    }

    public FieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
    {
        Id = fieldCodeOptionsDto.Id;
        Option = fieldCodeOptionsDto.Option;
        FieldCodeId = fieldCodeOptionsDto.FieldCodeId;
    }

    [Column("fieldCodeId")]
    [ForeignKey("FieldCode")]
    public int FieldCodeId { get; set; }

    [Column("option")] public string? Option { get; set; }

    public virtual FieldCode? FieldCode { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public FieldCodeOptionsDto ToDto()
    {
        return new FieldCodeOptionsDto(
                                       Id,
                                       FieldCodeId,
                                       Option!
                                      );
    }
}