using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCabinet.DataDB;
using System.Diagnostics;

namespace BackendCabinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CabinetContext _context;

        public UsersController(CabinetContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'CabinetContext.Users' is null.");
            }

            // Check if the email already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("User with the provided email already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] User model)
        {
            try
            {
                // Check if the Users DbSet is null
                if (_context.Users == null)
                {
                    return Problem("Entity set 'CabinetContext.Users' is null.");
                }

                Debug.WriteLine($"Data: {model.Email} {model.Password}");

                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Assuming User has properties like Email and Password
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (existingUser == null)
                {
                    // User not found or credentials are incorrect
                    return BadRequest("Invalid email or password");
                }

                // TODO: Implement your authentication logic here
                // For simplicity, returning only necessary user details
                var userResponseModel = new
                {
                    Id = existingUser.Id,
                    Prenom = existingUser.Prenom,
                    Nom = existingUser.Nom,
                    Rôle = existingUser.Rôle,
                    Adresse = existingUser.Adresse,
                    DateNaissance = existingUser.DateNaissance,
                };

                // For security reasons, you should not return the password in the response
                // Also, consider using a token-based authentication mechanism for better security

                // Return the authenticated user details
                return Ok(userResponseModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                // It's a good practice to log the exception details for troubleshooting
                // You can use a logging framework or log to a file, database, etc.
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("validate/{email}")]
        public async Task<ActionResult<string>> ValidatePatient(string email)
        {
            try
            {
                // Find the user based on the email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound($"User with email {email} not found");
                }

                // Update the user's state to 1
                user.Etat = 1;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok($"User with email {email} successfully validated");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

