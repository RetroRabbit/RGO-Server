using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models;

public record EmployeeAccessDto(     
    int Id,
    int Condition,              //view, edit, Hiden
    bool Internal,              //Custom or internal
    string PropName,            //unique identifier
    string Label,               //Just for display
    string Type,                //text, bool, int,
    string? Description,        //additional front-end info
    string? Regex,              //additional validation for front-end
    List<string>? Options);     //for dropdowns



