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
        public async Task<ActionResult<IEnumerable<object>>> GetDossiersWithExists()
        {
            if (_context.Dossiers == null)
            {
                return NotFound();
            }

            var dossiers = await _context.Dossiers.ToListAsync();

            var result = dossiers.Select(dossier => new
            {
                // Include existing Dossier properties
                dossier.DossierId,
                dossier.PatientId,
                dossier.DateCreation,
                
                // Add a new boolean field 'Exists' to the result
                Exists = _context.Users.Any(u => u.Id == dossier.PatientId)
            });

            return Ok(result);
        }


        // GET: api/Dossiers/5
        [HttpGet("{id}")]
        public ActionResult<Dossier> GetDossier(int id)
        {
            if (_context.Dossiers == null || id == 0)
            {
                return NotFound();
            }

            var data = _context.Dossiers
                .Where(ds => ds.PatientId == id)
                .Select(ds => new
                {
                    Message = "Les patients et leurs Dossiers Médicaux",
                    DossierInformation = new
                    {
                        ds.DossierId,
                        ds.PatientId,
                        ds.DateCreation,
                        ds.PatDescription
                    },
                    PatientInfo = _context.Users
                        .Where(u => u.Id == ds.PatientId)
                        .Select(u => new
                        {
                            u.Prenom,
                            u.Nom,
                            u.DateNaissance
                        })
                        .FirstOrDefault(),
                    Consultations = _context.DossierDetails
                        .Where(c => c.DossierId == ds.DossierId)
                        .Select(c => new
                        {
                            c.Id,
                            c.DossierId,
                            c.Description,
                            c.Total
                            // Ajoutez d'autres propriétés de DossierDetails au besoin
                        })
                        .ToList()
                })
                .ToList();
            if(data.Count > 0)
            {
                return Ok(data);
            }
            return NotFound();
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
