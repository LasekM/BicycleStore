using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using System.Linq;
using BicycleStore.DbContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BicycleStore.Controllers
{
    public class BikeController : Controller
    {
        private readonly AppDbContext _context;

        public BikeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bikes = _context.Bikes.ToList();
            return View(bikes);
        }

        public IActionResult Details(int id)
        {
            var bike = _context.Bikes.Include(b => b.Supplier).FirstOrDefault(b => b.Id == id);
            if (bike == null)
            {
                return NotFound();
            }
            return View(bike);
        }


        public IActionResult Create(int supplierId)
        {
            /*var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View();*/
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            ViewBag.SelectedSupplierId = supplierId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Bike bike)
        {
            /* if (ModelState.IsValid)
             {
                 _context.Bikes.Add(bike);
                 _context.SaveChanges();
                 return RedirectToAction(nameof(Index));
             }
             var suppliers = _context.Suppliers.ToList();
             ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
             return View(bike);*/
            if (ModelState.IsValid)
            {
                _context.Bikes.Add(bike);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = bike.Id });
            }
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View(bike);
        }

        public IActionResult Edit(int id)
        {
            var bike = _context.Bikes.FirstOrDefault(b => b.Id == id);
            if (bike == null)
            {
                return NotFound();
            }
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
            return View(bike);
        }

        [HttpPost]
        public IActionResult Edit(Bike bike)
        {
            if (ModelState.IsValid)
            {
                _context.Bikes.Update(bike);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = bike.Id });
            }
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
            return View(bike);
        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var bikes = from b in _context.Bikes.Include(b => b.Supplier)
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                bikes = bikes.Where(s => s.Model.Contains(searchString));
            }

            return View(bikes.ToList());
        }

        [HttpGet]
        public JsonResult SearchBikes(string searchString)
        {
            var bikes = from b in _context.Bikes.Include(b => b.Supplier)
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                bikes = bikes.Where(s => s.Model.ToLower().Contains(searchString));
            }

            var result = bikes.Select(b => new {
                b.Id,
                b.Model,
                b.Price,
                SupplierName = b.Supplier.Name
            }).ToList();

            return Json(result);
        }








        public IActionResult Delete(int id)
        {
            var bike = _context.Bikes.FirstOrDefault(b => b.Id == id);
            if (bike == null)
            {
                return NotFound();
            }
            return View(bike);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var bike = _context.Bikes.FirstOrDefault(b => b.Id == id);
            if (bike != null)
            {
                _context.Bikes.Remove(bike);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
