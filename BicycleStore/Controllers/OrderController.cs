using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BicycleStore.Models;
using System.Linq;
using System.Threading.Tasks;
using BicycleStore.DbContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BicycleStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(AppDbContext context, ILogger<OrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Create()
        {
            ViewBag.Bikes = new SelectList(_context.Bikes.Where(b => !b.IsReserved), "Id", "Model");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BikeId,OrderDate,UserName")] Order order)
        {
            _logger.LogInformation("Create order POST action called.");
            _logger.LogInformation("UserName: {UserName}", order.UserName);

            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.LastName == order.UserName);

            if (customer == null)
            {
                customer = new Customer { LastName = order.UserName };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            order.CustomerId = customer.CustomerId;
            ModelState.Remove("Bike");
            ModelState.Remove("Customer");

            if (ModelState.IsValid)
            {
                var bike = await _context.Bikes.FindAsync(order.BikeId);
                if (bike == null)
                {
                    ModelState.AddModelError("BikeId", "Invalid Bike");
                    _logger.LogWarning("Invalid Bike ID: {BikeId}", order.BikeId);
                }
                else
                {
                    bike.IsReserved = true; // Oznacz rower jako zarezerwowany
                    order.Bike = bike;
                    order.Customer = customer;
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Order created successfully.");
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Bikes = new SelectList(_context.Bikes.Where(b => !b.IsReserved), "Id", "Model", order.BikeId);
            return View(order);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int BikeId)
        {
            _logger.LogInformation("Reserve bike POST action called.");
            var bike = await _context.Bikes.FindAsync(BikeId);
            if (bike == null)
            {
                _logger.LogWarning("Invalid Bike ID: {BikeId}", BikeId);
                return RedirectToAction(nameof(Search)); // Return to Search view if the bike is invalid
            }

            // Redirect to Reserve view with preselected bike
            ViewBag.Bikes = new SelectList(_context.Bikes, "Id", "Model", BikeId);
            var order = new Order
            {
                BikeId = BikeId,
                OrderDate = DateTime.Now // Set the current date as default
            };
            return View("Reserve", order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReservation([Bind("BikeId,OrderDate,UserName")] Order order)
        {
            _logger.LogInformation("Submit reservation POST action called.");
            _logger.LogInformation("UserName: {UserName}", order.UserName);

            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.LastName == order.UserName);

            if (customer == null)
            {
                customer = new Customer { LastName = order.UserName };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            order.CustomerId = customer.CustomerId;
            ModelState.Remove("Bike");
            ModelState.Remove("Customer");

            if (ModelState.IsValid)
            {
                var bike = await _context.Bikes.FindAsync(order.BikeId);
                if (bike == null)
                {
                    ModelState.AddModelError("BikeId", "Invalid Bike");
                    _logger.LogWarning("Invalid Bike ID: {BikeId}", order.BikeId);
                }
                else
                {
                    order.Bike = bike;
                    order.Customer = customer;
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Reservation created successfully.");
                    return RedirectToAction(nameof(ReservationSuccess), new { orderId = order.OrderId });
                }
            }

            ViewBag.Bikes = new SelectList(_context.Bikes, "Id", "Model", order.BikeId);
            return View("Reserve", order);
        }

        public async Task<IActionResult> ReservationSuccess(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Customer)
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> Search()
        {
            var bikes = await _context.Bikes.Include(b => b.Supplier).ToListAsync();
            return View(bikes);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
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
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                var bike = await _context.Bikes.FindAsync(order.BikeId);
                if (bike != null)
                {
                    bike.IsReserved = false; // Oznacz rower jako niezarezerwowany
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
