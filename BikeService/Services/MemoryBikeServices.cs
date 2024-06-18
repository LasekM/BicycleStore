using BikeService.Data;
using BikeService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikeService.Services
{
    public class MemoryBikeService : IBikeService
    {
        private readonly AppDbContext _context;

        public MemoryBikeService(AppDbContext context)
        {
            _context = context;
        }

        public int Add(Bike bike)
        {
            _context.Bikes.Add(bike);
            _context.SaveChanges();
            return bike.Id;
        }

        public void DeleteById(Bike bike)
        {
            _context.Bikes.Remove(bike);
            _context.SaveChanges();
        }

        public List<Bike> FindAll()
        {
            return _context.Bikes.Include(s => s.Supplier).ToList();
        }

        public Bike? FindById(int id)
        {
            return _context.Bikes.Include(s => s.Supplier).FirstOrDefault(a => a.Id == id);
        }

        public void Update(Bike bike)
        {
            var actualBike = _context.Bikes.Find(bike.Id);
            if (actualBike != null)
            {
                _context.Entry(actualBike).CurrentValues.SetValues(bike);
                _context.SaveChanges();
            }
        }
    }
}
