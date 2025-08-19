using ProvaPub.Domain.Interfaces;

namespace ProvaPub.Infrastructure.Repository
{
    public class CreditCardPayment : IPaymentStrategy
    {
        public string Name => "creditcard";

        public Task ProcessPayment(decimal amount, int customerId)
        {
            // lógica de pagamento via Cartão
            return Task.CompletedTask;
        }
    }
}
