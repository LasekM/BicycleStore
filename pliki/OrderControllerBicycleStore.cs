using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using BicycleStore.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BicycleStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly BikeService _bikeService;

        public OrderController(BikeService bikeService)
        {
            _bikeService = bikeService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _bikeService.GetOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _bikeService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            var bikes = await _bikeService.GetBikesAsync();
            ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BikeId,OrderDate,UserName")] Order order)
        {
            if (ModelState.IsValid)
            {
                var createdOrder = await _bikeService.CreateOrderAsync(order);
                return RedirectToAction(nameof(Details), new { id = createdOrder.OrderId });
            }

            var bikes = await _bikeService.GetBikesAsync();
            ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model", order.BikeId);
            return View(order);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _bikeService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bikeService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
