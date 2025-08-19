using ProvaPub.Domain.Interfaces;

namespace ProvaPub.Infrastructure.Repository
{
    public class PaypalPayment : IPaymentStrategy
    {
        public string Name => "paypal";

        public Task ProcessPayment(decimal amount, int customerId)
        {
            // lógica de pagamento via Paypal
            return Task.CompletedTask;
        }
    }
}
