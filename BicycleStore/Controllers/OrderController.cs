using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using BicycleStore.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using BicycleStore.DbContext;
using System.Linq;

namespace BicycleStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly AppDbContext _context;

        public OrderController(IOrderService orderService, ICustomerService customerService, AppDbContext context)
        {
            _orderService = orderService;
            _customerService = customerService;
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders();
            return View(orders);
        }

        public IActionResult Create()
        {
            var bikes = _context.Bikes.ToList();
            ViewBag.Bikes = new SelectList(bikes, "Id", "Model");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                // Check if customer with the same last name already exists
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.LastName == order.Customer.LastName);

                if (existingCustomer == null)
                {
                    // Create new customer if it doesn't exist
                    var newCustomer = new Customer
                    {
                        LastName = order.Customer.LastName
                    };
                    _context.Customers.Add(newCustomer);
                    _context.SaveChanges();
                    order.CustomerId = newCustomer.CustomerId;
                }
                else
                {
                    order.CustomerId = existingCustomer.CustomerId;
                }

                _orderService.CreateOrder(order);
                return RedirectToAction("Index");
            }

            var bikes = _context.Bikes.ToList();
            ViewBag.Bikes = new SelectList(bikes, "Id", "Model");
            return View(order);
        }
    }
}
