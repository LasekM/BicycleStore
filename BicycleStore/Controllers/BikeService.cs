using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BicycleStore.Models;
using Newtonsoft.Json;

namespace BicycleStore.Services
{
    public class BikeService
    {
        private readonly HttpClient _httpClient;

        public BikeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // <<==== BIKE ====>>


        public async Task<List<Bike>> GetBikesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Bike>>("https://localhost:7042/api/bike");
        }

        public async Task<Bike> GetBikeAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7042/api/bike/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Bike>();
        }

        public async Task<Bike> CreateBikeAsync(Bike bike)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7042/api/bike", bike);
            response.EnsureSuccessStatusCode();

            var createdBike = await response.Content.ReadFromJsonAsync<Bike>();

            // Immediately retrieve the bike to ensure it's available
            return await GetBikeAsync(createdBike.Id);
        }

        public async Task<Bike> UpdateBikeAsync(int id, Bike bike)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7042/api/bike/{id}", bike);
            response.EnsureSuccessStatusCode();

            // Check if the content is empty
            if (response.Content.Headers.ContentLength == 0)
            {
                throw new JsonException("Response content is empty.");
            }

            return await response.Content.ReadFromJsonAsync<Bike>();
        }

        public async Task DeleteBikeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7042/api/bike/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Bike>> GetBikesBySupplierIdAsync(int supplierId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7042/api/bike?supplierId={supplierId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Bike>>();
        }

        public async Task<Bike> GetBikeByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Bike>($"https://localhost:7042/api/bike/{id}");
        }


        // <<==== SUPPLIER ====>>

        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7042/api/supplier");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Supplier>>();
        }

        public async Task<Supplier> GetSupplierAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7042/api/supplier/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Supplier>();
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7042/api/supplier", supplier);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Supplier>();
        }

        public async Task<Supplier> UpdateSupplierAsync(int id, Supplier supplier)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7042/api/supplier/{id}", supplier);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Supplier>();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7042/api/supplier/{id}");
            response.EnsureSuccessStatusCode();
        }




        // <<==== ORDERS====>>

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("https://localhost:7042/api/order");
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Order>($"https://localhost:7042/api/order/{id}");
        }

        public async Task CreateOrderAsync(Order order)
        {
            // Tworzymy nowy obiekt DTO z minimalnym zestawem danych
            var orderDto = new
            {
                order.BikeId,
                order.OrderDate,
                order.UserName,
                order.CustomerId
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7042/api/order", orderDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to create order: {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7042/api/order/{id}", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7042/api/order/{id}");
            response.EnsureSuccessStatusCode();
        }





    }
}
