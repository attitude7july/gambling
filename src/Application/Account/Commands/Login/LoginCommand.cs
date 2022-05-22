using AutoMapper;
using gambling.Application.Account.Models;
using gambling.Application.Common.Helpers;
using gambling.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gambling.Application.Account.Commands.Login;
public class LoginCommand : IRequest<ApplicationUserDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApplicationUserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IMapper _mapper;

    public LoginCommandHandler(ILogger<LoginCommandHandler> logger, IMapper mapper, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApplicationUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var record = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == request.UserName);

            // check if username exists
            if (record == null)
                return null;

            // check if password is correct
            if (!EncryptionHelper.VerifyPasswordHash(request.Password, record.PasswordHash, record.PasswordSalt))
                return null;

            return await Task.FromResult(_mapper.Map<ApplicationUserDto>(record));
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, nameof(LoginCommandHandler));
            throw;
        }
    }
}
