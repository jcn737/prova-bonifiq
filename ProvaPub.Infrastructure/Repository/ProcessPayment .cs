using ProvaPub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaPub.Infrastructure.Repository
{
    public class ProcessPayment : IPaymentStrategyResolver
    {
        private readonly IDictionary<string, IPaymentStrategy> _strategies;
        private string Normalize(string input) => (input ?? "").Trim().ToLowerInvariant();

        public ProcessPayment(IEnumerable<IPaymentStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(
                s => Normalize(s.Name),
                s => s
            );
        }

        public IPaymentStrategy Resolve(string method)
        {
            var key = Normalize(method);

            if (_strategies.TryGetValue(key, out var strategy))
                return strategy;

            throw new InvalidOperationException($"Método de pagamento '{method}' não suportado.");
        }
    }
}
