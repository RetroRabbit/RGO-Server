using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS;

public class ChartDataSetTestData
{
    public static ChartDataSetDto chartData1 = new ChartDataSetDto
    {
        Label = "Lable 1",
        Data = new List<int> { 10, 20, 30 }
    };

    public static ChartDataSetDto chartData2 = new ChartDataSetDto
    {
        Label = "Lable 2",
        Data = new List<int> { 10, 20, 30 }
    };

    public static List<ChartDataSetDto> chartDataSetDtoList = new List<ChartDataSetDto> { chartData1, chartData2 };
}
