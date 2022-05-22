using gambling.Application.Common.Mappings;
using gambling.Domain.Entities;

namespace gambling.Application.Bet.Models;
public class ApplicationUserBetDto : IMapFrom<ApplicationUserBet>
{
    public double BetAmount { get; set; }

    public int NumberofChoice { get; set; }

    public int LotNumberSelected { get; set; }

    public DateTime CreatedOn { get; set; }

    public Status Status { get; set; }
}

