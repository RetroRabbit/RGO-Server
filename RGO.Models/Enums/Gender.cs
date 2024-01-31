using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Reflection;

namespace RGO.Models.Enums;

public enum Gender
{
    [Description("Prefer Not To Say")] PreferNotToSay,
    [Description("Male")] Male,
    [Description("Female")] Female,
}