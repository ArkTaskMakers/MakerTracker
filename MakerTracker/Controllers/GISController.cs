using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace MakerTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GISController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public GISController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetFile()
        {
            var firstNameAttribute = "firstname";
            var lastNameAttribute = "lastname";

            //create geometry factory
            var geomFactory = NtsGeometryServices.Instance.CreateGeometryFactory();

            //create the default table with fields - alternately use DBaseField classes
            var t1 = new AttributesTable();
            t1.Add(firstNameAttribute, "John");
            t1.Add(lastNameAttribute, "Doe");

            var t2 = new AttributesTable();
            t2.Add(firstNameAttribute, "Jane");
            t2.Add(lastNameAttribute, "Smith");

            //create geometries and features
            Geometry g1 = geomFactory.CreatePoint(new Coordinate(300000, 5000000));
            Geometry g2 = geomFactory.CreatePoint(new Coordinate(300200, 5000300));

            var feat1 = new Feature(g1, t1);
            var feat2 = new Feature(g2, t2);

            //create attribute list
            IList<Feature> features = new List<Feature> {feat1, feat2};

            var savePath = Path.Combine(_env.WebRootPath, "gis");
            var path = Path.Combine(savePath, "maker");

            var writer = new ShapefileDataWriter(path)
                {Header = ShapefileDataWriter.GetHeader(features[0], features.Count)};

            writer.Write(features);

            return Ok($"Files created at {savePath}");
        }
    }
}