using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MakerTracker.Controllers
{
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var firstName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            ViewBag.ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            ViewBag.FullName = firstName + " "+ lastName; 

            base.OnActionExecuting(filterContext);
        }
    }
}