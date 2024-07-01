using HRIS.Models.Enums.QualificationEnums;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS
{
    public class EmployeeQualificationTestData
    {
        public static EmployeeQualification EmployeeQualification = new()
        {
            Id = 1,
            EmployeeId = 1,
            HighestQualification = HighestQualification.Bachelor,
            School = "University of Africa",
            FieldOfStudy = "Computer Science",
            NQFLevel = NQFLevel.Level7,
            Year = new DateOnly(2020, 1, 1),
            ProofOfQualification = "qualification",
            DocumentName = "DocName"
        };
    }
}
