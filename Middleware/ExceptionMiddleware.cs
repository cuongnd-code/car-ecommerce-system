using System.Text.Json;

namespace ManageCars.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		public ExceptionMiddleware(RequestDelegate requestDelegate,ILogger<ExceptionMiddleware> logger) {
			_logger=logger;
			_next = requestDelegate;
		}


		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong: {ex}");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private  Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
		{
			httpContext.Response.ContentType = "application/json";

			int statusCode = GetStatusCode(ex);
			var result = JsonSerializer.Serialize(new
			{
				StatusCode = statusCode,
				Message = GetErrorMessage(ex),
				Detail = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment()
			? ex.Message
			: "No detail available"
			});

			return httpContext.Response.WriteAsync(result);
		}

		private static int GetStatusCode(Exception ex) => ex switch
		{
			ArgumentNullException => StatusCodes.Status400BadRequest,
			UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
			KeyNotFoundException => StatusCodes.Status404NotFound,
			InvalidCastException => StatusCodes.Status409Conflict,
			_ => StatusCodes.Status500InternalServerError,
		};
		private static string GetErrorMessage(Exception ex) => ex switch
		{
			ArgumentNullException => "A required argument was null.",
			UnauthorizedAccessException => "You are not authorized to perform this action.",
			KeyNotFoundException => "The specified key was not found.",
			InvalidCastException => "An error occurred while casting an object to a different type.",
			_ => "An unexpected error occurred.",
		};
	}
}
