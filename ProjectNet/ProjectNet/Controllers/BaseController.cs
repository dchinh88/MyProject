using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectNet.Controllers
{
    public class BaseController : Controller
    {
        public string CurrentUser
        {
            get
            {
                //doc tu sesson
                return HttpContext.Session.GetString("USER_NAME");
            }
            set
            {
                //gan du lieu cho session 
                HttpContext.Session.SetString("USER_NAME", value);
            }
        }
        public bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(CurrentUser);
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
