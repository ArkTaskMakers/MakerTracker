namespace MakerTracker.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Geocoding.Google;
    using MakerTracker.DBModels;
    using MakerTracker.Models.Profiles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProfilesController(MakerTrackerContext context, IConfiguration Configuration, IMapper mapper) : base(context)
        {
            _configuration = Configuration;
            _mapper = mapper;
        }

        // GET: api/Profiles
        [HttpGet()]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var profile = _mapper.Map<ProfileDto>(await GetLoggedInProfile());

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // PUT: api/Profiles/5
        [HttpPut()]
        public async Task<IActionResult> PutProfile(UpdateProfileDto model)
        {
            var profile = await GetLoggedInProfile();

            var googleAddress = await this.GeoCodeLocation(updatedProfile);
            updatedProfile.Latitude = googleAddress?.Coordinates.Latitude ?? 0;
            updatedProfile.Longitude = googleAddress?.Coordinates.Longitude ?? 0;

            if (updatedProfile.Id > 0)
            {
                _context.Entry(updatedProfile).State = EntityState.Modified;
            }
            else if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                updatedProfile.Auth0Id = User.Identity.Name;
                _context.Profiles.Add(updatedProfile);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

            await _context.SaveChangesAsync();

            return Ok(true);
        }

        private async Task<GoogleAddress> GeoCodeLocation(DBModels.Profile p)
        {
            if (string.IsNullOrWhiteSpace(_configuration["GoogleAPIKey"]))
            {
                return null;
            }
            var geocoder = new GoogleGeocoder(_configuration["GoogleAPIKey"]);
            var addresses = await geocoder.GeocodeAsync(string.Join(' ', p.Address, p.Address2, p.City, p.State, p.ZipCode));
            return addresses.First();
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DBModels.Profile>> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return profile;
        }
    }
}
