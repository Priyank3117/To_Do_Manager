using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace To_Do_Manager.Filters
{
    public class CheckSessionFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Session.GetString("UserId") == null)
            {
                context.Result = new RedirectToActionResult("Logout", "Account", null);
            }
            
            base.OnActionExecuting(context);
        }
    }
}
