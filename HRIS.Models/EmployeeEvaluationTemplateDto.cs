﻿namespace HRIS.Models;

public class EmployeeEvaluationTemplateDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeEvaluationTemplateDto(int Id,
                                         string Description)
    {
        this.Id = Id;
        this.Description = Description;
    }

    public int Id { get; set; }
    public string Description { get; set; }
}