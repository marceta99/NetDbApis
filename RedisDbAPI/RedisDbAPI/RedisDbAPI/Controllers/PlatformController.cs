using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisDbAPI.Data;
using RedisDbAPI.Models;

namespace RedisDbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repo;

        public PlatformController(IPlatformRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<Platform> GetPlatformById(string id)
        {
            var platform = _repo.GetPlatformById(id); 

            if(platform != null)
            {
                return Ok(platform);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _repo.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platform.Id}, platform);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
        {
            return Ok(_repo.GetPlatforms());
        }
    }
}
