using ProvaPub.Domain.Interfaces;
using ProvaPub.Domain.Utils;

namespace ProvaPub.Utils
{    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
