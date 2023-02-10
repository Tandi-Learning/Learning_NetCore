using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace AuthenticationSchema;

public class VisitorAutoHandler : CookieAuthenticationHandler
{
	public VisitorAutoHandler(
		IOptionsMonitor<CookieAuthenticationOptions> options, 
		ILoggerFactory logger, 
		UrlEncoder encoder, 
		ISystemClock clock)
		: base(options, logger, encoder, clock)
	{
	}

	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var result = await base.HandleAuthenticateAsync();
		if (result.Succeeded)
		{
			return result;
		} 
		else
		{
			var claims = new List<Claim>();
			claims.Add(new Claim("usr", "scarlet.visitor"));
			claims.Add(new Claim("passport", "us"));
			var identity = new ClaimsIdentity(claims, Constants.VISITOR_AUTH_SCHEME);
			var user = new ClaimsPrincipal(identity);

			await Context.SignInAsync(Constants.VISITOR_AUTH_SCHEME, user);

			return AuthenticateResult.Success(new AuthenticationTicket(user, Constants.VISITOR_AUTH_SCHEME));
		} 

	}
}