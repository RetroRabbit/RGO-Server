using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class SMTPSettings
    {
        public string? Name { get; set; }
        public string? Host { get; set; }
        public string? Mail { get; set; }
        public string? Password { get; set; }
        public string? Port { get; set; }
    }
}