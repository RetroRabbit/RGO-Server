using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("ChartDataSet")]
public class ChartDataSet : IModel<ChartDataSetDto>
{
    public ChartDataSet() { }
    public ChartDataSet(ChartDataSetDto chartDataSetDto)
    {
        Label = chartDataSetDto.Label;
        Data = chartDataSetDto.Data;
    }
    [Key][Column("id")] public int Id { get; set; }

    [Column("label")] public string? Label { get; set; }

    [Column("data")] public List<int>? Data { get; set; }


    [Column("chartId")]
    [ForeignKey("Chart")]
    public int ChartId { get; set; }

    public ChartDataSetDto ToDto()
    {
        return new ChartDataSetDto
        {
            Label = Label,
            Data = Data
        };
    }
}

