using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSD412ProjectGroup00000100.Data;
using CSD412ProjectGroup00000100.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CSD412ProjectGroup00000100.Controllers
{
    [Authorize]
    public class VotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public VotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Votes
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Votes.Include(v => v.Item).Include(v => v.Voter);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Item)
                .Include(v => v.Voter)
                .FirstOrDefaultAsync(m => m.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Votes/Create
        public IActionResult Create(int itemId)
        {
            ViewData["ItemId"] = itemId;
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoteId,ItemId")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                bool goodTOGO = true;
                
                ApplicationUser user = await _userManager.GetUserAsync(User);
                
                Item itemHolder = _context.Items.Find(vote.ItemId);

                Poll pollHolder = _context.Polls.Find(itemHolder.PollId);

                List<Item> itemList = _context.Items.Where(x=>x.PollId == itemHolder.PollId).ToList();

                foreach (Item item in itemList)
                {
                    IQueryable<Vote> otherVotes = _context.Votes.Include(p => p.Item).Where(x => x.Voter == user && x.Item == item);
                    if (otherVotes.Any())
                    { 
                        goodTOGO = false;
                        break;
                    }
                }
                if ( pollHolder.State == true && (goodTOGO))
                {
                    vote.Voter = user;
                    vote.VoteDateTime = DateTime.Now;
                    _context.Add(vote);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details","Polls", new { id = pollHolder.PollId });
                }
            }
            ViewData["ItemId"] = vote.ItemId;
            return View(vote);
        }

        // GET: Votes/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", vote.ItemId);
            ViewData["VoterId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", vote.VoterId);
            return View(vote);
        }

        // POST: Votes/Edit/5
        [Authorize(Roles = "Administrator")]
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoteId,ItemId,VoterId,VoteDateTime")] Vote vote)
        {
            if (id != vote.VoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.VoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", vote.ItemId);
            ViewData["VoterId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", vote.VoterId);
            return View(vote);
        }

        // GET: Votes/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Item)
                .Include(v => v.Voter)
                .FirstOrDefaultAsync(m => m.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.VoteId == id);
        }
    }
}
