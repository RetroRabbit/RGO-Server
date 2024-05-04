using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class ClientProject
    {
        public int Id { get; set; }
        public string NameOfClient { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UploadProjectUrl { get; set; }
    }
}

