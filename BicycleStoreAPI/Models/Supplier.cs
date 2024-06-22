using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;



namespace BicycleStoreAPI.Models
{
    public class Supplier
    {

        public int Id { get; set; }

        public string Name { get; set; }//

        public ICollection<Bike> Bikes { get; set; } = new HashSet<Bike>();
    }

   

}
