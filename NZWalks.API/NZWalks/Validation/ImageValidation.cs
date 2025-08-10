using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NZWalk.DataAccess.Model.Domin;
using NZWalk.DataAccess.Model.DTOs; // ✅ correct namespace for ImageFile

namespace NZWalks.Validation
{
    public class ImageValidation : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
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
