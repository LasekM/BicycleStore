using BicycleStore.Models;
namespace BicycleStore.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string LastName { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
