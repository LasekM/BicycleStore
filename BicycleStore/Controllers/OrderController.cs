using Microsoft.AspNetCore.Mvc;
using BicycleStore.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

public class OrderController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderController> _logger;

    public OrderController(HttpClient httpClient, ILogger<OrderController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("https://localhost:7265/api/Order");
        response.EnsureSuccessStatusCode();

        var ordersJsonString = await response.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersJsonString);

        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Order/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<Order>(jsonString);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    public async Task<IActionResult> Create()
    {
        var bikesResponse = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        bikesResponse.EnsureSuccessStatusCode();
        var bikesJsonString = await bikesResponse.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(bikesJsonString);

        ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BikeId,OrderDate,UserName")] Order order)
    {
        _logger.LogInformation("Create order POST action called.");
        _logger.LogInformation("UserName: {UserName}", order.UserName);

        var customer = await GetOrCreateCustomer(order.UserName);

        if (customer == null)
        {
            ModelState.AddModelError("UserName", "Failed to create or retrieve customer.");
            await PopulateBikesSelectList(order.BikeId);
            return View(order);
        }

        order.CustomerId = customer.CustomerId;
        ModelState.Remove("Bike");
        ModelState.Remove("Customer");

        if (ModelState.IsValid)
        {
            var bike = await GetBikeById(order.BikeId);
            if (bike == null)
            {
                ModelState.AddModelError("BikeId", "Invalid Bike");
                _logger.LogWarning("Invalid Bike ID: {BikeId}", order.BikeId);
            }
            else
            {
                bike.IsReserved = true; // Mark bike as reserved
                order.Bike = bike;
                order.Customer = customer;

                var createOrderResponse = await CreateOrder(order);

                if (createOrderResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Failed to create order.");
            }
        }

        await PopulateBikesSelectList(order.BikeId);
        return View(order);
    }

    private async Task<Customer> GetOrCreateCustomer(string userName)
    {
        _logger.LogInformation("Attempting to get customer by name: {userName}", userName);
        var customerResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Customer/ByName/{userName}");
        if (customerResponse.IsSuccessStatusCode)
        {
            var customerJsonString = await customerResponse.Content.ReadAsStringAsync();
            _logger.LogInformation("Customer found: {customerJsonString}", customerJsonString);
            return JsonConvert.DeserializeObject<Customer>(customerJsonString);
        }
        else
        {
            _logger.LogWarning("Customer not found, status code: {StatusCode}", customerResponse.StatusCode);
        }

        var newCustomer = new Customer { LastName = userName };
        _logger.LogInformation("Creating new customer: {userName}", userName);
        var createCustomerResponse = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Customer", newCustomer);
        if (createCustomerResponse.IsSuccessStatusCode)
        {
            var customerJsonString = await createCustomerResponse.Content.ReadAsStringAsync();
            _logger.LogInformation("Customer created: {customerJsonString}", customerJsonString);
            return JsonConvert.DeserializeObject<Customer>(customerJsonString);
        }
        else
        {
            _logger.LogError("Failed to create customer, status code: {StatusCode}", createCustomerResponse.StatusCode);
        }

        return null;
    }

    private async Task<Bike> GetBikeById(int bikeId)
    {
        var bikeResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Bike/{bikeId}");
        if (bikeResponse.IsSuccessStatusCode)
        {
            var bikeJsonString = await bikeResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Bike>(bikeJsonString);
        }
        return null;
    }

    private async Task<HttpResponseMessage> CreateOrder(Order order)
    {
        var createOrderResponse = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Order", order);
        return createOrderResponse;
    }

    private async Task PopulateBikesSelectList(int? selectedBikeId = null)
    {
        var bikesResponse = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        bikesResponse.EnsureSuccessStatusCode();
        var bikesJsonString = await bikesResponse.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(bikesJsonString);
        ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model", selectedBikeId);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Order/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<Order>(jsonString);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Order order)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Order/{order.OrderId}", order);
        response.EnsureSuccessStatusCode();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Order/{id}");
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<Order>(jsonString);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7265/api/Order/{id}");
        response.EnsureSuccessStatusCode();
        return RedirectToAction(nameof(Index));
    }
}
