namespace BicycleStore.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }

        public ICollection<Bike> Bikes { get; set; }
    }
}
