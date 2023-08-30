using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public record  ChartDto(
         int Id,
         string Name,
         string Type,
         List<string> Labels,
         List<int> Data
        );  
}
