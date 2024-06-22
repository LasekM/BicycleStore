using System;
using System.Text.Json.Serialization;
using BicycleStore.Models;

namespace BicycleStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int BikeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserName { get; set; }  // Add this property
        public Bike Bike { get; set; }
        public Customer Customer { get; set; }
    }
}

