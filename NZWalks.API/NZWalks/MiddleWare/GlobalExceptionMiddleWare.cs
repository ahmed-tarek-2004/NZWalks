using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NZWalks.MiddleWare
{
    public class GlobalExceptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleWare> logger;
        public GlobalExceptionMiddleWare(RequestDelegate next, ILogger<GlobalExceptionMiddleWare> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                logger.LogError($"ErrorId{errorId} : {ex.Message} ");
                logger.LogError($"{context.Request.Path}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    errorId = errorId,
                    message = "Some Thing Went Wrong plz Check Your Logger",
                };
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
