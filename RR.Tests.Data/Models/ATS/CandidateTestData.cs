using ATS.Models.Enums;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.ATS;

namespace RR.Tests.Data.Models.ATS;

public class CandidateTestData
{
    public static Candidate CandidateOne = new()
    {
        Id = 1,
        Name = "Jane",
        Surname = "Doe",
        PersonalEmail = "example.jane@gmail.com",
        PotentialLevel = 4,
        JobPosition = PositionType.Developer,
        LinkedIn = "",
        ProfilePicture = "",
        CellphoneNumber = "",
        Location = "",
        CV = "",
        PortfolioLink = "",
        PortfolioPdf = "",
        Gender = Gender.Female,
        Race = Race.Asian,
        IdNumber = "123456789",
        Referral = 123,
        HighestQualification = "Master's Degree",
        School = "University of California, Berkeley",
        QualificationEndDate = 2020,
    };

    public static Candidate CandidateTwo = new()
    {
        Id = 1,
        Name = "Joe",
        Surname = "Doe",
        PersonalEmail = "example.joe@gmail.com",
        PotentialLevel = 6,
        JobPosition = PositionType.Designer,
        LinkedIn = "",
        ProfilePicture = "",
        CellphoneNumber = "",
        Location = "",
        CV = "",
        PortfolioLink = "",
        PortfolioPdf = "",
        Gender = Gender.Female,
        Race = Race.Asian,
        IdNumber = "123456789",
        Referral = 123,
        HighestQualification = "Master's Degree",
        School = "University of California, Berkeley",
        QualificationEndDate = 2020,
    };
}
