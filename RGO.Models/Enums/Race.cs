using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;

namespace RGO.Models.Enums;

public enum Race 
{
    [Description("Black")] Black,
    [Description("White")] White,
    [Description("Indian")] Indian,
    [Description("Coloured")] Coloured,
    [Description("Asian")] Asian,
}
