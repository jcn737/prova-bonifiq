using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Utils;
using ProvaPub.Infrastructure.Data;
using ProvaPub.Utils;

namespace ProvaPub.Application.Services
{
    /// <summary>
    /// Serviço genérico para paginação de entidades
    /// </summary>
    public class BaseService<T> where T : class
    {
        protected readonly TestDbContext _ctx;
        private const int PageSize = 10;

        public BaseService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedResult<T>> GetPagedAsync(int page)
        {
            if (page < 1) page = 1;

            var totalCount = await _ctx.Set<T>().CountAsync();
            var items = await _ctx.Set<T>()
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Page = page,
                PageSize = PageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}

