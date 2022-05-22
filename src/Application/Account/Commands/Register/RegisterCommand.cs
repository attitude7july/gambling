using gambling.Application.Common.Helpers;
using gambling.Application.Common.Interfaces;
using gambling.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gambling.Application.Account.Commands.Register;
public class RegisterCommand : IRequest<bool>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(ILogger<RegisterCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {

            byte[] passwordHash, passwordSalt;
            EncryptionHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            var entity = new ApplicationUser
            {
                UserId = Guid.NewGuid(),
                Balance = 10000,
                CreatedOn = DateTime.UtcNow,
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.ApplicationUsers.Add(entity);
            var count = await _context.SaveChangesAsync(cancellationToken);

            return (count > 0);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, nameof(RegisterCommandHandler));
            throw;
        }
    }
}
