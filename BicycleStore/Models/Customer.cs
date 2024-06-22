using BicycleStore.Models;
using System.Text.Json.Serialization;
namespace BicycleStore.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}