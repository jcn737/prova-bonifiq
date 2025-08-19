using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Infrastructure.Data;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Tests
{
    public class OrderServiceTests
    {
        private TestDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestDbContext(options);
        }

        private IEnumerable<IPaymentStrategy> GetPaymentStrategies()
        {
            return new List<IPaymentStrategy>
            {
                new PixPayment(),
                new CreditCardPayment(),
                new PaypalPayment()
            };
        }

        [Fact]
        public async Task Throws_When_PaymentMethod_Invalid()
        {
            var ctx = GetInMemoryDb();
            var service = new OrderService(ctx, GetPaymentStrategies());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.PayOrder("invalid", 100, 1)
            );

            Assert.Equal("Método de pagamento 'invalid' não suportado.", ex.Message);
        }

        [Fact]
        public async Task CreatesOrder_When_PaymentMethod_Valid()
        {
            var ctx = GetInMemoryDb();
            var customer = new Customer { Id = 1, Name = "John" };
            ctx.Customers.Add(customer);
            ctx.SaveChanges();

            var service = new OrderService(ctx, GetPaymentStrategies());
            var order = await service.PayOrder("pix", 100, 1);

            Assert.NotNull(order);
            Assert.Equal(100, order.Value);
            Assert.Equal(1, order.CustomerId);
            Assert.True(order.OrderDate <= DateTime.UtcNow);
        }
    }
}
