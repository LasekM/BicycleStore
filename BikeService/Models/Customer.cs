using BikeService.Models;
namespace BikeService.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string LastName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}