using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Models
{
    public record SocialDto(
    int id,
    UserDto userid,
    string discord,
    string codewars,
    string github,
    string linkedin);
}
