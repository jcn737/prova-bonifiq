using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Interfaces;
using ProvaPub.Domain.Utils;

namespace ProvaPub.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize = 10);
    }
}
