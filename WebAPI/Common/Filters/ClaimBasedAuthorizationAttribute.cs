using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Common.Filters
{
    public class ClaimBasedAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly string _operationName;

        public ClaimBasedAuthorizationAttribute(string operationName)
        {
            _operationName = operationName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.Controller.GetType().Name.Replace("Controller", "");
            var requiredClaim = $"{controllerName}:{_operationName}";

            var user = context.HttpContext.User;
            if (!user.Claims.Any(c => c.Type == "Permission" && c.Value == requiredClaim))
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
