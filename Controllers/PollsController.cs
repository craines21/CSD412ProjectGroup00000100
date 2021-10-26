using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSD412ProjectGroup00000100.Data;
using CSD412ProjectGroup00000100.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Collections;

namespace CSD412ProjectGroup00000100.Controllers
{
    [Authorize]
    public class PollsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PollsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Polls
        public async Task<IActionResult> Index()
        {
            ApplicationUser theUser = await _userManager.GetUserAsync(User);
            IQueryable<Poll> applicationDbContext = _context.Polls.Include(p => p.User).Where(r => r.User == theUser);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> AllPolls()
        {
            IQueryable<Poll> applicationDbContext = _context.Polls.Include(p => p.User).Where(r => r.State == true);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Polls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ApplicationUser theUser = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls.Include(p => p.Items).FirstOrDefaultAsync(m => m.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }

            
            var model = new ViewPoll { CurrentPoll = poll, Items = poll.Items.ToArray() };

            // check if the user has any votes 
            foreach (Item item in poll.Items.ToArray())
            {
                var vote = _context.Votes.Include(p => p.Item).Where(x => x.Voter == theUser && x.Item == item);
                if (vote.Count() >= 1) 
                {
                    // a vote exists set show results to true  
                    model.ShowResult = true;
                    break;
                }
            }  
            // if there is any votes result 
            if (model.ShowResult) {
                List<object> iData = new List<object>();
                DataTable dt = new DataTable();
                dt.Columns.Add("Item Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Votes", System.Type.GetType("System.Int32"));
                dt.Columns.Add("Color", System.Type.GetType("System.String"));
                foreach (Item item in poll.Items.ToArray())
                {
                    DataRow dr = dt.NewRow();
                    dr["Item Name"] = item.Name;
                    dr["Votes"] = _context.Votes.Include(p => p.Item).Where(x => x.Item == item).Count();// test value 
                    dr["Color"] = item.ColorValue;
                    dt.Rows.Add(dr);
                }
                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                ViewBag.labels = iData.ElementAt(0);
                ViewBag.data = iData.ElementAt(1);
                ViewBag.color = iData.ElementAt(2);
            }
            return View(model);
        }

        // GET: Polls/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Polls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Poll poll)
        {
            if (ModelState.IsValid)
            {
                poll.User = await _userManager.GetUserAsync(User);
                poll.State = false;
                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", poll.UserId);
            return View(poll);
        }

        // GET: Polls/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Poll poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", poll.UserId);
            return View(poll);
        }

        // POST: Polls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PollId,Name,Description")] Poll poll)
        {
            if (id != poll.PollId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Poll pollHolder = await _context.Polls.FindAsync(id);
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    if (pollHolder.User == user && pollHolder.State == false)
                    {
                        pollHolder.Name = poll.Name;
                        pollHolder.Description = poll.Description;
                        _context.Update(pollHolder);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.PollId))
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
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", poll.UserId);
            return View(poll);
        }

        // GET: Polls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // POST: Polls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Poll poll = await _context.Polls.FindAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (poll.User == user)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Polls/State/5
        public async Task<IActionResult> State(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }
        [HttpPost, ActionName("State")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StateConfirmed(int id)
        {
            Poll poll = await _context.Polls.FindAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (poll.User == user)
            {
                _context.Polls.Find(id).State = true;
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }
        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.PollId == id);
        }
    }
}
