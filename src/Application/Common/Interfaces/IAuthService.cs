using gambling.Domain.Entities;

namespace gambling.Application.Common.Interfaces;

public interface IAuthService
{
    Task RegisterUser(string username, string password);
    Task<bool> IsUsernamePresent(string username);
    Task<ApplicationUser> LoginUser(string username, string password);

}
