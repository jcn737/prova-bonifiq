using Microsoft.AspNetCore.Mvc;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Entities;
using ProvaPub.Domain.Utils;

namespace ProvaPub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Parte2Controller : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        /// <summary>
        /// 1 - Corrige bug de paginação
        /// 2 - Usa Injeção de Dependência para os serviços
        /// 3 - Substitui CustomerList/ProductList por estrutura genérica
        /// 4 - Reduz repetição com serviço genérico
        /// </summary>
        public Parte2Controller(ProductService productService, CustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }

        [HttpGet("products")]
        public async Task<PagedResult<Product>> ListProducts(int page)
        {
            return await _productService.GetPagedAsync(page);
        }

        [HttpGet("customers")]
        public async Task<PagedResult<Customer>> ListCustomers(int page)
        {
            return await _customerService.GetPagedAsync(page);
        }
    }
}
