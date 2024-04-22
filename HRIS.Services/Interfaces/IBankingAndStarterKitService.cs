using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Services.Interfaces;

public interface IBankingAndStarterKitService
{
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitAsync();
}
