using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nutmeg.Data;

namespace Nutmeg.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Challenges")]
    public class ChallengesController : Controller
    {
        private readonly NutmegContext _context;

        public ChallengesController(NutmegContext context)
        {
            _context = context;
        }

        // GET: api/Challenges
        [HttpGet]
        public IEnumerable<Challenge> GetChallenge()
        {
            return _context.Challenge;
        }

        // GET: api/Challenges/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChallenge([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var challenge = await _context.Challenge.SingleOrDefaultAsync(m => m.Id == id);

            if (challenge == null)
            {
                return NotFound();
            }

            return Ok(challenge);
        }

        // PUT: api/Challenges/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallenge([FromRoute] Guid id, [FromBody] Challenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != challenge.Id)
            {
                return BadRequest();
            }

            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(id))
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

        // POST: api/Challenges
        [HttpPost]
        public async Task<IActionResult> PostChallenge([FromBody] Challenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Challenge.Add(challenge);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChallengeExists(challenge.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChallenge", new { id = challenge.Id }, challenge);
        }

        // DELETE: api/Challenges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallenge([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var challenge = await _context.Challenge.SingleOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }

            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();

            return Ok(challenge);
        }

        private bool ChallengeExists(Guid id)
        {
            return _context.Challenge.Any(e => e.Id == id);
        }
    }
}