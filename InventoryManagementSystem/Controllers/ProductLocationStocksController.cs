using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Controllers
{
    public class ProductLocationStocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductLocationStocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductLocationStocks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductLocationsStocks.Include(p => p.LocationStock).Include(p => p.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        

        // GET: ProductLocationStocks/Create
        public IActionResult Create()
        {
            ViewData["LocationStockId"] = new SelectList(_context.LocationStocks, "Id", "NameLocation");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductNr");
            return View();
        }

        // POST: ProductLocationStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,LocationStockId,TotalInStock")] ProductLocationStock productLocationStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productLocationStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationStockId"] = new SelectList(_context.LocationStocks, "Id", "NameLocation", productLocationStock.LocationStockId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductNr", productLocationStock.ProductId);
            return View(productLocationStock);
        }

        // GET: ProductLocationStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productLocationStock = await _context.ProductLocationsStocks.FindAsync(id);
            if (productLocationStock == null)
            {
                return NotFound();
            }
            ViewData["LocationStockId"] = new SelectList(_context.LocationStocks, "Id", "NameLocation", productLocationStock.LocationStockId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductNr", productLocationStock.ProductId);
            return View(productLocationStock);
        }

        // POST: ProductLocationStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,LocationStockId,TotalInStock")] ProductLocationStock productLocationStock)
        {
            if (id != productLocationStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productLocationStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductLocationStockExists(productLocationStock.Id))
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
            ViewData["LocationStockId"] = new SelectList(_context.LocationStocks, "Id", "NameLocation", productLocationStock.LocationStockId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductNr", productLocationStock.ProductId);
            return View(productLocationStock);
        }

        // GET: ProductLocationStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productLocationStock = await _context.ProductLocationsStocks
                .Include(p => p.LocationStock)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productLocationStock == null)
            {
                return NotFound();
            }

            return View(productLocationStock);
        }

        // POST: ProductLocationStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productLocationStock = await _context.ProductLocationsStocks.FindAsync(id);
            if (productLocationStock != null)
            {
                _context.ProductLocationsStocks.Remove(productLocationStock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductLocationStockExists(int id)
        {
            return _context.ProductLocationsStocks.Any(e => e.Id == id);
        }
    }
}
