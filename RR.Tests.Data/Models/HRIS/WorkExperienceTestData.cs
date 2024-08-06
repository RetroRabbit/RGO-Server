using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS
{
    public class WorkExperienceTestData
    {
        public static WorkExperience WorkExperienceOne = new()
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2024, 1, 1)
        };

        public static WorkExperience WorkExperienceTwo = new()
        {
            Id = 2,
            ClientName = "ABSA",
            ProjectName = "Project2",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 2,
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2024, 1, 1)
        };
    }
}
