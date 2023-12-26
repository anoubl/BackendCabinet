using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCabinet.DataDB;

namespace BackendCabinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly CabinetContext _context;

        public RendezVousController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/RendezVous
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RendezVou>>> GetRendezVous()
        {
          if (_context.RendezVous == null)
          {
              return NotFound();
          }
            return await _context.RendezVous.ToListAsync();
        }

        // GET: api/RendezVous/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RendezVou>> GetRendezVou(int id)
        {
          if (_context.RendezVous == null)
          {
              return NotFound();
          }
            var rendezVou = await _context.RendezVous.FindAsync(id);

            if (rendezVou == null)
            {
                return NotFound();
            }

            return rendezVou;
        }

        // PUT: api/RendezVous/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRendezVou(int id, RendezVou rendezVou)
        {
            if (id != rendezVou.Id)
            {
                return BadRequest();
            }

            _context.Entry(rendezVou).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RendezVouExists(id))
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

        // POST: api/RendezVous
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RendezVou>> PostRendezVou(RendezVou rendezVou)
        {
          if (_context.RendezVous == null)
          {
              return Problem("Entity set 'CabinetContext.RendezVous'  is null.");
          }
            _context.RendezVous.Add(rendezVou);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRendezVou", new { id = rendezVou.Id }, rendezVou);
        }

        // DELETE: api/RendezVous/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRendezVou(int id)
        {
            if (_context.RendezVous == null)
            {
                return NotFound();
            }
            var rendezVou = await _context.RendezVous.FindAsync(id);
            if (rendezVou == null)
            {
                return NotFound();
            }

            _context.RendezVous.Remove(rendezVou);
            await _context.SaveChangesAsync();

            return NoContent();
        }
      
       
        private bool RendezVouExists(int id)
        {
            return (_context.RendezVous?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
