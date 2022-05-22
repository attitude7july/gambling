using FluentValidation;
using gambling.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gambling.Application.Account.Commands.Login;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    private readonly IApplicationDbContext _context;

    public LoginCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("username is required.").EmailAddress();

        RuleFor(v => v.Password)
           .NotEmpty().WithMessage("password is required.");

    }

    public async Task<bool> BeUniqueUserName(string username, CancellationToken cancellationToken)
    {
        return await _context.ApplicationUsers
            .AllAsync(l => l.UserName != username, cancellationToken);
    }
}
