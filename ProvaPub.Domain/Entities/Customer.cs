namespace ProvaPub.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Campos adicionais úteis
        public string? Email { get; set; }
        public string? Phone { get; set; }

        // Controle de criação e atualização
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relacionamento 1:N
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
