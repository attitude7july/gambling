using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using gambling.Application.Account.Commands.Login;
using gambling.Application.Account.Commands.Register;
using gambling.Application.Account.Models;
using gambling.Application.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
public class AccountController : ApiBaseController
{

    [HttpPost(nameof(Register))]
    public async Task<ActionResult> Register(RegisterCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(command));
    }

    [HttpPost(nameof(Login))]
    public async Task<ActionResult> Login(LoginCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var user = await Mediator.Send(command);

        if (user == null)
            return NotFound("user not found!");

        return Ok(GetAccessToken(user));
    }


    private List<Claim> GetUserClaims(ApplicationUserDto user)
    {

        return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Balance", user.Balance.ToString()),
            };
    }

    private object GetAccessToken(ApplicationUserDto user)
    {
        //Token
        var token = JwtHelper.GetJwtToken(
            user.UserId.ToString(),
            JwtAppSettings.Secret,
            JwtAppSettings.ValidIssuer,
             JwtAppSettings.ValidAudience,
             JwtAppSettings.SessionMinutes,
             GetUserClaims(user).ToArray()
            );
        var tokenHandler = new JwtSecurityTokenHandler();
        return new
        {
            access_token = tokenHandler.WriteToken(token),
        };
    }


}
