using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using RGO.Models.Enums;

namespace RGO.Models;

public record SimpleEmployeeDocumentDto(
    int Id,
    int EmployeeId,
    string FileName,
    string File,
    DateTime UploadDate);
