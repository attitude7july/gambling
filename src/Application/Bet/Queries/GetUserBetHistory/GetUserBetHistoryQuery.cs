using AutoMapper;
using gambling.Application.Bet.Models;
using gambling.Application.Common.Interfaces;
using MediatR;

namespace gambling.Application.Bet.Queries.GetUserBetHistory;

public record GetUserBetHistoryQuery : IRequest<List<ApplicationUserBetDto>>
{
    public string UserId { get; set; }
}

public class GetUserAmountQueryHandler : IRequestHandler<GetUserBetHistoryQuery, List<ApplicationUserBetDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetUserAmountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<ApplicationUserBetDto>> Handle(GetUserBetHistoryQuery request, CancellationToken cancellationToken)
    {
        var betList = _context.ApplicationUserBets
                    .Where(x => x.UserId == new Guid(request.UserId)).ToList();

        return await Task.FromResult(_mapper.Map<List<ApplicationUserBetDto>>(betList));
    }
}
