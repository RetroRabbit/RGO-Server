using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class AuthManagement
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? ClientSecret { get; set; }
        public string? ClientId { get; set; }
    }
}