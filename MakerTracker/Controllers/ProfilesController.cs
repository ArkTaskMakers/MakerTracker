using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MakerTracker.DBModels;
using Microsoft.AspNetCore.Authorization;

namespace MakerTracker.Controllers
{
    [Authorize()]
    public class ProfilesController : BaseController
    {
        private readonly MakerTrackerContext _context;

        public ProfilesController(MakerTrackerContext context)
        {
            _context = context;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            var maker = GetMakerProfile(User.Identity.Name);
            var profile = await _context.Profiles.FindAsync(maker.OwnerProfile.Id);

            return View(profile);
        }


        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, [Bind("Id,FirstName,LastName,Bio,Phone,Email,Address,City,State,IsSelfQuarantined,ZipCode")] Profile model)
        {
            var maker = GetMakerProfile(User.Identity.Name);
            var existingProfile = await _context.Profiles.FindAsync(maker.OwnerProfile.Id);

            if (ModelState.IsValid)
            {
                existingProfile.Address = model.Address;
                existingProfile.Bio = model.Bio;
                existingProfile.City = model.City;
                existingProfile.State = model.State;
                existingProfile.Email = model.Email;
                existingProfile.FirstName = model.FirstName;
                existingProfile.LastName = model.LastName;
                existingProfile.IsSelfQuarantined = model.IsSelfQuarantined;
                existingProfile.ZipCode = model.ZipCode;

                _context.Update(existingProfile);
                await _context.SaveChangesAsync();
            }

            return View(existingProfile);
        }

        private Maker GetMakerProfile(string auth0Id)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.Auth0Id == auth0Id);

            if (profile == null)
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var firstName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var lastName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

                profile = new Profile()
                {
                    Email = email,
                    CreatedDate = DateTime.Now,
                    FirstName = firstName,
                    LastName = lastName,
                    Auth0Id = auth0Id
                };
                _context.Profiles.Add(profile);
                _context.SaveChanges();
            }

            var maker = _context.Makers.FirstOrDefault(m => m.OwnerProfile == profile);
            if (maker == null)
            {
                maker = new Maker()
                {
                    OwnerProfile = profile,

                };
                _context.Makers.Add(maker);
                _context.SaveChanges();
            }

            return maker;

        }

    }
}
