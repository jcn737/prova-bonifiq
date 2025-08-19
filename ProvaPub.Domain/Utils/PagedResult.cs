namespace ProvaPub.Domain.Utils
{
    /// <summary>
    /// Estrutura genérica para listas paginadas (clientes, produtos, etc)
    /// </summary>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages =>
            PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => Page * PageSize < TotalCount;

    }
}
