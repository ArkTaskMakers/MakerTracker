using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakerTracker.DBModels;
using MakerTracker.Models.AdminReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MakerTracker.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminReportsController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AdminReportsController(MakerTrackerContext context, IConfiguration configuration, IMapper mapper) :
            base(context)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("suppliers")]
        public async Task<ActionResult<List<SupplierReportDto>>> GetSuppliers()
        {
            var allSuppliers = await _context.Profiles.Where(x=>x.IsSupplier).ProjectTo<SupplierReportDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return allSuppliers;
        }

        [HttpGet("requestors")]
        public async Task<ActionResult<List<RequestorReportDto>>> GetRequestors()
        {
            var allRequestors = await _context.Profiles.Where(x=>x.IsRequestor).ProjectTo<RequestorReportDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return allRequestors;
        }
    }
}