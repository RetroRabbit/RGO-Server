using ATS.Models;
using ATS.Models.Enums;
using HRIS.Models.Enums;

namespace RR.Tests.Data.Models.ATS;

public class CandidateDtoTestData
{
    public static CandidateDto CandidateDto = new CandidateDto
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

    public static CandidateDto CandidateDtoTwo = new CandidateDto
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
