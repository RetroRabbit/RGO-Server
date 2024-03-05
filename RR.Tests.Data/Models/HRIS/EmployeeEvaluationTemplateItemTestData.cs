using HRIS.Models;
using HRIS.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeEvaluationTemplateItemTestData
{
    public static EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto = new EmployeeEvaluationTemplateItemDto

    {
        Id = 1,
        Template = new EmployeeEvaluationTemplateDto { Id = 101, Description = "Template 1" },
