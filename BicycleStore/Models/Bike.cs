using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BicycleStore.Models
{    public class Bike
    {
        [HiddenInput]
        [Key]
        public int Id { get; set; }

        public string Model { get; set; }

        public string Category { get; set; }

        public string GroupSet { get; set; }

        public decimal Price { get; set; }

        public int SupplierID { get; set; }
        public Supplier? Supplier { get; set; }
        public bool IsReserved { get; set; }
    }

    public class BikeDto
    {
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int SupplierID { get; set; }
        public bool IsReserved { get; set; }
    }

}
