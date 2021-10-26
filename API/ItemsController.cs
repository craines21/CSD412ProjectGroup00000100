using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSD412ProjectGroup00000100.Data;
using CSD412ProjectGroup00000100.Models;

namespace CSD412ProjectGroup00000100.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem(int id)
        {

            IEnumerable<Item> itemList = await _context.Items.Where(x => x.PollId == id).ToListAsync();
            List<Item> items = new List<Item>();
            foreach (Item item in itemList)
            {
                items.Add(item);
            }
            

            return items;
        }


        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
