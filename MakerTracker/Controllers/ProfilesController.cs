using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using MakerTracker.Models.Profiles;
using Microsoft.AspNetCore.Authorization;
using Profile = MakerTracker.DBModels.Profile;

namespace MakerTracker.Controllers
{
    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ApiBaseController
    {
        private readonly IMapper _mapper;

        public ProfilesController(MakerTrackerContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        // GET: api/Profiles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> GetProfiles()
        {
            return await _context.Profiles.ProjectTo<ProfileDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // GET: api/Profiles/5
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

            _context.Entry(updatedProfile).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(true);
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
