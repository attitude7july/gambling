using AutoMapper;
using gambling.Application.Bet.Models;
using gambling.Application.Common.Helpers;
using gambling.Application.Common.Interfaces;
using gambling.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gambling.Application.Bet.Commands.Bet;
public class BetCommand : IRequest<BetOutputDto>
{
    public string UserId { get; set; }
    public BetInputDto Bet { get; set; }
}

public class BetCommandHandler : IRequestHandler<BetCommand, BetOutputDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<BetCommandHandler> _logger;
    private readonly IMapper _mapper;

    public BetCommandHandler(ILogger<BetCommandHandler> logger, IMapper mapper, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<BetOutputDto> Handle(BetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.ApplicationUsers.FindAsync(new Guid(request.UserId));
            Status betStatus = Status.Loss;
            int numberSlot = ExtensionHelper.GetNumber();
            if(numberSlot == request.Bet.Number)
            {
                betStatus = Status.Win;
                request.Bet.Points = request.Bet.Points * 9;
                user.Balance = user.Balance + request.Bet.Points;
            }
            else
            {
                user.Balance = user.Balance - request.Bet.Points;

            }
            var applicationUserBet = new ApplicationUserBet
            {

                UserId = new Guid(request.UserId),
                BetAmount = request.Bet.Points,
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                NumberofChoice = request.Bet.Number,
                LotNumberSelected = numberSlot,
                Status = (Domain.Enums.Status)betStatus,
            };

            _context.ApplicationUsers.Update(user);
            _context.ApplicationUserBets.Add(applicationUserBet);
            await _context.SaveChangesAsync(cancellationToken);

            return new BetOutputDto
            {
                Account = user.Balance,
                Points = request.Bet.Points,
                Status = betStatus,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(BetCommandHandler));
            throw;
        }
    }
}
