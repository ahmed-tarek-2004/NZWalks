using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs;

namespace NZWalks.Validation
{
    public class ImageValidation : ActionFilterAttribute
    {
        private readonly ILogger<ImageValidation> logger;
        //private readonly IMapper map;
        public ImageValidation(ILogger<ImageValidation> logger)
        {
          
            this.logger = logger;
            // this.httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
          
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestResult();
            }
            if (context.ActionArguments.TryGetValue("request", out var request)&&request is ImageFileDTO image)
            {
                string[] Extenstions= { ".JPG",".PNG",".JPEG"};
                var p = Path.GetExtension(image.File.FileName);
                if (!Extenstions.Contains(Path.GetExtension(image.File.FileName).ToUpper()))
                {
                context.Result = new BadRequestResult();
                }
                if (image.File.Length > 10485760)
                {
                    context.Result = new BadRequestResult();
                }
            }
            else
                context.Result = new BadRequestResult();
            
                base.OnActionExecuting(context);
        }
    }
}
