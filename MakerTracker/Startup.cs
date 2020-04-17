using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MakerTracker.DBModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MakerTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuth0(services);

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<MakerTrackerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private void ConfigureAuth0(IServiceCollection services)
        {
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = "https://makertracker.com/roles"
                };
            });
        }

        private static async Task EnsureProfileExists(TokenValidatedContext context)
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<MakerTrackerContext>();
            if (context.SecurityToken is JwtSecurityToken accessToken)
            {
                var auth0Id = accessToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var profile = await db.Profiles.FirstOrDefaultAsync(p => p.Auth0Id == auth0Id);

                if (profile == null)
                {
                    var email = accessToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var firstName = accessToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                    var lastName = accessToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

                    profile = new DBModels.Profile()
                    {
                        Email = email,
                        CreatedDate = DateTime.Now,
                        FirstName = firstName,
                        LastName = lastName,
                        Auth0Id = auth0Id
                    };
                    await db.Profiles.AddAsync(profile);
                    await db.SaveChangesAsync();
                }

                var maker = db.Makers.FirstOrDefault(m => m.OwnerProfile == profile);
                if (maker == null)
                {
                    maker = new Maker()
                    {
                        OwnerProfile = profile,
                    };
                    await db.Makers.AddAsync(maker);
                    await db.SaveChangesAsync();
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
