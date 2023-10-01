using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace webApp.Controllers
{
    public class SaleController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<SaleController> _logger;

        public SaleController(MyDbContext dbContext, ILogger<SaleController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }



        [HttpGet]
        [Authorize(Roles = "Admin, Sales")]
        public async Task<ActionResult> SaleEdit(int itemId, string section = "Information")
        {
            SaleItemEF model = await _dbContext.SaleItems
                                                    .Where(i => i.Id == itemId)
                                                    .Include(si => si.SalePhotos)
                                                    .Include(si => si.ItemAvailability)
                                                    .Include(si => si.ItemCategory)
                                                    .FirstOrDefaultAsync();

            ViewBag.Section = section;
			
			//Create alert
			TempData.SetAlert("success", "Data úspěšně načtena");

            return View("SaleEdit", model);
        }

    }
}
