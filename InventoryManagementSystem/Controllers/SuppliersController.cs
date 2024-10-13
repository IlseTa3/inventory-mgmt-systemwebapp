using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        // GET: Suppliers

        public async Task<IActionResult> Index()

        {
            if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
            {
                return View(await _context.Suppliers.ToListAsync());
            }
            return Forbid();

        }

        // GET: Suppliers/Create
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public async Task<IActionResult> Create([Bind("Id,NameSupplier,Address,PostalCode,Municipality,Country,VatNr,PhoneNr,Email")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        //[Authorize(Policy = "StockemployeeReadUpdate")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
            {
                return View(supplier);
            }
            return Forbid();
            //return View(supplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "StockemployeeReadUpdate")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameSupplier,Address,PostalCode,Municipality,Country,VatNr,PhoneNr,Email")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
                    {
                        _context.Update(supplier);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return Forbid();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "StockmanagerOrAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }

        public IActionResult LoadAllSuppliers()
        {
            try
            {
                var supplierData = (from s in _context.Suppliers select s).ToList<Supplier>();
                return Json(new { data = supplierData });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
