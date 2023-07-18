using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Models
{
    public record FieldDto( int formid, int type, bool required, string label, string description, string errormessage);
        
}
