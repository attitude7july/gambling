using gambling.Application.Common.Interfaces;

namespace gambling.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
