using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RGO.UnitOfWork.Entities;

public class MetaPropertyOptions : IModel<MetaPropertyOptionsDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("metaPropertyId")]
    [ForeignKey("MetaProperty")]
    public int MetaPropertyId { get; set; }

    [Column("property")]
    public string Property { get; set; }

    [Column("option")]
    public string Option { get; set; }

    public virtual MetaProperty MetaProperty { get; set; }


    public MetaPropertyOptions() {}

    public MetaPropertyOptions(MetaPropertyOptionsDto metaPropertyOptions, MetaPropertyDto metaProperty)
    {
        Id = metaPropertyOptions.Id;
        MetaPropertyId = metaProperty.Id;
        Property = metaPropertyOptions.Property;
        Option = metaPropertyOptions.Option;
    }

    public MetaPropertyOptionsDto ToDto()
    {
        return new MetaPropertyOptionsDto(
            Id,
            MetaProperty.ToDto(),
            Property,
            Option
            );
    }
}
