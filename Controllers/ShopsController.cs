using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TijanasBlog.Data;
using TijanasBlog.Models;
using TijanasBlog.ViewModels;

namespace TijanasBlog.Controllers
{
    public class ShopsController : Controller
    {
        private readonly TijanasBlogContext _context;
        private readonly IWebHostEnvironment _environment;

        public ShopsController(TijanasBlogContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Shops
        public async Task<IActionResult> Index()
        {
            var shops = await _context.Shops
                .Include(s => s.Items)
                .ThenInclude(i => i.Item)
                .ToListAsync();
            return View(await _context.Shops.ToListAsync());
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shops = await _context.Shops
                .Include(s => s.Items)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shops == null)
            {
                return NotFound();
            }

            return View(shops);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            var viewModel = new ShopsCreateEditViewModel
            {
                Shops = new Shops()
            };

            return View(viewModel);
        }

        // POST: Shops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShopsCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Image != null && viewModel.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.Image.CopyToAsync(fileStream);
                    }

                    // Save file path in the database
                    viewModel.Shops.Logo = "/images/" + uniqueFileName;
                    //book.FrontPage = filePath;
                }

                try
                {
                    _context.Update(viewModel.Shops);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving the entity changes.");
                    Console.WriteLine(ex.InnerException?.Message);
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shops = await _context.Shops.FindAsync(id);

            if (shops == null)
            {
                return NotFound();
            }

            ShopsCreateEditViewModel viewModel = new ShopsCreateEditViewModel
            {
                Shops = shops
            };

            return View(viewModel);
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShopsCreateEditViewModel viewModel)
        {
            if (id != viewModel.Shops.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.Image != null && viewModel.Image.Length > 0)
                    {
                        string uniqueFrontPageFileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                        string frontPageFilePath = Path.Combine(_environment.WebRootPath, "images", uniqueFrontPageFileName);

                        using (var fileStream = new FileStream(frontPageFilePath, FileMode.Create))
                        {
                            await viewModel.Image.CopyToAsync(fileStream);
                        }

                        viewModel.Shops.Logo = "/images/" + uniqueFrontPageFileName;
                    }

                    if (viewModel.Image == null)
                    {
                        var existingShop = _context.Shops.AsNoTracking().FirstOrDefault(b => b.Id == id);
                        if (existingShop != null)
                        {
                            viewModel.Shops.Logo = existingShop.Logo;
                        }
                    }
                    _context.Update(viewModel.Shops);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopsExists(viewModel.Shops.Id))
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
            return View(viewModel.Shops);
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shops = await _context.Shops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shops == null)
            {
                return NotFound();
            }

            return View(shops);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shops = await _context.Shops.FindAsync(id);
            if (shops != null)
            {
                _context.Shops.Remove(shops);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopsExists(int id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }
    }
}
