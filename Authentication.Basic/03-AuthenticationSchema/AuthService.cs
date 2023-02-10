using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace AuthenticationSchema;

public class AuthService
{
	public readonly IDataProtectionProvider dataProtector;
	public readonly IHttpContextAccessor contextAccessor;

	public AuthService(IDataProtectionProvider dataProtector, IHttpContextAccessor contextAccessor)
	{
		this.dataProtector = dataProtector;
		this.contextAccessor = contextAccessor;
	}

	public void SignIn()
	{
		var protector = dataProtector.CreateProtector("auth-cookie");
		var authCookie = contextAccessor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:tandi.sunarto")}";
	}

	public void Authenticate(HttpContext context)
	{
		var protector = dataProtector.CreateProtector("auth-cookie");
		var authCookie = context.Request.Cookies.FirstOrDefault(c => c.Key == "auth");
		var protectedData = authCookie.Value;
		var data = protector.Unprotect(protectedData);
		var claimData = data.Split(":");
		
		var claims = new List<Claim>();
		claims.Add(new Claim(claimData[0], claimData[1]));
		var identity = new ClaimsIdentity(claims);
		context.User = new ClaimsPrincipal(identity);
	}
}