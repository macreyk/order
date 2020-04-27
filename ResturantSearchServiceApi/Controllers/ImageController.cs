using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ResturantSearchServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        public ImageController(IHostingEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetImage(int id)
        {
            var webroot = _env.WebRootPath;
            var path = Path.Combine(webroot + "/pics/" + "FoodItem-" + id + ".jpg" +
                "");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");

        }
    }
}