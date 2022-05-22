using gambling.Application.Common.Interfaces;
using MediatR;

namespace gambling.Application.Account.Queries.GetUserAmount;

public record GetUserAmountQuery : IRequest<double>
{
    public string UserId { get; set; }
}

public class GetUserAmountQueryHandler : IRequestHandler<GetUserAmountQuery, double>
{
    private readonly IApplicationDbContext _context;
    public GetUserAmountQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<double> Handle(GetUserAmountQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.ApplicationUsers.FindAsync(new Guid(request.UserId),cancellationToken);
        return user.Balance;
    }
}