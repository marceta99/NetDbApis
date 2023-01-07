using CachingWtihRedisDb.Data;
using CachingWtihRedisDb.Models;
using CachingWtihRedisDb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CachingWtihRedisDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _context;

        public DriverController(ICacheService cacheService, AppDbContext context)
        {
            this._cacheService = cacheService;
            this._context = context;
        }


        [HttpGet("drivers")]
        public async Task<IActionResult> GetDrivers()
        {
            //check cache data
            var cacheData = _cacheService.GetData<IEnumerable<Driver>>("drivers"); //key of data is drivers
            
            //if data exists in redis cache
            if(cacheData != null && cacheData.Count() > 0)
            {
                return Ok(cacheData);
            }
            //if data doesn't exists in redis cache, we get data from postgresSql database
            cacheData = await _context.Drivers.ToListAsync();

            //set data in cache with some expiryTime
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            _cacheService.SetData<IEnumerable<Driver>>("drivers", cacheData, expiryTime);

            return Ok(cacheData);
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> Post(Driver value)
        {
            var addedObject = await _context.Drivers.AddAsync(value);

            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            _cacheService.SetData<Driver>($"driver{value.Id}", addedObject.Entity, expiryTime);

            await _context.SaveChangesAsync();

            return Ok(addedObject.Entity);
        }
        [HttpDelete("DeleteDriver")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);

            if(exists != null)
            {
                _context.Remove(exists);
                _cacheService.RemoveData($"driver{id}");
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return NotFound();
        }
    }
}
