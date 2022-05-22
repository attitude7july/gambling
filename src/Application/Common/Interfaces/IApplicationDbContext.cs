using gambling.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace gambling.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> ApplicationUsers { get; set; }
    DbSet<ApplicationUserBet> ApplicationUserBets { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}
