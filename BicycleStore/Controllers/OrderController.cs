using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using BicycleStore.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BicycleStore.DbContext;
using System;

namespace BicycleStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly BikeService _bikeService;
        private readonly AppDbContext _context;

        public OrderController(BikeService bikeService, AppDbContext context)
        {
            _bikeService = bikeService;
            _context = context;
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
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.LastName == order.UserName);

            if (customer == null)
            {
                customer = new Customer { LastName = order.UserName };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            order.CustomerId = customer.CustomerId;
            order.Customer = customer;

            ModelState.Remove("Bike");
            ModelState.Remove("Customer");

            if (ModelState.IsValid)
            {
                var bike = await _bikeService.GetBikeByIdAsync(order.BikeId);

                if (bike == null)
                {
                    ModelState.AddModelError("BikeId", "Invalid Bike");
                }
                else
                {
                    bike.IsReserved = true;
                    order.Bike = bike;
                    order.Customer = customer;
                    await _bikeService.CreateOrderAsync(order);
                    return RedirectToAction(nameof(Index));
                }
            }

            var bikes = await _bikeService.GetBikesAsync();
            ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model", order.BikeId);
            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _bikeService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var bikes = await _bikeService.GetBikesAsync();
            ViewBag.Bikes = new SelectList(bikes, "Id", "Model", order.BikeId);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                await _bikeService.UpdateOrderAsync(order.OrderId, order);
                return RedirectToAction(nameof(Details), new { id = order.OrderId });
            }
            var bikes = await _bikeService.GetBikesAsync();
            ViewBag.Bikes = new SelectList(bikes, "Id", "Model", order.BikeId);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bikeService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
