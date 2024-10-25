
using SharpAgent.Domain.Exceptions;

namespace SharpAgent.API.Middlewares
{
    // IMiddleware allows our class to be recognized and used in our applications pipeline
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFound)  // Our custom Exception in the Domain project
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFound.Message);

                logger.LogWarning(notFound.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong!");
            }
        }
    }
}
