using Elmah.Io.AspNetCore;
using System.Net;

namespace DevIO.API.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                HandleException(httpContext, ex);
            }
        }

        private static void HandleException(HttpContext context, Exception exception)
        {
            exception.Ship(context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //return Task.CompletedTask;
        }
    }
}
