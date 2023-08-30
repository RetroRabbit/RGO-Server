using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models;

/*public record EmployeeAccessDto(Dictionary<string, object> Fields);*/
public record EmployeeAccessDto(     
    int Id,
    int Condition,              //view, edit, Hiden
    int FieldType,              //Custom or meta
    string PropName,            //related to FieldCode.Code or PropertyAccess.MetaField
    string Label,               //Just for display
    string Type,                //text, bool, int,
    string? Description,         // for feldcode
    string? Regex,               //for fieldcode
    List<string>? Options);     //for dropdowns



