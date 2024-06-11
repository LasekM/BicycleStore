using BicycleStore.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using BicycleStore.Models;

namespace BicycleStore.Services
{
    public class MemoryBicycleService : IBicycleService
    {
        private readonly AppDbContext _context;

        public MemoryBicycleService(AppDbContext context)
        {
            _context = context;
        }

        public int Add(Bike bike)
        {
            _context.Bikes.Add(bike);
            _context.SaveChanges();
            return bike.BikeId;
        }

        public void DeleteById(Bike bike)
        {
            _context.Bikes.Remove(bike);
            _context.SaveChanges();
        }

        public List<Bike> FindAll()
        {
            return _context.Bikes.Include(b => b.Supplier).ToList();
        }

        public Bike? FindById(int id)
        {
            return _context.Bikes.FirstOrDefault(b => b.BikeId == id);
        }

        public void Update(Bike bike)
        {
            var actualBike = _context.Bikes.Find(bike.BikeId);
            if (actualBike != null)
            {
                _context.Entry(actualBike).CurrentValues.SetValues(bike);
                _context.SaveChanges();
            }
        }
    }
}
