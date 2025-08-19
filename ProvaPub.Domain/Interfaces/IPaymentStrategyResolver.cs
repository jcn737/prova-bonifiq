using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaPub.Domain.Interfaces
{
    public interface IPaymentStrategyResolver
    {
        IPaymentStrategy Resolve(string method);
    }
}
