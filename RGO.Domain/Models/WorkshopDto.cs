﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Models
{
    public record WorkshopDto(EventsDto events, string presenter);
}
