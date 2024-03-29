﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCabinet.DataDB;
using Azure;
using System.Xml.Linq;

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
        public IActionResult GetRendezVous()
        {
          if (_context.RendezVous == null)
          {
              return NotFound();
          }
           var rendezvous = (from rdvs  in _context.RendezVous join users in _context.Users 
                             on rdvs.Patientemail equals users.Email 
                             select new
                             {
                                 Id=rdvs.Id,
                                 patientName=users.Prenom + " " +users.Nom,
                                 Daterendezvous=rdvs.Daterendezvous,
                                 Plage=rdvs.Plage,
                                 Etat= rdvs.Etat,
                                 Patientemail = rdvs.Patientemail,
                                 Description= rdvs.Description,
                                 DocteurId = rdvs.DctrId
                             }
                             ).ToList();
            return Ok(rendezvous);
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

        [HttpPut("validate/{email}")]
        public IActionResult validate(string email)
        {
            if (_context.RendezVous == null)
            {
                return NotFound();
            }
            var rdv = _context.RendezVous.Where((rdv) => rdv.Patientemail == email).First();
            rdv.Etat = 1;
            _context.SaveChanges();
            return Ok(rdv);
           
        }
        [HttpGet("/Heures/{date}/{dctrId}")]
        public IActionResult HeuresReserver(string date,int dctrId)
        {
            if (date == null || !DateTime.TryParse(date, out _))
            {
                return BadRequest("Invalid date format");
            }

            var parsedDate = DateTime.Parse(date).Date;  // Extracting the date part

            var data = _context.RendezVous
                .Where(element => element.Daterendezvous == parsedDate && element.DctrId == dctrId)
                .Select(element => $"{element.Plage:hh\\:mm tt}")
                .ToArray();

            return Ok(data);
        }
        [HttpGet("/RendezVousParDocteur/{Id}")]
        public IActionResult RendezVousDocteur(int Id)
        {
            if(Id==0)
            {
                return BadRequest("Id required");
            }
            var data = (from rdvs in _context.RendezVous
                        join users in _context.Users
                              on rdvs.Patientemail equals users.Email
                        where rdvs.DctrId == Id
                        select new
                        {
                            plage = rdvs.Plage,
                            daterendezvous = rdvs.Daterendezvous,
                            PatientName = users.Prenom + " " + users.Nom
                        }).ToArray();
            return Ok(data);
        }
        [HttpGet("/RendezVousPatient/{email}")]
        public IActionResult RendezVousPatient(string email)
        {
            if (email == null)
            {
                return BadRequest("Id required");
            }
            var data = (from rdvs in _context.RendezVous
                        join users in _context.Users
                              on rdvs.Patientemail equals users.Email
                        where rdvs.Patientemail == email
                        select new
                        {
                            plage = rdvs.Plage,
                            daterendezvous = rdvs.Daterendezvous,
                            PatientName = users.Prenom + " " + users.Nom
                        }).ToArray();
            return Ok(data);
        }
        private bool RendezVouExists(int id)
        {
            return (_context.RendezVous?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
