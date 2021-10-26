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
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace CSD412ProjectGroup00000100.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public ItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Items.Include(i => i.Poll);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ViewPollItems(int pollId)
        {
            var applicationDbContext = _context.Items.Include(i => i.Poll).Where(r => r.PollId == pollId);
            ViewData["PollId"] = pollId;
            ViewData["State"] = _context.Polls.Find(pollId).State;
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public JsonResult ShowPollChart(int pollId)
        {
            List<object> iData = new List<object>();
            IQueryable<Item> pollItems = _context.Items.Include(p => p.Votes).Where(r => r.PollId == pollId);


            DataTable dt = new DataTable();
            dt.Columns.Add("Item Name",System.Type.GetType("System.String"));
            dt.Columns.Add("Votes", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Color", System.Type.GetType("System.String"));

            foreach(Item item in pollItems) 
            {
                DataRow dr = dt.NewRow();
                dr["Item Name"] = item.Name;
                dr["Votes"] = item.Votes.Count;
                dr["Color"] = item.ColorValue;
                dt.Rows.Add(dr);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Poll)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create(int pollId)
        {
            ViewData["PollId"] = pollId;
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,PollId,Name,Description,ColorValue")] Item item)
        {
            if (ModelState.IsValid)
            {
                Poll pollHolder = await _context.Polls.FindAsync(item.PollId);
                ApplicationUser user = await _userManager.GetUserAsync(User);
                if (pollHolder.User == user && pollHolder.State == false)
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(ViewPollItems), new { pollId = item.PollId });
            }
            ViewData["PollId"] = item.PollId;
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["PollId"] = item.PollId;
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,PollId,Name,Description")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Poll pollHolder = await _context.Polls.FindAsync(item.PollId);
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    if (pollHolder.User == user && pollHolder.State == false)
                    {
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewPollItems), new { pollId = item.PollId });
            }
            ViewData["PollId"] =  item.PollId;
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Poll)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Item item = await _context.Items.FindAsync(id);
            Poll pollHolder = await _context.Polls.FindAsync(item.PollId);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (pollHolder.User == user && pollHolder.State == false)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }


    }
}
