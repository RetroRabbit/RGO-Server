﻿using HRIS.Models.Enums;

namespace HRIS.Models;

public class SimpleEmployeeDocumentDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string FileName { get; set; }
    public FileCategory FileCategory { get; set; }
    public string Blob { get; set; }
    public DateTime UploadDate { get; set; }
}