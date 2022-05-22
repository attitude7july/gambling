using FluentValidation;
using gambling.Application.Common.Interfaces;

namespace gambling.Application.Bet.Commands.Bet;
public class BetCommandValidator : AbstractValidator<BetCommand>
{

    private readonly IApplicationDbContext _context;

    public BetCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("userId is required.");

        RuleFor(v => v.Bet)
           .NotNull().WithMessage("bet is required.");
        ;
        RuleFor(v => v.Bet.Number)
          .NotEmpty().WithMessage("number is required.").GreaterThan(0).LessThan(10);

        RuleFor(v => v.Bet.Points)
         .NotEmpty().WithMessage("points is required.").GreaterThan(0)
         .LessThanOrEqualTo(x => EnoughAmount(x)).WithMessage("Points exceeds your current credit");


    }

    private double EnoughAmount(BetCommand input)
    {
        var user = _context.ApplicationUsers.FindAsync(new Guid(input.UserId)).Result;
        return user.Balance;
    }
}
