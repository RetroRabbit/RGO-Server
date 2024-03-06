using ATS.Models;
using ATS.Models.Enums;

namespace RR.Tests.Data.Models.ATS;

public class ApplicantDtoTestData
{
    public static ApplicantDto ApplicantDto = new ApplicantDto
    {
        Id = 1,
        Name = "Jane",
        Surname = "Doe",
        PersonalEmail = "example.jane@gmail.com",
        PotentialLevel = 4,
        JobPosition = PositionType.Developer,
        LinkedIn = "",
        ProfilePicture = ""
    };
    
    public static ApplicantDto ApplicantDtoTwo = new ApplicantDto
    {
        Id = 1,
        Name = "Joe",
        Surname = "Doe",
        PersonalEmail = "example.joe@gmail.com",
        PotentialLevel = 6,
        JobPosition = PositionType.Designer,
        LinkedIn = "",
        ProfilePicture = ""
    };
}
