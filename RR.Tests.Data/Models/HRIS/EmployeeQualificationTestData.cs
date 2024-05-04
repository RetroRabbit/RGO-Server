using HRIS.Models;
using System;
using System.Collections.Generic;

namespace RGO.Tests.Data.Models;

public static class EmployeeQualificationTestData
{
    public static EmployeeQualificationDto BachelorQualification => new EmployeeQualificationDto
    {
        Id = 1,
        EmployeeId = 1,
        Qualification = "Bachelor's Degree",
        School = "University of Pretoria",
        Degree = "Computer Science",
        FieldOfStudy = "Software Engineering",
        NQF = "NQF Level 7",
        StartDate = new DateTime(2016, 9, 1),
        EndDate = new DateTime(2020, 6, 30)
    };

    public static EmployeeQualificationDto MasterQualification => new EmployeeQualificationDto
    {
        Id = 2,
        EmployeeId = 1,
        Qualification = "Master's Degree",
        School = "University of Johannesburg",
        Degree = "Information Technology",
        FieldOfStudy = "Data Science",
        NQF = "NQF Level 9",
        StartDate = new DateTime(2021, 1, 1),
        EndDate = new DateTime(2023, 12, 31)
    };

    public static List<EmployeeQualificationDto> AllQualifications => new List<EmployeeQualificationDto>
    {
        BachelorQualification,
        MasterQualification
    };
}
