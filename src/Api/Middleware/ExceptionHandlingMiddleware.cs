using ApplicationLayer.Exceptions;

namespace Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ProductNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await WriteErrorResponseAsync(context, "Not Found", StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteErrorResponseAsync(context, "Internal Server Error", StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task WriteErrorResponseAsync(HttpContext context, string title, int statusCode, string errorMessage)
        {
            var traceId = context.TraceIdentifier;
            var errorResponse = new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                title,
                status = statusCode,
                traceId
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
