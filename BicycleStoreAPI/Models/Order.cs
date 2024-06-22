using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BicycleStoreAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int BikeId { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public string UserName { get; set; }

        [ForeignKey("BikeId")]
        public Bike Bike { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
