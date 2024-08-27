using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;

public class IsNotLoggedOutAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var username = context.HttpContext.User.Identity.Name;

        if (!string.IsNullOrEmpty(username))
        {
            var authService = context.HttpContext.RequestServices.GetRequiredService<AuthService>();
            if (authService.IsUserLoggedOut(username))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        base.OnActionExecuting(context);
    }
}
