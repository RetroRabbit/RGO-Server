using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Models
{
    public record EventsDto(int groupid, string title, string description, int userType, DateTime startDate, DateTime endDate, int eventType);
}
