using HRIS.Models;
using HRIS.Models.Enums.QualificationEnums;

namespace RGO.Tests.Data.Models;

public static class EmployeeQualificationTestData
{
    public static EmployeeQualificationDto BachelorQualification => new EmployeeQualificationDto
    {
        Id = 1,
        EmployeeId = 1,
        Qualification = "Bachelor's Degree",
        School = "University of Pretoria",
        Degree = (DegreeType)Enum.Parse(typeof(DegreeType), "Bachelor"),
        FieldOfStudy = (FieldOfStudy)Enum.Parse(typeof(FieldOfStudy), "Software Engineering"),
        NQF = (NQFLevel)Enum.Parse(typeof(NQFLevel), "NQF Level 7"),
        StartDate = new DateTime(2016, 9, 1),
        EndDate = new DateTime(2020, 6, 30)
    };

    public static EmployeeQualificationDto MasterQualification => new EmployeeQualificationDto
    {
        Id = 2,
        EmployeeId = 1,
        Qualification = "Master's Degree",
        School = "University of Johannesburg",
        Degree = (DegreeType)Enum.Parse(typeof(DegreeType), "Master"),
        FieldOfStudy = (FieldOfStudy)Enum.Parse(typeof(FieldOfStudy), "Data Science"),
        NQF = (NQFLevel)Enum.Parse(typeof(NQFLevel), "NQF Level 9"),
        StartDate = new DateTime(2021, 1, 1),
        EndDate = new DateTime(2023, 12, 31)
    };

    public static List<EmployeeQualificationDto> AllQualifications => new List<EmployeeQualificationDto>
    {
        BachelorQualification,
        MasterQualification
    };
}
