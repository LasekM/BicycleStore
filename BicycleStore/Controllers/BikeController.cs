using BicycleStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using BicycleStore.Services;
using Newtonsoft.Json;

namespace BicycleStore.Controllers
{
    [Authorize]
    public class BikeController : Controller
    {
        private readonly IBicycleService _rowerekService;
        private readonly ISupplierService _supplierService;

        public BikeController(IBicycleService rowerekService, ISupplierService supplierService)
        {
            _rowerekService = rowerekService;
            _supplierService = supplierService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var rowerDictionary = _rowerekService.FindAll().ToDictionary(b => b.BikeId);
            return View(rowerDictionary);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var suppliers = _supplierService.FindAll();
            ViewBag.Suppliers = new SelectList(suppliers, "SupplierId", "Name");
            return View(new Bike());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(Bike model)
        {
            if (ModelState.IsValid)
            {
                _rowerekService.Add(model);
                return RedirectToAction("Index");
            }
            var suppliers = _supplierService.FindAll();
            ViewBag.Suppliers = new SelectList(suppliers, "SupplierId", "Name");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Details(int id)
        {
            var rowerek = _rowerekService.FindById(id);
            if (rowerek == null)
            {
                return NotFound();
            }
            return View(rowerek);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var rowerek = _rowerekService.FindById(id);
            if (rowerek == null)
            {
                return NotFound();
            }
            var suppliers = _supplierService.FindAll();
            ViewBag.Suppliers = new SelectList(suppliers, "SupplierId", "Name");
            return View(rowerek);
        }

        // Edycja roweru
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult UpdateBike(int id)
        {
            var rowerek = _rowerekService.FindById(id);
            if (rowerek == null)
            {
                return NotFound();
            }
            return View(rowerek);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult UpdateBike(Bike model)
        {
            Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented)); // Logowanie całego modelu
            if (ModelState.IsValid)
            {
                _rowerekService.Update(model);
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(model);
        }

        // Edycja dostawcy
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult UpdateSupplier(int id)
        {
            var supplier = _supplierService.FindById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult UpdateSupplier(Supplier model)
        {
            if (ModelState.IsValid)
            {
                _supplierService.Update(model);
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var rowerek = _rowerekService.FindById(id);
            if (rowerek == null)
            {
                return NotFound();
            }
            return View(rowerek);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var rowerekToDelete = _rowerekService.FindById(id);
            if (rowerekToDelete != null)
            {
                _rowerekService.DeleteById(rowerekToDelete);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
