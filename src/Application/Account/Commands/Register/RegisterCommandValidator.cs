using FluentValidation;
using gambling.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gambling.Application.Account.Commands.Register;
public class LoginCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IApplicationDbContext _context;

    public LoginCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("username is required.").EmailAddress()
            .MustAsync(BeUniqueUserName).WithMessage("This username already exists.");

        RuleFor(v => v.Password)
           .NotEmpty().WithMessage("password is required.");

        RuleFor(v => v.ConfirmPassword)
          .NotEmpty().WithMessage("confirm password is required.").Equal(customer => customer.Password).WithMessage("confirm password doesnt match password");

    }

    public async Task<bool> BeUniqueUserName(string username, CancellationToken cancellationToken)
    {
        return await _context.ApplicationUsers
            .AllAsync(l => l.UserName != username, cancellationToken);
    }
}
