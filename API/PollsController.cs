using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSD412ProjectGroup00000100.Data;
using CSD412ProjectGroup00000100.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace CSD412ProjectGroup00000100.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PollsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Polls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiPoll>>> GetPolls()
        {
            IEnumerable < Poll > someList = await _context.Polls.ToListAsync();
            List<ApiPoll> apiList = new List<ApiPoll>();
            foreach(Poll poll in someList)
            {
                ApiPoll apiPoll = new ApiPoll();
                apiPoll.Name = poll.Name;
                apiPoll.Description = poll.Description;
                apiPoll.PollId = poll.PollId;
                apiPoll.Items = poll.Items;
                apiList.Add(apiPoll);
            }

            return apiList;
        }

        // GET: api/Polls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiPoll>> GetPoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);

            if (poll == null)
            {
                return NotFound();
            }
            ApiPoll getApiPool = new ApiPoll();
            getApiPool.Name = poll.Name;
            getApiPool.Description = poll.Description;
            getApiPool.PollId = poll.PollId;
            getApiPool.Items = poll.Items;
            return getApiPool;
        }

        // PUT: api/Polls/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(int id, Poll poll)
        {
            if (id != poll.PollId)
            {
                return BadRequest();
            }

            _context.Entry(poll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(id))
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

        // POST: api/Polls
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Poll>> PostPoll(Poll poll)
        {
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoll", new { id = poll.PollId }, poll);
        }

        // DELETE: api/Polls/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Poll>> DeletePoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return poll;
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.PollId == id);
        }
    }
}
