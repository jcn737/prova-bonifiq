using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.DTOs;
using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Domain.Utils;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class CustomerService
    {
        private readonly TestDbContext _ctx;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int PageSize = 10;

        public CustomerService(TestDbContext ctx, IDateTimeProvider dateTimeProvider)
        {
            _ctx = ctx;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Retorna clientes de forma paginada (modo legacy).
        /// </summary>
        public CustomerList ListCustomers(int page)
        {
            if (page < 1) page = 1;

            var totalCount = _ctx.Customers.Count();
            var customers = _ctx.Customers
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize)
                                .ToList();

            return new CustomerList
            {
                TotalCount = totalCount,
                HasNext = page * PageSize < totalCount,
                Customers = customers
            };
        }

        /// <summary>
        /// Retorna clientes paginados usando PagedResult.
        /// </summary>
        public async Task<PagedResult<Customer>> GetPagedAsync(int page)
        {
            if (page < 1) page = 1;

            var totalCount = await _ctx.Customers.CountAsync();
            var customers = await _ctx.Customers
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize)
                                    .ToListAsync();

            return new PagedResult<Customer>
            {
                Page = page,
                PageSize = PageSize,
                TotalCount = totalCount,
                Items = customers
            };
        }


        /// <summary>
        /// Regras de negócio para validar se um cliente pode realizar uma compra.
        /// </summary>
        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));
            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            var customer = await _ctx.Customers.FindAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer Id {customerId} does not exist");

            // Cliente só pode comprar 1x por mês
            var baseDate = _dateTimeProvider.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders
                .CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;

            // Se nunca comprou antes, 1ª compra máxima de 100
            var haveBoughtBefore = await _ctx.Orders
                .AnyAsync(s => s.CustomerId == customerId);
            if (!haveBoughtBefore && purchaseValue > 100)
                return false;

            // Horário comercial (8h às 18h, dias úteis)
            var now = _dateTimeProvider.UtcNow;
            if (now.Hour < 8 || now.Hour > 18 ||
                now.DayOfWeek == DayOfWeek.Saturday ||
                now.DayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }
    }
}
