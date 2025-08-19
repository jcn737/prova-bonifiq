using ProvaPub.Domain.Entities;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    public class ProductService : BaseService<Product>
    {
        public ProductService(TestDbContext ctx) : base(ctx) { }
    }
}
