using System.Diagnostics;
using AutoMapper;
namespace NZWalks.MiddleWare
{
    public class TimeEstimate
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<TimeEstimate> logger;

        public TimeEstimate(RequestDelegate next, ILogger<TimeEstimate> logger)
        {
            Next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var time = new Stopwatch();
            time.Start();
            await Next.Invoke(context);
            time.Stop();
            logger.LogDebug($"Path From : {context.Request.Path}");
            logger.LogInformation($"Time Taken For Request : {time.ElapsedMilliseconds} ms");
        }
    }
}
