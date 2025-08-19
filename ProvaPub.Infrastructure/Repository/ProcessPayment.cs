using ProvaPub.Domain.Interfaces;

namespace ProvaPub.Infrastructure.Repository
{
    public class PixPayment : IPaymentStrategy
    {
        public string Name => "pix";

        public Task ProcessPayment(decimal amount, int customerId)
        {
            // lógica de pagamento via Pix
            return Task.CompletedTask;
        }
    }
}
