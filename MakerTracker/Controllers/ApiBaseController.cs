using System.Linq;
using MakerTracker.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace MakerTracker.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected readonly MakerTrackerContext _context;

        public ApiBaseController(MakerTrackerContext context)
        {
            _context = context;
        }
        protected Profile GetProfile()
        {
            var auth0Id = User.Identity.Name;
            if (!string.IsNullOrWhiteSpace(auth0Id))
            {
                return _context.Profiles.FirstOrDefault(p => p.Auth0Id == auth0Id);
            }

            return null;
        }
    }
}