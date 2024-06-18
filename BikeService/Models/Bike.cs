using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BikeService.Models
{
    public class Bike
    {
        [HiddenInput]
        [Key]
        public int Id { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public int SupplierID { get; set; }
        [JsonIgnore]
        public Supplier? Supplier { get; set; }
        public bool IsReserved { get; set; }
    }
}

