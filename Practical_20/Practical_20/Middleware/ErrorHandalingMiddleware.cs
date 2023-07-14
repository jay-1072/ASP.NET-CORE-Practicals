using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using Practical_20.Middleware;
using System.Net;

namespace Practical_20.Middleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ErrorHandalingMiddleware
{
	private readonly RequestDelegate _next;
	private const string notFoundViewPath = "/Error/NotFoundEx";
    private const string ambiguousViewPath = "/Error/Ambiguous";
    private const string badRequestViewPath = "/Error/BadRequestEx";
    private const string internalServalErrorViewPath = "/Error/InternalServerError";
    private const string loopDetectedViewPath = "/Error/LoopDetected";
    private const string unAuthorizedViewPath = "/Error/UnAuthorized";
    public ErrorHandalingMiddleware(RequestDelegate next)
	{
		_next = next;
	}
	public async Task Invoke(HttpContext httpContext)
	{
		await _next(httpContext);
		var statusCode = httpContext.Response.StatusCode;
		bool IsResponseStarted = !httpContext.Response.HasStarted;
		bool IsValidRequestPath = httpContext.Request.Path.StartsWithSegments("/Error");

        if (statusCode == (int)HttpStatusCode.NotFound && IsResponseStarted && IsValidRequestPath)
		{
			httpContext.Response.Redirect(notFoundViewPath);
		}

		if (statusCode == (int)HttpStatusCode.NotFound && IsResponseStarted && !IsValidRequestPath)
		{
			httpContext.Response.Redirect(notFoundViewPath);
		}

		if (statusCode == (int)HttpStatusCode.Ambiguous && IsResponseStarted && !IsValidRequestPath)
		{
			httpContext.Response.Redirect(ambiguousViewPath);
		}

		if (statusCode == (int)HttpStatusCode.BadRequest && IsResponseStarted && !IsValidRequestPath)
		{
			httpContext.Response.Redirect(badRequestViewPath);
		}

		if (statusCode == (int)HttpStatusCode.InternalServerError && IsResponseStarted && !IsValidRequestPath)
		{
			httpContext.Response.Redirect(internalServalErrorViewPath);
		}

		if (statusCode == (int)HttpStatusCode.LoopDetected && IsResponseStarted && !IsValidRequestPath)
		{
			httpContext.Response.Redirect(loopDetectedViewPath);
		}

		if (statusCode == (int)HttpStatusCode.Unauthorized && IsResponseStarted && !IsValidRequestPath)
		{
			httpContext.Response.Redirect(unAuthorizedViewPath);
		}
	}
}

public static class ErrorHandalingMiddlewareExtensions
{
	public static IApplicationBuilder UseErrorHandalingMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ErrorHandalingMiddleware>();
	}
}