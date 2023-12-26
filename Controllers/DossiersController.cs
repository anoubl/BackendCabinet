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
    public class DossiersController : ControllerBase
    {
        private readonly CabinetContext _context;

        public DossiersController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/Dossiers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dossier>>> GetDossiers()
        {
          if (_context.Dossiers == null)
          {
              return NotFound();
          }
            return await _context.Dossiers.ToListAsync();
        }

        // GET: api/Dossiers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dossier>> GetDossier(int id)
        {
          if (_context.Dossiers == null)
          {
              return NotFound();
          }
            var dossier = await _context.Dossiers.FindAsync(id);

            if (dossier == null)
            {
                return NotFound();
            }

            return dossier;
        }

        // PUT: api/Dossiers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDossier(int id, Dossier dossier)
        {
            if (id != dossier.DossierId)
            {
                return BadRequest();
            }

            _context.Entry(dossier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DossierExists(id))
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

        // POST: api/Dossiers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dossier>> PostDossier(Dossier dossier)
        {
          if (_context.Dossiers == null)
          {
              return Problem("Entity set 'CabinetContext.Dossiers'  is null.");
          }
            _context.Dossiers.Add(dossier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDossier", new { id = dossier.DossierId }, dossier);
        }

        // DELETE: api/Dossiers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDossier(int id)
        {
            if (_context.Dossiers == null)
            {
                return NotFound();
            }
            var dossier = await _context.Dossiers.FindAsync(id);
            if (dossier == null)
            {
                return NotFound();
            }

            _context.Dossiers.Remove(dossier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DossierExists(int id)
        {
            return (_context.Dossiers?.Any(e => e.DossierId == id)).GetValueOrDefault();
        }
    }
}
