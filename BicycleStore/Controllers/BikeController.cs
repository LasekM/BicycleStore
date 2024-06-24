using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using BicycleStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using BicycleStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using PagedList;

public class BikeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BikeController> _logger;

    public BikeController(HttpClient httpClient, ILogger<BikeController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(jsonString);
        return View(bikes);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Bike/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var bike = JsonConvert.DeserializeObject<Bike>(jsonString);
        if (bike == null)
        {
            return NotFound();
        }
        return View(bike);
    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create()
    {
        var suppliersResponse = await _httpClient.GetAsync("https://localhost:7265/api/Supplier");
        suppliersResponse.EnsureSuccessStatusCode();
        var suppliersJsonString = await suppliersResponse.Content.ReadAsStringAsync();
        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJsonString);

        ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Bike bike)
    {
        bike.IsReserved = false; // Ustawienie domyślnej wartości

        if (ModelState.IsValid)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Bike", bike);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating bike: {@Bike}", bike);
                ModelState.AddModelError(string.Empty, "Could not create bike. Please try again.");
            }
        }

        var suppliersResponse = await _httpClient.GetAsync("https://localhost:7265/api/Supplier");
        suppliersResponse.EnsureSuccessStatusCode();
        var suppliersJsonString = await suppliersResponse.Content.ReadAsStringAsync();
        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJsonString);

        ViewBag.Suppliers = new SelectList(suppliers, "SupplierID", "Name");
        return View(bike);
    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var bikeResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Bike/{id}");
        bikeResponse.EnsureSuccessStatusCode();
        var bikeJsonString = await bikeResponse.Content.ReadAsStringAsync();
        var bike = JsonConvert.DeserializeObject<Bike>(bikeJsonString);
        if (bike == null)
        {
            return NotFound();
        }

        var suppliersResponse = await _httpClient.GetAsync("https://localhost:7265/api/Supplier");
        suppliersResponse.EnsureSuccessStatusCode();
        var suppliersJsonString = await suppliersResponse.Content.ReadAsStringAsync();
        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJsonString);

        ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
        return View(bike);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Bike bike)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Bike/{bike.Id}", bike);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Details), new { id = bike.Id });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating bike: {@Bike}", bike);
                ModelState.AddModelError(string.Empty, "Could not update bike. Please try again.");
            }
        }

        var suppliersResponse = await _httpClient.GetAsync("https://localhost:7265/api/Supplier");
        suppliersResponse.EnsureSuccessStatusCode();
        var suppliersJsonString = await suppliersResponse.Content.ReadAsStringAsync();
        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJsonString);

        ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", bike.SupplierID);
        return View(bike);
    }


    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Bike/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var bike = JsonConvert.DeserializeObject<Bike>(jsonString);
        if (bike == null)
        {
            return NotFound();
        }
        return View(bike);
    }
    [Authorize(Roles = "admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7265/api/Bike/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error deleting bike with ID: {Id}", id);
            return BadRequest("Could not delete bike. Please try again.");
        }
    }

    public async Task<IActionResult> Search(string searchString, string selectedCategory, int? selectedSupplierId)
    {
        var response = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(jsonString);

        if (!string.IsNullOrEmpty(searchString))
        {
            bikes = bikes.Where(s => s.Model.ToLower().Contains(searchString.ToLower())).ToList();
        }

        if (!string.IsNullOrEmpty(selectedCategory))
        {
            bikes = bikes.Where(b => b.Category == selectedCategory).ToList();
        }

        if (selectedSupplierId.HasValue)
        {
            bikes = bikes.Where(b => b.SupplierID == selectedSupplierId.Value).ToList();
        }

        foreach (var bike in bikes)
        {
            var supplierResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Supplier/{bike.SupplierID}");
            supplierResponse.EnsureSuccessStatusCode();
            var supplierJsonString = await supplierResponse.Content.ReadAsStringAsync();
            bike.Supplier = JsonConvert.DeserializeObject<Supplier>(supplierJsonString);
        }

        var categoriesResponse = await _httpClient.GetAsync("https://localhost:7265/api/Bike/categories");
        categoriesResponse.EnsureSuccessStatusCode();
        var categoriesJsonString = await categoriesResponse.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<List<string>>(categoriesJsonString);

        var suppliersResponse = await _httpClient.GetAsync("https://localhost:7265/api/Supplier");
        suppliersResponse.EnsureSuccessStatusCode();
        var suppliersJsonString = await suppliersResponse.Content.ReadAsStringAsync();
        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJsonString);

        ViewBag.Categories = categories;
        ViewBag.Suppliers = suppliers;

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_BikeListPartial", bikes);
        }

        return View(bikes);
    }

    public async Task<IActionResult> SearchIndex(string searchString)
    {
        var response = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(jsonString);

        if (!string.IsNullOrEmpty(searchString))
        {
            bikes = bikes.Where(s => s.Model.ToLower().Contains(searchString.ToLower())).ToList();
        }

        foreach (var bike in bikes)
        {
            var supplierResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Supplier/{bike.SupplierID}");
            supplierResponse.EnsureSuccessStatusCode();
            var supplierJsonString = await supplierResponse.Content.ReadAsStringAsync();
            bike.Supplier = JsonConvert.DeserializeObject<Supplier>(supplierJsonString);
        }

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_BikeListPartialindex", bikes);
        }

        return View("Index", bikes);
    }


}
