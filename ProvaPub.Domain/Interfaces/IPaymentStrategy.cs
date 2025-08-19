namespace ProvaPub.Domain.Interfaces
{
    public interface IPaymentStrategy
    {
        string Name { get; }
        Task ProcessPayment(decimal amount, int customerId);
    }
}
