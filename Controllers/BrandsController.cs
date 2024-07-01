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
    public class BrandsController : Controller
    {
        private readonly TijanasBlogContext _context;
        private readonly IWebHostEnvironment _environment;

        public BrandsController(TijanasBlogContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Brands
        public async Task<IActionResult> Index(string name)
        {
            var brandsQuery = _context.Brands.Include(a => a.Items).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                brandsQuery = brandsQuery.Where(a => a.Name.Contains(name));
            }

            var viewModel = new BrandsViewModel
            {
                SearchName = name,
                Brands = await brandsQuery.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .Include(b => b.Items)
                .ThenInclude(i => i.Shops)
                .ThenInclude(s => s.Shop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            var viewModel = new BrandsCreateEditViewModel
            {
                Brands = new Brands()
            };

            return View(viewModel);
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandsCreateEditViewModel viewModel)
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
                    viewModel.Brands.Image = "/images/" + uniqueFileName;
                    //book.FrontPage = filePath;
                }

                if (viewModel.PdfFile != null && viewModel.PdfFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + viewModel.PdfFile.FileName;
                    string pdfFilePath = Path.Combine(uploadsFolder, uniquePdfFileName);

                    using (var pdfFileStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        await viewModel.PdfFile.CopyToAsync(pdfFileStream);
                    }

                    // Save PDF file path in the database
                    viewModel.Brands.DownloadCatalog = "/images/" + uniquePdfFileName;
                    //book.DownloadURL = pdfFilePath;
                }

                try
                {
                    _context.Update(viewModel.Brands);
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

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .Where(b => b.Id == id)
                .Include(b => b.Items)
                .FirstOrDefaultAsync();

            if (brands == null)
            {
                return NotFound();
            }

            BrandsCreateEditViewModel viewModel = new BrandsCreateEditViewModel
            {
                Brands = brands
            };

            return View(viewModel);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandsCreateEditViewModel viewModel)
        {
            if (id != viewModel.Brands.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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
                        viewModel.Brands.Image = "/images/" + uniqueFileName;
                        //book.FrontPage = filePath;
                    }

                    if (viewModel.PdfFile != null && viewModel.PdfFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                        string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + viewModel.PdfFile.FileName;
                        string pdfFilePath = Path.Combine(uploadsFolder, uniquePdfFileName);

                        using (var pdfFileStream = new FileStream(pdfFilePath, FileMode.Create))
                        {
                            await viewModel.PdfFile.CopyToAsync(pdfFileStream);
                        }

                        // Save PDF file path in the database
                        viewModel.Brands.DownloadCatalog = "/images/" + uniquePdfFileName;
                        //book.DownloadURL = pdfFilePath;
                    }

                    if (viewModel.Image == null && viewModel.PdfFile == null)
                    {
                        var existingBrand = _context.Brands.AsNoTracking().FirstOrDefault(b => b.Id == id);
                        if (existingBrand != null)
                        {
                            viewModel.Brands.Image = existingBrand.Image;
                            viewModel.Brands.DownloadCatalog = existingBrand.DownloadCatalog;
                        }
                    }

                    _context.Update(viewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandsExists(viewModel.Brands.Id))
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
            return View(viewModel);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .Include(b => b.Items)
                .ThenInclude(i => i.Shops)
                .ThenInclude(s => s.Shop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brands = await _context.Brands.FindAsync(id);
            if (brands != null)
            {
                _context.Brands.Remove(brands);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandsExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
