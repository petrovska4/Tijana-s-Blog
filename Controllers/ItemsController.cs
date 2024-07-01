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
using static System.Reflection.Metadata.BlobBuilder;

namespace TijanasBlog.Controllers
{
    public class ItemsController : Controller
    {
        private readonly TijanasBlogContext _context;
        private readonly IWebHostEnvironment _environment;

        public ItemsController(TijanasBlogContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Items
        public async Task<IActionResult> Index(string name, string brand, string type, string shop)
        {
            var shopsQuery = _context.Shops
               .OrderBy(g => g.Name)
               .Select(g => g.Name)
               .Distinct();

            var itemsQuery = _context.Items
                .Include(b => b.Brand)
                .Include(b => b.Shops)
                .ThenInclude(bg => bg.Shop)
                .AsQueryable();

            var brandsQuery = _context.Brands.AsQueryable();

            if (!string.IsNullOrEmpty(shop))
            {
                itemsQuery = itemsQuery.Where(b => b.Shops.Any(bg => bg.Shop.Name == shop));
            }

            if (!string.IsNullOrEmpty(name))
            {
                itemsQuery = itemsQuery.Where(b => b.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(type))
            {
                itemsQuery = itemsQuery.Where(b => b.Type.Contains(type));
            }

            if (!string.IsNullOrEmpty(brand))
            {
                itemsQuery = itemsQuery.Where(a => (a.Brand.Name).Contains(brand));
            }

            var shops = await shopsQuery.ToListAsync();
            var items = await itemsQuery.ToListAsync();
            var brands = await brandsQuery.ToListAsync();

            var viewModel = new ItemsViewModel
            {
                Items = items,
                Shops = new SelectList(shops),
                ItemShop = shop,
                SearchName = name,
                Brands = brands,
                SearchType = type,
                SearchBrand = brand
            };

            return View(viewModel);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .Include(i => i.Brand)
                .Include(i => i.Shops)
                .ThenInclude(s => s.Shop)
                .Include(i => i.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            var shops = _context.Shops.OrderBy(g => g.Name).ToList();

            var viewModel = new ItemsCreateEditViewModel
            {
                Item = new Items(),
                BrandsList = _context.Brands
                    .OrderBy(a => a.Name)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name,
                    }).ToList(),
                ShopsList = new SelectList(shops, "Id", "Name")
            };

            return View(viewModel);
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemsCreateEditViewModel viewModel)
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
                    viewModel.Item.Image = "/images/" + uniqueFileName;
                    //book.FrontPage = filePath;
                }

                try
                {
                    _context.Update(viewModel.Item);
                    await _context.SaveChangesAsync();

                    foreach (int shopId in viewModel.SelectedShops)
                    {
                        _context.ItemsShops.Add(new ItemsShops { ShopId = shopId, ItemId = viewModel.Item.Id });
                    }
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

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .Where(i => i.Id == id)
                .Include(i => i.Shops)
                .FirstOrDefaultAsync();

            if (items == null)
            {
                return NotFound();
            }

            var shops = _context.Shops.AsEnumerable();
            shops = shops.OrderBy(s => s.Name);

            ItemsCreateEditViewModel viewModel = new ItemsCreateEditViewModel
            {
                Item = items,
                ShopsList = new MultiSelectList(shops, "Id", "Name"),
                SelectedShops = items.Shops.Select(s => s.ShopId).ToList(),
                BrandsList = _context.Brands
                    .OrderBy(a => a.Name)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name,
                    }).ToList()
            };

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", items.BrandId);
            return View(viewModel);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemsCreateEditViewModel viewModel)
        {
            if (id != viewModel.Item.Id)
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

                        viewModel.Item.Image = "/images/" + uniqueFrontPageFileName;
                    }

                    if (viewModel.Image == null)
                    {
                        var existingItem = _context.Items.AsNoTracking().FirstOrDefault(b => b.Id == id);
                        if (existingItem != null)
                        {
                            viewModel.Item.Image = existingItem.Image;
                        }
                    }

                    _context.Update(viewModel.Item);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> newShopList = viewModel.SelectedShops ?? new List<int>();
                    IEnumerable<int> prevShopList = _context.ItemsShops.Where(s => s.ItemId == id).Select(s => s.ShopId).ToList();

                    IQueryable<ItemsShops> toBeRemoved = _context.ItemsShops.Where(s => s.ItemId == id);
                    if (newShopList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newShopList.Contains(s.ShopId));
                        foreach (int Id in newShopList)
                        {
                            if (!prevShopList.Any(s => s == Id))
                            {
                                _context.ItemsShops.Add(new ItemsShops { ShopId = Id, ItemId = id });
                            }
                        }
                    }

                    _context.ItemsShops.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsExists(viewModel.Item.Id))
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

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .Include(i => i.Brand)
                .Include(i => i.Reviews)
                .Include(i => i.Shops)
                .ThenInclude(s => s.Shop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var items = await _context.Items.FindAsync(id);
            if (items != null)
            {
                _context.Items.Remove(items);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
