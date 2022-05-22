using gambling.Application.Common.Interfaces;
using gambling.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    private ISender _mediator = null!;
    private JwtAppSettings _jwtAppSettings = null!;
    private ICurrentUserService _currentUserService = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ICurrentUserService CurrentUserService => _currentUserService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
    protected JwtAppSettings JwtAppSettings => _jwtAppSettings ??= HttpContext.RequestServices.GetRequiredService<IOptions<JwtAppSettings>>().Value;
}
