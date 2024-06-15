using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using System.Collections.Generic;
using System.Linq;
using BicycleStore.DbContext;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var suppliers = _context.Suppliers.ToList();
            return View(suppliers);
        }

        public IActionResult Details(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            var bikes = _context.Bikes.Where(b => b.SupplierID == id).ToList();
            ViewBag.Bikes = bikes;
            return View(supplier);
        }

        public IActionResult CreateSupplier()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSupplier(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        public IActionResult CreateBike()
        {
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateBike(Bike bike)
        {
            if (ModelState.IsValid)
            {
                _context.Bikes.Add(bike);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View(bike);
        }



        public IActionResult EditSupplier(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public IActionResult EditSupplier(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        public IActionResult EditBike(int id)
        {
            var bike = _context.Bikes.FirstOrDefault(b => b.Id == id);
            if (bike == null)
            {
                return NotFound();
            }
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View(bike);
        }

        [HttpPost]
        public IActionResult EditBike(Bike bike)
        {
            if (ModelState.IsValid)
            {
                _context.Bikes.Update(bike);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = bike.SupplierID });
            }
            var suppliers = _context.Suppliers.ToList();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View(bike);
        }












        public IActionResult DeleteBike(int id)
        {
            var bike = _context.Bikes.FirstOrDefault(b => b.Id == id);
            if (bike == null)
            {
                return NotFound();
            }
            return View(bike);
        }

        [HttpPost, ActionName("DeleteBikeConfirmed")]
        public IActionResult DeleteBikeConfirmed(int id)
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







        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteSupplierConfirmed(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                var bikes = _context.Bikes.Where(b => b.SupplierID == id).ToList();
                _context.Bikes.RemoveRange(bikes);
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}
