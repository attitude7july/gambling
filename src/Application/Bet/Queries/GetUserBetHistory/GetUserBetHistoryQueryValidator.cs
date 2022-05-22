using FluentValidation;

namespace gambling.Application.Bet.Queries.GetUserBetHistory;
public class GetUserBetHistoryQueryValidator : AbstractValidator<GetUserBetHistoryQuery>
{

    public GetUserBetHistoryQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("userid is required.");

    }
}
