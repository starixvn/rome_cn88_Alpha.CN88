namespace CN88.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using CN88.Common.Models;
    using CN88.Web.Helpers;
    using CN88.Web.Validators;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AntiXsrfFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var antiXSRFResult = AntiForgeryValidator.ValidateRequest();
            if (antiXSRFResult.ErrCode != 0)
            {
                ActionDenied(filterContext, antiXSRFResult);
            }

            base.OnActionExecuting(filterContext);
        }

        private static void ActionDenied(ActionExecutingContext filterContext, Result antiXSRFResult)
        {
            var request = filterContext.HttpContext.Request;
            if (request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new Result
                    {
                        ErrCode = antiXSRFResult.ErrCode,
                        ErrMsg = antiXSRFResult.ErrMsg
                    }
                };
            }
            else
            {
                filterContext.Result = new RedirectResult(MicroSites.MainUrl(FormsAuthentication.LoginUrl));
            }
        }
    }
}
