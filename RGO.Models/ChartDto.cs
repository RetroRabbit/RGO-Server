namespace RGO.Models;

public record  ChartDto(
     int Id,
     string Name,
     string Type,
     string DataType,
     List<string> Labels,
     List<int> Data
    );  
