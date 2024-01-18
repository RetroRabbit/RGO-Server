using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public class ChurnRateDto
    {
        public double ChurnRate { get; set; }

        public double DeveloperChurnRate { get; set; }

        public double DesignerChurnRate { get; set; }

        public double ScrumMasterChurnRate { get; set; }

        public double BusinessSupportChurnRate { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }
    }
}
