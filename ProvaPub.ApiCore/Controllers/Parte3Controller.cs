// Parte3Controller.cs
using Microsoft.AspNetCore.Mvc;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Entities;

namespace ProvaPub.Controllers
{
    /// <summary>
    /// Esse teste simula um pagamento de uma compra.
    /// O método PayOrder aceita diversas formas de pagamento. Dentro desse método é feita uma estrutura de diversos "if" para cada um deles.
    /// Sabemos, no entanto, que esse formato não é adequado, em especial para futuras inclusões de formas de pagamento.
    /// Como você reestruturaria o método PayOrder para que ele ficasse mais aderente com as boas práticas de arquitetura de sistemas?
    /// 
    /// Outra parte importante é em relação à data (OrderDate) do objeto Order. Ela deve ser salva no banco como UTC mas deve retornar para o cliente no fuso horário do Brasil. 
    /// Demonstre como você faria isso.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Parte3Controller : ControllerBase
    {
        private readonly RandomService _randomService;
        private readonly OrderService _orderService;

        public Parte3Controller(RandomService randomService, OrderService orderService)
        {
            _randomService = randomService;
            _orderService = orderService;
        }

        [HttpGet("random")]
        public async Task<int> GetRandom()
        {
            return await _randomService.GetRandom();
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            try
            {
                var order = await _orderService.PayOrder(paymentMethod, paymentValue, customerId);

                // Converte UTC → Horário de Brasília
                var brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                order.OrderDate = TimeZoneInfo.ConvertTimeFromUtc(order.OrderDate, brasilTimeZone);

                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PlaceOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            try
            {
                await _orderService.PayOrder(paymentMethod, paymentValue, customerId);
                return Ok($"Pagamento de {paymentValue:C} via {paymentMethod} processado com sucesso!");
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
