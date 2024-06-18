using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BikeService.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Bike> Bikes { get; set; } = new HashSet<Bike>();
    }
}

