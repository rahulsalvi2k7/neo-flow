using neo.flow.core.Interfaces;

namespace neo.flow.app
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}

