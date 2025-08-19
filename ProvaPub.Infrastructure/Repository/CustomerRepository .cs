using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Domain.Utils;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TestDbContext _context;

        public CustomerRepository(TestDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize = 10)
        {
            var query = _context.Customers.AsQueryable();

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Customer>
            {
                TotalCount = total,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }
    }
}
