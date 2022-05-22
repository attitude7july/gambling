using FluentValidation;

namespace gambling.Application.Account.Queries.GetUserAmount;
public class GetUserAmountQueryValidator : AbstractValidator<GetUserAmountQuery>
{

    public GetUserAmountQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("userid is required.");

    }
}
