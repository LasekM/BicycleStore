using System;
using System.Text.Json.Serialization;
using BikeService.Models;

namespace BikeService.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int BikeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserName { get; set; }

        public Customer Customer { get; set; }
        public Bike Bike { get; set; }
    }
}

