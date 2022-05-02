using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleTax
{
    public class CustomAuthorization : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string StrId = (String.IsNullOrEmpty(Convert.ToString(httpContext.Session[SessionStatus.Admin.ToString()]))) ?
                Convert.ToString(httpContext.Session[SessionStatus.Customer.ToString()]) : Convert.ToString(httpContext.Session[SessionStatus.Admin.ToString()]);

            return (StrId.Equals(SessionStatus.Admin.ToString()));

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Index" }
                });
        }
    }
}