using BicycleStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using BicycleStore.Services;

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
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
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
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            return View(rowerek);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Update(Bike model)
        {
            if (ModelState.IsValid)
            {
                _rowerekService.Update(model);
                return RedirectToAction("Index");
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
