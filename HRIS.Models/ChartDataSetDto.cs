using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class ChartDataSetDto
    {
        public int Id { get; set; }
        public List<string>? Labels { get; set; }
        public List<int>? Data { get; set; }
    }
}
