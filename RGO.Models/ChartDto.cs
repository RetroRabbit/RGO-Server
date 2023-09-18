namespace RGO.Models;

public record  ChartDto(
     int Id,
     string Name,
     string Type,
     List<string> Labels,
     List<int> Data
    );  
