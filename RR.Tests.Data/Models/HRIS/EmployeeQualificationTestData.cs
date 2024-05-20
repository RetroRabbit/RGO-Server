using HRIS.Models.Enums.QualificationEnums;
using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS
{
    public class EmployeeQualificationTestData
    {
        public static EmployeeQualificationDto EmployeeQualification = new EmployeeQualificationDto
        {
            Id = 1,
            EmployeeId = 1,
            HighestQualification = HighestQualification.Bachelor,
            School = "University of Africa",
            Degree = "Bachelor of Science",
            FieldOfStudy = "Computer Science",
            NQFLevel = NQFLevel.Level7,
            Year = new DateOnly(2020, 1, 1)
        };
    }
}
