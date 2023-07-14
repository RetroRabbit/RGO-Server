using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Entities
{
    public class Field
    {
        public int id { get; set; }
        public int formid { get; set; }
        public int type { get; set; }
        public bool required { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public string errormessage { get; set;}

    }
}
