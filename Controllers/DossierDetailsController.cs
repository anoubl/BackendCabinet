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
    public class DossierDetailsController : ControllerBase
    {
        private readonly CabinetContext _context;

        public DossierDetailsController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/DossierDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DossierDetail>>> GetDossierDetails()
        {
          if (_context.DossierDetails == null)
          {
              return NotFound();
          }
            return await _context.DossierDetails.ToListAsync();
        }

        // GET: api/DossierDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DossierDetail>> GetDossierDetail(int id)
        {
          if (_context.DossierDetails == null)
          {
              return NotFound();
          }
            var dossierDetail = await _context.DossierDetails.FindAsync(id);

            if (dossierDetail == null)
            {
                return NotFound();
            }

            return dossierDetail;
        }

        // PUT: api/DossierDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDossierDetail(int id, DossierDetail dossierDetail)
        {
            if (id != dossierDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(dossierDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DossierDetailExists(id))
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

        // POST: api/DossierDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DossierDetail>> PostDossierDetail(DossierDetail dossierDetail)
        {
          if (_context.DossierDetails == null)
          {
              return Problem("Entity set 'CabinetContext.DossierDetails'  is null.");
          }
            _context.DossierDetails.Add(dossierDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDossierDetail", new { id = dossierDetail.Id }, dossierDetail);
        }

        // DELETE: api/DossierDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDossierDetail(int id)
        {
            if (_context.DossierDetails == null)
            {
                return NotFound();
            }
            var dossierDetail = await _context.DossierDetails.FindAsync(id);
            if (dossierDetail == null)
            {
                return NotFound();
            }

            _context.DossierDetails.Remove(dossierDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DossierDetailExists(int id)
        {
            return (_context.DossierDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
