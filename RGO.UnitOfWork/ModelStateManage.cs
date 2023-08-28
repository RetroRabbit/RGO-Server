using RGO.Models.Enums;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork;

public static class ModelStateManage
{
    public static EmployeeType[] EmployeeTypeSet()
    {
        return new EmployeeType[]
        {
            new EmployeeType { Id = 1, Name = "Executive"},
            new EmployeeType { Id = 2, Name = "Developer"},
            new EmployeeType { Id = 3, Name = "Designer"},
            new EmployeeType { Id = 4, Name = "Scrum Master"},
            new EmployeeType { Id = 5, Name = "Business Support"},
            new EmployeeType { Id = 6, Name = "Account Manager"},
        };
    }

    public static FieldCode[] FieldCodeSet()
    {
        return new FieldCode[]
        {
            new FieldCode { Id = 1, Name = "Degree", Code = "degree", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 2, Name = "Tenure", Code = "tenure", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 3, Name = "NQF Level", Code = "nqf", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 4, Name = "Institution", Code = "institution", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 5, Name = "Experience", Code = "experience", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 6, Name = "CV Link", Code = "cv", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 7, Name = "Client", Code = "client", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 8, Name = "Tech Stack", Code = "skills", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 9, Name = "Engagement", Code = "engagement", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 10, Name = "T-Shirt Size", Code = "tsize", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 11, Name = "Dietary Restriction", Code = "dietary", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 12, Name = "Location", Code = "location", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 13, Name = "People's Champion", Code = "champion", Status = ItemStatus.Active, Type = FieldCodeType.String },
            new FieldCode { Id = 14, Name = "Risk", Code = "risk", Status = ItemStatus.Active, Type = FieldCodeType.String },
        };
    }

    public static FieldCodeOptions[] FieldCodeOptionSet()
    {
        var id = 1;
        return new FieldCodeOptions[]
        {
            new FieldCodeOptions { Id = id++, FieldCodeId = 14, Option = "Very Low"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 14, Option = "Low"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 14, Option = "Medium"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 14, Option = "High"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 14, Option = "Very High"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 14, Option = "Unknown"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 13, Option = "Mandy"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 13, Option = "Ashleigh"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 13, Option = "Lebo"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 13, Option = "James"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 12, Option = "JHB"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 12, Option = "PTA"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 12, Option = "CP"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 12, Option = "Other"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 11, Option = "None"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 11, Option = "Halaal"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 11, Option = "Kosher"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 11, Option = "Vegeterian"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 11, Option = "Vegan"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "XS"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "S"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "M"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "L"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "XL"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "XXL"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "XXXL"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "XXXXL"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 10, Option = "Unknown"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 9, Option = "Very Low"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 9, Option = "Low"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 9, Option = "Average"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 9, Option = "High"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 9, Option = "Unknown"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Discovery"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Standard Bank"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Telesure"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Kalido"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Capitec"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "MTN"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Nedbank"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Bench"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Atlantic Ventures"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "Yuzzu"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 7, Option = "FNB"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "Grad"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "1+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "2+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "3+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "4+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "5+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "6+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "7+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "8+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "9+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "10+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "15+"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 5, Option = "20+"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "TUT"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "Belgium Campus"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "University of Limpopo"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "University of Pretoria"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "NWU - Potch"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "Pearson"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "University of Johannesburg"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "Other"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "Open Window"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "University of Cape Town"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "Rhodes"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "University of Kwa-Zulu Natal"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "UNISA"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 4, Option = "University of Witwatersrand"},

            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 4"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 5"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 6"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 7"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 8"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 9"},
            new FieldCodeOptions { Id = id++, FieldCodeId = 3, Option = "NQF 10"},
        };
    }

    public static MetaProperty[] MetaPropertySet()
    {
        var id = 1;
        return new MetaProperty[]
        {
            new MetaProperty { Id = id++, Table = "Employee" },
        };
    }
}