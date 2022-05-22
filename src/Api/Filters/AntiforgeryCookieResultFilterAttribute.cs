using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace gambling.Api.Filters;

public class AntiforgeryCookieResultFilterAttribute : ResultFilterAttribute
{
    private readonly IAntiforgery antiforgery;
    public AntiforgeryCookieResultFilterAttribute(IAntiforgery antiforgery)
    {
        this.antiforgery = antiforgery;
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ViewResult)
        {
            var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);
            context.HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
            {
                HttpOnly = true,
            });
        }
    }
}
