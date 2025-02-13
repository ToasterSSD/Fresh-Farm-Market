using Fresh_Farm_Market.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Fresh_Farm_Market.MiddleWare
{
	public class SessionTimeoutMiddleware
	{
		private readonly RequestDelegate _next;

		public SessionTimeoutMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
		{
			if (context.Session.GetString("UserId") == null && context.User.Identity.IsAuthenticated)
			{
				await context.SignOutAsync(IdentityConstants.ApplicationScheme);
				context.Response.Redirect("/Login");
			}
			else
			{
				var userId = context.Session.GetString("UserId");
				var securityStamp = context.Session.GetString("SecurityStamp");

				if (userId != null && securityStamp != null)
				{
					var user = await userManager.FindByIdAsync(userId);
					if (user != null && user.SecurityStamp != securityStamp)
					{
						await context.SignOutAsync(IdentityConstants.ApplicationScheme);
						context.Response.Redirect("/Login");
					}
				}

				await _next(context);
			}
		}
	}
}

