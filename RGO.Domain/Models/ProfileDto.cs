using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Models
{
    public record ProfileDto(
        int id,
        string firstname,
        string lastname,
        string email,
        string phonenumber,
        string level,
        string bio,
       
        string dicord,
        string codewars,
        string github,
        string linkedin,

        object skills,
        object certififcations,
        object projects
    );
}
