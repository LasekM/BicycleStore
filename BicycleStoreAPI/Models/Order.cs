using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BicycleStoreAPI.Models;

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
        [JsonIgnore]
        public Bike Bike { get; set; }

        [ForeignKey("CustomerId")]
        [JsonIgnore]
        public Customer Customer { get; set; }
    }
}

