using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Entities
{
    public class User
    {

        public int id { get; set; }
        public int groupid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public int type { get; set; }
        public DateTime joindate { get; set; }
        public int status { get; set; }

    }
}
