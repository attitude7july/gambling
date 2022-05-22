using gambling.Application.Common.Mappings;
using gambling.Domain.Entities;
namespace gambling.Application.Account.Models;

public class ApplicationUserDto : IMapFrom<ApplicationUser>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public double Balance { get; set; }
}
