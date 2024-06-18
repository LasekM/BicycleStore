/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeService.Data;
using BikeService.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(o => o.Bike).Include(o => o.Customer).ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.Bike).Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
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

            var bike = await _context.Bikes.FindAsync(order.BikeId);
            if (bike == null)
            {
                return BadRequest("Invalid Bike");
            }

            bike.IsReserved = true; // Mark the bike as reserved
            order.Bike = bike;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes.FindAsync(order.BikeId);
            if (bike != null)
            {
                bike.IsReserved = false; // Mark the bike as unreserved
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
*/


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeService.Data;
using BikeService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(o => o.Bike).Include(o => o.Customer).ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.Bike).Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
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

            var bike = await _context.Bikes.FindAsync(order.BikeId);
            if (bike == null)
            {
                return BadRequest(new { error = "Invalid Bike" });
            }

            bike.IsReserved = true;
            order.Bike = bike;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes.FindAsync(order.BikeId);
            if (bike != null)
            {
                bike.IsReserved = false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
