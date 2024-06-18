using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BicycleStore.Models;

namespace BicycleStore.Services
{
    public class BikeService
    {
        private readonly HttpClient _httpClient;

        public BikeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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

            return await response.Content.ReadFromJsonAsync<Bike>();
        }

        public async Task DeleteBikeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7042/api/bike/{id}");
            response.EnsureSuccessStatusCode();
        }

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


        public async Task<List<Bike>> GetBikesBySupplierIdAsync(int supplierId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7042/api/bike?supplierId={supplierId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Bike>>();
        }



        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Order>>("https://localhost:7042/api/order");
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7042/api/order/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7042/api/order", order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7042/api/order/{id}");
            response.EnsureSuccessStatusCode();
        }




    }
}
