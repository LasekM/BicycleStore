namespace BicycleStore.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int BikeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

        public Bike Bike { get; set; }
        public Customer Customer { get; set; }
    }
}
