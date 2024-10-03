using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class LocationStocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationStocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LocationStocks
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
            {
                return View(await _context.LocationStocks.ToListAsync());
            }
            return Forbid();

        }



        // GET: LocationStocks/Create
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public async Task<IActionResult> Create([Bind("Id,NameLocation,LocationAddress,PostalCode,Municipality,Country")] LocationStock locationStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locationStock);
        }

        // GET: LocationStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationStock = await _context.LocationStocks.FindAsync(id);
            if (locationStock == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
            {
                return View(locationStock);
            }
            return Forbid();
            //return View(locationStock);
        }

        // POST: LocationStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameLocation,LocationAddress,PostalCode,Municipality,Country")] LocationStock locationStock)
        {
            if (id != locationStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
                    {
                        _context.Update(locationStock);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return Forbid();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationStockExists(locationStock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return View(locationStock);
        }

        // GET: LocationStocks/Delete/5
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationStock = await _context.LocationStocks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationStock == null)
            {
                return NotFound();
            }

            return View(locationStock);
        }

        // POST: LocationStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationStock = await _context.LocationStocks.FindAsync(id);
            if (locationStock != null)
            {
                _context.LocationStocks.Remove(locationStock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationStockExists(int id)
        {
            return _context.LocationStocks.Any(e => e.Id == id);
        }

        public IActionResult LoadAllStockLocations()
        {
            try
            {
                var stockLocationData = (from s in _context.LocationStocks select s).ToList<LocationStock>();
                return Json(new { data = stockLocationData });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
