#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyAPI.Data;
using ToyAPI.Models;

namespace ToyAPI.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationModel>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationModel>> GetLocationModel(int id)
        {
            var locationModel = await _context.Locations.FindAsync(id);

            if (locationModel == null)
            {
                return NotFound();
            }

            return locationModel;
        }

        // PUT: api/Location/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationModel(int id, LocationModel locationModel)
        {
            if (id != locationModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(locationModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Location
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocationModel>> PostLocationModel(LocationModel locationModel)
        {
            _context.Locations.Add(locationModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationModel", new { id = locationModel.Id }, locationModel);
        }

        // DELETE: api/Location/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationModel(int id)
        {
            var locationModel = await _context.Locations.FindAsync(id);
            if (locationModel == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(locationModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationModelExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
