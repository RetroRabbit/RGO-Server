using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class ChartDataSetTestData
{
    public static ChartDataSet ChartDataOne = new()
    {
        Label = "Lable 1",
        Data = new List<int> { 10, 20, 30 }
    };

    public static ChartDataSet ChartDataTwo = new()
    {
        Label = "Lable 2",
        Data = new List<int> { 10, 20, 30 }
    };

    public static List<ChartDataSet> ChartDataSetList = new() { ChartDataOne, ChartDataTwo };
}
