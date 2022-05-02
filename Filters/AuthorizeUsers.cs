using System;
using System.Web;
using System.Web.Mvc;

namespace SimpleTax
{
    public class AuthorizeUsers : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Account" },
                    {"Action", "LogIn" }
                });

            }

            base.OnActionExecuted(filterContext);
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session[SessionStatus.Admin.ToString()])))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}