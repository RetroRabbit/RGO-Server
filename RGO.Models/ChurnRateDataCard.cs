﻿namespace HRIS.Models;

public class ChurnRateDataCard
{
    public double ChurnRate { get; set; }

    public double DeveloperChurnRate { get; set; }

    public double DesignerChurnRate { get; set; }

    public double ScrumMasterChurnRate { get; set; }

    public double BusinessSupportChurnRate { get; set; }

    public string Month { get; set; }

    public int Year { get; set; }
}