using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using BicycleStore.Models;

public class SupplierController : Controller
{
    private readonly HttpClient _httpClient;

    public SupplierController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("https://localhost:7265/api/Supplier");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonString);
        return View(suppliers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Supplier/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var supplier = JsonConvert.DeserializeObject<Supplier>(jsonString);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Supplier", supplier);
        response.EnsureSuccessStatusCode();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Supplier/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var supplier = JsonConvert.DeserializeObject<Supplier>(jsonString);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Supplier supplier)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Supplier/{supplier.Id}", supplier);
        response.EnsureSuccessStatusCode();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Supplier/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var supplier = JsonConvert.DeserializeObject<Supplier>(jsonString);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7265/api/Supplier/{id}");
        response.EnsureSuccessStatusCode();
        return RedirectToAction(nameof(Index));
    }
}
