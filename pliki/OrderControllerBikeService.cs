using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeService.Data;
using BikeService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BikeController : ControllerBase
{
    private readonly AppDbContext _context;

    public BikeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bike>>> GetBikes([FromQuery] int? supplierId)
    {
        IQueryable<Bike> query = _context.Bikes.Include(b => b.Supplier);

        if (supplierId.HasValue)
        {
            query = query.Where(b => b.SupplierID == supplierId.Value);
        }

        var bikes = await query.ToListAsync();
        return Ok(bikes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bike>> GetBike(int id)
    {
        var bike = await _context.Bikes.Include(b => b.Supplier).FirstOrDefaultAsync(b => b.Id == id);

        if (bike == null)
        {
            return NotFound();
        }

        return bike;
    }

    [HttpPost]
    public async Task<ActionResult<Bike>> PostBike(Bike bike)
    {
        _context.Bikes.Add(bike);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBike", new { id = bike.Id }, bike);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBike(int id, Bike bike)
    {
        if (id != bike.Id)
        {
            return BadRequest();
        }

        _context.Entry(bike).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBike(int id)
    {
        var bike = await _context.Bikes.FindAsync(id);
        if (bike == null)
        {
            return NotFound();
        }

        _context.Bikes.Remove(bike);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
