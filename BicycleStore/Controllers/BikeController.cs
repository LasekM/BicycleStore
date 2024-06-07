using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BicycleStore.Entities;
using BicycleStore.DbContext;
using System.Threading.Tasks;

namespace RowerStore.Controllers
{
    public class BikeController : Controller
    {
        private readonly AppDbContext _context;

        public BikeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bikes = await _context.Bikes.ToListAsync();
            return View(bikes);
        }





       
    }
}
