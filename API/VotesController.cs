using CSD412ProjectGroup00000100.Data;
using CSD412ProjectGroup00000100.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSD412ProjectGroup00000100.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }

        // POST: api/Votes
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("", new { id = vote.VoteId }, vote);
        }
    }
}
