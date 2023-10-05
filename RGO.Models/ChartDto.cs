namespace RGO.Models;

public record  ChartDto(
     int Id,
     string Name,
     string Type,
     List<string> DataTypes,
     List<string> Labels,
     List<int> Data
    );  
