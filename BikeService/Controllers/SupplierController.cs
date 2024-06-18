using Microsoft.AspNetCore.Mvc;
using BikeService.Data;
using BikeService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BikeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupplierController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Supplier
        [HttpGet]
        public IActionResult Get()
        {
            var suppliers = _context.Suppliers.Include(s => s.Bikes).ToList();
            return Ok(suppliers);
        }

        // GET: api/Supplier/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var supplier = _context.Suppliers
                                    .Include(s => s.Bikes)
                                    .FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        // POST: api/Supplier
        [HttpPost]
        public IActionResult Post([FromBody] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Supplier/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // DELETE: api/Supplier/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();

            return Ok(supplier);
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
