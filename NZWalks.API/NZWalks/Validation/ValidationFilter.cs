using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.Validation
{
    public class ValidationFilter : Attribute,IActionFilter
    {
        private readonly ILogger<ValidationFilter> logger;
        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var args in context.ActionArguments)
            {
                logger.LogInformation($"Key Passed: {args.Key},Value Passed : {args.Value}");
            }


            if(!context.ModelState.IsValid)
            {
                context.Result = new BadRequestResult();
            }
            
        }
    }
}
