using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Service_Contract
{
    public interface IResponseCashingService
    {
        Task CashResponseAsync(string cashKey, object Response, TimeSpan ExpiredTime);
        Task<string?> GetCashedResponseAsync(string cashKey);
    }
}
