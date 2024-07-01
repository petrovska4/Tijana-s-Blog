using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TijanasBlog.Data;
using TijanasBlog.Models;

namespace TijanasBlog.Controllers
{
    public class ItemsShopsController : Controller
    {
        private readonly TijanasBlogContext _context;

        public ItemsShopsController(TijanasBlogContext context)
        {
            _context = context;
        }

        // GET: ItemsShops
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemsShops.ToListAsync());
        }

        // GET: ItemsShops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemsShops = await _context.ItemsShops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemsShops == null)
            {
                return NotFound();
            }

            return View(itemsShops);
        }

        // GET: ItemsShops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemsShops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] ItemsShops itemsShops)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemsShops);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemsShops);
        }

        // GET: ItemsShops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemsShops = await _context.ItemsShops.FindAsync(id);
            if (itemsShops == null)
            {
                return NotFound();
            }
            return View(itemsShops);
        }

        // POST: ItemsShops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] ItemsShops itemsShops)
        {
            if (id != itemsShops.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemsShops);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsShopsExists(itemsShops.Id))
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
            return View(itemsShops);
        }

        // GET: ItemsShops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemsShops = await _context.ItemsShops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemsShops == null)
            {
                return NotFound();
            }

            return View(itemsShops);
        }

        // POST: ItemsShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemsShops = await _context.ItemsShops.FindAsync(id);
            if (itemsShops != null)
            {
                _context.ItemsShops.Remove(itemsShops);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsShopsExists(int id)
        {
            return _context.ItemsShops.Any(e => e.Id == id);
        }
    }
}
