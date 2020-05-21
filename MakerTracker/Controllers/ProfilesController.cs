using System;
using System.Collections.Generic;
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
using NetTopologySuite.Geometries;
using Profile = MakerTracker.DBModels.Profile;

namespace MakerTracker.Controllers
{
    [Authorize]
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
        [HttpGet]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var profile = _mapper.Map<ProfileDto>(await GetLoggedInProfile());

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // GET: api/Profiles
        [HttpGet("ProfilesNearMe")]
        public async Task<ActionResult<List<ProfilesNearMeDto>>> GetProfilesNearMe(double radiusInMiles = 10)
        {
            var profile = await GetLoggedInProfile();

            const double metersToMiles = 1610;
            double radiusMeters = metersToMiles * radiusInMiles;
            var profilesWithInRadius = await _context.Profiles
                .Where(x => x.Location.Distance(profile.Location) <= radiusMeters)
                .Select(p => new
                {
                    p.Id,
                    p.CompanyName,
                    p.FirstName,
                    p.LastName,
                    p.City,
                    p.State,
                    p.IsRequestor,
                    p.IsSupplier,
                    p.Location,
                    //Note this distance calculation is done in sql since it knows the projections, once in memory we lose that.
                    DistanceInMeters = p.Location.Distance(profile.Location)
                })
                .OrderBy(p => p.Location.Distance(profile.Location))
                .Take(20).ToListAsync();

            var result = profilesWithInRadius.Select(p =>
                new ProfilesNearMeDto
                {
                    Id = p.Id,
                    CompanyName = p.CompanyName,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    City = p.City,
                    State = p.State,
                    IsRequestor = p.IsRequestor,
                    IsSupplier = p.IsSupplier,
                    DistanceInMiles = Math.Round(p.DistanceInMeters / metersToMiles, 2)
                }).ToList();

            return result;
        }

        // PUT: api/Profiles/5
        [HttpPut]
        public async Task<IActionResult> PutProfile(UpdateProfileDto model)
        {
            var profile = await GetLoggedInProfile();

            var updatedProfile = _mapper.Map(model, profile);
            var googleAddress = await GeoCodeLocation(updatedProfile);
            updatedProfile.Latitude = googleAddress?.Coordinates.Latitude ?? 0;
            updatedProfile.Longitude = googleAddress?.Coordinates.Longitude ?? 0;
            updatedProfile.Location = new Point(googleAddress?.Coordinates.Longitude ?? 0,
                googleAddress?.Coordinates.Latitude ?? 0)
            { SRID = 4326 };
            if (updatedProfile.Id > 0)
            {
                _context.Entry(updatedProfile).State = EntityState.Modified;
            }
            else if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                updatedProfile.CreatedDate = DateTime.UtcNow;
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
