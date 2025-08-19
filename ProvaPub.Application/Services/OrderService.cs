using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class OrderService
    {
        private readonly TestDbContext _ctx;
        private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;

        public OrderService(TestDbContext ctx, IEnumerable<IPaymentStrategy> paymentStrategies)
        {
            _ctx = ctx;
            _paymentStrategies = paymentStrategies;
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            var strategy = _paymentStrategies.FirstOrDefault(s => s.Name == paymentMethod.ToLower());
            if (strategy == null)
                throw new InvalidOperationException($"Método de pagamento '{paymentMethod}' não suportado.");

            // Executa a estratégia correta
            await strategy.ProcessPayment(paymentValue, customerId);

            // Cria pedido em UTC
            var order = new Order()
            {
                Value = paymentValue,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow
            };

            return await InsertOrder(order);
        }

        public async Task<Order> InsertOrder(Order order)
        {
            var entity = (await _ctx.Orders.AddAsync(order)).Entity;
            await _ctx.SaveChangesAsync();
            return entity;
        }
    }
}
