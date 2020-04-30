namespace MakerTracker.Controllers
{
    using System.Threading.Tasks;
    using MakerTracker.DBModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected readonly MakerTrackerContext _context;

        public ApiBaseController(MakerTrackerContext context)
        {
            _context = context;
        }

        protected async Task<Profile> GetLoggedInProfile()
        {
            var auth0Id = User.Identity.Name;
            if (!string.IsNullOrWhiteSpace(auth0Id))
            {
                return await _context.Profiles.FirstOrDefaultAsync(p => p.Auth0Id == auth0Id);
            }

            return null;
        }
    }
}