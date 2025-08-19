using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Entities;
using ProvaPub.Infrastructure.Data;

namespace ProvaPub.Application.Services
{
    /// <summary>
    /// Serviço para gerar números aleatórios únicos e salvar no banco.
    /// </summary>
    public class RandomService
    {
        private readonly TestDbContext _ctx;

        public RandomService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Gera um número aleatório entre 0 e 99, garante que não exista no banco,
        /// salva no banco e retorna para o cliente.
        /// </summary>
        public async Task<int> GetRandom()
        {
            int number;

            // Continua gerando até achar um número único
            do
            {
                number = Random.Shared.Next(100);
            }
            while (await _ctx.Numbers.AnyAsync(n => n.Number == number));

            await _ctx.Numbers.AddAsync(new RandomNumber { Number = number });
            await _ctx.SaveChangesAsync();

            return number;
        }
    }
}
