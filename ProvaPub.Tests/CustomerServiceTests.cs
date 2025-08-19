
using Microsoft.EntityFrameworkCore;
using Moq;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Tests
{   

    public class CustomerServiceTests
    {
        private TestDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestDbContext(options);
        }

        [Fact]
        public async Task Throws_When_CustomerId_Is_Invalid()
        {
            var ctx = GetInMemoryDb();
            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            var service = new CustomerService(ctx, dateMock.Object);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => service.CanPurchase(0, 50)
            );
        }

        [Fact]
        public async Task Throws_When_Customer_Not_Found()
        {
            var ctx = GetInMemoryDb();
            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            var service = new CustomerService(ctx, dateMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CanPurchase(1, 50)
            );
        }

        [Fact]
        public async Task Returns_False_When_Customer_Already_Bought_This_Month()
        {
            var ctx = GetInMemoryDb();
            var customer = new Customer { Id = 1, Name = "John" };
            ctx.Customers.Add(customer);
            ctx.Orders.Add(new Order { CustomerId = 1, OrderDate = DateTime.UtcNow });
            ctx.SaveChanges();

            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            var service = new CustomerService(ctx, dateMock.Object);

            var result = await service.CanPurchase(1, 50);

            Assert.False(result);
        }

        [Fact]
        public async Task Returns_False_When_First_Purchase_Over_100()
        {
            var ctx = GetInMemoryDb();
            ctx.Customers.Add(new Customer { Id = 1, Name = "John" });
            ctx.SaveChanges();

            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(d => d.UtcNow).Returns(new DateTime(2023, 10, 10, 10, 0, 0)); // horário comercial

            var service = new CustomerService(ctx, dateMock.Object);

            var result = await service.CanPurchase(1, 200);

            Assert.False(result);
        }

        [Fact]
        public async Task Returns_False_When_Outside_Business_Hours()
        {
            var ctx = GetInMemoryDb();
            ctx.Customers.Add(new Customer { Id = 1, Name = "John" });
            ctx.SaveChanges();

            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(d => d.UtcNow).Returns(new DateTime(2023, 10, 10, 22, 0, 0)); // fora do horário

            var service = new CustomerService(ctx, dateMock.Object);

            var result = await service.CanPurchase(1, 50);

            Assert.False(result);
        }

        [Fact]
        public async Task Returns_True_When_All_Rules_Passed()
        {
            var ctx = GetInMemoryDb();
            ctx.Customers.Add(new Customer { Id = 1, Name = "John" });
            ctx.SaveChanges();

            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(d => d.UtcNow).Returns(new DateTime(2023, 10, 10, 10, 0, 0)); // terça-feira 10h

            var service = new CustomerService(ctx, dateMock.Object);

            var result = await service.CanPurchase(1, 50);

            Assert.True(result);
        }
    }

}
