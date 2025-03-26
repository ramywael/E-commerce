using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using E_Commerce510.Models;

namespace E_Commerce510.Middlewares
{
    public class CheckBlockedUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CheckBlockedUserMiddleware> _logger;

        public CheckBlockedUserMiddleware(RequestDelegate next, ILogger<CheckBlockedUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            try
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var user = await userManager.GetUserAsync(context.User);
                    if (user != null && user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
                    {
                        _logger.LogWarning($"Blocked user detected: {user.Email}");
                        await signInManager.SignOutAsync();
                        context.Response.Redirect("/Identity/Account/Login?blocked=true");
                        return;
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Middleware Error: {ex.Message}");
                throw;
            }
        }
    }
}
