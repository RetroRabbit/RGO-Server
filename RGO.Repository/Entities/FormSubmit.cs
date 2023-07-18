using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Entities
{
    public  class FormSubmit
    {
        /*
         id SERIAL PRIMARY KEY,
          userId INTEGER REFERENCES users(id),
          formId INTEGER REFERENCES forms(id),
          createDate TIMESTAMP,
          status INTEGER,
          rejectionReason VARCHAR(255)
         */

        public int id { get; set; }
        public int userid { get; set; }
        public int formid { get; set; }
        public DateTime createDate { get; set; }
        public int status { get; set; }
        public string rejectionreason { get; set; }
    }
}
