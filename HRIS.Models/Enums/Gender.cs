using System.ComponentModel;

namespace HRIS.Models.Enums;

public enum Gender
{
    [Description("Prefer Not To Say")] PreferNotToSay,
    [Description("Male")] Male,
    [Description("Female")] Female
}