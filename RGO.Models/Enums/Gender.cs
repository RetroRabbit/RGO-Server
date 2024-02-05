using System.ComponentModel;

namespace RGO.Models.Enums;

public enum Gender
{
    [Description("Prefer Not To Say")] PreferNotToSay,
    [Description("Male")] Male,
    [Description("Female")] Female,
}