using gambling.Application.Bet.Commands.Bet;
using gambling.Application.Bet.Models;
using gambling.Application.Bet.Queries.GetUserBetHistory;
using gambling.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class BetController : ApiBaseController
{
    [HttpPost(nameof(Bet))]
    public async Task<ActionResult> Bet(BetInputDto input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await Mediator.Send(new BetCommand { UserId = CurrentUserService.UserId, Bet = input });

        return Ok(result);
       
    }


    [HttpGet(nameof(GetBetHistory))]
    public async Task<ActionResult> GetBetHistory()
    {

        var result = await Mediator.Send(new GetUserBetHistoryQuery { UserId = CurrentUserService.UserId });

        return Ok(result);

    }
}
