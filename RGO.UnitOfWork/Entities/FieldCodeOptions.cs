using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities
{
    [Table("FieldCodeOptions")]
    public class FieldCodeOptions : IModel<FieldCodeOptionsDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("fieldCodeId")]
        [ForeignKey("FieldCode")]
        public int FieldCodeId { get; set; }

        [Column("option")]
        public string Option { get; set; }  

        public virtual FieldCode FieldCode { get; set; }

        public FieldCodeOptions() { }
        public FieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
        {
            Id = fieldCodeOptionsDto.Id;
            Option = fieldCodeOptionsDto.Option;
            FieldCodeId = fieldCodeOptionsDto.FieldCode.Id;
        }

        public FieldCodeOptionsDto ToDto()
        {
            return new FieldCodeOptionsDto(
                Id,
                FieldCode.ToDto(),
                Option
                );
        }
    }
}
