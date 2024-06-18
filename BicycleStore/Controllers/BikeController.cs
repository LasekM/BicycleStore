using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using BicycleStore.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BicycleStore.Controllers
{
    public class BikeController : Controller
    {
        private readonly BikeService _bikeService;

        public BikeController(BikeService bikeService)
        {
            _bikeService = bikeService;
        }

        public async Task<IActionResult> Index()
        {
            var bikes = await _bikeService.GetBikesAsync();
            return View(bikes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var bike = await _bikeService.GetBikeAsync(id);
            if (bike == null)
            {
                return NotFound();
            }
            if (bike.SupplierID > 0)
            {
                bike.Supplier = await _bikeService.GetSupplierAsync(bike.SupplierID);
            }
            return View(bike);
        }

        public async Task<IActionResult> Create(int supplierId)
        {
            var suppliers = await _bikeService.GetSuppliersAsync(); // Assume this method is implemented in BikeService
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            ViewBag.SelectedSupplierId = supplierId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bike bike)
        {
            if (ModelState.IsValid)
            {
                var createdBike = await _bikeService.CreateBikeAsync(bike);
                return RedirectToAction(nameof(Details), new { id = createdBike.Id });
            }

            var suppliers = await _bikeService.GetSuppliersAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
            return View(bike);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var bike = await _bikeService.GetBikeAsync(id);
            if (bike == null)
            {
                return NotFound();
            }
            var suppliers = await _bikeService.GetSuppliersAsync(); // Assume this method is implemented in BikeService
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
            return View(bike);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Bike bike)
        {
            if (ModelState.IsValid)
            {
                await _bikeService.UpdateBikeAsync(bike.Id, bike);
                return RedirectToAction(nameof(Details), new { id = bike.Id });
            }
            var suppliers = await _bikeService.GetSuppliersAsync(); // Assume this method is implemented in BikeService
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
            return View(bike);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bike = await _bikeService.GetBikeAsync(id);
            if (bike == null)
            {
                return NotFound();
            }
            return View(bike);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bikeService.DeleteBikeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var bikes = await _bikeService.GetBikesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                bikes = bikes.Where(s => s.Model.ToLower().Contains(searchString.ToLower())).ToList();
            }

            foreach (var bike in bikes)
            {
                bike.Supplier = await _bikeService.GetSupplierAsync(bike.SupplierID);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_BikeListPartial", bikes);
            }

            return View(bikes);
        }



    }
}
