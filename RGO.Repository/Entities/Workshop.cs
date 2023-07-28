using Microsoft.Extensions.Logging;
using RGO.Domain.Models;
using System.Text.RegularExpressions;

namespace RGO.Repository.Entities;

public record Workshop(
    int id,
    int eventId,
    string presenter);
