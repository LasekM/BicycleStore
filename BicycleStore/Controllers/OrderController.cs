using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BicycleStore.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
    [Authorize(Roles = "admin")]
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

    public async Task<IActionResult> DetailsUser(int id)
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
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create()
    {
        var bikesResponse = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        bikesResponse.EnsureSuccessStatusCode();
        var bikesJsonString = await bikesResponse.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(bikesJsonString);

        ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model");
        return View();
    }
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BikeId,OrderDate,UserName")] Order order)
    {
        // Usunięcie walidacji dla Bike i Customer
        ModelState.Remove("Bike");
        ModelState.Remove("Customer");

        if (!ModelState.IsValid)
        {
            await PopulateBikesSelectList(order.BikeId);
            return View(order);
        }

        var customer = await GetOrCreateCustomer(order.UserName);
        if (customer == null)
        {
            ModelState.AddModelError("UserName", "Failed to create or retrieve customer.");
            await PopulateBikesSelectList(order.BikeId);
            return View(order);
        }

        var bike = await GetBikeById(order.BikeId);
        if (bike == null || bike.IsReserved)
        {
            ModelState.AddModelError("BikeId", "Invalid or already reserved bike.");
            await PopulateBikesSelectList(order.BikeId);
            return View(order);
        }

        var newOrder = new
        {
            BikeId = order.BikeId,
            UserName = order.UserName,
            OrderDate = order.OrderDate
        };

        var createOrderResponse = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Order", newOrder);

        if (createOrderResponse.IsSuccessStatusCode)
        {
            bike.IsReserved = true;
            await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Bike/{bike.Id}", bike);
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Failed to create order.");
        await PopulateBikesSelectList(order.BikeId);
        return View(order);
    }
    [Authorize(Roles = "admin")]
    private async Task<Customer> GetOrCreateCustomer(string userName)
    {
        var customerResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Customer/ByName/{userName}");
        if (customerResponse.IsSuccessStatusCode)
        {
            var customerJsonString = await customerResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Customer>(customerJsonString);
        }

        var newCustomer = new Customer { LastName = userName };
        var createCustomerResponse = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Customer", newCustomer);
        if (createCustomerResponse.IsSuccessStatusCode)
        {
            var customerJsonString = await createCustomerResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Customer>(customerJsonString);
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

    private async Task PopulateBikesSelectList(int? selectedBikeId = null)
    {
        var bikesResponse = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        bikesResponse.EnsureSuccessStatusCode();
        var bikesJsonString = await bikesResponse.Content.ReadAsStringAsync();
        var bikes = JsonConvert.DeserializeObject<List<Bike>>(bikesJsonString);
        ViewBag.Bikes = new SelectList(bikes.Where(b => !b.IsReserved), "Id", "Model", selectedBikeId);
    }
    [Authorize(Roles = "admin")]
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

    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int OrderId)
    {
        var orderResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Order/{OrderId}");
        if (!orderResponse.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve the order.");
            return View();
        }

        var orderJsonString = await orderResponse.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<Order>(orderJsonString);
        if (order == null)
        {
            return NotFound();
        }






        var bike = await GetBikeById(order.BikeId);
        if (bike == null)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve the bike.");
            return View();
        }

        // Zaktualizuj stan roweru
        bike.IsReserved = false;
        var updateBikeResponse = await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Bike/{bike.Id}", bike);
        if (!updateBikeResponse.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to update the bike.");
            return View();
        }



        var response = await _httpClient.DeleteAsync($"https://localhost:7265/api/Order/{OrderId}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        // Opcjonalnie: obsłuż sytuację, gdy usunięcie nie powiodło się
        ModelState.AddModelError(string.Empty, "Failed to delete the order.");
        return View();
    }

    [HttpPost, ActionName("DeleteMyOrderConfirmed")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteMyOrderConfirmed(int id)
    {
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        // Fetch the order to ensure it belongs to the current user
        var orderResponse = await _httpClient.GetAsync($"https://localhost:7265/api/Order/{id}");
        if (!orderResponse.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve the order.");
            return View();
        }

        var orderJsonString = await orderResponse.Content.ReadAsStringAsync();
        var order = JsonConvert.DeserializeObject<Order>(orderJsonString);
        if (order == null || order.UserName != userName)
        {
            return Unauthorized(); // Return unauthorized if the order does not belong to the current user
        }

        // Retrieve and update the bike
        var bike = await GetBikeById(order.BikeId);
        if (bike == null)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve the bike.");
            return View();
        }

        bike.IsReserved = false;
        var updateBikeResponse = await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Bike/{bike.Id}", bike);
        if (!updateBikeResponse.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to update the bike.");
            return View();
        }

        // Delete the order
        var response = await _httpClient.DeleteAsync($"https://localhost:7265/api/Order/{id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(MyOrders));
        }

        ModelState.AddModelError(string.Empty, "Failed to delete the order.");
        return View();
    }






    [Authorize]
    public async Task<IActionResult> Reserve(int bikeId)
    {
        var bike = await GetBikeById(bikeId);
        if (bike == null || bike.IsReserved)
        {
            return NotFound();
        }

        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var order = new Order
        {
            BikeId = bikeId,
            OrderDate = DateTime.Now.AddDays(2), // ustawienie daty na bieżącą datę plus dwa dni
            UserName = userName // użytkownik pobrany z tokenu JWT
        };

        ViewBag.Bikes = new SelectList(await GetAvailableBikes(), "Id", "Model", bikeId);
        return View(order);
    }

    [HttpPost, ActionName("ReserveConfirm")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ReserveConfirm([Bind("BikeId,OrderDate")] Order order)
    {
        // Usunięcie walidacji dla Bike i Customer
        ModelState.Remove("Bike");
        ModelState.Remove("Customer");
        ModelState.Remove("UserName");
        if (!ModelState.IsValid)
        {
            ViewBag.Bikes = new SelectList(await GetAvailableBikes(), "Id", "Model", order.BikeId);
            return View(order);
        }

        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var customer = await GetOrCreateCustomer(userName);
        if (customer == null)
        {
            ModelState.AddModelError("UserName", "Failed to create or retrieve customer.");
            ViewBag.Bikes = new SelectList(await GetAvailableBikes(), "Id", "Model", order.BikeId);
            return View(order);
        }

        var bike = await GetBikeById(order.BikeId);
        if (bike == null || bike.IsReserved)
        {
            ModelState.AddModelError("BikeId", "Invalid or already reserved bike.");
            ViewBag.Bikes = new SelectList(await GetAvailableBikes(), "Id", "Model", order.BikeId);
            return View(order);
        }

        var newOrder = new
        {
            BikeId = order.BikeId,
            UserName = userName,
            OrderDate = order.OrderDate
        };

        var createOrderResponse = await _httpClient.PostAsJsonAsync("https://localhost:7265/api/Order", newOrder);

        if (createOrderResponse.IsSuccessStatusCode)
        {
            bike.IsReserved = true;
            await _httpClient.PutAsJsonAsync($"https://localhost:7265/api/Bike/{bike.Id}", bike);
            return RedirectToAction(nameof(ReservationSuccess), new { id = order.BikeId });
        }

        ModelState.AddModelError(string.Empty, "Failed to create order.");
        ViewBag.Bikes = new SelectList(await GetAvailableBikes(), "Id", "Model", order.BikeId);
        return View(order);
    }



    public async Task<IActionResult> ReservationSuccess(int id)
    {
        var bike = await GetBikeById(id);
        if (bike == null)
        {
            return NotFound();
        }
        return View(bike);
    }

    private async Task<IEnumerable<Bike>> GetAvailableBikes()
    {
        var bikesResponse = await _httpClient.GetAsync("https://localhost:7265/api/Bike");
        bikesResponse.EnsureSuccessStatusCode();
        var bikesJsonString = await bikesResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Bike>>(bikesJsonString).Where(b => !b.IsReserved);
    }

    [Authorize]
    public async Task<IActionResult> MyOrders()
    {
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var response = await _httpClient.GetAsync($"https://localhost:7265/api/Order/ByUserName/{userName}");

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewBag.Message = "You have no orders.";
                return View(new List<Order>()); // zwróć pustą listę zamówień
            }
            response.EnsureSuccessStatusCode(); // rzuci wyjątek dla innych błędów
        }

        var ordersJsonString = await response.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersJsonString);

        return View(orders);
    }



}
