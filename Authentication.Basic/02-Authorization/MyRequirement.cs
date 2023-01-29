using Microsoft.AspNetCore.Authorization;

namespace Authorization;

public class MyRequirement : IAuthorizationRequirement
{
	public MyRequirement()
	{
	}
}

public class MyRequirementHandler : AuthorizationHandler<MyRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRequirement requirement)
	{
		return Task.CompletedTask;
	}
}