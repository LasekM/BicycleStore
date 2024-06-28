using Microsoft.AspNetCore.Mvc;
using BicycleStoreAPI.Data;
using BicycleStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderController> _logger;

    public OrderController(AppDbContext context, ILogger<OrderController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.Include(o => o.Bike).Include(o => o.Customer).ToListAsync();
    }

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

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            
            _logger.LogInformation("Creating order for BikeId: {BikeId}, UserName: {UserName}, OrderDate: {OrderDate}", orderDto.BikeId, orderDto.UserName, orderDto.OrderDate);

            var bike = await _context.Bikes.FindAsync(orderDto.BikeId);
            if (bike == null)
            {
                _logger.LogWarning("Bike with ID {BikeId} not found", orderDto.BikeId);
                return NotFound("Bike not found");
            }

            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.LastName == orderDto.UserName);
            if (customer == null)
            {
                _logger.LogInformation("Customer with LastName {UserName} not found. Creating new customer.", orderDto.UserName);
                customer = new Customer { LastName = orderDto.UserName };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("Customer found: {CustomerId}", customer.CustomerId);
            }

            var order = new Order
            {
                BikeId = orderDto.BikeId,
                CustomerId = customer.CustomerId,
                OrderDate = orderDto.OrderDate,
                UserName = orderDto.UserName 
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order created successfully with ID {OrderId}", order.OrderId);
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex, "Error creating order");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, Order order)
    {
        if (id != order.OrderId)
        {
            return BadRequest();
        }

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Orders.Any(e => e.OrderId == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Order deleted successfully" });
    }

    [HttpGet("ByUserName/{userName}")]
    public async Task<IActionResult> GetOrdersByUserName(string userName)
    {
        var orders = await _context.Orders
            .Include(o => o.Bike)
            .Where(o => o.UserName == userName)
            .ToListAsync();

        if (orders == null || !orders.Any())
        {
            return NotFound();
        }

        return Ok(orders);
    }



}


public class CreateOrderDto
{
    public int BikeId { get; set; }
    public string UserName { get; set; }
    public DateTime OrderDate { get; set; }
}
