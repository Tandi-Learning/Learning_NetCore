using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace AuthenticationSchema;

public static class Handlers
{
	public static Action<HttpContext> Login = async (context) =>
	{
		var claims = new List<Claim>();
		claims.Add(new Claim("usr", "scarlet.local"));
		claims.Add(new Claim("passport", "heaven"));
		var identity = new ClaimsIdentity(claims, Constants.LOCAL_AUTH_SCHEME);
		var user = new ClaimsPrincipal(identity);

		await context.SignInAsync(Constants.LOCAL_AUTH_SCHEME, user);
	};

	public static Action<HttpContext> LoginPatreon = async (context) =>	await context.ChallengeAsync(
		Constants.PATREON_AUTH,
		new AuthenticationProperties() {
			RedirectUri = "/"
		}
	);
	
	public static Func<HttpContext, IEnumerable<Claim>> Claims = (context) => {
		List<Claim> claims = new();
		foreach(var identity in context.User.Identities)
		{
			foreach(var claim in identity.Claims)
			{
				Claim c = new Claim(claim.Type, claim.Value);
				claims.Add(c);
			}
		};
		return claims;
	};

	public static Func<HttpContext, IDataProtectionProvider, string> Username = (context, dataProtector) =>
	{
		Console.WriteLine(context.User.Identity);
    return context.User.FindFirst("usr").Value;
	};

	public static Func<HttpContext, string> America = (context) => {
		// if (!context.User.Identity.IsAuthenticated)
		// {
		// 	context.Response.StatusCode = 401;
		// 	return "";
		// }

		// if (!context.User.HasClaim(claim => claim.Type == "passport" && claim.Value == "us"))
		// {
		// 	context.Response.StatusCode = 403;
		// 	return "";
		// }

		return "United State Of America";
	};

	public static Func<HttpContext, string> Canada = (context) => {
		// if (!context.User.Identity.IsAuthenticated)
		// {
		// 	context.Response.StatusCode = 401;
		// 	return "";
		// }

		// if (!context.User.HasClaim(claim => claim.Type == "passport" && claim.Value == "canada"))
		// {
		// 	context.Response.StatusCode = 403;
		// 	return "";
		// }

		return "Canada";
	};
}
