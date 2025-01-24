using Business.Wrappers;
using Common.Exceptions;

namespace Presentation
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionMiddleware(RequestDelegate requestDelegate)
        {
            next = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var response = new Response();
                switch (ex)
                {
                    case ValidationException exception:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Errors = exception.Errors;
                        break;
                    case NotFoundException exception:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        response.Errors = exception.Errors;
                        break;
                    case UnauthorizedException exception:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        response.Errors = exception.Errors;
                        break;
                    default:
                        response.Message = "Error occured";
                        break;
                }
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
