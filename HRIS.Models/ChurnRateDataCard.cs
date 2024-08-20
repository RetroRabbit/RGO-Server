using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class ChurnRateDataCardDto
{
    [Required(ErrorMessage = "Churn Rate Data Card 'ChurnRate' field is missing.")]
    public double ChurnRate { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'DeveloperChurnRate' field is missing.")]
    public double DeveloperChurnRate { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'DesignerChurnRate' field is missing.")]
    public double DesignerChurnRate { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'ScrumMasterChurnRate' field is missing.")]
    public double ScrumMasterChurnRate { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'BusinessSupportChurnRate' field is missing.")]
    public double BusinessSupportChurnRate { get; set; }
    public string? Month { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'Year' field is missing.")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'ChurnRateDifference' field is missing.")]
    public double ChurnRateDifference { get; set; }
    [Required(ErrorMessage = "Churn Rate Data Card 'IsIncrease' field is missing.")]
    public bool IsIncrease { get; set; }
}
