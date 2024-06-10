namespace BicycleStore.Models
{
    public class Bike
    {
        public int BikeId { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }

        public int SupplierId { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Supplier Supplier { get; set; }
    }
}
