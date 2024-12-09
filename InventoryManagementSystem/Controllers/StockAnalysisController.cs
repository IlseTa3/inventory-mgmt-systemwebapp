using InventoryManagementSystem.Models;
using InventoryManagementSystem.ViewModels.ProductLocationStock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class StockAnalysisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StockAnalysisController> _logger;

        public StockAnalysisController(ApplicationDbContext context, ILogger<StockAnalysisController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ProductLocationStocks
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.ProductLocationsStocks.Include(p => p.LocationStock).Include(p => p.Product);
            if (User.IsInRole("Stockemployee") || User.IsInRole("Stockmanager") || User.IsInRole("Administrator"))
            {
                var applicationContext = from pls in _context.ProductLocationsStocks
                                         select new IndexViewModel()
                                         {
                                             Id = pls.Id,
                                             ProductNr = pls.Product.ProductNr,
                                             LocationName = pls.LocationStock.NameLocation,
                                             TotalInStock = pls.TotalInStock
                                         };
                return View(await applicationContext.ToListAsync());
            }
            return Forbid();

        }

    
        public IActionResult GetStockComparisonData(string[] locations)
        {

            if (locations == null || locations.Length == 0)
            {
                _logger.LogInformation("Geen locaties ontvangen. Alle data wordt opgehaald.");
                var alleData = _context.ProductLocationsStocks
                    .GroupBy(pls => new { pls.Product.ProductNr, pls.LocationStock.NameLocation })
                    .Select(group => new IndexViewModel
                    {
                        Id = group.First().Id,
                        ProductNr = group.Key.ProductNr,
                        LocationName = group.Key.NameLocation,
                        TotalInStock = group.Sum(pls => pls.TotalInStock)
                    }).ToList();
                _logger.LogInformation("Aantal records opgehaald: {Count}", alleData.Count);
                return Json(new { data = alleData });
            }

            _logger.LogInformation("Ontvangen locaties: {Locations}", string.Join(", ", locations));

            try
            {

                var filteredData = _context.ProductLocationsStocks
                    .Include(pls => pls.Product)
                    .Include(pls => pls.LocationStock)
                   .Where(pls => locations.Contains(pls.LocationStock.NameLocation))
                   .GroupBy(pls => new { pls.Product.ProductNr, pls.LocationStock.NameLocation })
                   .Select(group => new IndexViewModel
                   {
                       Id = group.First().Id,
                       ProductNr = group.Key.ProductNr,
                       LocationName = group.Key.NameLocation,
                       TotalInStock = group.Sum(pls => pls.TotalInStock)
                   }).ToList();

                _logger.LogInformation("Aantal resultaten: {Count}", filteredData.Count);
                foreach (var data in filteredData)
                {
                    _logger.LogInformation("ProductNr: {ProductNr}, Location: {LocationName}, Totaal in Stock: {TotalInStock}",
                        data.ProductNr, data.LocationName, data.TotalInStock);
                }

                return Json(new { data = filteredData });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout opgetreden bij het ophalen van voorraadgegevens");
                return StatusCode(500, "Er is een fout opgetreden bij het ophalen van de gegevens");
            }
        }

    }
}
