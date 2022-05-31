using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace DigitNow.Domain.DocumentManagement.utils
{
    public class DevOnlyActionFilter : ActionFilterAttribute
    {
        public DevOnlyActionFilter(IHostEnvironment hostingEnv)
        {
            HostingEnv = hostingEnv;
        }

        private IHostEnvironment HostingEnv { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HostingEnv.IsDevelopment())
            {
                context.Result = new NotFoundResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}