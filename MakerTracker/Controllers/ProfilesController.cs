using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Geocoding.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using MakerTracker.Models.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Profile = MakerTracker.DBModels.Profile;

namespace MakerTracker.Controllers
{
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
            var profile = _mapper.Map<ProfileDto>(this.GetLoggedInProfile());

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
            var profile = this.GetLoggedInProfile();

            var updatedProfile = _mapper.Map(model, profile);
            var googleAddress = await this.GeoCodeLocation(updatedProfile);
            updatedProfile.Latitude = googleAddress.Coordinates.Latitude;
            updatedProfile.Longitude = googleAddress.Coordinates.Longitude;

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

        private async Task<GoogleAddress> GeoCodeLocation(Profile p)
        {
            var geocoder = new GoogleGeocoder(_configuration["GoogleAPIKey"]);
            var addresses = await geocoder.GeocodeAsync(string.Join(' ', p.Address, p.Address2, p.City, p.State, p.ZipCode));
            return addresses.First();
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Profile>> DeleteProfile(int id)
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
