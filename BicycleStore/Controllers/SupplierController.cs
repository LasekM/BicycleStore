using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using BicycleStore.Services;
using System.Threading.Tasks;

namespace BicycleStore.Controllers
{
    public class SupplierController : Controller
    {
        private readonly BikeService _bikeService;

        public SupplierController(BikeService bikeService)
        {
            _bikeService = bikeService;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _bikeService.GetSuppliersAsync();
            return View(suppliers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _bikeService.GetSupplierAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            var bikes = await _bikeService.GetBikesBySupplierIdAsync(id);
            supplier.Bikes = bikes;

            return View(supplier);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _bikeService.CreateSupplierAsync(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _bikeService.GetSupplierAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _bikeService.UpdateSupplierAsync(supplier.Id, supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _bikeService.GetSupplierAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bikeService.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Method to redirect to bike creation with supplierId
        public IActionResult CreateBike(int id)
        {
            return RedirectToAction("Create", "Bike", new { supplierId = id });
        }
    }
}
